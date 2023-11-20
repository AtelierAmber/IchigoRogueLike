using System;
using Ichigo.Engine;
using Ichigo.Engine.MapObjects;
using Ichigo.Engine.MapObjects.Components;
using Ichigo.Engine.Maps;
using Ichigo.Engine.Screens;
using Ichigo.MapObjects;
using Ichigo.Maps;
using Ichigo.Screens.Controls;
using SadConsole;
using SadConsole.UI;
using SadConsole.UI.Controls;

namespace Ichigo.Screens
{
  public class MainMenu : ControlsConsole
  {

    public MainMenu()
        : base(15, 8)
    {

      // Position controls console
      Position = (Core.WindowWidth / 2 - Width / 2, Core.WindowHeight / 2 - Height / 2);

      // Add buttons
      var newGame = new MainMenuButton(Width)
      {
        Name = "NewGameBtn",
        Text = "mainmenu.newgame".Localize(),
        Position = (0, 0),
      };
      newGame.Click += NewGameOnClick;
      Controls.Add(newGame);

      var loadGame = new MainMenuButton(Width)
      {
        Name = "LoadGameBtn",
        Text = "mainmenu.loadgame".Localize(),
        Position = (0, 2)
      };
      loadGame.Click += LoadGameOnClick;
      Controls.Add(loadGame);

      var settings = new MainMenuButton(Width)
      {
        Name = "SettingsBtn",
        Text = "mainmenu.settings".Localize(),
        Position = (0, 4)
      };
      settings.Click += SettingsOnClick;
      Controls.Add(settings);

      var exit = new MainMenuButton(Width)
      {
        Name = "ExitBtn",
        Text = "mainmenu.exit".Localize(),
        Position = (0, 6)
      };
      exit.Click += ExitOnClick;
      Controls.Add(exit);
    }

    private void NewGameOnClick(object sender, EventArgs e)
    {
      // Create player entity
      Game.Player = MapObjectFactory.Player();

      // Create a MapScreen and set it as the active screen so that it processes input and renders itself.
      Core.Instance.ChangeRootScreen(new GameScreen());
    }

    private void LoadGameOnClick(object sender, EventArgs e)
    {
      //throw new NotImplementedException();
    }

    private void ExitOnClick(object sender, EventArgs e)
    {
      SadConsole.Game.Instance.MonoGameInstance.Exit();
    }

    private void SettingsOnClick(object sender, EventArgs e)
    {

    }
  }
}