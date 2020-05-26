using Microsoft.EntityFrameworkCore;

namespace AnCoFT
{
    using System;
    using System.Linq;
	using AnCoFT.Database;
    using AnCoFT.Networking.Server;

    internal class GameServerProgramm
    {

		private static void Main()
        {
			Console.WriteLine("--------------------------------------------");
            Console.WriteLine("AnCoFT - Fantasy Tennis Game Server Emulator");
            Console.WriteLine("--------------------------------------------");

            Console.Write("Reading in config files... ");
			Configuration serverConfig = Configuration.loadConfiguration();
			if (serverConfig == null)
			{
				return;
			}
			Console.WriteLine("OK!");

            Console.Write("Initializing Database... ");
			DatabaseContext databaseContext  = new DatabaseContext(serverConfig.dbConfig);
            try
            {
                //databaseContext.Database.EnsureDeleted();
                databaseContext.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occured while initializing database [{ex.Message}]");
                Console.WriteLine("Press any key to exit...");
                Console.ReadLine();
                return;
            }
            Console.WriteLine("OK!");

            Console.WriteLine($"Initializing GameServer{(serverConfig.gameServAddresses.Length > 1 ? "s" : "")}... ");
            GameServer[] gameServers = new GameServer[serverConfig.gameServAddresses.Length];
            for (int i = 0; i < serverConfig.gameServAddresses.Length; i++)
            {
                Console.Write($"\tStarting GameServer-{i}... ");
                try
                {
                    gameServers[i] = new GameServer(serverConfig.gameServAddresses[i].ip, serverConfig.gameServAddresses[i].port, databaseContext, $"GAME-{i}");
                    gameServers[i].Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error occured while initializing GameServer [{ex.Message}]");
                    Console.WriteLine("Press any key to exit...");
                    Console.ReadLine();
                    return;
                }
                Console.WriteLine("OK!");
            }
            Console.SetCursorPosition(serverConfig.gameServAddresses.Length > 1 ? 28 : 27, Console.CursorTop - 4);
            Console.Write("OK!");
            Console.SetCursorPosition(0, Console.CursorTop + 4);

			while(true)
			{
				string input = Console.ReadLine();

				if (input == "exit" || input == "shutdown")
				{
					break;
				}
			}

			Console.Write("Shutting down... ");
			// Add sending of disconnect packet
			gameServers.ToList().ForEach(gs => gs.Stop());
			databaseContext.Database.CloseConnection();
		}

	}
}
