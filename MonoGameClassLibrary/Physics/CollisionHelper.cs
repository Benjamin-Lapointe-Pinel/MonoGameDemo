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
				if (aabb.Intersects(copy))
				{
					sender.CollisionNotification(aabb);
				}
			}
			copy.Inflate(-1, -1);
		}

		//Use with caution
		public static void ClassicCollision(Box sender, SpatialGrid spatialGrid)
		{
			if (sender.InteractWithSolid)
			{
				Vector2 exitVector = Vector2.Zero;

				int total = 0;
				foreach (AABB collision in spatialGrid.GetProbableSolidCollisions(sender))
				{
					if (collision.Intersects(sender))
					{
						exitVector += ExitVector(sender, collision);
						total++;
					}
				}
				if (total > 0)
				{
					exitVector /= total;
					AABB copy = ResolveMovementCollision(sender, exitVector, spatialGrid);
					sender.Location = copy.Location;
				}
			}
		}

		public static void ExpulseCollision(AABB sender, SpatialGrid spatialGrid)
		{
			if (sender.Solid)
			{
				foreach (AABB aabb in spatialGrid.GetProbableCollisions(sender))
				{
					if (aabb is Box)
					{
						Box box = aabb as Box;
						if ((box.InteractWithSolid) && (box.Intersects(sender)))
						{
							ClassicCollision(box, spatialGrid);
						}
					}
				}
			}
		}

		private static Vector2 ExitVector(AABB collision, AABB collisionWith)
		{
			AABB intersection = AABB.Intersect(collision, collisionWith);
			Vector2 intersectionVector = new Vector2(intersection.Width, intersection.Height);
			Vector2 directionVector = collision.Center - collisionWith.Center;

			if (directionVector == Vector2.Zero)
			{
				directionVector.X = -1;
			}

			Vector2 exitVector = intersectionVector * (directionVector / directionVector.Length());

			if ((exitVector.Y != 0) && (Math.Abs(exitVector.X) > Math.Abs(exitVector.Y)))
			{
				exitVector.X = 0;
			}
			else if ((exitVector.X != 0) && (Math.Abs(exitVector.Y) > Math.Abs(exitVector.X)))
			{
				exitVector.Y = 0;
			}

			return exitVector;
		}

		public static void MovementCollision(Box box, SpatialGrid spatialGrid)
		{
			//S'il y a vraiment collision
			if (Intersect(box, spatialGrid.GetProbableSolidCollisions(box)))
			{
				Vector2 exitVector = box.OldLocation - box.Location;

				Box copy = ResolveMovementCollision(box, exitVector, spatialGrid);

				Vector2 finalMovement = box.Location - copy.Location;
				IEnumerable<AABB> solids = spatialGrid.GetProbableSolidCollisions(copy);
				if (((finalMovement.X < 0) && (LeftCollision(copy, solids))) ||
					((finalMovement.X > 0) && ((RightCollision(copy, solids)))))
				{
					finalMovement.X = 0;
				}
				if (((finalMovement.Y < 0) && (TopCollision(copy, solids))) ||
					((finalMovement.Y > 0) && (BottomCollision(copy, solids))))
				{
					finalMovement.Y = 0;
				}

				if (finalMovement != Vector2.Zero)
				{
					copy.Offset(finalMovement);
					MovementCollision(copy, spatialGrid);
				}
				else
				{
					box.Speed = Vector2.Zero;
				}

				box.Location = copy.Location;
			}
		}

		private static Box ResolveMovementCollision(Box box, Vector2 exitVector, SpatialGrid spatialGrid)
		{
			Box copy = new Box(box);
			//Tant que la collision n'est pas résolue
			while (copy.Location != copy.OldLocation) //Pourrait être optimisé contre une perte de précision?
			{
				if (Intersect(copy, spatialGrid.GetProbableSolidCollisions(copy)))
				{
					//Défait le mouvement qui créer la collion
					copy.Offset(exitVector);
				}
				else
				{
					//Avance plus lentement vers la collion
					exitVector /= 2;
					copy.Offset(-exitVector);
				}
			}

			return copy;
		}

		public static bool Intersect(AABB main, IEnumerable<AABB> aabbs)
		{
			foreach (AABB aabb in aabbs)
			{
				if (aabb.Intersects(main))
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
				if (aabb.RightCollision(main))
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
				if (aabb.LeftCollision(main))
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
				if (aabb.BottomCollision(main))
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
				if (aabb.TopCollision(main))
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
			if (((box.Speed.X <= 0) && (LeftCollision(box, solids))) ||
				((box.Speed.X >= 0) && ((RightCollision(box, solids)))))
			{
				box.Speed.X = 0;
			}
			if (((box.Speed.Y <= 0) && (TopCollision(box, solids))) ||
				((box.Speed.Y >= 0) && (BottomCollision(box, solids))))
			{
				box.Speed.Y = 0;
			}
		}
	}
}
