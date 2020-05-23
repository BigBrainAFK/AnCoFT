namespace AnCoFT.Networking.Packet
{
    using System;
    using System.Text;

    public class Packet
    {
        private int _readPosition = 0;

        public Packet(Packet packet)
        {
            this.CheckSerial = packet.CheckSerial;
            this.CheckSum = packet.CheckSum;
            this.PacketId = packet.PacketId;
            this.DataLength = packet.DataLength;

            this.Data = new byte[this.DataLength];
            Buffer.BlockCopy(packet.Data, 0, this.Data, 0, this.DataLength);
        }

        public Packet(byte[] rawData)
        {
            this.CheckSerial = BitConverter.ToUInt16(rawData, 0);
            this.CheckSum = BitConverter.ToUInt16(rawData, 2);
            this.PacketId = BitConverter.ToUInt16(rawData, 4);
            this.DataLength = BitConverter.ToUInt16(rawData, 6);

            this.Data = new byte[this.DataLength];
            Buffer.BlockCopy(rawData, 8, this.Data, 0, this.DataLength);
        }

        public Packet(ushort packetId)
        {
            this.PacketId = packetId;
            this.DataLength = 0;
            this.Data = new byte[4096];
        }

        public ushort CheckSerial { get; set; }

        public ushort CheckSum { get; set; }

        public ushort PacketId { get; set; }

        public ushort DataLength { get; set; }

        public byte[] Data { get; set; }

        public static int IndexOf(byte[] array, byte[] pattern, int offset)
        {
            int success = 0;
            for (int i = offset; i < array.Length; i++)
            {
                if (array[i] == pattern[success])
                {
                    success++;
                }
                else
                {
                    success = 0;
                }

                if (pattern.Length == success)
                {
                    return i - pattern.Length + 1;
                }
            }

            return -1;
        }

        public void Write(params object[] dataList)
        {
            foreach (object o in dataList)
            {
                this.Write(o);
            }
        }

        public byte[] AddByteToArray(byte[] byteArray, byte newByte)
        {
            byte[] newArray = new byte[byteArray.Length + 1];
            byteArray.CopyTo(newArray, 1);
            newArray[0] = newByte;
            return newArray;
        }

        public void Write(object element)
        {
            byte[] dataElement;
            switch (Type.GetTypeCode(element.GetType()))
            {
                case TypeCode.Int16:
                    dataElement = BitConverter.GetBytes(Convert.ToInt16(element));
                    Buffer.BlockCopy(dataElement, 0, this.Data, this.DataLength, 2);
                    this.DataLength += 2;
                    break;
                case TypeCode.UInt16:
                    dataElement = BitConverter.GetBytes(Convert.ToUInt16(element));
                    Buffer.BlockCopy(dataElement, 0, this.Data, this.DataLength, 2);
                    this.DataLength += 2;
                    break;

                case TypeCode.Int32:
                    dataElement = BitConverter.GetBytes(Convert.ToInt32(element));
                    Buffer.BlockCopy(dataElement, 0, this.Data, this.DataLength, 4);
                    this.DataLength += 4;
                    break;
                case TypeCode.UInt32:
                    dataElement = BitConverter.GetBytes(Convert.ToUInt32(element));
                    Buffer.BlockCopy(dataElement, 0, this.Data, this.DataLength, 4);
                    this.DataLength += 4;
                    break;

                case TypeCode.Int64:
                    dataElement = BitConverter.GetBytes(Convert.ToInt64(element));
                    Buffer.BlockCopy(dataElement, 0, this.Data, this.DataLength, 8);
                    this.DataLength += 8;
                    break;
                case TypeCode.UInt64:
                    dataElement = BitConverter.GetBytes(Convert.ToUInt64(element));
                    Buffer.BlockCopy(dataElement, 0, this.Data, this.DataLength, 8);
                    this.DataLength += 8;
                    break;

                case TypeCode.DateTime:
                    dataElement = BitConverter.GetBytes(Convert.ToDateTime(element).ToFileTime());
                    Buffer.BlockCopy(dataElement, 0, this.Data, this.DataLength, 8);
                    this.DataLength += 8;
                    break;

                case TypeCode.String:
                    dataElement = Encoding.Unicode.GetBytes(Convert.ToString(element));
                    Buffer.BlockCopy(dataElement, 0, this.Data, this.DataLength, dataElement.Length);
                    this.DataLength += Convert.ToUInt16(dataElement.Length);

                    Buffer.BlockCopy(new byte[] { 0, 0 }, 0, this.Data, this.DataLength, 2);
                    this.DataLength += 2;
                    break;

                case TypeCode.Byte:
                    dataElement = BitConverter.GetBytes((byte)element);
                    Buffer.BlockCopy(dataElement, 0, this.Data, this.DataLength, 1);
                    this.DataLength += 1;
                    break;

                case TypeCode.Boolean:
                    dataElement = BitConverter.GetBytes((bool)element);
                    Buffer.BlockCopy(dataElement, 0, this.Data, this.DataLength, 1);
                    this.DataLength += 1;
                    break;

                default:
                    break;
            }
        }

        public int ReadInteger()
        {
            int element = BitConverter.ToInt32(this.Data, this._readPosition);
            this._readPosition += 4;
            return element;
        }

        public byte ReadByte()
        {
            byte result = this.Data[this._readPosition];
            this._readPosition += 1;
            return result;
        }

        public bool ReadBoolean()
        {
            byte result = this.Data[this._readPosition];
            this._readPosition += 1;
            return Convert.ToBoolean(result);
        }

        public void ReadByte(out byte element)
        {
            element = this.Data[this._readPosition];
            this._readPosition += 1;
        }

        public short ReadShort()
        {
            short result = BitConverter.ToInt16(this.Data, this._readPosition);
            this._readPosition += 2;
            return result;
        }

        public string ReadUnicodeString()
        {
            string result = string.Empty;
            int stringLength = IndexOf(this.Data, new byte[] { 0x00, 0x00 }, this._readPosition) + 1 - this._readPosition;
            if (stringLength > 1)
            {
                result = Encoding.Unicode.GetString(this.Data, this._readPosition, stringLength);
                this._readPosition += stringLength + 2;
            }
            else
            {
                result = string.Empty;
                this._readPosition += 2;
            }

            return result;
        }

        public string ReadString()
        {
            string result = string.Empty;
            int stringLength = IndexOf(this.Data, new byte[] { 0x00 }, this._readPosition) - this._readPosition;
            if (stringLength > 0)
            {
                result = Encoding.ASCII.GetString(this.Data, this._readPosition, stringLength);
                this._readPosition += stringLength + 1;
            }

            return result;
        }

        public byte[] GetRawPacket()
        {
            byte[] p = new byte[8 + this.DataLength];

            byte[] serial = BitConverter.GetBytes(this.CheckSerial);
            byte[] check = BitConverter.GetBytes(this.CheckSum);
            byte[] id = BitConverter.GetBytes(this.PacketId);
            byte[] dataLength = BitConverter.GetBytes(this.DataLength);

            Buffer.BlockCopy(serial, 0, p, 0, 2);
            Buffer.BlockCopy(check, 0, p, 2, 2);
            Buffer.BlockCopy(id, 0, p, 4, 2);
            Buffer.BlockCopy(dataLength, 0, p, 6, 2);
            Buffer.BlockCopy(this.Data, 0, p, 8, this.DataLength);

            return p;
        }
    }
}