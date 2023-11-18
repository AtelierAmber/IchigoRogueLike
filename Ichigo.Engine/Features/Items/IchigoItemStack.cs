using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ichigo.Engine.Features.Items
{
  public class IchigoItemStack
  {
    public IchigoItem ItemRef { get; private set; }
    public int Count { get; private set; }

    public event Action<IchigoItemStack> StackUpdated;

    public readonly MetaData MetaData;

    public IchigoItemStack(IchigoItem item, int count)
    {
      MetaData = new MetaData();
      ItemRef = item;
      Count = count;
    }

    public bool IsEmpty { get
      {
        return Count <= 0;
      } 
    }

    // Returns amount that was NOT added
    public int AddItems(int amount = 1)
    {
      int rem = Math.Max((Count + amount) - ItemRef.MaxStackSize, 0);
      Count += (amount - rem);

      StackUpdated?.Invoke(this);

      return rem;
    }

    // Returns a new Item stack with the removed items
    public IchigoItemStack TakeItems(int amount = 1, bool copyHandler = false)
    {
      int rem = Math.Max((Count - amount), 0);

      DeleteItems(Count - rem);

      IchigoItemStack newStack = new IchigoItemStack(ItemRef, Math.Min(Count, amount));
      if (copyHandler)
      {
        newStack.StackUpdated = StackUpdated;
        newStack.StackUpdated?.Invoke(this);
      }

      StackUpdated?.Invoke(this);

      return newStack;
    }

    // Returns true if the item stack still has items, false if it is now empty
    public bool DeleteItems(int amount = 1)
    {
      Count = Math.Max(Count - amount, 0);
      return !IsEmpty;
    }
  }
}
