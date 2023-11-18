
using Ichigo.Engine.Features.Entities;
using SadRogue.Primitives;

namespace Ichigo.Engine.Features.Items
{
  public class IchigoItem
  {
    public int ID { get; private set; }

    public virtual string Name { get; protected set; } = "engine.item.name.noname";

    // Item Params
    public virtual int MaxStackSize { get; protected set; } = 10;

    public virtual string LocalizedName
    {
      get => Localizer.Localized(Name);
    }

    public virtual string Description { get; private set; } = "engine.item.description.nodescription";

    public virtual string LocalizedDescription
    {
      get => Localizer.Localized(Description);
    }

    public Color ItemColor { get; private set; }
    public int Glyph { get; private set; }
    public Type EntityType { get; private set; } = typeof(IchigoEntity);

    public IchigoItem(int id, string nameLoc, string descriptionLoc, Color color, int glyph)
    {
      ID = id;
      Name = nameLoc;
      Description = descriptionLoc;
      ItemColor = color;
      Glyph = glyph;
    }
    public IchigoItem(int id, string nameLoc, string descriptionLoc, Color color, int glyph, Type entityType)
      : this(id, nameLoc, descriptionLoc, color, glyph)
    {
      if (entityType.IsAssignableTo(typeof(IchigoEntity)))
      {
        Logger.Warning("Could not designate custom entity type of type: " + entityType + " for item: " + nameLoc);
        return;
      }
      EntityType = entityType;
    }

  }
}
