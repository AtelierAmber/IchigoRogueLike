using SadRogue.Integration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ichigo.Engine.Features
{
  public class IchigoItem
  {
    public int ID { get; private set; }

    public string Name { get; private set; } = "engine.item.name.noname";

    public string LocalizedName
    {
      get => Localizer.Localized(Name);
    }

    public string Description { get; private set; } = "engine.item.description.nodescription";

    public string LocalizedDescription
    {
      get => Localizer.Localized(Description);
    }
  }
}
