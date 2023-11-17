using System;
using SadRogue.Integration;
using SadRogue.Integration.Components;
using SadConsole;
using Ichigo.Engine.Features.Entities;

/// <summary>
/// Component for entities that allows them to have health and attack.
/// </summary>

namespace Ichigo.Engine.MapObjects.Components
{
  public abstract class IchigoCombatant : RogueLikeComponentBase<RogueLikeEntity>, IBumpable
  {
    protected IchigoCombatant()
        : base(false, false, false, false) { }

    public abstract void DamageTarget(IchigoCombatant target);

    public bool OnBumped(RogueLikeEntity source)
    {
      var combatant = (source as IchigoEntity)?.GetComponent<IchigoCombatant>();
      if (combatant == null) return false;

      combatant.DamageTarget(this);
      return true;
    }
  }
}