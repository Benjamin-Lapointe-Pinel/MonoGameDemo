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
		public static void SetCollisionFlag(Box mainBox, SpatialGrid spatialGrid)
		{
			foreach (Box box in spatialGrid.GetProbableCollisions(mainBox))
			{
				if (mainBox.Intersects(box))
				{
					mainBox.AddCollision(box, Box.Side.Unknown);
				}
				if ((mainBox.Speed.X <= 0) && (LeftCollision(mainBox, box)))
				{
					mainBox.AddCollision(box, Box.Side.Left);
				}
				if ((mainBox.Speed.X >= 0) && (RightCollision(mainBox, box)))
				{
					mainBox.AddCollision(box, Box.Side.Right);
				}
				if ((mainBox.Speed.Y <= 0) && (TopCollision(mainBox, box)))
				{
					mainBox.AddCollision(box, Box.Side.Top);
				}
				if ((mainBox.Speed.Y >= 0) && (BottomCollision(mainBox, box)))
				{
					mainBox.AddCollision(box, Box.Side.Bottom);
				}
			}
		}

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
				if (box.Speed != Vector2.Zero)
				{
					//Resolve new movement
					PhysicalCollisions(gameTime, box, spatialGrid);
				}

				return true;
			}

			return false;
		}

		public static void StopSpeed(Box box, SpatialGrid spatialGrid)
		{
			//Restore l'ancienne vitesse en vérifiant quels côtés ont fait la collision
			IEnumerable<Box> solids = spatialGrid.GetProbableSolidCollisions(box);
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

		public static bool LeftCollision(Box mainBox, Box box)
		{
			mainBox.X -= 1;
			bool result = mainBox.Intersects(box);
			mainBox.X += 1;

			return result;
		}

		public static bool RightCollision(Box mainBox, Box box)
		{
			mainBox.X += 1;
			bool result = mainBox.Intersects(box);
			mainBox.X -= 1;

			return result;
		}

		public static bool TopCollision(Box mainBox, Box box)
		{
			mainBox.Y -= 1;
			bool result = mainBox.Intersects(box);
			mainBox.Y += 1;

			return result;
		}

		public static bool BottomCollision(Box mainBox, Box box)
		{
			mainBox.Y += 1;
			bool result = mainBox.Intersects(box);
			mainBox.Y -= 1;

			return result;
		}

		public static bool RightCollision(Box box, IEnumerable<Box> boxes)
		{
			bool result = false;

			box.X += 1;
			if (Intersect(box, boxes))
			{
				result = true;
			}
			box.X -= 1;

			return result;
		}

		public static bool LeftCollision(Box box, IEnumerable<Box> boxes)
		{
			bool result = false;

			box.X -= 1;
			if (Intersect(box, boxes))
			{
				result = true;
			}
			box.X += 1;

			return result;
		}

		public static bool TopCollision(Box box, IEnumerable<Box> boxes)
		{
			bool result = false;

			box.Y -= 1;
			if (Intersect(box, boxes))
			{
				result = true;
			}
			box.Y += 1;

			return result;
		}

		public static bool BottomCollision(Box box, IEnumerable<Box> boxes)
		{
			bool result = false;

			box.Y += 1;
			if (Intersect(box, boxes))
			{
				result = true;
			}
			box.Y -= 1;

			return result;
		}

		private static bool Intersect(Box box, IEnumerable<Box> axisAlignedBoundingBoxes)
		{
			foreach (Box axisAlignedBoundingBoxe in axisAlignedBoundingBoxes)
			{
				if (box.Intersects(axisAlignedBoundingBoxe))
				{
					return true;
				}
			}
			return false;
		}
	}
}
