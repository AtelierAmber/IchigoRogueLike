using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SadConsole.UI.Controls;

namespace Ichigo.Screens.Controls
{
  internal class MainMenuButton : Button
  {
    public MainMenuButton(int width, int height = 1) : base(width, height)
    {
      LeftEndGlyph = '[';
      RightEndGlyph = ']';
    }
  }
}
