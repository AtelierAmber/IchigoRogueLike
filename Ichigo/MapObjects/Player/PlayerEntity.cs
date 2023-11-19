using Ichigo.Engine.Features.Entities;
using Ichigo.Engine.Features.Items;
using Ichigo.Engine.MapObjects.Components;
using Ichigo.Engine.Maps;
using GoRogue.GameFramework;
using SadRogue.Primitives;
using SadRogue.Integration.Maps;

namespace Ichigo.MapObjects.Player
{
  public class PlayerEntity : IchigoEntity, IControlable
  {
    public PlayerEntity()
      : base(Color.White, '@', false, false, (int)MapFactory.Layer.Characters)
    {
      AddComponent(new Inventory(10));
    }

    public void Move(Direction direction, int moveType = 0, object[] extraData = null)
    {
      if (CurrentMap == null) return;

      // Move if nothing blocks
      if (this.CanMoveIn(direction))
      {
        Position += direction;
        return;
      }

      // Bump anything bumpable
      var newPosition = Position + direction;
      foreach (var obj in CurrentMap.GetObjectsAt(newPosition))
        foreach (var bumpable in obj.GoRogueComponents.GetAll<IBumpable>())
          if (bumpable.OnBumped(this))
            return;
    }

    public void PerformExtraAction(int actionType)
    {
      throw new System.NotImplementedException();
    }

    public void TakeNoAction()
    {
      throw new System.NotImplementedException();
    }
  }
}
