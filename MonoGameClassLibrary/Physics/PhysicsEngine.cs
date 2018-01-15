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
			IEnumerable<Box> acceleratingBox;
			acceleratingBox = ApplyMovement(gameTime);

			IEnumerable<Box> movingBox;
			movingBox = ApplyPhysics(gameTime, acceleratingBox);

			ApplyCollisions(movingBox);
		}

		public void ApplyCollisions(IEnumerable<Box> movingBox)
		{
			foreach (Box box in movingBox)
			{
				SpatialGrid.RemoveAxisAlignedBoundingBox(box);

				CollisionHelper.SetCollisionFlag(box, SpatialGrid);

				SpatialGrid.AddAxisAlignedBoundingBox(box);
			}
		}

		private IEnumerable<Box> ApplyMovement(GameTime gameTime)
		{
			List<Box> acceleratingBox = new List<Box>();

			foreach (Box box in boxes)
			{
				if (box.Acceleration != Vector2.Zero)
				{
					box.UpdateSpeed(gameTime);
					acceleratingBox.Add(box);
				}
				box.PhysicsUpdate(gameTime);
			}

			return acceleratingBox;
		}

		//BUG : résolution de collision de deux objets qui bougent, pas précis
		private IEnumerable<Box> ApplyPhysics(GameTime gameTime, IEnumerable<Box> acceleratingBox)
		{
			List<Box> movingBox = new List<Box>();

			int collisions = 0;
			foreach (Box box in acceleratingBox)
			{
				SpatialGrid.RemoveAxisAlignedBoundingBox(box);

				if (box.Speed != Vector2.Zero)
				{
					movingBox.Add(box);

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

					for (int i = 0; i < steps; i++)
					{
						box.UpdateLocation(relativeGameTime);

						if (box.Speed == Vector2.Zero)
						{
							break;
						}
						else
						{
							collisions++;
							CollisionHelper.PhysicalCollisions(relativeGameTime, box, SpatialGrid);
						}
					}
					CollisionHelper.StopSpeed(box, SpatialGrid);
				}

				SpatialGrid.AddAxisAlignedBoundingBox(box);
			}

			Console.WriteLine("Collisions : " + collisions);

			return movingBox;
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
