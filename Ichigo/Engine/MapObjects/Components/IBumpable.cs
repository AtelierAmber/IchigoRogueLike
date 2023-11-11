
/// <summary>
/// Interface implemented by any components that react to bumps.
/// </summary>

using SadRogue.Integration;

namespace Ichigo.Engine.MapObjects.Components
{
  public interface IBumpable
  {
    /// <summary>
    /// Does whatever bump action is needed, using the given entity as the source.  Returns true if a bump action was
    /// taken, false otherwise.
    /// </summary>
    bool OnBumped(RogueLikeEntity source);
  }
}