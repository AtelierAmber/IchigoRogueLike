using System;
using SadConsole;
using SadConsole.UI;
using SadConsole.UI.Controls;
using SadRogue.Primitives;
using Ichigo.Engine.MapObjects.Components;
using Ichigo.Engine;

/// <summary>
/// A ControlsConsole subclass which resides on the main game screen and displays the player's health and similar "hud" statistics and info.
/// </summary>
namespace Ichigo.Screens.Menus
{
    public class StatusPanel : ControlsConsole
    {
        public readonly ProgressBar HPBar;
        public readonly Label LookInfo;

        public StatusPanel(int width, int height)
            : base(width, height)
        {
            // Create an HP bar with the appropriate coloring and background glyphs
            HPBar = new ProgressBar(Width, 1, HorizontalAlignment.Left)
            {
                DisplayTextColor = Color.White
            };

            // Add HP bar to controls, and ensure HP bar updates when the player's health changes
            Controls.Add(HPBar);
            Core.Instance.Player.AllComponents.GetFirst<UnitStats>().HPChanged += OnPlayerHPChanged;
            UpdateHPBar();

            // Create a label to display information about the tile the player is looking at
            LookInfo = new Label(width)
            {
                DisplayText = "",
                Position = (0, 1)
            };

            // Add label to controls
            Controls.Add(LookInfo);
        }

        private void OnPlayerHPChanged(object sender, EventArgs e)
        {
            UpdateHPBar();
        }

        private void UpdateHPBar()
        {
            var stats = Core.Instance.Player.AllComponents.GetFirst<UnitStats>();
            HPBar.DisplayText = $"HP: {stats.HP} / {stats.MaxHP}";
            HPBar.Progress = (float)stats.HP / stats.MaxHP;
        }
    }
}
