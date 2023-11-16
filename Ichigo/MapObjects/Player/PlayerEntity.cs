using Ichigo.Engine.Features.Entities;
using Ichigo.Engine.MapObjects.Components;
using Ichigo.Engine.Maps;
using SadRogue.Primitives;

namespace Ichigo.MapObjects.Player
{
  public class PlayerEntity : IchigoEntity, IControlable
  {
    PlayerEntity()
      : base(Color.White, '@', false, false, (int)MapFactory.Layer.Characters)
    {
      AddComponent(new Inventory(10));
    }

    public void Move(Direction direction, int moveType = 0, object[] extraData = null)
    {
      throw new System.NotImplementedException();
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
