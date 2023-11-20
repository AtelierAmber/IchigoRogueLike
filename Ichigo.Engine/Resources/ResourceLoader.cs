
namespace Ichigo.Engine.Resources
{
  public static class ResourceController
  {
    private static readonly Dictionary<string, Resource> resources = new();
    internal static readonly Dictionary<Type, ResourceLoader> Loaders = new();

    public static bool GetOrLoadResource<TDataType>(string resourceName, out TDataType data, bool persistant = true)
    {
      if (resources.TryGetValue(resourceName, out Resource loadedRes))
      {
        data = (TDataType)loadedRes.Data;
        return true;
      }
      if (!Loaders.TryGetValue(typeof(TDataType), out ResourceLoader loader))
      {
        Logger.Error("No Resource Loader with type <" + typeof(TDataType) + "> has been registered! Could not load resource \"" + resourceName + "\"!");
        data = default;
        return false;
      }

      Resource res = loader.Load(resourceName);

      if (persistant)
      {
        resources.Add(resourceName, res);
      }

      data = (TDataType)res.Data;
      return true;
    }

    public static void ReloadResources()
    {
      foreach (var resource in resources.Values)
      {
        if (!Loaders.TryGetValue(resource.GetType(), out ResourceLoader loader)) continue;
        resources[resource.Name] = loader.Load(resource.Name);
      }
    }

    public static void RegisterLoader<T>() where T : ResourceLoader, new()
    {
      T loader = new T();
      if (Loaders.ContainsKey(loader.Type))
      {
        Logger.Error("Tried to register a loader with type <" + typeof(T) + "> but that type has already been registered!");
        return;
      }

      Loaders.Add(loader.Type, loader);
    }
  }

  public abstract class ResourceLoader
  {
    public readonly Type Type;
    public readonly string Name;
    public readonly string Extension;

    public string ResourceLocation => "./res/" + Name + "/";

    public ResourceLoader(Type type, string dataName, string dataExtension)
    {
      if (type == null) throw new ArgumentNullException("ResourceLoader():type");
      if (ResourceController.Loaders.ContainsKey(type)) throw new ArgumentException("Instantiating a ResourceLoader is not allowed! Use the ResourceController to manage resources!");

      Type = type;
      Name = dataName;
      Extension = dataExtension;

      Directory.CreateDirectory(ResourceLocation);
    }

    internal Resource Load(string resourceName)
    {
      string path = ResourceLocation + resourceName + Extension;
      object data = LoadData(path);
      Resource resource = new(Type, resourceName, path, data);

      return resource;
    }
    
    protected abstract object LoadData(string resourcePath);
  }
}
