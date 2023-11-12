using System;
using SadRogue.Integration;
using SadRogue.Integration.Components;
using Ichigo.Engine.Themes;
using SadConsole;

/// <summary>
/// Component for entities that allows them to have health and attack.
/// </summary>

namespace Ichigo.Engine.MapObjects.Components
{
  internal class Combatant : RogueLikeComponentBase<RogueLikeEntity>, IBumpable
  {

    private UnitStats? stats = null;

    public Combatant()
        : base(false, false, false, false)
    {
    }

    public void Damage(Combatant target)
    {
      if (stats == null || target.stats == null) return;

      float damage = stats.Strength - target.stats.BluntDefense;
      string attackDesc = $"{Parent!.Name} attacks {target.Parent!.Name}";

      var atkTextColor = Parent == Core.Instance.Player ? MessageColors.PlayerAtkAppearance : MessageColors.EnemyAtkAppearance;
      if (damage > 0)
      {
        Core.Instance.MessageLog.Add(new($"{attackDesc} for {damage} damage.", atkTextColor));
        target.stats.HP -= damage;
      }
      else
        Core.Instance.MessageLog.Add(new($"{attackDesc} but does no damage.", atkTextColor));
    }

    public bool OnBumped(RogueLikeEntity source)
    {
      var combatant = source.AllComponents.GetFirstOrDefault<Combatant>();
      if (combatant == null) return false;

      combatant.Damage(this);
      return true;
    }
    public override void OnAdded(IScreenObject host)
    {
      base.OnAdded(host);

      RogueLikeEntity RLParent = (RogueLikeEntity)host;
      if (RLParent != null && RLParent.HasSadComponent(out stats))
      {
        return;
      }

      RLParent?.AllComponents.Remove(this);
    }
  }
}