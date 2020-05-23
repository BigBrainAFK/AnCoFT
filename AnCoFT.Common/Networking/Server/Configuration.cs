using System;
using System.IO;
using System.Threading;
using Newtonsoft.Json;

namespace AnCoFT.Networking.Server
{
    public interface IServerAddress
    {
        string ip { get; set; }
        ushort port { get; set; }
    }

    public class LoginServerAddress : IServerAddress
    {
        public string ip { get; set; }
        public ushort port { get; set; }
        public LoginServerAddress(string ip, ushort port)
        {
            this.ip = ip;
            this.port = port;
        }
    }

    public class GameServerAddress : IServerAddress
    {
        public string ip { get; set; }
        public ushort port { get; set; }
        public GameServerAddress(string ip, ushort port)
        {
            this.ip = ip;
            this.port = port;
        }
    }

    public class DatabaseConfiguration
    {
        public string ip;
        public ushort port;
        public string username;
        public string password;
        public string database;

        public DatabaseConfiguration(string ip, ushort port, string username, string password, string database)
        {
            this.ip = ip;
            this.port = port;
            this.username = username;
            this.password = password;
            this.database = database;
        }
    }

	public class EMailConfiguration
	{
		public string SMTP;
		public int SMTP_Port;
		public string Username;
		public string Password;
		public string EMail;

		public EMailConfiguration(string smtp, int port, string username, string password, string email)
		{
			this.SMTP = smtp;
			this.SMTP_Port = port;
			this.Username = username;
			this.Password = password;
			this.EMail = email;
		}
	}

    public class Configuration
    {
        public DatabaseConfiguration dbConfig;
        public LoginServerAddress loginServAddress;
        public GameServerAddress[] gameServAddresses;
		public EMailConfiguration EMail;
        public string serverName;
		public string url;
		public string secret;

		public Configuration(string name, string url, LoginServerAddress loginServ, GameServerAddress[] gameServ, EMailConfiguration EMail, DatabaseConfiguration dbConfig, string secret)
        {
			this.secret = secret;
			this.url = url;
            this.serverName = name;
            this.loginServAddress = loginServ;
            this.gameServAddresses = gameServ;
			this.EMail = EMail;
            this.dbConfig = dbConfig;
        }

		public static Configuration loadConfiguration(string path = "./server.json")
		{
			Configuration temp;

			if (File.Exists(path))
			{
				while (!IsFileAccessible(path))
				{
					Thread.Sleep(100);
				}

				string json = File.ReadAllText(path) ?? "{}";
				temp = JsonConvert.DeserializeObject<Configuration>(json);

				if (temp.loginServAddress == null)
				{
					Console.WriteLine($"Error occured while reading in server configuration [NO_LOGIN_SERVER]");
					Console.WriteLine("Press any key to exit...");
					Console.ReadLine();
					return null;
				}
				if (temp.gameServAddresses == null)
				{
					Console.WriteLine($"Error occured while reading in server configuration [NO_GAME_SERVER]");
					Console.WriteLine("Press any key to exit...");
					Console.ReadLine();
					return null;
				}
			}
			else
			{
				DatabaseConfiguration dbConfig = new DatabaseConfiguration("127.0.0.1", 5432, "postgres", "", "AnCoFT");

				LoginServerAddress loginServAddress = new LoginServerAddress("127.0.0.1", 5894);

				GameServerAddress[] gameServerAddresses = new GameServerAddress[1];
				gameServerAddresses.SetValue(new GameServerAddress("127.0.0.1", 5895), 0);

				EMailConfiguration EMail = new EMailConfiguration("smtp.yourprovider.com", 587, "username", "password", "exampe@yourprovider.com");

				temp = new Configuration("AnCoFT", "AnCoFT.com", loginServAddress, gameServerAddresses, EMail, dbConfig, "InsertASecretHere");

				string serialized = JsonConvert.SerializeObject(temp, Formatting.Indented);
				File.WriteAllText("./server.json", serialized);
			}

			return temp;
		}

		public static bool IsFileAccessible(string path)
		{
			try
			{
				File.ReadAllText(path);
			}
			catch
			{
				return false;
			}

			return true;
		}
    }
}
