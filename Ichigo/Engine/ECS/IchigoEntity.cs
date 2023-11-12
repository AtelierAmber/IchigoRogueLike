using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoRogue.Components;
using SadConsole;
using SadConsole.Components;
using SadRogue.Integration;
using SadRogue.Primitives;

namespace Ichigo.Engine.ECS
{
  internal class IchigoEntity : RogueLikeEntity
  {
    #region Constructors
    public IchigoEntity(int glyph, bool walkable = true, bool transparent = true, int layer = 1, Func<uint>? idGenerator = null) 
      : base(glyph, walkable, transparent, layer, idGenerator) { }

    public IchigoEntity(Point position, int glyph, bool walkable = true, bool transparent = true, int layer = 1, Func<uint>? idGenerator = null) 
      : base(position, glyph, walkable, transparent, layer, idGenerator) { }

    public IchigoEntity(Color foreground, int glyph, bool walkable = true, bool transparent = true, int layer = 1, Func<uint>? idGenerator = null) 
      : base(foreground, glyph, walkable, transparent, layer, idGenerator) { }

    public IchigoEntity(Point position, Color foreground, int glyph, bool walkable = true, bool transparent = true, int layer = 1, Func<uint>? idGenerator = null) 
      : base(position, foreground, glyph, walkable, transparent, layer, idGenerator) { }

    public IchigoEntity(Color foreground, Color background, int glyph, bool walkable = true, bool transparent = true, int layer = 1, Func<uint>? idGenerator = null) 
      : base(foreground, background, glyph, walkable, transparent, layer, idGenerator) { }

    public IchigoEntity(Point position, Color foreground, Color background, int glyph, bool walkable = true, bool transparent = true, int layer = 1, Func<uint>? idGenerator = null) 
      : base(position, foreground, background, glyph, walkable, transparent, layer, idGenerator) { }

    public IchigoEntity(ColoredGlyph appearance, bool walkable = true, bool transparent = true, int layer = 1, Func<uint>? idGenerator = null) 
      : base(appearance, walkable, transparent, layer, idGenerator) { }

    public IchigoEntity(Point position, ColoredGlyph appearance, bool walkable = true, bool transparent = true, int layer = 1, Func<uint>? idGenerator = null) 
      : base(position, appearance, walkable, transparent, layer, idGenerator) { }
    #endregion

    public void AddComponents(IComponent[] components)
    {
      foreach (var component in components)
      {
        AddComponent(component);
      }
    }

    public IComponent AddComponent(IComponent newComponent)
    {
      AllComponents.Add(newComponent);

      return newComponent;
    }

    public IComponent RemoveComponent<T>() where T : class, IComponent
    {
      var remComponent = AllComponents.GetFirst<T>();
      AllComponents.Remove(remComponent);

      return remComponent;
    }

    public bool HasComponent<T>() where T : class, IComponent
    {
      return HasSadComponent(out T _);
    }
    public bool HasComponent<T>(out T outComponent) where T : class, IComponent
    {
      return HasSadComponent(out outComponent);
    }

    public T? GetComponent<T>() where T : class, IComponent
    {
      return AllComponents.GetFirst<T>();
    }

    IEnumerable<T> GetAll<T>() where T : class
    {
      return AllComponents.GetAll<T>();
    }
  }
}
