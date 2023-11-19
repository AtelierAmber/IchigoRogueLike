using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ichigo.Engine.Resources
{
  public class Resource
  {
    internal Type DataType;
    internal readonly string Name;
    internal readonly string Path;

    internal readonly object Data = null;

    public Resource(Type type, string name, string path, object data)
    {
      DataType = type;
      Name = name;
      Path = path;
      Data = data;
    }
  }
}
