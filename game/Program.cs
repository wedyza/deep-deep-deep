using System;
using game;
namespace deep_deep_deep

{
#if WINDOWS || LINUX
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            GameplayPresenter g = new GameplayPresenter(new GameCycleView(), new GameCycle());
            g.LaunchGame();
        }
    }
#endif
}