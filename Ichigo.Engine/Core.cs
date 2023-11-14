
using Ichigo.Engine.Screens;
using SadConsole;
using SadConsole.Configuration;
using SadRogue.Integration;

/// <summary>
/// Main class containing the program's entry point, which runs the game's core loop.
/// </summary>
namespace Ichigo.Engine
{
  public class Core
  {
    private static Core instance;
    public static Core Instance
    {
      get
      {
        if (instance != null) return instance;
        Logger.Error("Register game with Core.Start before accessing the instance!");
        return instance;
      }
    }

    private static int GAME_WIDTH = 80, GAME_HEIGHT = 50;

    public static int WindowWidth => GameHost.Instance.ScreenCellsX;

    public static int WindowHeight => GameHost.Instance.ScreenCellsY;

    public static void Start<TStartingScreen>(int width, int height, bool setStartAsGame = false,
      string defaultFont = "Engine/Fonts/Cheepicus12.font") where TStartingScreen : IchigoScreen, new()
    {
      if (instance != null) return;
      GAME_WIDTH = width; GAME_HEIGHT = height;
      instance = new Core();
      instance.InitializeAndRun<TStartingScreen>();
    }

    public ActionController ActionController { get; private set; }

    // Null override because it's initialized via Init
    public MessageLog MessageLog = null!;

    // Null override because it's initialized via new-game/load game
    public RogueLikeEntity Player = null!;

    public void InitializeAndRun<TStartingScreen>(bool setStartAsGame = false, string defaultFont = "Engine/Fonts/Cheepicus12.font") where TStartingScreen : IchigoScreen, new()
    {
      Settings.WindowTitle = "Ichigo Core";
      try
      {
        ActionController = new ActionController();

        Builder startup = new Builder()
            .SetScreenSize(GAME_WIDTH, GAME_HEIGHT)
            .SetStartingScreen<TStartingScreen>()
            .IsStartingScreenFocused(false)
            .ConfigureFonts(true)
            .ConfigureFonts(defaultFont)
          ;

        SadConsole.Game.Create(startup);
        SadConsole.Game.Instance.Started += Init;
        (SadConsole.Game.Instance.Screen as IchigoScreen).Initialize();
        SadConsole.Game.Instance.Run();
      }
      catch (Exception e)
      {
        Logger.Error("Exception caught in Game.Instance.Run()", e);
      }
      finally
      {
        SadConsole.Game.Instance.Dispose();
      }
    }

    private void Init(object sender, GameHost host)
    {
      MessageLog = new MessageLog(1000);
    }

    public void ChangeScreen<T>(T newScreen) where T : IchigoScreen
    {
      newScreen.Initialize();
      CurrentScreen().Uninitialize();
      GameHost.Instance.Screen = newScreen;
    }

    public IchigoScreen CurrentScreen()
    {
      return CurrentScreen<IchigoScreen>();
    }

    public CastTo CurrentScreen<CastTo>()
    {
      return (CastTo)GameHost.Instance.Screen;
    }
  }
}