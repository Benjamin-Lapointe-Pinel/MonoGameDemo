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

		public Character(AnimationSheet animationSheet, Rectangle boundingBox)
		{
			this.animationSheet = animationSheet;
			this.BoundingBox = new Box(this, boundingBox, false, true, true);

			MaxSpeed = new Vector2(800, 0);
			Acceleration = new Vector2(MaxSpeed.X * 8, 1600);
		}

		public override void Update(GameTime gameTime)
		{
			inertia(gameTime);
		}

		public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
		{
			if (!IsStanding())
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

			animationSheet.Update(gameTime);

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

		protected bool IsStanding()
		{
			foreach (KeyValuePair<Box, Box.Side> collisions in BoundingBox.Collisions)
			{
				if (collisions.Key.Solid)
				{
					if (collisions.Value.HasFlag(Box.Side.Bottom))
					{
						return true;
					}
				}
			}

			return false;
		}

		protected bool CanJump()
		{
			if (IsStanding())
			{
				foreach (KeyValuePair<Box, Box.Side> collisions in BoundingBox.Collisions)
				{
					if (collisions.Key.Solid)
					{
						if (collisions.Value.HasFlag(Box.Side.Top))
						{
							return false;
						}

					}
				}
				return true;
			}
			return false;
		}

		public void Jump(GameTime gameTime)
		{
			if (CanJump())
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
