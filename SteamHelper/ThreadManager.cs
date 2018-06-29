using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SteamKit2;
using System.Threading;

namespace SteamHelper
{
    static class ThreadManager
    {
        static Thread logonm;
        static LogonManager manager;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            manager = new LogonManager();
            logonm = new Thread(manager.Run);
            logonm.Start();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new UI(manager));
            Console.Write("hi");
        }

        public static void close()
        {
            LogonManager.RUN_FLAG = false;
            logonm.Join();
        }

        // Step 1: Log on
        // Step 2: Fetch user data, load/store game data from steam
        // Step 3: Display list of games
        // Step 4a: Display data about games (start with data directly available from Steam)
        // Step 4b: Get as much data from web about game rating/genre/etc as possible (start small, add later)
        // Step 5: Suggest a game: use highest rating or most similar tags
    }
}
