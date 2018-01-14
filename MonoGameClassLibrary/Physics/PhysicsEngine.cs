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
	//TODO: Spatial Grid
	public class PhysicsEngine : EntityManager.Drawable
	{
		public SpatialGrid spatialGrid { get; protected set; }
		protected List<AxisAlignedBoundingBox> boxes;
		public Vector2 Gravity;

		public PhysicsEngine()
		{
			spatialGrid = new SpatialGrid(100, 100);
			boxes = new List<AxisAlignedBoundingBox>();
			Gravity = new Vector2(0, 5000);

			DrawOrder = int.MaxValue;
		}

		public void Add(AxisAlignedBoundingBox box)
		{
			boxes.Add(box);
			spatialGrid.AddAxisAlignedBoundingBox(box);
		}

		public void Remove(AxisAlignedBoundingBox box)
		{
			boxes.Remove(box);
			spatialGrid.RemoveAxisAlignedBoundingBox(box);
		}

		//BUG potentiel : résolution de collision de deux objets qui bougent
		public override void Update(GameTime gameTime)
		{
			foreach (AxisAlignedBoundingBox box in boxes)
			{
				if (box is Box)
				{
					UpdateBox(gameTime, box as Box);
				}
				
			}
		}

		private void UpdateBox(GameTime gameTime, Box box)
		{
			spatialGrid.RemoveAxisAlignedBoundingBox(box);

			gravity(gameTime, box);

			int steps = 1;
			Vector2 oldSpeed = box.Speed;
			if (box.Speed.Length() > SpatialGrid.TILE_SIZE)
			{
				//Tunneling resolution
				steps = (int)Math.Ceiling(box.Speed.Length() / SpatialGrid.TILE_SIZE);
				TimeSpan slowMotion = new TimeSpan(gameTime.ElapsedGameTime.Ticks / steps);
				gameTime = new GameTime(gameTime.TotalGameTime, slowMotion);
			}
			for (int i = 0; i < steps; i++)
			{
				box.Update(gameTime);
				if (box.Solid)
				{
					CollisionHelper.Collision(gameTime, box, spatialGrid.GetProbableCollisions(box));
				}
			}
			if (steps > 1)
			{
				if (box.Speed.X != 0)
				{
					box.Speed.X = oldSpeed.X;
				}
				if (box.Speed.Y != 0)
				{
					box.Speed.Y = oldSpeed.Y;
				}
			}

			//Get surrounding tiles
			AxisAlignedBoundingBox axisAlignedBoundingBox = new AxisAlignedBoundingBox(box);
			axisAlignedBoundingBox.Inflate(2, 2);
			CollisionHelper.setCollisionFlags(box, spatialGrid.GetProbableCollisions(axisAlignedBoundingBox));

			spatialGrid.AddAxisAlignedBoundingBox(box);
		}

		private void gravity(GameTime gameTime, Box box)
		{
			float totalSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
			if (box.IsAffectedByGravity)
			{
				box.Speed += Gravity * totalSeconds;
			}
		}

		public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
		{
			foreach (AxisAlignedBoundingBox box in boxes)
			{
				spriteBatch.Draw(DrawHelper.Pixel, box.Rectangle, new Color(Color.Magenta, 0.5f));
			}
			spatialGrid.Draw(spriteBatch, gameTime);
		}

		public class Updatable
		{
			public virtual void Update(GameTime gameTime) { }
		}
	}
}
