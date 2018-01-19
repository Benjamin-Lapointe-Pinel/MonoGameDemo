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
		public static void SetCollisionNotification(AABB sender, SpatialGrid spatialGrid)
		{
			AABB copy = new AABB(sender);
			copy.Inflate(1, 1);
			foreach (AABB aabb in spatialGrid.GetProbableCollisions(sender))
			{
				if (copy.Intersects(aabb))
				{
					sender.CollisionNotification(aabb);
				}
			}
			copy.Inflate(-1, -1);
		}

		public static void ResolveClassicCollision(AABB sender, SpatialGrid spatialGrid)
		{
			if (sender.Solid)
			{
				foreach (AABB aabb in spatialGrid.GetProbableCollisions(sender))
				{
					if ((aabb is Box) &&
						((aabb as Box).InteractWithSolid) &&
						(sender.Intersects(aabb)))
					{
							Vector2 movement = sender.Center - aabb.Center;
							AABB copy = ResolveCollision(aabb, movement, spatialGrid);
							aabb.Location = copy.Location;
					}
				}
			}
		}

		public static void ResolveMovementPhysics(AABB aabb, SpatialGrid spatialGrid)
		{
			//S'il y a vraiment collision
			if (Intersect(aabb, spatialGrid.GetProbableSolidCollisions(aabb)))
			{
				Vector2 end = aabb.Location;
				Vector2 start = aabb.OldLocation;
				Vector2 movement = end - start;

				AABB copy = ResolveCollision(aabb, movement, spatialGrid);

				//Fin du mouvement
				if (aabb is Box)
				{
					movement = end - copy.Location;

					IEnumerable<AABB> solids = spatialGrid.GetProbableSolidCollisions(copy);
					if (((movement.X < 0) && (LeftCollision(copy, solids))) ||
						((movement.X > 0) && ((RightCollision(copy, solids)))))
					{
						movement.X = 0;
					}
					if (((movement.Y < 0) && (TopCollision(copy, solids))) ||
						((movement.Y > 0) && (BottomCollision(copy, solids))))
					{
						movement.Y = 0;
					}

					if (movement != Vector2.Zero)
					{
						copy.Offset(movement);
						ResolveMovementPhysics(copy, spatialGrid);
					}
					else
					{
						(aabb as Box).Speed = Vector2.Zero;
					}
				}
				aabb.Location = copy.Location;
			}
		}

		private static AABB ResolveCollision(AABB aabb, Vector2 movement, SpatialGrid spatialGrid)
		{
			AABB copy = new AABB(aabb);
			//Tant que la collision n'est pas résolue
			while (copy.Location != copy.OldLocation) //Pourrait être optimisé contre une perte de précision?
			{
				if (Intersect(copy, spatialGrid.GetProbableSolidCollisions(copy)))
				{
					//Défait le mouvement qui créer la collion
					copy.Offset(-movement);
				}
				else
				{
					//Avance plus lentement vers la collion
					movement /= 2;
					copy.Offset(movement);
				}
			}

			return copy;
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
