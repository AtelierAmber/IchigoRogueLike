using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using SadConsole;
using SadRogue.Integration;
using SadRogue.Primitives;
using Ichigo.Engine.MapObjects.Components;
using Ichigo.Engine.MapObjects.Components.AI;
using Ichigo.Engine.Maps;


public readonly record struct TerrainAppearanceDefinition(ColoredGlyph Light, ColoredGlyph Dark);

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
                new ColoredGlyph(Color.Gray, Color.White, 0)
            )
        },
    };

    public static Terrain Floor(Point position)
        => new(position, AppearanceDefinitions["Floor"], (int)MapFactory.Layer.Terrain);

    public static Terrain Wall(Point position)
        => new(position, AppearanceDefinitions["Wall"], (int)MapFactory.Layer.Terrain, false, false);

    public static RogueLikeEntity Corpse([NotNull] RogueLikeEntity entity)
        => new((entity.AppearanceSingle == null) ? new ColoredGlyph(Color.White, Color.Black, '?') : (ColoredGlyph)entity.AppearanceSingle.Appearance, layer: (int)MapFactory.Layer.Items)
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