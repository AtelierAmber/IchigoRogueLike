using System;
using System.Linq;
using GoRogue.GameFramework;
using Ichigo.Engine;
using Ichigo.Engine.MapObjects.Components;
using Ichigo.Engine.MapObjects.Components.AI;
using Ichigo.Engine.Maps;
using SadConsole;
using SadRogue.Integration;
using SadRogue.Integration.Maps;
using SadRogue.Primitives;
using SadRogue.Primitives.SpatialMaps;


/// <summary>
/// Basic game map which determines layers based on a Layer enumeration and implements some core rules/actions, as well as corpse handling.
/// </summary>

namespace Ichigo.Maps
{
  public class GameMap : IchigoMap
  {

    public GameMap(int width, int height, DefaultRendererParams? defaultRendererParams)
        : base(width, height, defaultRendererParams, Enum.GetValues<MapFactory.Layer>().Length - 1, Distance.Chebyshev)
    {
      // Ensures HostileDeath is triggered when anything except the player dies so that corpses appear and messages trigger.
      Entities.ItemAdded += EntitiesOnItemAdded;
      Entities.ItemRemoved += EntitiesOnItemRemoved;
      AllComponents.Add(new MapEntityForwarderComponent());
      IsFocused = true;
    }

    /// <summary>
    /// Causes the entity specified to move in the specified direction if it can, or generate a "bump" action in that direction if a move is not possible.
    /// </summary>
    /// <remarks>
    /// If the entity cannot move, any map objects which have components implementing IBumpable will get their "Bump" function called, with the given
    /// entity being used as the source.  All components on all entities at the position being bumped will have their "Bump" function called
    /// in sequence, until one of them returns "true".
    /// </remarks>
    /// <param name="entity">The entity performing the bump.</param>
    /// <param name="direction">The direction of the adjacent square the specified entity is moving/bumping in.</param>
    /// <returns>True if either the entity moves, or some component's "Bump" function returned true; false otherwise.</returns>
    public static bool MoveOrBump(RogueLikeEntity entity, Direction direction)
    {
      if (entity.CurrentMap == null) return false;

      // Move if nothing blocks
      if (entity.CanMoveIn(direction))
      {
        entity.Position += direction;
        return true;
      }

      // Bump anything bumpable
      var newPosition = entity.Position + direction;
      foreach (var obj in entity.CurrentMap.GetObjectsAt(newPosition))
        foreach (var bumpable in obj.GoRogueComponents.GetAll<IBumpable>())
          if (bumpable.OnBumped(entity))
            return true;

      if (entity == Game.Player)
        Core.Instance.MessageLog.Add(new("That way is blocked.", MessageColors.ImpossibleActionAppearance));

      return false;
    }

    /// <summary>
    /// Causes all objects with a HostileAI component to take their turns.  In this simple example, we don't really need a full turn system, so this
    /// is sufficient.
    /// </summary>
    public void TakeEnemyTurns()
    {
      var enemies = Entities.GetLayer((int)MapFactory.Layer.Characters).Items.ToArray();
      var playerStats = Game.Player.GoRogueComponents.GetFirst<HealthComponent>();
      foreach (var enemy in enemies)
      {
        if (playerStats.HP <= 0) break;

        var ai = enemy.GoRogueComponents.GetFirstOrDefault<AIBase>();
        ai?.TakeTurn();
      }
    }

    private static void EntitiesOnItemAdded(object sender, ItemEventArgs<IGameObject> e)
    {
      if (e.Item != Game.Player)
      {
        var health = e.Item.GoRogueComponents.GetFirstOrDefault<HealthComponent>();
        if (health == null) return;

        health.HPDepleted += HostileDeath;
      }
    }

    private static void EntitiesOnItemRemoved(object sender, ItemEventArgs<IGameObject> e)
    {
      if (e.Item != Game.Player)
      {
        var health = e.Item.GoRogueComponents.GetFirstOrDefault<HealthComponent>();
        if (health == null) return;

        health.HPDepleted -= HostileDeath;
      }
    }

    private static void HostileDeath(object s, EventArgs e)
    {
      var hostile = ((IchigoCombatant)s!).Parent!;

      // Display message in log
      Core.Instance.MessageLog.Add(new ColoredString($"The {hostile.Name} dies!",
          MessageColors.EnemyDiedAppearance));

      // Switch entity for corpse
      var map = hostile.CurrentMap!;
      map.RemoveEntity(hostile);
      map.AddEntity(Engine.MapObjects.ObjectFactory.Corpse(hostile));
    }
  }
}