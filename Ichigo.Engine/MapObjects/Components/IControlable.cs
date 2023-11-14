using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SadRogue.Integration;
using SadRogue.Primitives;

namespace Ichigo.Engine.MapObjects.Components
{
  public interface IControlable
  {
    public void TakeNoAction();
    public void Move(Direction direction, int moveType = 0, object[] extraData = null);
    public void PerformExtraAction(int actionType);
  }
}
