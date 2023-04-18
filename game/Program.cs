using System;
using game;
namespace deep_deep_deep

{

#if WINDOWS || LINUX

    /// <summary>

    /// The main class.

    /// </summary>

    public static class Program

    {

        /// <summary>

        /// The main entry point for the application.

        /// </summary>

        [STAThread]

        static void Main()

        {
            //using (var game = new GameCycleView())
            //  game.Run();
            GameplayPresenter g = new GameplayPresenter(new GameCycleView(), new GameCycle());
            g.LaunchGame();
        }

    }

#endif

}