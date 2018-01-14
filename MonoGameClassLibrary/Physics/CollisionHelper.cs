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
		public static void Collision(GameTime gameTime, Box box, IEnumerable<AxisAlignedBoundingBox> boxes)
		{
			box.ResetCollisionFlags();

			List<AxisAlignedBoundingBox> solids;
			List<AxisAlignedBoundingBox> nonsolids;
			SeperateSolidBoxes(boxes, out solids, out nonsolids);

			if (solids.Count > 0)
			{
				SolidCollisions(gameTime, box, solids);
			}
			if (nonsolids.Count > 0)
			{
				//TODO
			}
		}

		public static void SolidCollisions(GameTime gameTime, Box box, List<AxisAlignedBoundingBox> solids)
		{
			//S'il y a vraiment collision
			if (Intersect(box, solids))
			{
				Vector2 oldSpeed = box.Speed;
				Rectangle OldRectangle = new Rectangle();
				//Tant que la collision n'est pas résolue
				while (box.Rectangle != OldRectangle)
				{
					OldRectangle = box.Rectangle;
					if (Intersect(box, solids))
					{
						//Défait le mouvement qui créer la collion
						box.Speed = Vector2.Negate(box.Speed);
						box.Update(gameTime);
						box.Speed = Vector2.Negate(box.Speed);
					}
					else
					{
						//Avance plus lentement vers la collion
						box.Speed /= 2;
						box.Update(gameTime);
					}
				}

				//Restore l'ancienne vitesse en vérifiant quels côtés ont fait la collision
				box.Speed = oldSpeed;
				if (((box.Speed.X < 0) && (leftCollision(box, solids))) ||
					((box.Speed.X > 0) && ((rightCollision(box, solids)))))
				{
					box.Speed.X = 0;
				}
				if (((box.Speed.Y < 0) && (topCollision(box, solids))) ||
					((box.Speed.Y > 0) && (bottomCollision(box, solids))))
				{
					box.Speed.Y = 0;
				}

				box.Update(gameTime);

				//New collision possible
				if (!box.Speed.Equals(Vector2.Zero))
				{
					//Resolve new movement
					SolidCollisions(gameTime, box, solids);
				}
			}
		}

		public static void setCollisionFlags(AxisAlignedBoundingBox axisAlignedBoundingBox, IEnumerable<AxisAlignedBoundingBox> boxes)
		{
			List<AxisAlignedBoundingBox> solids;
			SeperateSolidBoxes(boxes, out solids, out _);

			if (leftCollision(axisAlignedBoundingBox, solids))
			{
				axisAlignedBoundingBox.FlagLeftCollision();
			}
			if (rightCollision(axisAlignedBoundingBox, solids))
			{
				axisAlignedBoundingBox.FlagRightCollision();
			}
			if (topCollision(axisAlignedBoundingBox, solids))
			{
				axisAlignedBoundingBox.FlagTopCollision();
			}
			if (bottomCollision(axisAlignedBoundingBox, solids))
			{
				axisAlignedBoundingBox.FlagBottomCollision();
			}
		}

		public static bool rightCollision(AxisAlignedBoundingBox box, List<AxisAlignedBoundingBox> boxes)
		{
			bool result = false;

			box.X += 1;
			if (Intersect(box, boxes))
			{
				result =  true;
			}
			box.X -= 1;

			return result;
		}

		public static bool leftCollision(AxisAlignedBoundingBox box, List<AxisAlignedBoundingBox> boxes)
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

		public static bool topCollision(AxisAlignedBoundingBox box, List<AxisAlignedBoundingBox> boxes)
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

		public static bool bottomCollision(AxisAlignedBoundingBox box, List<AxisAlignedBoundingBox> boxes)
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

		private static bool Intersect(AxisAlignedBoundingBox box, IEnumerable<AxisAlignedBoundingBox> axisAlignedBoundingBoxes)
		{
			foreach (AxisAlignedBoundingBox axisAlignedBoundingBoxe in axisAlignedBoundingBoxes)
			{
				if (box.Intersects(axisAlignedBoundingBoxe))
				{
					return true;
				}
			}
			return false;
		}

		private static void SeperateSolidBoxes(IEnumerable<AxisAlignedBoundingBox> boxes, out List<AxisAlignedBoundingBox> solids, out List<AxisAlignedBoundingBox> nonSolides)
		{
			solids = new List<AxisAlignedBoundingBox>();
			nonSolides = new List<AxisAlignedBoundingBox>();

			foreach (AxisAlignedBoundingBox box in boxes)
			{
				if (box.Solid)
				{
					solids.Add(box);
				}
				else
				{
					nonSolides.Add(box);
				}
			}
		}
	}
}
