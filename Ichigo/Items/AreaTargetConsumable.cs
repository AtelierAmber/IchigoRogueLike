using SadRogue.Integration.Components;
using SadRogue.Integration;
using System;
using SadConsole.Components;
using SadRogue.Primitives;
using Ichigo.Engine.Screens.States;
using Ichigo.Engine;
using Ichigo.Engine.Features.Items;
using Ichigo.Engine.Screens;
using Ichigo.Screens.States;
using Ichigo.Screens;

namespace Ichigo.Items
{
  internal abstract class AreaTargetConsumable : RogueLikeComponentBase<RogueLikeEntity>, IActivatable
  {
    // Private for now because the player is the only one who can use items so there is no need to set it outside the class, but this could change.
    private Point? _target;

    public readonly int Radius;

    private readonly Radius? _radiusShape;

    public Radius? RadiusShape => _radiusShape ?? Core.Instance.ActiveRootScreen<IchigoScreen>()!.GetMap()!.DistanceMeasurement;
    public readonly bool AllowTargetNonVisible;

    protected AreaTargetConsumable(int radius, Radius? radiusShape, bool allowTargetNonVisible)
      : base(false, false, false, false)
    {
      Radius = radius;
      _radiusShape = radiusShape;
      AllowTargetNonVisible = allowTargetNonVisible;
    }

    public IComponent GetStateHandler(RogueLikeEntity consumer)
    {
      if (_target != null) return null;

      if (Core.Instance.ActiveRootScreen<IchigoScreen>() is not GameScreen screen)
        throw new InvalidOperationException("Cannot target an item without a game screen.");

      var targetingState = new TargetAreaState(Game.Player, screen, Radius, RadiusShape, AllowTargetNonVisible,
            positionSelected: pos =>
            {
              _target = pos.MapPosition;
              screen.StateMachine.EnterState(new MainMapState(screen));
              //PlayerActionHelper.PlayerTakeAction(entity => entity.AllComponents.GetFirst<Inventory>().Consume(Parent!));
            });
      return targetingState;
    }

    // TODO: Effects?
    public bool Consume(RogueLikeEntity consumer)
    {
      if (_target == null)
        throw new InvalidOperationException($"A {nameof(AreaTargetConsumable)} must have a target set when consumed.");


      // In any case we're going to try to use the item, so we'll remove the target in case it fails
      // and we use it again later.
      var target = _target;
      _target = null;

      return OnUse(target.Value);
    }

    protected abstract bool OnUse(Point target);
  }
}