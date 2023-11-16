using SadRogue.Integration.Components;

namespace Ichigo.Engine.Features.Entities
{
  public class IchigoEntityComponent : RogueLikeComponentBase<IchigoEntity>
  {
    protected IchigoEntityComponent(bool isUpdate, bool isRender, bool isMouse, bool isKeyboard, uint sortOrder = 5) 
      : base(isUpdate, isRender, isMouse, isKeyboard, sortOrder) { }
  }
}
