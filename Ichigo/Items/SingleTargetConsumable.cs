using System;
using SadConsole.Components;
using SadRogue.Integration;
using SadRogue.Integration.Components;
using Ichigo.Engine.Screens.States;
using Ichigo.Engine;
using Ichigo.Engine.Features.Items;
using Ichigo.Engine.Screens;
using Ichigo.Screens;
using Ichigo.Screens.States;

namespace Ichigo.Items
{
  internal abstract class SingleTargetConsumable : RogueLikeComponentBase<RogueLikeEntity>, IConsumable
  {
    // Private for now because the player is the only one who can use items so there is no need to set it outside the class, but this could change.
    private RogueLikeEntity _target;

    private readonly bool _allowTargetSelf;
    private readonly bool _allowTargetNonVisible;

    protected SingleTargetConsumable(bool allowTargetSelf, bool allowTargetNonVisible)
      : base(false, false, false, false)
    {
      _allowTargetSelf = allowTargetSelf;
      _allowTargetNonVisible = allowTargetNonVisible;
    }

    public IComponent GetStateHandler(RogueLikeEntity consumer)
    {
      if (_target != null) return null;

      if (Core.Instance.ActiveRootScreen<IchigoScreen>() is not GameScreen screen)
        throw new InvalidOperationException("Cannot target an item without a game screen.");

      var targetingState = new TargetSingleEntityState(Game.Player, screen, _allowTargetSelf, _allowTargetNonVisible,
        positionSelected: (pos) =>
        {
          _target = consumer.CurrentMap!.GetEntityAt<RogueLikeEntity>(pos.MapPosition);
          screen.StateMachine.EnterState(new MainMapState(screen));
          //PlayerActionHelper.PlayerTakeAction(entity => entity.AllComponents.GetFirst<Inventory>().Consume(Parent!));
        });
      return targetingState;
    }

    // TODO: Effects?
    public bool Consume(RogueLikeEntity consumer)
    {
      if (_target == null)
        throw new InvalidOperationException(
          $"A {nameof(SingleTargetConsumable)} must have a target set when consumed.");


      // In any case we're going to try to use the item, so we'll remove the target in case it fails
      // and we use it again later.
      var target = _target;
      _target = null;

      return OnUse(target);

    }

    protected abstract bool OnUse(RogueLikeEntity target);
  }
}