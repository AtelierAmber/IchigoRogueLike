using SadConsole;
using SadRogue.Integration.Components;


/// <summary>
/// Component representing a state of the main game screen.
/// </summary>

namespace Ichigo.Engine.Screens.States
{
  public abstract class IchigoState : RogueLikeComponentBase<IScreenSurface>
  {
    public IchigoState(bool isUpdate, bool isRender, bool isMouse, bool isKeyboard)
        : base(isUpdate, isRender, isMouse, isKeyboard) { }

    public sealed override void OnAdded(IScreenObject host)
    {
      base.OnAdded(host);
    }

    public sealed override void OnRemoved(IScreenObject host)
    {
      base.OnRemoved(host);
    }

    public abstract void OnEnter();
    public abstract void OnExit();
  }
}