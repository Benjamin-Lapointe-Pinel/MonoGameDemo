using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static MonoGameClassLibrary.EntityManager;

namespace MonoGameClassLibrary.Physics
{
	public class PhysicsEngine : EntityManager.Drawable
	{
		public SpatialGrid SpatialGrid { get; protected set; }
		public int Width { get { return SpatialGrid.Width; } }
		public int Height { get { return SpatialGrid.Width; } }
		protected List<Box> boxes;

		public PhysicsEngine(int width, int height)
		{
			SpatialGrid = new SpatialGrid(width / SpatialGrid.TILE_SIZE, height / SpatialGrid.TILE_SIZE);
			boxes = new List<Box>();
			DrawOrder = int.MaxValue;
		}

		public void Add(Box box)
		{
			boxes.Add(box);
			SpatialGrid.AddAxisAlignedBoundingBox(box);
		}

		public void Remove(Box box)
		{
			boxes.Remove(box);
			SpatialGrid.RemoveAxisAlignedBoundingBox(box);
		}

		public override void EntityUpdate(GameTime gameTime)
		{
			IEnumerable<Box> movingBox = ApplyUpdates(gameTime);
			ApplyPhysics(gameTime, movingBox);
		}

		private IEnumerable<Box> ApplyUpdates(GameTime gameTime)
		{
			List<Box> movingBoxes = new List<Box>();

			foreach (Box box in boxes)
			{
				if (box.Acceleration != Vector2.Zero)
				{
					box.UpdateSpeed(gameTime);
					if (box.Speed != Vector2.Zero)
					{
						movingBoxes.Add(box);
					}
				}
				box.PhysicsUpdate(gameTime);
				box.ClearCollisions();
			}

			return movingBoxes;
		}

		//BUG : résolution de collision de deux objets qui bougent, pas précis
		private void ApplyPhysics(GameTime gameTime, IEnumerable<Box> movingBox)
		{
			int collisions = 0;
			foreach (Box box in movingBox)
			{
				SpatialGrid.RemoveAxisAlignedBoundingBox(box);

				int steps = 1;

				int maxSpeed = SpatialGrid.TILE_SIZE;
				if (false) //precise collsion... Oui, non? Flag?
				{
					maxSpeed = Math.Min(box.Width, box.Height);
				}
				if (box.Speed.Length() > maxSpeed)
				{
					steps = (int)Math.Ceiling(box.Speed.Length() / maxSpeed);
				}

				GameTime relativeGameTime = new GameTime(gameTime.TotalGameTime, gameTime.ElapsedGameTime);
				relativeGameTime.ElapsedGameTime = new TimeSpan(gameTime.ElapsedGameTime.Ticks / steps);

				bool colllision = false;
				for (int i = 0; i < steps; i++)
				{
					box.UpdateLocation(relativeGameTime);

					if (box.Speed == Vector2.Zero)
					{
						break;
					}
					else
					{
						colllision = CollisionHelper.PhysicalCollisions(relativeGameTime, box, SpatialGrid);
						if (colllision)
						{
							CollisionHelper.SetCollisionFlag(box, SpatialGrid);
						}
					}
				}
				if (colllision)
				{
					CollisionHelper.StopSpeed(box, SpatialGrid);
					collisions++;
				}

				SpatialGrid.AddAxisAlignedBoundingBox(box);
			}

			Console.WriteLine("PhysicalCollisions : " + collisions);
		}

		public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
		{
			foreach (Box box in boxes)
			{
				Vector2 end = box.Speed;
				end *= (float)gameTime.ElapsedGameTime.TotalSeconds;

				DrawHelper.DrawOutline(spriteBatch, box.Rectangle, Color.Black);
				DrawHelper.DrawLine(spriteBatch, box.Center, box.Center + end.ToPoint(), Color.Red);
			}
			SpatialGrid.Draw(spriteBatch, gameTime);
		}
	}
}
