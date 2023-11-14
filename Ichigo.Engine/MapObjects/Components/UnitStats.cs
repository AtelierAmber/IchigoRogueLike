using SadRogue.Integration;
using SadRogue.Integration.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ichigo.Engine.MapObjects.Components
{
  public class UnitStats : RogueLikeComponentBase<RogueLikeEntity>
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
          Died?.Invoke(this, EventArgs.Empty);
      }
    }
    private float ep;
    public float MaxEP { get; private set; }
    public float EP
    {
      get => ep;
      set
      {
        if (ep == value) return;

        ep = Math.Clamp(value, 0f, MaxEP);
        EPChanged?.Invoke(this, EventArgs.Empty);
      }
    }

    // Combat Stats
    public float SlashDefense { get; private set; } = 1; // Defends against slashing attacks
    public float BluntDefense { get; private set; } = 1; // Defends against blunt attacks
    public float PierceDefense { get; private set; } = 1; // Defends against pierce attacks

    public float ElecResistance { get; private set; } = 1; // Defends against electric attacks
    public float FireResistance { get; private set; } = 1; // Defends against fire attacks
    public float DigitalResistance { get; private set; } = 1; // Defends against computer based, hacks, and other digital attacks
    public float WaterResistance { get; private set; } = 1; // Defends against water attacks. Protects from the wet status etc.
    public float Resistance { get; private set; } = 1; // Defends against pierce attacks

    public float Constitution { get; private set; } = 1; // Deternines your bodies internal resistance.
                                                         // Defends against poison damage and effects and from bad consumed effects
    public float Stamina { get; private set; } = 1; // Used to defend against exhaustion (EP) attacks
    public float Dexterity { get; private set; } = 1; // Used for many things. Grapple res, move speed, dodge chance, etc. High weight greatly affects Dex
    public float Strength { get; private set; } = 1; // Unarmed attacks and lifting things. Also determines carry weight, backpacks determine carry amount

    public event EventHandler? HPChanged;
    public event EventHandler? Died;

    public event EventHandler? EPChanged;
    public event EventHandler? EPDepleted;

    public UnitStats(float maxHP, float maxEP) : base(false, false, false, false, Priority)
    {
      hp = maxHP;
      ep = maxEP;
    }

    public float Heal(float amount)
    {
      float healthBeforeHeal = HP;

      HP += amount;
      return HP - healthBeforeHeal;
    }
  }
}
