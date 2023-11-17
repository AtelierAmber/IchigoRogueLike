using SadRogue.Integration;
using SadRogue.Primitives;
using Ichigo.Engine.Maps;
using Ichigo.Engine.Exceptions;
using Ichigo.Engine.Features.Entities;

/// <summary>
/// Simple class with some static functions for creating items.
/// </summary>

namespace Ichigo.Engine.Features.Items
{
  public static class ItemFactory
  {
    private static readonly Dictionary<string, IchigoItem> Items = new();

    public static IchigoItem GetOrCreate(string resourceID, string descriptionLoc, Color color, int glyph)
    {
      return GetOrCreate<IchigoItem>(resourceID, descriptionLoc, color, glyph);
    }
    public static IchigoItem GetOrCreate<ItemType>(string resourceID, string descriptionLoc, Color color, int glyph) where ItemType : IchigoItem
    {
      if ( Items.GetValueOrDefault(resourceID) != null)
      {
        return Items.GetValueOrDefault(resourceID);
      }
      if (Activator.CreateInstance(typeof(ItemType), new { Items.Count, resourceID, descriptionLoc, color, glyph }) is not ItemType item)
      {
        throw new ItemTypeException(typeof(ItemType));
      }
      Items.Add(resourceID, item);
      return item;
    }

    public static IchigoItem GetItem(string resourceID)
    {
      return Items.GetValueOrDefault(resourceID);
    }

    public static IchigoEntity GenerateItemEntity(string resourceID, object[] genArgs = null)
    {
      IchigoItem item = GetItem(resourceID);
      if (item == null)
      {
        throw new ItemTypeException(resourceID);
      }
      return GenerateItemEntity(item, genArgs);
    }
    public static IchigoEntity GenerateItemEntity(IchigoItem item, object[] genArgs = null)
    {
      IchigoEntity entity = Activator.CreateInstance(item.EntityType, args: new object[] { item.ItemColor, item.Glyph, (int)MapFactory.Layer.Items }.Append(genArgs)) as IchigoEntity;
      if (entity == null)
      {
        throw new ItemTypeException(item.Name);
      }
      entity.SetName(item.Name);
      return entity;
    }
  }
}