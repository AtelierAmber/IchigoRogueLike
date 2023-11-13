using Ichigo.Engine;
using Ichigo.Screens;

namespace Ichigo {
  internal class Game
  {

    private const int StartingWidth = 80;
    private const int StartingHeight = 50;


    private static void Main()
    {
      Core.Start<MainMenu>(StartingWidth, StartingHeight);
    }
  }
}

