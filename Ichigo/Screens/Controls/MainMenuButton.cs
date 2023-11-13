using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SadConsole.UI;
using SadConsole.UI.Controls;
using SadRogue.Primitives;

namespace Ichigo.Screens.Controls
{
  internal class MainMenuButton : Button
  {
    public MainMenuButton(int width, int height = 1) : base(width, height)
    {
      LeftEndGlyph = '[';
      RightEndGlyph = ']';

      EndsThemeState.MouseOver.Foreground = Color.Black;
    }

    ///<inheritdoc/>
    protected override void RefreshThemeStateColors(Colors colors)
    {
      base.RefreshThemeStateColors(colors);

      EndsThemeState.RefreshTheme(colors);
      EndsThemeState.MouseOver.Foreground = Color.Black;
      EndsThemeState.Normal.Foreground = colors.Lines;
    }

  }
}
