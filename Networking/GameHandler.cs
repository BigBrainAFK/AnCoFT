namespace AnCoFT.Networking
{
    using System.Collections.Generic;

    public class GameHandler
    {
        public GameHandler()
        {
            this.Clients = new List<Client>();
        }

        private List<Client> Clients { get; set; }

        public void AddClient(Client client)
        {
            this.Clients.Add(client);
        }
    }
}