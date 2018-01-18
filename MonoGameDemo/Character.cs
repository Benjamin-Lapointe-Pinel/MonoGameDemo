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
	public partial class Character : SmartBox
	{
		protected AnimationSheet animationSheet;

		protected float RunningAcceleration;
		protected float JumpingSpeed;
		protected float MaxRunningSpeed;

		public Character(Game game, AnimationSheet animationSheet, float x, float y, float width, float height)
			: base(game, x, y, width, height, false, true, Math.Min(width, height))
		{
			this.animationSheet = animationSheet;

			Acceleration.Y = 6000;

			MaxRunningSpeed = 1000;
			RunningAcceleration = MaxRunningSpeed * 8;
			JumpingSpeed = 1750;
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
			if (!SolidBottomCollision())
			{
				animationSheet.CycleIndex = 2;
			}
			else if (Speed.X != 0)
			{
				animationSheet.CycleIndex = 1;
			}
			else
			{
				animationSheet.CycleIndex = 0;
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
			return SolidBottomCollision() && !SolidTopCollision();
		}

		public void Jump()
		{
			if (CanJump())
			{
				Speed.Y = -JumpingSpeed;
			}
		}
	}
}
