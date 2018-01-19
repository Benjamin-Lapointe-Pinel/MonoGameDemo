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
	public partial class Character : Box
	{
		protected AnimationSheet animationSheet;

		protected float RunningAcceleration;
		protected float JumpingAcceleration;
		protected float MaxRunningSpeed;

		public Character(Game game, AnimationSheet animationSheet, float x, float y, float width, float height)
			: base(game, x, y, width, height, false, true, Math.Min(width, height))
		{
			this.animationSheet = animationSheet;

			Acceleration.Y = 6000;

			MaxRunningSpeed = 600;
			RunningAcceleration = MaxRunningSpeed * 8;
			JumpingAcceleration = 1750;
		}

		public override void Update(GameTime gameTime)
		{
			Speed.X = MathHelper.Clamp(Speed.X, -MaxRunningSpeed, MaxRunningSpeed);

			float slowing = RunningAcceleration * 0.75f;
			float movement = slowing * (float)gameTime.ElapsedGameTime.TotalSeconds;

			if (Speed.X > movement)
			{
				Acceleration.X = -slowing;
			}
			else if (Speed.X < -movement)
			{
				Acceleration.X = slowing;
			}
			else
			{
				Speed.X = 0;
				Acceleration.X = 0;
			}

			animationSheet.Update(gameTime);

			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			SpriteBatch spriteBatch = Game.Services.GetService<SpriteBatch>();
			if (SolidCollisionSide.HasFlag(CollisionDirection.Bottom) && Speed.Y >= 0)
			{
				if (Speed.X != 0)
				{
					animationSheet.CycleIndex = 1;
				}
				else
				{
					animationSheet.CycleIndex = 0;
				}
			}
			else
			{
				animationSheet.CycleIndex = 2;
			}

			animationSheet.DestinationRectangle = Rectangle;
			animationSheet.Draw(gameTime);
		}

		public void WalkLeft()
		{
			Acceleration.X = -RunningAcceleration;

			animationSheet.SpriteEffects = SpriteEffects.FlipHorizontally;
		}

		public void WalkRight()
		{
			Acceleration.X = RunningAcceleration;

			animationSheet.SpriteEffects = SpriteEffects.None;
		}

		protected bool CanJump()
		{
			return Speed.Y == 0
				&& SolidCollisionSide.HasFlag(CollisionDirection.Bottom)
				&& !SolidCollisionSide.HasFlag(CollisionDirection.Top);
		}

		public void Jump()
		{
			if (CanJump())
			{
				Speed.Y = -JumpingAcceleration;
			}
		}
	}
}
