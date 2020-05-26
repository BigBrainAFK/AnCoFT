using Microsoft.EntityFrameworkCore;

namespace AnCoFT
{
    using System;
	using AnCoFT.Database;
    using AnCoFT.Networking.Server;

    internal class LoginServerProgramm
    {

		private static void Main()
        {
			Console.WriteLine("---------------------------------------------");
            Console.WriteLine("AnCoFT - Fantasy Tennis Username Server Emulator");
            Console.WriteLine("---------------------------------------------");

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

            Console.Write("Initializing LoginServer... ");
			LoginServer loginServer;
            try
            {
                loginServer = new LoginServer(serverConfig.loginServAddress.ip, serverConfig.loginServAddress.port, databaseContext);
                loginServer.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occured while initializing LoginServer [{ex.Message}]");
                Console.WriteLine("Press any key to exit...");
                Console.ReadLine();
                return;
            }
            Console.WriteLine("OK!");

			while(true)
			{
				string input = Console.ReadLine();

				if (input == "exit" || input == "shutdown")
				{
					break;
				}
			}

			Console.Write("Shutting down... ");
			loginServer.Stop();
			databaseContext.Database.CloseConnection();
		}

	}
}
