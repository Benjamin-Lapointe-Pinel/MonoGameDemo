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
		public static void SetCollisionNotification(Box sender, SpatialGrid spatialGrid)
		{
			Box copy = new Box(sender);
			copy.Inflate(1, 1);
			foreach (Box aabb in spatialGrid.GetProbableCollisions(sender))
			{
				if (aabb.Intersects(copy))
				{
					sender.CollisionNotification(aabb);
				}
			}
			copy.Inflate(-1, -1);
		}

		//Use with caution
		public static void ClassicCollision(KineticBox sender, SpatialGrid spatialGrid)
		{
			if (sender.InteractWithSolid)
			{
				Vector2 exitVector = Vector2.Zero;

				int total = 1;
				foreach (Box collision in spatialGrid.GetProbableSolidCollisions(sender))
				{
					if (collision.Intersects(sender))
					{
						exitVector += sender.ExitVector(collision);
						total++;
					}
				}
				if (exitVector != Vector2.Zero)
				{
					exitVector /= total;
					sender.Offset(exitVector);
				}
			}
		}

		public static void ExpulseCollision(Box sender, SpatialGrid spatialGrid)
		{
			if (sender.Solid)
			{
				foreach (Box aabb in spatialGrid.GetProbableCollisions(sender))
				{
					if (aabb is KineticBox)
					{
						KineticBox box = aabb as KineticBox;
						if ((box.InteractWithSolid) && (box.Intersects(sender)))
						{
							ClassicCollision(box, spatialGrid);
						}
					}
				}
			}
		}

		//private static Vector2 ExitVector(Box collision, Box collisionWith)
		//{
		//	Box intersection = Box.Intersect(collision, collisionWith);
		//	Vector2 intersectionVector = new Vector2(intersection.Width, intersection.Height);
		//	Vector2 directionVector = collision.Center - collisionWith.Center;
		//
		//	if (directionVector == Vector2.Zero)
		//	{
		//		directionVector.X = -1;
		//	}
		//
		//	Vector2 exitVector = intersectionVector * (directionVector / directionVector.Length());
		//
		//	if ((exitVector.Y != 0) && (Math.Abs(exitVector.X) > Math.Abs(exitVector.Y)))
		//	{
		//		exitVector.X = 0;
		//	}
		//	else if ((exitVector.X != 0) && (Math.Abs(exitVector.Y) > Math.Abs(exitVector.X)))
		//	{
		//		exitVector.Y = 0;
		//	}
		//
		//	return exitVector;
		//}

		public static void MovementCollision(KineticBox box, SpatialGrid spatialGrid)
		{
			//S'il y a vraiment collision
			if (Intersect(box, spatialGrid.GetProbableSolidCollisions(box)))
			{
				Vector2 exitVector = box.OldLocation - box.Location;

				KineticBox copy = ResolveMovementCollision(box, exitVector, spatialGrid);

				Vector2 finalMovement = box.Location - copy.Location;
				IEnumerable<Box> solids = spatialGrid.GetProbableSolidCollisions(copy);
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

		private static KineticBox ResolveMovementCollision(KineticBox box, Vector2 exitVector, SpatialGrid spatialGrid)
		{
			KineticBox copy = new KineticBox(box);
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

		public static bool Intersect(Box main, IEnumerable<Box> aabbs)
		{
			foreach (Box aabb in aabbs)
			{
				if (aabb.Intersects(main))
				{
					return true;
				}
			}
			return false;
		}

		public static bool LeftCollision(Box main, IEnumerable<Box> aabbs)
		{
			foreach (Box aabb in aabbs)
			{
				if (aabb.RightCollision(main))
				{
					return true;
				}
			}
			return false;
		}

		public static bool RightCollision(Box main, IEnumerable<Box> aabbs)
		{
			foreach (Box aabb in aabbs)
			{
				if (aabb.LeftCollision(main))
				{
					return true;
				}
			}
			return false;
		}

		public static bool TopCollision(Box main, IEnumerable<Box> aabbs)
		{
			foreach (Box aabb in aabbs)
			{
				if (aabb.BottomCollision(main))
				{
					return true;
				}
			}
			return false;
		}

		public static bool BottomCollision(Box main, IEnumerable<Box> aabbs)
		{
			foreach (Box aabb in aabbs)
			{
				if (aabb.TopCollision(main))
				{
					return true;
				}
			}
			return false;
		}

		//C'est sacré
		public static void StopSpeed(KineticBox box, SpatialGrid spatialGrid)
		{
			//Restore l'ancienne vitesse en vérifiant quels côtés ont fait la collision
			IEnumerable<Box> solids = spatialGrid.GetProbableSolidCollisions(box);
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
