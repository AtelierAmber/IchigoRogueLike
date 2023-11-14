using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SadRogue.Integration;

namespace Ichigo.Engine.MapObjects.Components
{
  public interface IControlable
  {
    public void TakeNoAction();
    public void TakeAction<T>(Func<RogueLikeEntity, bool> action) => TakeAction((entity, _) => action(entity), false);
    public void TakeAction<T>(Func<RogueLikeEntity, T, bool> action, T performParams);
  }
}
