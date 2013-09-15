using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstSDL
{
	public class CollisionManager
	{
		private GridManager gridManager;

		public CollisionManager(int mapWidth, int mapHeight)
		{
			gridManager = new GridManager(mapWidth, mapHeight);
		}

		/// <summary>
		/// Overload to check a single entity against collision with many entities in the second list
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="T2"></typeparam>
		/// <param name="collidableEntity"></param>
		/// <param name="collidableEntities2"></param>
		public void HandleCollisions(ICollidable collidableEntity, IEnumerable<ICollidable> collidableEntities2)
		{
			List<ICollidable> collidableEntities = new List<ICollidable>();
			collidableEntities.Add(collidableEntity);
			HandleCollisions(collidableEntities, collidableEntities2);
		}

		/// <summary>Checks the entities from the first list to see if they collide with entities in the second list.
		/// </summary>
		/// <typeparam name="T">Type must implement ICollidable interface</typeparam>
		/// <typeparam name="T2">Type must implement ICollidable interface</typeparam>
		/// <param name="collidableEntities1">List of collidable entities</param>
		/// <param name="collidableEntities2">Another list of collidable entities</param>
		public void HandleCollisions(IEnumerable<ICollidable> collidableEntities1, IEnumerable<ICollidable> collidableEntities2)
		{
			// clear the grid manager prior to checking collision
			gridManager.ClearCells();

			AddCollidablesToGrid(collidableEntities1);
			AddCollidablesToGrid(collidableEntities2);

			// loop through all entities in the first list
			foreach (ICollidable collidable in collidableEntities1)
			{
				// if the collidable has no sub-collidables, just resolve its own collision
				//if (collidable.CollidableComponents.Count == 0)
					DoCollisionResolution(collidable);
				//else
				//{
				//	// otherwise get all of the sub-collidables and do resolution on them
				//	foreach (ICollidable collidableComponent in collidable.CollidableComponents)
				//		DoCollisionResolution(collidableComponent);
				//}
			}
		}

		/// <summary>Using the passed enumerable of collidables, this method adds the appropriate entities to the grid for collision management.
		/// </summary>
		/// <param name="collidableEntities"></param>
		private void AddCollidablesToGrid(IEnumerable<ICollidable> collidableEntities)
		{
			// go through the first set of collidables
			foreach (ICollidable collidableEntity in collidableEntities)
			{
				// if this collidable has no sub-collidables, just at the collidable
				//if (collidableEntity.CollidableComponents.Count == 0)
					gridManager.AddCollidable(collidableEntity);
				//else
				//{
				//	// otherwise get all of the sub-collidables and add those
				//	foreach (ICollidable collidableComponent in collidableEntity.CollidableComponents)
				//		gridManager.AddCollidable(collidableComponent);
				//}
			}
		}

		/// <summary>Gets all collidable entities that occupy the same grid cells as the passed collidable entity, checks if
		/// they are colliding, and then resolves collision for both of the entities
		/// </summary>
		/// <param name="collidable"></param>
		private void DoCollisionResolution(ICollidable collidable)
		{
			// get a list of all entities that are in any grids that we are in (nearby neighbors)
			List<ICollidable> nearbyCollidables = new List<ICollidable>(gridManager.GetNearbyCollidables(collidable));

			// loop through all our nearby neighbors and resolve collision (if necessary)
			foreach (ICollidable nearbyCollidable in nearbyCollidables)
			{
				if (IsColliding(collidable, nearbyCollidable))
				{
					collidable.ResolveCollision(nearbyCollidable);
					nearbyCollidable.ResolveCollision(collidable);
				}
			}
		}

		private bool IsColliding(ICollidable collidableEntity, ICollidable collidableEntity2)
		{
			bool isColliding = false;

			if (collidableEntity.CollisionBox.Intersects(collidableEntity2.CollisionBox))
				isColliding = true;

			return isColliding;
		}
	}
}
