using SadConsole;
using SadRogue.Integration.Components;


/// <summary>
/// Component representing a state of the main game screen.
/// </summary>

namespace Ichigo.Engine.Screens.States
{
  internal class StateBase : RogueLikeComponentBase<IScreenSurface>
  {
    protected readonly MainGame GameScreen;

    public StateBase(MainGame gameScreen, bool isUpdate, bool isRender, bool isMouse, bool isKeyboard)
        : base(isUpdate, isRender, isMouse, isKeyboard)
    {
      GameScreen = gameScreen;
    }
  }
}