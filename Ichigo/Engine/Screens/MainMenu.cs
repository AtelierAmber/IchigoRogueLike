using System;
using Ichigo.Engine.MapObjects;
using Ichigo.Engine.MapObjects.Components;
using SadConsole;
using SadConsole.UI;
using SadConsole.UI.Controls;
using SadRogue.Primitives;


/// <summary>
/// The main menu screen.
/// </summary>

namespace Ichigo.Engine.Screens
{
  public class MainMenu : ControlsConsole
  {

    public MainMenu()
        : base(15, 8)
    {

      // Position controls console
      Position = (Core.ScreenWidth / 2 - Width / 2, Core.ScreenHeight / 2 - Height / 2);

      // Add buttons
      var newGame = new Button(Width)
      {
        Name = "NewGameBtn",
        Text = "New Game",
        Position = (0, 0)
      };
      newGame.Click += NewGameOnClick;
      Controls.Add(newGame);

      var loadGame = new Button(Width)
      {
        Name = "LoadGameBtn",
        Text = "Load Game",
        Position = (0, 2)
      };
      loadGame.Click += LoadGameOnClick;
      Controls.Add(loadGame);

      var settings = new Button(Width)
      {
        Name = "SettingsBtn",
        Text = "Settings",
        Position = (0, 4)
      };
      settings.Click += SettingsOnClick;
      Controls.Add(settings);

      var exit = new Button(Width)
      {
        Name = "ExitBtn",
        Text = "Exit",
        Position = (0, 6)
      };
      exit.Click += ExitOnClick;
      Controls.Add(exit);
    }

    private void NewGameOnClick(object? sender, EventArgs e)
    {
      // Create player entity
      Core.Instance.Player = Factory.Player();

      // Generate a dungeon map, and spawn the player/enemies
      var map = Maps.Factory.Dungeon(new(100, 60, 20, 30, 8, 12, 2, 2));

      // Calculate initial FOV for player
      Core.Instance.Player.AllComponents.GetFirst<PlayerFOVController>().CalculateFOV();

      // Create a MapScreen and set it as the active screen so that it processes input and renders itself.
      Core.Instance.GameScreen = new MainGame(map);
      Core.Instance.ChangeScreen(Core.Instance.GameScreen);
    }

    private void LoadGameOnClick(object? sender, EventArgs e)
    {
      //throw new NotImplementedException();
    }

    private void ExitOnClick(object? sender, EventArgs e)
    {
      Game.Instance.MonoGameInstance.Exit();
    }

    private void SettingsOnClick(object? sender, EventArgs e)
    {

    }
  }
}