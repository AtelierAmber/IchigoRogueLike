using SadConsole.Components;
using SadRogue.Integration;
using SadRogue.Integration.Components;
using Ichigo.Engine.Themes;


/// <summary>
/// Consumable that restores up to the given amount of HP.
/// </summary>

namespace Ichigo.Engine.MapObjects.Components.Items
{
  internal class HealingConsumable : RogueLikeComponentBase<RogueLikeEntity>, IConsumable
  {
    public int Amount { get; }

    public HealingConsumable(int amount)
      : base(false, false, false, false)
    {
      Amount = amount;
    }

    public IComponent? GetStateHandler(RogueLikeEntity consumer) => null;

    public bool Consume(RogueLikeEntity consumer)
    {
      var isPlayer = consumer == Core.Instance.Player;

      var combatant = consumer.AllComponents.GetFirst<Combatant>();
      var amountRecovered = combatant.Heal(Amount);
      if (amountRecovered > 0)
      {
        if (isPlayer)
          Core.Instance.MessageLog.Add(new(
            $"You consume the {Parent!.Name}, and recover {amountRecovered} HP!",
            MessageColors.HealthRecoveredAppearance));
        return true;
      }

      if (isPlayer)
        Core.Instance.MessageLog.Add(new("Your health is already full.", MessageColors.ImpossibleActionAppearance));
      return false;
    }
  }
}