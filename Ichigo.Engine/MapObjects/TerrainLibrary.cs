using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SadRogue.Integration;

namespace Ichigo.Engine.MapObjects
{
  public static class TerrainLibrary
  {
    internal readonly static Dictionary<string, Func<RogueLikeCell>> Components = new();

    public static Func<RogueLikeCell> GetComponentType(string name) => Components[name];

    public static void RegisterTerrain(Func<RogueLikeCell> terrainBuilder, string name) => Components.Add(name, terrainBuilder);
  }
}
