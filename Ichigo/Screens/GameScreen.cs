using System;
using SadConsole.Components;
using Ichigo.Engine.MapObjects.Components;
using Ichigo.Engine;
using Ichigo.Engine.Screens;
using Ichigo.Screens.Menus;
using Ichigo.Engine.Maps;
using SadConsole;
using SadRogue.Primitives;
using GoRogue.MapGeneration;
using GoRogue.MapGeneration.ContextComponents;
using SadRogue.Primitives.GridViews;
using Ichigo.Engine.MapObjects;
using Ichigo.Maps;

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

    public GameScreen()
    {
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
      // Generate a dungeon map, and spawn the player/enemies
      //var map = MapFactory.Dungeon(Game.Player, new(120, 80, 20, 30, 8, 12, 2, 2));
      // Generate a dungeon maze map
      var generator = new Generator(120, 80)
          .ConfigAndGenerateSafe(gen =>
          {
            //gen.AddSteps(DefaultAlgorithms.RectangleMapSteps());
            gen.AddSteps(DefaultAlgorithms.DungeonMazeMapSteps(minRooms: 10, maxRooms: 20,
                  roomMinSize: 20, roomMaxSize: 30, saveDeadEndChance: 0));
          });

      // Extract components from the map GoRogue generated which hold basic information about the map
      var generatedMap = generator.Context.GetFirst<ISettableGridView<bool>>("WallFloor");
      var rooms = generator.Context.GetFirst<ItemList<Rectangle>>("Rooms");

      // Create actual integration library map with a proper component for the character "memory" system.
      var map = new GameMap(generator.Context.Width, generator.Context.Height, null);
      //map.AllComponents.Add(new TerrainFOVVisibilityHandler());

      Game.Player.Position = rooms.Items[0].Center;

      // Translate GoRogue's terrain data into actual integration library objects.
      map.ApplyTerrainOverlay(generatedMap, (pos, val) => val ? ObjectFactory.Floor(pos) : ObjectFactory.Wall(pos));

      map.AddEntity(Game.Player);

      // Calculate initial FOV for player
      //Game.Player.AllComponents.GetFirst<PlayerFOVController>().CalculateFOV();

      // Record the map we're rendering
      Map = map;

      // Create a renderer for the map, specifying viewport size.
      Map.DefaultRenderer = Map.CreateRenderer((Core.WindowWidth, Core.WindowHeight - BottomPanelHeight));

      // Make the Map (which is also a screen object) a child of this screen, and ensure the default renderer receives input focus.
      Children.Add(Map);
      //Map.DefaultRenderer.IsFocused = true;

      // Center view on player as they move (by default)
      ViewLock = new SurfaceComponentFollowTarget { Target = Game.Player };
      Map.DefaultRenderer.SadComponents.Add(ViewLock);

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
