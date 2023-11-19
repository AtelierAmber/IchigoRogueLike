
using Ichigo.Engine.Screens;
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

    private int WINDOW_WIDTH = 80, WINDOW_HEIGHT = 50;

    public static int WindowWidth => SadConsole.GameHost.Instance.ScreenCellsX;

    public static int WindowHeight => SadConsole.GameHost.Instance.ScreenCellsY;

    public static void Start<TStartingScreen>(int width, int height, string title,
      string defaultFont = null) where TStartingScreen : SadConsole.IScreenSurface, new()
    {
      if (instance != null) return;
      instance = new Core();
      instance.InitializeAndRun<TStartingScreen>(width, height, title, defaultFont);
    }

    public ActionController ActionController { get; private set; }

    // Null override because it's initialized via Init
    public MessageLog MessageLog = null!;

    private void InitializeAndRun<TStartingScreen>(int width, int height, string title, string defaultFont)
      where TStartingScreen : SadConsole.IScreenSurface, new()
    {
      bool usingFont = File.Exists(defaultFont);
      if (defaultFont != null)
      {
        if (!usingFont) { Logger.Error("Font file provided is missing, falling back on default font."); }
      }
      SadConsole.Settings.WindowTitle = title;
      WINDOW_WIDTH = width; WINDOW_HEIGHT = height;
      try
      {
        ActionController = new ActionController();

        Builder startup = new Builder()
            .SetScreenSize(WINDOW_WIDTH, WINDOW_HEIGHT)
            .SetStartingScreen<TStartingScreen>()
            .IsStartingScreenFocused(false)
            .ConfigureFonts(usingFont)
            .ConfigureFonts(defaultFont)
          ;

        SadConsole.Game.Create(startup);
        SadConsole.Game.Instance.Started += Init;
        (SadConsole.Game.Instance.Screen as IchigoScreen)?.Initialize();
        SadConsole.Game.Instance.Run();
      }
      catch (Exception e)
      {
        Logger.Error("Exception caught in Game.Instance.Run() " + e.Message, e);
      }
      finally
      {
        try
        {
          SadConsole.Game.Instance.Dispose();
        }
        catch (Exception _)
        {
          Environment.Exit(1);
        }
      }
    }

    private void Init(object sender, SadConsole.GameHost host)
    {
      MessageLog = new MessageLog(1000);
    }

    public bool AddScreenToRoot<T>(T newScreen) where T : SadConsole.IScreenSurface
    {
      (newScreen as IchigoScreen)?.Initialize();
      if (ActiveRootScreen() == null)
      {
        return false;
      }
      ActiveRootScreen().Children.Add(newScreen);
      return true;
    }

    public void ChangeRootScreen<T>(T newScreen) where T : SadConsole.IScreenSurface
    {
      (newScreen as IchigoScreen)?.Initialize();
      ActiveRootScreen<IchigoScreen>()?.Uninitialize();
      SadConsole.GameHost.Instance.Screen = newScreen;
    }

    public SadConsole.IScreenSurface ActiveRootScreen()
    {
      return ActiveRootScreen<SadConsole.IScreenSurface>();
    }

    public TCastTo ActiveRootScreen<TCastTo>() where TCastTo: class, SadConsole.IScreenSurface
    {
      return SadConsole.GameHost.Instance.Screen as TCastTo;
    }
  }
}