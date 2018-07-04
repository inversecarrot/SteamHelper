using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SteamKit2;
using System.Threading;
using System.Collections.Concurrent;

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
            ConcurrentQueue<string> q1 = new ConcurrentQueue<string>();
            ConcurrentQueue<string> q2 = new ConcurrentQueue<string>();
            manager = new LogonManager();
            logonm = new Thread(manager.Run);
            Application.SetCompatibleTextRenderingDefault(false);
            UI ui = new UI(manager);
            manager.LoadUI(ui);
            manager.loadMessagePipelineUI(q1, q2);
            ui.loadMessagePipelineLM(q2, q1);
            logonm.Start();
            Application.EnableVisualStyles();
            Application.Run(ui);
        }

        public static void Close()
        {
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
