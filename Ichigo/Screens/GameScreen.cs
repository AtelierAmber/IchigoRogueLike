using Ichigo.Engine.Maps;
using Ichigo.Engine.Screens.Surfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SadConsole.Components;
using Ichigo.Engine.MapObjects.Components;
using Ichigo.Engine.Screens.Menus;
using Ichigo.Engine.Themes;
using Ichigo.Engine;
using Ichigo.Engine.Screens;
using Ichigo.Engine.Screens.States;

namespace Ichigo.Screens
{
  public class GameScreen : IchigoScreen
  {
    public readonly GameMap Map;
    public readonly MessageLogPanel MessagePanel;
    public readonly StatusPanel StatusPanel;

    public readonly SurfaceComponentFollowTarget ViewLock;

    private const int StatusBarWidth = 25;
    private const int BottomPanelHeight = 5;

    public GameScreen(GameMap map)
    {
      // Record the map we're rendering
      Map = map;

      // Create a renderer for the map, specifying viewport size.
      Map.DefaultRenderer = Map.CreateRenderer((Core.WindowWidth, Core.WindowHeight - BottomPanelHeight));

      // Make the Map (which is also a screen object) a child of this screen, and ensure the default renderer receives input focus.
      Children.Add(Map);
      Map.DefaultRenderer.IsFocused = true;

      // Center view on player as they move (by default)
      ViewLock = new SurfaceComponentFollowTarget { Target = Core.Instance.Player };
      Map.DefaultRenderer.SadComponents.Add(ViewLock);

      // Create message log
      MessagePanel = new MessageLogPanel(Core.WindowWidth - StatusBarWidth - 1, BottomPanelHeight)
      {
        Parent = this,
        Position = new(StatusBarWidth + 1, Core.WindowHeight - BottomPanelHeight)
      };

      // Create status panel
      StatusPanel = new(StatusBarWidth, BottomPanelHeight)
      {
        Parent = this,
        Position = new(0, Core.WindowHeight - BottomPanelHeight)
      };

      // Set main map state as default
      CurrentState = new MainMapState(this);

      // Add player death handler
      Core.Instance.Player.AllComponents.GetFirst<UnitStats>().Died += PlayerDeath;

      // Write welcome message
      Core.Instance.MessageLog.Add(new("Hello and welcome, adventurer, to yet another dungeon!", MessageColors.WelcomeTextAppearance));
    }

    /// <summary>
    /// Called when the player dies.
    /// </summary>
    private void PlayerDeath(object? s, EventArgs e)
    {
      Core.Instance.MessageLog.Add(new("You have died!", MessageColors.PlayerDiedAppearance));

      Core.Instance.Player.AllComponents.GetFirst<UnitStats>().Died -= PlayerDeath;

      // Switch to game over screen
      Children.Add(new GameOver());

    }
  }
}
