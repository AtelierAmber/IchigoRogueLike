
using System;
using Ichigo.Engine.Screens;
using SadConsole;
using SadConsole.Configuration;
using SadConsole.UI;
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
        Logger.Error("Register game with Core.Start before accessing the instance!");
        return instance;
      }
    }

    private static int GAME_WIDTH = 80, GAME_HEIGHT = 50;

    public static void Start<TStartingScreen>(int width, int height, bool setStartAsGame = false, string defaultFont = "Engine/Fonts/Cheepicus12.font") where TStartingScreen : IScreenObject, new()
    {
      if (instance != null) return;
      GAME_WIDTH = width; GAME_HEIGHT = height;
      instance = new Core();
      instance.InitializeAndStart<TStartingScreen>();
    }

    private IScreenObject? gameScreen;

    public IchigoScreen? GameScreen
    {
      get => (IchigoScreen?)gameScreen;
      set
      {
        if (gameScreen == value) return;

        GameScreen?.Uninitialize();
        gameScreen = value;
      }
    }

    // Null override because it's initialized via Init
    public MessageLog MessageLog = null!;

    // Null override because it's initialized via new-game/load game
    public RogueLikeEntity Player = null!;

    public void InitializeAndStart<TStartingScreen>(bool setStartAsGame = false, string defaultFont = "Engine/Fonts/Cheepicus12.font") where TStartingScreen : IScreenObject, new()
    {
      try
      {
        Settings.WindowTitle = "Ichigo Core";

        Builder startup = new Builder()
            .SetScreenSize(GAME_WIDTH, GAME_HEIGHT)
            .SetStartingScreen<TStartingScreen>()
            .IsStartingScreenFocused(false)
            .ConfigureFonts(true)
            .ConfigureFonts(defaultFont)
          ;

        SadConsole.Game.Create(startup);
        SadConsole.Game.Instance.Started += Init;
        gameScreen = SadConsole.Game.Instance.Screen;
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