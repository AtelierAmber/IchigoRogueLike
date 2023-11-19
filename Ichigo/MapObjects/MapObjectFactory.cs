using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ichigo.Engine.MapObjects.Components;
using Ichigo.Engine.Screens.Components;
using Ichigo.MapObjects.Components;
using Ichigo.MapObjects.Player;

namespace Ichigo.MapObjects
{
  public class MapObjectFactory
  {
    public static PlayerEntity Player()
    {
      PlayerEntity player = new PlayerEntity();

      player.AddComponent(new CharacterCombatStats(100));
      player.AddComponent(new HealthComponent(100));
      player.AddComponent(new PlayerFOVController() {FOVRadius = 8});
      player.AddComponent(new MovementKeybindingsComponent());

      return player;
    }
  }
}
