namespace AnCoFT.Networking
{
    using System.Collections.Generic;
    using System.Linq;
    using AnCoFT.Database;
    using AnCoFT.Database.Models;
    using AnCoFT.Game.MatchPlay.Room;

    public class GameHandler
    {
        public GameHandler(DatabaseContext databaseContext)
        {
            this.DatabaseContext = databaseContext;
            this.Clients = new List<Client>();
            this.Challenges = this.LoadChallenges();
            this.Rooms = new List<Room>();
        }

        private DatabaseContext DatabaseContext { get; set; }
        private List<Client> Clients { get; set; }
        private List<Challenge> Challenges { get; set; }
        public List<Room> Rooms { get; set; }

        private List<Challenge> LoadChallenges()
        {
            return this.DatabaseContext.Challenge.ToList();
        }

        public Challenge GetChallenge(short challengeId)
        {
            return this.Challenges.Find(c => c.ChallengeId == challengeId);
        }

        public List<Character> GetCharactersInLobby()
        {
            return this.Clients.Where(cl => cl.InLobby == true).Select(c => c.ActiveCharacter).ToList();
        }

        public List<Client> GetClientsInRoom(short roomId)
        {
            return this.Clients.Where(c => c.ActiveRoom?.Id == roomId).ToList();
        }

        public List<Client> GetClientsInLobby()
        {
            return this.Clients.Where(c => c.InLobby).ToList();
        }

        public void AddClient(Client client)
        {
            this.Clients.Add(client);
        }

        public void RemoveClient(Client client)
        {
			client.Account.Status = 0;
			client.DatabaseContext.Update(client.Account);
			client.DatabaseContext.SaveChanges();

            this.Clients.Remove(client);
        }
    }
}
