using System.Linq;
using Ichigo.Engine.Features.Entities;
using SadRogue.Primitives;
using Ichigo.Engine.Maps;


/// <summary>
/// Simple component that moves its parent toward the player, if the player is visible to it.  If the entity bumps into the player,
/// they will attack it.  If the player is not visible, they will move toward the player's last known position.
/// </summary>
/// <remarks>
/// Any entities with this component will take their "turn" via the TakeTurn function after the player takes their action for a turn.
/// </remarks>

namespace Ichigo.Engine.MapObjects.Components.AI
{
  public class BasicHostileAI : AIBase
  {
    private Point lastPlayerPosition = Point.None;

    private readonly IchigoEntity targetEntity;

    public BasicHostileAI(IchigoEntity target)
    {
      targetEntity = target;
    }

    public override void TakeTurn()
    {
      if (Parent?.CurrentMap == null) return;
      if (Parent.AllComponents.GetFirst<HealthComponent>().HP <= 0) return;

      // Path to the player if they're visible; otherwise, move toward the last known position of the player (if any)
      var moveToPosition = Parent.CurrentMap.PlayerFOV.CurrentFOV.Contains(Parent.Position)
        ? targetEntity.Position
        : lastPlayerPosition;
      if (moveToPosition == Point.None || Parent.Position == moveToPosition) return;

      lastPlayerPosition = moveToPosition; // Record the last known position of the player

      var path = Parent.CurrentMap.AStar.ShortestPath(Parent.Position, moveToPosition);
      if (path == null) return;
      var firstPoint = path.GetStep(0);
      //GameMap.MoveOrBump(Parent, Direction.GetDirection(Parent.Position, firstPoint));
    }
  }
}