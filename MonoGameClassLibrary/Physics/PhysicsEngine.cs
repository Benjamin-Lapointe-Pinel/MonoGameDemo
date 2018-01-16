using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameClassLibrary.Physics
{
	public class PhysicsEngine : DrawableGameComponent
	{
		public SpatialGrid SpatialGrid { get; protected set; }
		public int Width { get { return SpatialGrid.Width; } }
		public int Height { get { return SpatialGrid.Width; } }

		public PhysicsEngine(Game game, Rectangle rectangle)
			: base(game)
		{
			SpatialGrid = new SpatialGrid(Game, rectangle.Width / SpatialGrid.TILE_SIZE, rectangle.Height / SpatialGrid.TILE_SIZE);
			DrawOrder = int.MaxValue;
		}

		public void Update(GameTime gameTime, IEnumerable<GameComponent> components)
		{
			List<Box> boxes = new List<Box>();
			List<Box> movingBoxes = new List<Box>();
			foreach (GameComponent component in components)
			{
				if (component is Box)
				{
					Box box = component as Box;
					boxes.Add(box);
					if (box.Acceleration != Vector2.Zero)
					{
						box.UpdateSpeed(gameTime);
						if (box.Speed != Vector2.Zero)
						{
							movingBoxes.Add(box);
						}
					}
					if (box is CollisionBox)
					{
						CollisionBox collisionBox = box as CollisionBox;
						collisionBox.ClearCollisions();
					}
				}
				component.Update(gameTime);
			}

			ApplyPhysics(gameTime, movingBoxes);
			ApplyCollisions(boxes);
		}

		public void ApplyCollisions(IEnumerable<Box> boxes)
		{
			foreach (Box box in boxes)
			{
				SpatialGrid.RemoveBox(box);
				PhysicsHelper.SetCollisionFlag(box, SpatialGrid);
				SpatialGrid.AddBox(box);
			}
		}

		//BUG : résolution de collision de deux objets qui bougent, pas précis
		private void ApplyPhysics(GameTime gameTime, IEnumerable<Box> movingBox)
		{
			foreach (Box box in movingBox)
			{
				SpatialGrid.RemoveBox(box);

				int steps = 1;

				int maxSpeed = SpatialGrid.TILE_SIZE;
				if (box.PreciseCollision)
				{
					maxSpeed = Math.Min(box.Width, box.Height);
				}
				if (box.Speed.Length() > maxSpeed)
				{
					steps = (int)Math.Ceiling(box.Speed.Length() / maxSpeed);
				}

				GameTime relativeGameTime = new GameTime(gameTime.TotalGameTime, gameTime.ElapsedGameTime);
				relativeGameTime.ElapsedGameTime = new TimeSpan(gameTime.ElapsedGameTime.Ticks / steps);

				bool lastStepIsAPhysicalColllision = false;
				for (int i = 0; i < steps; i++)
				{
					box.UpdateLocation(relativeGameTime);

					lastStepIsAPhysicalColllision = false;
					if (box.Speed == Vector2.Zero)
					{
						break;
					}
					else if (PhysicsHelper.Intersect(box, SpatialGrid.GetProbableCollisions(box)))
					{
						if (box.InteractWithSolid)
						{
							PhysicsHelper.PhysicalCollisions(relativeGameTime, box, SpatialGrid);
							lastStepIsAPhysicalColllision = true;
						}
						PhysicsHelper.SetCollisionFlag(box, SpatialGrid);
					}
				}
				if (box.InteractWithSolid && lastStepIsAPhysicalColllision)
				{
					PhysicsHelper.StopSpeed(box, SpatialGrid);
				}

				SpatialGrid.AddBox(box);
			}
		}

		public override void Draw(GameTime gameTime)
		{
			SpriteBatch spriteBatch = Game.Services.GetService<SpriteBatch>();
			foreach (Box box in SpatialGrid.GetProbableCollisions(new Box(null, new Rectangle(0, 0, Width, Height))))
			{
				DrawHelper.DrawOutline(spriteBatch, box.Rectangle, Color.Black);

				Vector2 end = box.Acceleration;
				end *= (float)gameTime.ElapsedGameTime.TotalSeconds;
				DrawHelper.DrawLine(spriteBatch, box.Center, box.Center + end.ToPoint(), Color.Red);

				end = box.Speed;
				end *= (float)gameTime.ElapsedGameTime.TotalSeconds;
				DrawHelper.DrawLine(spriteBatch, box.Center, box.Center + end.ToPoint(), Color.Blue);
			}
			SpatialGrid.Draw(gameTime);
		}
	}
}
