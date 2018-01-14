using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameClassLibrary;
using MonoGameClassLibrary.Animation;
using MonoGameClassLibrary.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameDemo
{
	public partial class Character : EntityManager.Drawable
	{
		public Box BoundingBox { get; protected set; }
		protected AnimationSheet animationSheet;
		protected Vector2 Acceleration;
		protected Vector2 MaxSpeed;

		public Character(AnimationSheet animationSheet, Box boundingBox)
		{
			this.animationSheet = animationSheet;
			this.BoundingBox = boundingBox;

			MaxSpeed = new Vector2(800, 0);
			Acceleration = new Vector2(MaxSpeed.X * 8, 1600);
		}

		public override void Update(GameTime gameTime)
		{
			inertia(gameTime);

			if ((animationSheet.CycleIndex != 3) || (animationSheet.CurrentCycle.IsOver))
			{
				if (!BoundingBox.BottomCollision)
				{
					animationSheet.CycleIndex = 2;
				}
				else if (BoundingBox.Speed.X != 0)
				{
					animationSheet.CycleIndex = 1;
				}
				else
				{
					animationSheet.CycleIndex = 0;
				}
			}
			animationSheet.Update(gameTime);
		}

		public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
		{
			animationSheet.DestinationRectangle = BoundingBox.Rectangle;
			animationSheet.Draw(spriteBatch, gameTime);
		}

		public void WalkLeft(GameTime gameTime)
		{
			float totalSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
			float movement = Acceleration.X * totalSeconds;

			BoundingBox.Speed.X -= movement;
			animationSheet.SpriteEffects = SpriteEffects.FlipHorizontally;
		}

		public void WalkRight(GameTime gameTime)
		{
			float totalSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
			float movement = Acceleration.X * totalSeconds;

			BoundingBox.Speed.X += movement;
			animationSheet.SpriteEffects = SpriteEffects.None;
		}

		public void Jump(GameTime gameTime)
		{
			if (BoundingBox.BottomCollision)
			{
				BoundingBox.Speed.Y -= Acceleration.Y;
			}
		}

		private void inertia(GameTime gameTime)
		{
			float totalSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
			float movement = Acceleration.X * totalSeconds;
			movement *= 0.75f;
			if ((BoundingBox.Speed.X > movement) || (BoundingBox.Speed.X < -movement))
			{
				if (BoundingBox.Speed.X > 0)
				{
					BoundingBox.Speed.X -= movement;
				}
				else if (BoundingBox.Speed.X < 0)
				{
					BoundingBox.Speed.X += movement;
				}
			}
			else
			{
				BoundingBox.Speed.X = 0;
			}

			BoundingBox.Speed.X = MathHelper.Clamp(BoundingBox.Speed.X, -MaxSpeed.X, MaxSpeed.X);
		}
	}
}
