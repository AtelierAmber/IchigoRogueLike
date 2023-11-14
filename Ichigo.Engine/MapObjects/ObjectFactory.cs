using System.Collections.Generic;
using SadConsole;
using SadRogue.Integration;
using SadRogue.Primitives;
using Ichigo.Engine.MapObjects.Components;
using Ichigo.Engine.MapObjects.Components.AI;
using Ichigo.Engine.Maps;


internal readonly record struct TerrainAppearanceDefinition(ColoredGlyph Light, ColoredGlyph Dark);

/// <summary>
/// Simple class with some static functions for creating map objects.
/// </summary>

namespace Ichigo.Engine.MapObjects
{
  public static class ObjectFactory
  {
    /// <summary>
    /// Appearance definitions for various types of terrain objects.  It defines both their normal color, and their
    /// "explored but out of FOV" color.
    /// </summary>
    private static readonly Dictionary<string, TerrainAppearanceDefinition> AppearanceDefinitions = new()
    {
        {
            "Floor",
            new TerrainAppearanceDefinition(
                new ColoredGlyph(Color.White, Color.Black, 0),
                new ColoredGlyph(Color.White, Color.Black, 0)
            )
        },
        {
            "Wall",
            new TerrainAppearanceDefinition(
                new ColoredGlyph(Color.Gray, Color.White, 0),
                new ColoredGlyph(Color.Gray, Color.Black, 0)
            )
        },
    };

    //public static Terrain Floor(Point position)
    //    => new(position, AppearanceDefinitions["Floor"], (int)MapFactory.Layer.Terrain);

    //public static Terrain Wall(Point position)
    //    => new(position, AppearanceDefinitions["Wall"], (int)MapFactory.Layer.Terrain, false, false);

    public static RogueLikeEntity Player()
    {
      // Create entity with appropriate attributes
      var player = new RogueLikeEntity('@', false, layer: (int)MapFactory.Layer.Characters)
      {
        Name = "Player"
      };

      // Add component for updating map's player FOV as they move
      player.AllComponents.Add(new PlayerFOVController { FOVRadius = 8 });

      player.AllComponents.Add(new UnitStats(30, 0));

      // Player combatant
      player.AllComponents.Add(new Combatant());

      // Player inventory
      player.AllComponents.Add(new Inventory(26));

      return player;
    }

    public static RogueLikeEntity Corpse(RogueLikeEntity entity)
        => new((ColoredGlyph)entity.AppearanceSingle.Appearance, layer: (int)MapFactory.Layer.Items)
        {
          Name = $"Remains - {entity.Name}",
          Position = entity.Position,
          AppearanceSingle =
            {
              Appearance =
              {
                Glyph = '%'
              }
            }
        };
  }
}