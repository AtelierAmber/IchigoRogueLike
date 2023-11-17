using Ichigo.Engine.Maps;
using Ichigo.Engine.Screens.States;
using SadConsole;

namespace Ichigo.Engine.Screens
{
  public abstract class IchigoScreen : ScreenSurface
  {
    public IchigoStateMachine StateMachine { get; private set; }

    public IchigoScreen()
    : base(10, 10)
    {
      StateMachine = new IchigoStateMachine(this);
    }

    public abstract void Initialize();
    public abstract void Uninitialize();

    public abstract IchigoMap GetMap();
  }
}