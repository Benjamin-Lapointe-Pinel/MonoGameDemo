using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameClassLibrary;
using MonoGameClassLibrary.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameDemo
{
	public class Lever : AABB
	{
		public delegate void SwitchedHandler(Lever sender);
		public event SwitchedHandler Toggled;
		public event SwitchedHandler SwitchedOn;
		public event SwitchedHandler SwitchedOff;

		protected Sprite sprite;
		public bool SwitchState { get; protected set; }
		public TimeSpan StayOn { get; protected set; }
		public TimeSpan Counter { get; protected set; }

		public Lever(Game game, Point location, TimeSpan stayOn, bool state = false)
			: base(game, new Rectangle(location, new Point(32, 32)), false)
		{
			SwitchState = state;
			StayOn = stayOn;
			sprite = new Sprite(Game, Game.Content.Load<Texture2D>("lever"));
			sprite.SourceRectangle.Width = 32;

			OnCollision += Lever_OnCollision;
		}

		private void Lever_OnCollision(AABB sender, CollisionEventArgs e)
		{
			if (e.CollidedWith is Character)
			{
				SwitchState = true;
				Counter = TimeSpan.Zero;

				SwitchedOn?.Invoke(this);
				Toggled?.Invoke(this);
			}
		}

		public override void Update(GameTime gameTime)
		{
			if (SwitchState)
			{
				Counter += gameTime.ElapsedGameTime;

				if (Counter > StayOn)
				{
					SwitchState = false;

					SwitchedOff?.Invoke(this);
					Toggled?.Invoke(this);
				}
			}
		}

		public override void Draw(GameTime gameTime)
		{
			if (SwitchState)
			{
				sprite.SourceRectangle.X = 32;
			}
			else
			{
				sprite.SourceRectangle.X = 0;
			}

			sprite.DestinationRectangle = Rectangle;
			sprite.Draw(gameTime);
		}
	}
}
