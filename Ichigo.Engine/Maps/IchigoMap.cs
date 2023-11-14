using GoRogue.Components;
using GoRogue.FOV;
using GoRogue.GameFramework;
using GoRogue.Pathing;
using SadRogue.Integration.Maps;
using SadRogue.Primitives;
using SadRogue.Primitives.Pooling;

namespace Ichigo.Engine.Maps
{
  public class IchigoMap : RogueLikeMap
  {
    public IchigoMap(int width, int height, DefaultRendererParams? defaultRendererParams, int numberOfEntityLayers, Distance distanceMeasurement, Func<int, IListPool<IGameObject>> customListPoolCreator = null, uint layersBlockingWalkability = uint.MaxValue, uint layersBlockingTransparency = uint.MaxValue, uint entityLayersSupportingMultipleItems = uint.MaxValue, IFOV customPlayerFOV = null, IEqualityComparer<Point> pointComparer = null, AStar customPather = null, IComponentCollection customComponentContainer = null, bool useCachedGridViews = true) 
      : base(width, height, defaultRendererParams, numberOfEntityLayers, distanceMeasurement, customListPoolCreator, layersBlockingWalkability, layersBlockingTransparency, entityLayersSupportingMultipleItems, customPlayerFOV, pointComparer, customPather, customComponentContainer, useCachedGridViews){ }
  }
}
