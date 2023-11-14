using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ichigo.Engine.Exceptions
{
  public class ItemTypeException : Exception
  {
    public ItemTypeException(Type type) : base("Issue with Item of type: " + type) { }
    public ItemTypeException(string resourceID) : base("Issue with item with ID: " + resourceID) { }
  }
}
