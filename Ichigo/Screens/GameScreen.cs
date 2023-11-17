using System;
using SadConsole.Components;
using Ichigo.Engine.MapObjects.Components;
using Ichigo.Engine;
using Ichigo.Engine.Screens;
using Ichigo.Screens.Menus;
using Ichigo.Engine.Maps;
using SadConsole;
using SadRogue.Primitives;

namespace Ichigo.Screens
{
    public class GameScreen : IchigoScreen
  {
    public IchigoMap Map;
    public MessageLogPanel MessagePanel;
    public StatusPanel StatusPanel;

    public SurfaceComponentFollowTarget ViewLock;

    private const int StatusBarWidth = 25;
    private const int BottomPanelHeight = 5;

    public GameScreen(IchigoMap map)
    {
      // Record the map we're rendering
      Map = map;

      // Create a renderer for the map, specifying viewport size.
      Map.DefaultRenderer = Map.CreateRenderer((Core.WindowWidth, Core.WindowHeight - BottomPanelHeight));

      // Make the Map (which is also a screen object) a child of this screen, and ensure the default renderer receives input focus.
      Children.Add(Map);
      Map.DefaultRenderer.IsFocused = true;
    }

    /// <summary>
    /// Called when the player dies.
    /// </summary>
    private void PlayerDeath(object s, EventArgs e)
    {
      Core.Instance.MessageLog.Add(new("You have died!", MessageColors.PlayerDiedAppearance));

      Game.Player.AllComponents.GetFirst<HealthComponent>().HPDepleted -= PlayerDeath;

      // Switch to game over screen
      Children.Add(new GameOver());

    }

    public override void Initialize()
    {
      // Center view on player as they move (by default)
      ViewLock = new SurfaceComponentFollowTarget { Target = Game.Player };
      Map?.DefaultRenderer?.SadComponents.Add(ViewLock);

      // Create message log
      MessagePanel = new MessageLogPanel(Core.WindowWidth - StatusBarWidth - 1, BottomPanelHeight)
      {
        Parent = this,
        Position = new Point(StatusBarWidth + 1, Core.WindowHeight - BottomPanelHeight)
      };

      // Create status panel
      StatusPanel = new StatusPanel(StatusBarWidth, BottomPanelHeight)
      {
        Parent = this,
        Position = new Point(0, Core.WindowHeight - BottomPanelHeight)
      };

      // Set main map state as default
      //CurrentState = new MainMapState(this);

      // Add player death handler
      Game.Player.AllComponents.GetFirst<HealthComponent>().HPDepleted += PlayerDeath;

      // Write welcome message
      Core.Instance.MessageLog.Add(new ColoredString("Hello and welcome, adventurer, to yet another dungeon!", MessageColors.WelcomeTextAppearance));
    }

    public override void Uninitialize()
    {
      
    }

    public override IchigoMap GetMap()
    {
      return Map;
    }
  }
}
