using Microsoft.Xna.Framework;
using MonoGameClassLibrary.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameClassLibrary.Physics
{

	//TODO RELIRE
	public static class CollisionHelper
	{
		public static bool PhysicalCollisions(GameTime gameTime, Box box, SpatialGrid spatialGrid)
		{
			//S'il y a vraiment collision
			if (Intersect(box, spatialGrid.GetProbableSolidCollisions(box)))
			{
				Vector2 oldSpeed = box.Speed;
				Rectangle OldRectangle = new Rectangle();
				//Tant que la collision n'est pas résolue
				while (box.Rectangle != OldRectangle)
				{
					OldRectangle = box.Rectangle;
					if (Intersect(box, spatialGrid.GetProbableSolidCollisions(box)))
					{
						//Défait le mouvement qui créer la collion
						box.Speed = Vector2.Negate(box.Speed);
						box.UpdateLocation(gameTime);
						box.Speed = Vector2.Negate(box.Speed);
					}
					else
					{
						//Avance plus lentement vers la collion
						box.Speed /= 2;
						box.UpdateLocation(gameTime);
					}
				}

				box.Speed = oldSpeed;
				StopSpeed(box, spatialGrid);
				box.UpdateLocation(gameTime);
				box.Speed = oldSpeed;

				//New collision possible
				if (oldSpeed != Vector2.Zero)
				{
					//Resolve new movement
					PhysicalCollisions(gameTime, box, spatialGrid);
				}

				return true;
			}

			return false;
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
