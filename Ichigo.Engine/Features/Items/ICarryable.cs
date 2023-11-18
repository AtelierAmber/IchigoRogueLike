/// <summary>
/// Interface for things that can be picked up as items.
/// </summary>
namespace Ichigo.Engine.Features.Items
{
  public interface ICarryable
  {
    public IchigoItem GetAsItem();
  }
}