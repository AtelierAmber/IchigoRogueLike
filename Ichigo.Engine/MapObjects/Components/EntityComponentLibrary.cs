using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ichigo.Engine.MapObjects.Components
{
  public static class EntityComponentLibrary
  {
    internal readonly static Dictionary<string, Type> Components = new();

    public static Type GetComponentType(string name) => Components[name];

    public static void RegisterType<T>(string name) => RegisterType(typeof(T), name);
    public static void RegisterType(Type t, string name) => Components.Add(name, t);
  }
}
