using System;
using Ichigo.Engine.MapObjects.Components;
using Ichigo.Engine.Maps;
using Ichigo.Engine.Screens.Menus;
using Ichigo.Engine.Screens.States;
using Ichigo.Engine.Screens.Surfaces;
using Ichigo.Engine.Themes;
using SadConsole;
using SadConsole.Components;

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