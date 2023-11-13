﻿using System;
using Ichigo.Engine;
using Ichigo.Engine.Screens;
using Ichigo.Engine.Themes;
using SadConsole;
using SadRogue.Integration;
using SadRogue.Primitives;

namespace Ichigo.Screens.States
{
    internal class TargetSingleEntityState : SelectMapLocationState
    {
        private readonly bool _allowTargetSelf;
        private readonly bool _allowTargetNonVisible;

        public TargetSingleEntityState(IchigoScreen gameScreen, bool allowTargetSelf, bool allowTargetNonVisible,
            Action<LookMarkerPosition> positionChanged = null,
            Action<LookMarkerPosition> positionSelected = null,
            Func<Point> getLookMarkerSurfaceStartingLocation = null)
            : base(gameScreen, positionChanged: positionChanged, positionSelected: positionSelected, getLookMarkerSurfaceStartingLocation: getLookMarkerSurfaceStartingLocation)
        {
            _allowTargetSelf = allowTargetSelf;
            _allowTargetNonVisible = allowTargetNonVisible;
        }

        protected override bool ValidateSelectedPosition()
        {
            var target = GameScreen.Map.GetEntityAt<RogueLikeEntity>(LookMarkerPosition.MapPosition);
            if (target == null)
            {
                Core.Instance.MessageLog.Add(
                    new("You must select an enemy to target.", MessageColors.ImpossibleActionAppearance));
                return false;
            }

            if (!_allowTargetSelf && target == Core.Instance.Player)
            {
                Core.Instance.MessageLog.Add(
                        new("You cannot target yourself.", MessageColors.ImpossibleActionAppearance));
                return false;
            }

            if (!_allowTargetNonVisible && !GameScreen.Map.PlayerFOV.BooleanResultView[target.Position])
            {
                Core.Instance.MessageLog.Add(
                    new("You cannot target an area that you cannot see.", MessageColors.ImpossibleActionAppearance));
                return false;
            }

            return true;
        }

        public override void OnAdded(IScreenObject host)
        {
            base.OnAdded(host);

            Core.Instance.MessageLog.Add(new("Select an enemy to target.", MessageColors.NeedsTargetAppearance));
        }
    }
}