
using GoRogue.GameFramework;
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
    private static Core? instance;
    public static Core Instance
    {
      get
      {
        if (instance != null) return instance;
        instance = new Core();
        instance.InitializeAndStart();
        return instance;
      }
    }

    public static void Start()
    {
      if (instance != null) return;
      instance = new Core();
      instance.InitializeAndStart();
    }

    // Window width/height
    public const int ScreenWidth = 80;
    public const int ScreenHeight = 50;

    private MainGame? gameScreen;

    public MainGame? GameScreen
    {
      get => gameScreen;
      set
      {
        if (gameScreen == value) return;

        gameScreen?.Uninitialize();
        gameScreen = value;
      }
    }

    // Null override because it's initialized via Init
    public MessageLog MessageLog = null!;

    // Null override because it's initialized via new-game/load game
    public RogueLikeEntity Player = null!;

    public void InitializeAndStart()
    {
      Settings.WindowTitle = "SadRogue Tests - Ichigo";

      Builder startup = new Builder()
          .SetScreenSize(GameSettings.GAME_WIDTH, GameSettings.GAME_HEIGHT)
          .SetStartingScreen<MainMenu>()
          .IsStartingScreenFocused(false) // Don't want RootScreen to be focused because RootScreen automatically focuses the selected demo console
          .ConfigureFonts(true)
          .ConfigureFonts("Engine/Fonts/Cheepicus12.font")
          //.SetSplashScreen<SadConsole.SplashScreens.Ansi1>()
          //.KeyhookMonoGameDebugger()
        ;

      Game.Create(startup);
      Game.Instance.Started += Init;
      Game.Instance.Run();
      Game.Instance.Dispose();
    }

    private void Init(object? sender, GameHost host)
    {
      MessageLog = new MessageLog(1000); 
    }

    public void ChangeScreen<T> (T newScreen) where T : ScreenObject
    {
      GameHost.Instance.Screen = newScreen;
    }
  } 
}