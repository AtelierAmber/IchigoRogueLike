using Ichigo.Engine;
using Ichigo.Engine.MapObjects.Components;
using Ichigo.Engine.Screens.Components;
using Ichigo.Engine.Screens.Menus;
using Ichigo.Engine.Screens.States;
using SadConsole.Input;

namespace Ichigo.Screens.States
{
    /// <summary>
    /// "Main" state where the map is displayed and the player can move around.
    /// </summary>
    internal class MainMapState : IchigoState
    {
        private readonly MovementKeybindingsComponent _keybindings;

        public MainMapState()
            : base(false, false, false, false)
        {
            _keybindings = new MovementKeybindingsComponent();
            // Add controls for picking up items and getting to inventory screen.
            _keybindings.SetAction(Keys.G, () => PlayerActionHelper.PlayerTakeAction(e => e.AllComponents.GetFirst<Inventory>().PickUp()));

            // Controls for menus
            _keybindings.SetAction(Keys.C, () => Parent.Children.Add(new ConsumableSelect()));
            _keybindings.SetAction(Keys.M, () => Parent.Children.Add(new MessageLogMenu(50, 24, 1000)));

            // "Look" functionality Keybinding
            //_keybindings.SetAction(Keys.OemQuestion, () => Parent.CurrentState = new SelectMapLocationState(GameScreen));
        }

        public override void OnEnter()
        {
            Parent.SadComponents.Add(_keybindings);
        }

        public override void OnExit()
        {
            Parent.SadComponents.Remove(_keybindings);
        }
    }
}
