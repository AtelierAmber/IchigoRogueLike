using System;
using SadConsole;
using SadConsole.Components;
using Ichigo.Engine.MapObjects.Components;
using Ichigo.Engine.Maps;
using Ichigo.Engine.Screens.Menus;
using Ichigo.Engine.Screens.States;
using Ichigo.Engine.Themes;
using StatusPanel = Ichigo.Engine.Screens.Surfaces.StatusPanel;
using Ichigo.Engine.Screens.Surfaces;

/// <summary>
/// Main game screen that shows map, message log, etc.  It supports a number of "states", where states are components
/// which are attached to the map's DefaultRenderer and implement controls/logic for the state.
/// </summary>

namespace Ichigo.Engine.Screens
{
  public class MainGame : ScreenObject
  {
    public readonly GameMap Map;
    public readonly MessageLogPanel MessagePanel;
    public readonly StatusPanel StatusPanel;

    /// <summary>
    /// Component which locks the map's view onto an entity (usually the player).
    /// </summary>
    public readonly SurfaceComponentFollowTarget ViewLock;

    private IComponent? currentState;

    public IComponent CurrentState
    {
      get => currentState ?? throw new InvalidOperationException("Current game state should never be null.");
      set
      {
        if (currentState == value) return;

        if (currentState != null) Map.DefaultRenderer!.SadComponents.Remove(currentState);
        currentState = value;
        Map.DefaultRenderer!.SadComponents.Add(currentState);
      }
    }

    private const int StatusBarWidth = 25;
    private const int BottomPanelHeight = 5;

    public MainGame(GameMap map)
    {
      // Record the map we're rendering
      Map = map;

      // Create a renderer for the map, specifying viewport size.
      Map.DefaultRenderer = Map.CreateRenderer((Core.ScreenWidth, Core.ScreenHeight - BottomPanelHeight));

      // Make the Map (which is also a screen object) a child of this screen, and ensure the default renderer receives input focus.
      Children.Add(Map);
      Map.DefaultRenderer.IsFocused = true;

      // Center view on player as they move (by default)
      ViewLock = new SurfaceComponentFollowTarget { Target = Core.Instance.Player };
      Map.DefaultRenderer.SadComponents.Add(ViewLock);

      // Create message log
      MessagePanel = new MessageLogPanel(Core.ScreenWidth - StatusBarWidth - 1, BottomPanelHeight)
      {
        Parent = this,
        Position = new(StatusBarWidth + 1, Core.ScreenHeight - BottomPanelHeight)
      };

      // Create status panel
      StatusPanel = new(StatusBarWidth, BottomPanelHeight)
      {
        Parent = this,
        Position = new(0, Core.ScreenHeight - BottomPanelHeight)
      };

      // Set main map state as default
      CurrentState = new MainMapState(this);

      // Add player death handler
      Core.Instance.Player.AllComponents.GetFirst<UnitStats>().Died += PlayerDeath;

      // Write welcome message
      Core.Instance.MessageLog.Add(new("Hello and welcome, adventurer, to yet another dungeon!", MessageColors.WelcomeTextAppearance));
    }

    public void Uninitialize()
    {
      MessagePanel.Parent = null;
      StatusPanel.Parent = null;
      Core.Instance.Player.AllComponents.GetFirst<UnitStats>().Died -= PlayerDeath;
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