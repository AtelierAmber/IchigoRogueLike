using SadRogue.Integration.FieldOfView.Memory;
using Ichigo.Engine.MapObjects;

/// <summary>
/// Visibility handler that handles FOV cells going out of FOV and into memory by simply changing it to its darkened appearance field.
/// </summary>

namespace Ichigo.Engine.Maps
{
  internal class TerrainFOVVisibilityHandler : MemoryFieldOfViewHandlerBase
  {
    protected override void ApplyMemoryAppearance(MemoryAwareRogueLikeCell terrain)
    {
      terrain.LastSeenAppearance.CopyAppearanceFrom(((Terrain)terrain).DarkAppearance);
    }
  }
}