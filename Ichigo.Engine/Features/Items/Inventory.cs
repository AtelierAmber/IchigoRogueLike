using SadRogue.Integration;
using SadRogue.Integration.Components;

/// <summary>
/// Component representing an inventory which can hold a given number of items.
/// </summary>

namespace Ichigo.Engine.Features.Items
{
  public class Inventory : RogueLikeComponentBase<RogueLikeEntity>
  {
    public int Capacity { get; }

    public readonly List<IchigoItemStack> Items;

    public event Action<IchigoItemStack> OnItemDropped; 

    public Inventory(int capacity)
        : base(false, false, false, false)
    {
      Capacity = capacity;
      Items = new List<IchigoItemStack>(capacity);
    }

    /// <summary>
    /// Drops the given item from this inventory.
    /// </summary>
    public void Drop(IchigoItemStack item)
    {
      if (Parent == null)
        throw new InvalidOperationException(
            "Can't drop an entity from an inventory that's not connected to an object.");
      if (Parent.CurrentMap == null)
        throw new InvalidOperationException(
            "Objects are not allowed to drop items from their inventory when they're not part of a map.");

      if (!Items.Remove(item))
        throw new ArgumentException("Tried to drop an item from an inventory it was not a part of.", nameof(item));

      // TODO
      //item.Position = Parent.Position;
      //Parent.CurrentMap.AddEntity(item);

      OnItemDropped?.Invoke(item);
    }

    /// <summary>
    /// Tries to pick up the first item found at the Parent's location.
    /// </summary>
    /// <returns>True if an item was picked up; false otherwise.</returns>
    public bool PickUp()
    {
      if (Parent == null)
        throw new InvalidOperationException(
            "Can't pick up an item into an inventory that's not connected to an object.");

      if (Parent.CurrentMap == null)
        throw new InvalidOperationException("Entity must be part of a map to pick up items.");

      foreach (var entity in Parent.CurrentMap.GetEntitiesAt<RogueLikeEntity>(Parent.Position))
      {
        if (!entity.AllComponents.Contains<ICarryable>()) continue;

        if (Items.Count >= Capacity)
        {
          //if (isPlayer)
          //{
          //  // Core.Instance.MessageLog.Add(new("Your inventory is full.", MessageColors.ImpossibleActionAppearance));
          //}

          return false;
        }

        Parent.CurrentMap!.RemoveEntity(entity);
        Items.Add(new IchigoItemStack(entity.AllComponents.GetFirst<ICarryable>().GetAsItem(), 1));

        //if (isPlayer)
        //{
        //  //Core.Instance.MessageLog.Add(new($"You picked up the {item.Name}.", MessageColors.ItemPickedUpAppearance));
        //}

        return true;
      }

      //if (isPlayer)
      //{
      //  //Core.Instance.MessageLog.Add(new("There is nothing here to pick up.", MessageColors.ImpossibleActionAppearance));
      //}

      return false;
    }

    /// <summary>
    /// Causes the parent to consume the given consumable item.  The given entity must have some component implementing IConsumable.
    /// </summary>
    public bool Consume(IchigoItemStack item)
    {
      if (Parent == null)
        throw new InvalidOperationException("Cannot consume item from an inventory not attached to an object.");
      //var consumable = item.AllComponents.GetFirst<IConsumable>();

      int idx = Items.FindIndex(i => i == item);
      if (idx == -1)
        throw new ArgumentException("Tried to consume a consumable that was not in the inventory.");

      //var stateHandler = consumable.GetStateHandler(Parent);
      //if (stateHandler != null)
      //{
      //  //Core.Instance.GameScreen!.CurrentState = stateHandler;
      //  return false; // We haven't consumed the item, so we'll return false.
      //}

      //bool result = consumable.Consume(Parent);
      //if (!result) return false;

      Items.RemoveAt(idx);
      return true;
    }
  }
}