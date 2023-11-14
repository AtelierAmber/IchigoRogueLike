﻿using System;
using Ichigo.Engine;
using Ichigo.Engine.Screens;
using SadConsole;
using SadRogue.Primitives;


namespace Ichigo.Engine.Screens.States
{
  internal class TargetAreaState : SelectMapLocationState
  {
    private readonly bool _allowTargetNonVisible;

    public TargetAreaState(IchigoScreen gameScreen, int radius, Radius radiusShape, bool allowTargetNonVisible,
        Action<LookMarkerPosition> positionChanged = null,
        Action<LookMarkerPosition> positionSelected = null,
        Func<Point> getLookMarkerSurfaceStartingLocation = null)
        : base(gameScreen, radius, radiusShape, positionChanged, positionSelected, getLookMarkerSurfaceStartingLocation)
    {
      _allowTargetNonVisible = allowTargetNonVisible;
    }

    protected override bool ValidateSelectedPosition()
    {
      if (!_allowTargetNonVisible && !gameMap.PlayerFOV.BooleanResultView[LookMarkerPosition.MapPosition])
      {
        Core.Instance.MessageLog.Add(
                           new("You cannot target an area that you cannot see."));
        return false;
      }

      return true;
    }

    public override void OnEnter()
    {
      base.OnEnter();

      Core.Instance.MessageLog.Add(new("Select an area to target."));
    }
  }
}