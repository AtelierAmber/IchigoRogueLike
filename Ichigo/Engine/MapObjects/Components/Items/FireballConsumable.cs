using SadRogue.Primitives;
using SadRogue.Integration;


namespace Ichigo.Engine.MapObjects.Components.Items
{
  internal class FireballConsumable : AreaTargetConsumable
  {
    public readonly int Damage;

    public FireballConsumable(int damage, int radius)
      : base(radius, null, false)
    {
      Damage = damage;
    }

    protected override bool OnUse(Point target)
    {
      bool hitSomething = false;
      foreach (var pos in RadiusShape.PositionsInRadius(target, Radius,
                 Core.Instance.GameScreen!.Map.DefaultRenderer!.Surface.View))
      {
        foreach (var entity in Core.Instance.GameScreen.Map.GetEntitiesAt<RogueLikeEntity>(pos))
        {
          var stats = entity.AllComponents.GetFirstOrDefault<UnitStats>();
          if (stats == null) continue;

          Core.Instance.MessageLog.Add(
            new($"The {entity.Name} is engulfed in a fiery explosion, taking {Damage} damage!"));
          stats.HP -= Damage;
          hitSomething = true;
        }
      }

      if (!hitSomething)
        Core.Instance.MessageLog.Add(new("A fireball explodes but doesn't hit anything!"));

      return true;
    }
  }
}