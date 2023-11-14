using Ichigo.Engine.Screens.States;
using SadConsole;

namespace Ichigo.Engine.Screens
{
  public abstract class IchigoScreen : ScreenObject
  {
    private IchigoStateMachine stateMachine;

    public IchigoScreen()
    {
      stateMachine = new IchigoStateMachine(this);
    }

    public abstract void Initialize();
    public abstract void Uninitialize();

  }
}