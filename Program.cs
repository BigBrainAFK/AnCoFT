namespace AnCoFT
{
    using System;
    using System.Diagnostics;
    using System.IO;

    using AnCoFT.Database;
    using AnCoFT.Networking.Server;

    internal class Program
    {
        private static void Main()
        {
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("AnCoFT - Fantasy Tennis Server Emulator");
            Console.WriteLine("---------------------------------------");

            Console.Write("Initializing Database...");
            DatabaseContext databaseContext = new DatabaseContext();

            try
            {
                databaseContext.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occured while initializing database [{ex.Message}]");
                Console.WriteLine("Press any key to exit...");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Successful!");
            Console.Write("Initializing LoginServer...");
            LoginServer loginServer;
            try
            {
                loginServer = new LoginServer("127.0.0.1", 5894, databaseContext);
                loginServer.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occured while initializing LoginServer [{ex.Message}]");
                Console.WriteLine("Press any key to exit...");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Successful!");
            Console.Write("Initializing GameServer...");
            GameServer gameServer;
            try
            {
                gameServer = new GameServer("127.0.0.1", 5895, databaseContext);
                gameServer.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occured while initializing GameServer [{ex.Message}]");
                Console.WriteLine("Press any key to exit...");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Successful!");
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();

            loginServer.Stop();
            gameServer.Stop();
        }
    }
}