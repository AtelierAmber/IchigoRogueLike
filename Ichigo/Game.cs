using Ichigo.Engine;
using Ichigo.MapObjects.Player;
using Ichigo.Screens;

namespace Ichigo {
  internal class Game
  {

    private const int StartingWidth = 80;
    private const int StartingHeight = 50;

    public static PlayerEntity Player { get; set; }

    private static void Main()
    {
      Core.Start<MainMenu>(StartingWidth, StartingHeight, "Title", "./Fonts/Cheepicus12.font");
    }
  }
}

