using Microsoft.Xna.Framework;
using MonoGameClassLibrary.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameClassLibrary.Physics
{
	public static class CollisionHelper
	{
		public static void ResolveMovementPhysics(AABB aabb, SpatialGrid spatialGrid)
		{
			//S'il y a vraiment collision
			if (Intersect(aabb, spatialGrid.GetProbableSolidCollisions(aabb)))
			{
				AABB aabbCopy = new AABB(aabb);
				Vector2 start = aabb.Location;
				Vector2 collisionEnd = aabb.OldLocation;
				Vector2 movement = start - collisionEnd;
				Vector2 movementCopy = movement;

				//Tant que la collision n'est pas résolue
				while (aabbCopy.Location != aabbCopy.OldLocation) //Pourrait être optimisé contre une perte de précision?
				{
					if (Intersect(aabbCopy, spatialGrid.GetProbableSolidCollisions(aabbCopy)))
					{
						//Défait le mouvement qui créer la collion
						aabbCopy.Offset(-movementCopy);
					}
					else
					{
						//Avance plus lentement vers la collion
						movementCopy /= 2;
						aabbCopy.Offset(movementCopy);
					}
				}

				//Fin du mouvement
				if (aabb is Box)
				{
					//movementCopy = movement - (start - aabbCopy.Location);
					movementCopy = movement;

					IEnumerable<AABB> solids = spatialGrid.GetProbableSolidCollisions(new Box(aabb as Box));
					if (((movement.X < 0) && (LeftCollision(aabbCopy, solids))) ||
						((movement.X > 0) && ((RightCollision(aabbCopy, solids)))))
					{
						movementCopy.X = 0;
					}
					if (((movement.Y < 0) && (TopCollision(aabbCopy, solids))) ||
						((movement.Y > 0) && (BottomCollision(aabbCopy, solids))))
					{
						movementCopy.Y = 0;
					}

					if (movementCopy != Vector2.Zero)
					{
						aabbCopy.Offset(movementCopy);
						ResolveMovementPhysics(aabbCopy, spatialGrid);
					}
				}
				aabb.Location = aabbCopy.Location;
			}
		}

		public static bool Intersect(AABB main, IEnumerable<AABB> aabbs)
		{
			foreach (AABB aabb in aabbs)
			{
				if (main.Intersects(aabb))
				{
					return true;
				}
			}
			return false;
		}

		public static bool LeftCollision(AABB main, IEnumerable<AABB> aabbs)
		{
			foreach (AABB aabb in aabbs)
			{
				if (main.LeftCollision(aabb))
				{
					return true;
				}
			}
			return false;
		}

		public static bool RightCollision(AABB main, IEnumerable<AABB> aabbs)
		{
			foreach (AABB aabb in aabbs)
			{
				if (main.RightCollision(aabb))
				{
					return true;
				}
			}
			return false;
		}

		public static bool TopCollision(AABB main, IEnumerable<AABB> aabbs)
		{
			foreach (AABB aabb in aabbs)
			{
				if (main.TopCollision(aabb))
				{
					return true;
				}
			}
			return false;
		}

		public static bool BottomCollision(AABB main, IEnumerable<AABB> aabbs)
		{
			foreach (AABB aabb in aabbs)
			{
				if (main.BottomCollision(aabb))
				{
					return true;
				}
			}
			return false;
		}

		//C'est sacré
		public static void StopSpeed(Box box, SpatialGrid spatialGrid)
		{
			//Restore l'ancienne vitesse en vérifiant quels côtés ont fait la collision
			IEnumerable<AABB> solids = spatialGrid.GetProbableSolidCollisions(box);
			if (((box.Speed.X < 0) && (LeftCollision(box, solids))) ||
				((box.Speed.X > 0) && ((RightCollision(box, solids)))))
			{
				box.Speed.X = 0;
			}
			if (((box.Speed.Y < 0) && (TopCollision(box, solids))) ||
				((box.Speed.Y > 0) && (BottomCollision(box, solids))))
			{
				box.Speed.Y = 0;
			}
		}
	}
}
