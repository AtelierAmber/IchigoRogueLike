using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ichigo.Engine.Screens.States
{
  public class IchigoStateMachine
  {
    private readonly IchigoScreen parentScreen;

    private IchigoState currentState;

    public IchigoState CurrentState
    {
      get => currentState;
      private set
      {
        if (currentState != null)
        {
          parentScreen.SadComponents.Remove(currentState);
          parentScreen.SadComponents.Add(value);
        }
        currentState = value;
      }
    }

    public IchigoStateMachine(IchigoScreen parent, IchigoState initialState = null)
    {
      this.parentScreen = parent;
      if (initialState != null)
      {
        EnterState(initialState);
      }
    }

    public void EnterState(IchigoState state)
    {
      currentState?.OnExit();
      state.OnEnter();
      currentState = state;
    }

    public IchigoState LeaveState()
    {
      IchigoState oldState = currentState;
      oldState?.OnExit();
      currentState = null;
      return oldState;
    }
  }
}
