using SadRogue.Integration;
using Ichigo.Engine.MapObjects.Components.AI;
using Ichigo.Engine.Themes;
using Ichigo.Engine;


namespace Ichigo.Items
{
    internal class ConfusionConsumable : SingleTargetConsumable
    {
        public readonly int NumberOfTurns;

        public ConfusionConsumable(int numberOfTurns)
          : base(false, false)
        {
            NumberOfTurns = numberOfTurns;
        }

        protected override bool OnUse(RogueLikeEntity target)
        {
            var currentAI = target.AllComponents.GetFirstOrDefault<AIBase>();
            // TODO: Doing this here allows the targeter to have already accepted this position.  Ideally, this would be a targeter option instead
            if (currentAI == null)
            {
                Core.Instance.MessageLog.Add(
                  new("You cannot confuse an inanimate object.",
                    MessageColors.ImpossibleActionAppearance));
                return false;
            }

            target.AllComponents.Remove(currentAI);
            Core.Instance.MessageLog.Add(new($"The eyes of the {target.Name} look vacant, as it starts to stumble around!",
              MessageColors.StatusEffectAppliedAppearance));
            target.AllComponents.Add(new ConfusedAI(NumberOfTurns, currentAI));

            return true;
        }
    }
}