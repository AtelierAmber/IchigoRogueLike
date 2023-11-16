using Ichigo.Engine.Features.Entities;
using System;

namespace Ichigo.MapObjects.Components
{
  internal class CharacterCombatStats : IchigoEntityComponent
  {
    private const int Priority = 0;

    public CharacterCombatStats(float maxHP, float maxEP) : base(false, false, false, false, Priority) { }
    public void SetDefense(float slash, float blunt, float pierce)
    {
      SlashDefense = slash;
      BluntDefense = blunt;
      PierceDefense = pierce;

      DefenseChanged?.Invoke(this, EventArgs.Empty);
    }
    public void SetResistances(float elec, float fire, float digital, float water)
    {
      ElecResistance = elec;
      FireResistance = fire;
      DigitalResistance = digital; ;
      WaterResistance = water;

      ResistanceChanged?.Invoke(this, EventArgs.Empty);
    }
    public void SetBodyStats(float stamina, float dexterity, float strength, float constitution, float willpower, float intelligence, float sociability)
    {
      Stamina = stamina;
      Dexterity = dexterity;
      Strength = strength;
      Constitution = constitution;
      Willpower = willpower;
      Intelligence = intelligence;
      Sociability = sociability;

      BodyStatChanged?.Invoke(this, EventArgs.Empty);
    }

    // Resources
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

    public event EventHandler EPChanged;
    public event EventHandler EPDepleted;

    // Combat Defenses
    public float SlashDefense { get; private set; } = 1; // Defends against slashing attacks
    public float BluntDefense { get; private set; } = 1; // Defends against blunt attacks
    public float PierceDefense { get; private set; } = 1; // Defends against pierce attacks
    public event EventHandler DefenseChanged;

    public float ElecResistance { get; private set; } = 0.0f; // Defends against electric attacks
    public float FireResistance { get; private set; } = 0.0f; // Defends against fire attacks
    public float DigitalResistance { get; private set; } = 0.0f; // Defends against computer based, hacks, and other digital attacks
    public float WaterResistance { get; private set; } = 0.0f; // Defends against water attacks. Protects from the wet status etc.
    public float WoundResistance { get; private set; } = 0.0f; // Defends against pierce attacks
    public event EventHandler ResistanceChanged;

    // Body Stats
    public float Stamina { get; private set; } = 1; // Used to defend against exhaustion (EP) attacks
    public float Dexterity { get; private set; } = 1; // Used for many things. Grapple res, move speed, dodge chance, etc. High weight greatly affects Dex
    public float Strength { get; private set; } = 1; // Unarmed attacks and lifting things. Also determines carry weight, backpacks determine carry amount
    public float Constitution { get; private set; } = 1; // Deternines your bodies internal resistance. Helps with bleed effects and other similar ones
                                                         // Defends against poison damage and effects and from bad consumed effects
    public float Willpower { get; private set; } = 1; // How strong your will is. Resists mind altering effects
    public float Intelligence { get; private set; } = 1; // Ability to rationalize and understand thing, concepts, and information
    public float Sociability { get; private set; } = 1; // Ability to socialize with others. Works as a barter, charisma and likability stat. Also determines how well you lie.
    public event EventHandler BodyStatChanged;
  }
}
