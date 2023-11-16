using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace Ichigo.Engine.MapObjects.Components
{
  public class BasicStats : RogueLikeComponentBase<RogueLikeEntity>
  {
    private const int Priority = 0;

    private float hp;
    public float MaxHP { get; private set; }
    public float HP
    {
      get => hp;
      set
      {
        if (hp == value) return;

        hp = Math.Clamp(value, 0f, MaxHP);
        HPChanged?.Invoke(this, EventArgs.Empty);

        if (hp == 0)
          HPDepleted?.Invoke(this, EventArgs.Empty);
      }
    }

    public event EventHandler HPChanged;
    public event EventHandler HPDepleted;

    public BasicStats(float maxHP) : base(false, false, false, false, Priority)
    {
      hp = maxHP;
    }

    public virtual float Heal(float amount)
    {
      float healthBeforeHeal = HP;

      HP += amount;
      return HP - healthBeforeHeal;
    }
    public virtual float Damage(float amount)
    {
      float hpBeforeDmg = HP;

      HP -= amount;

      return HP - hpBeforeDmg;
    }
  }
}
