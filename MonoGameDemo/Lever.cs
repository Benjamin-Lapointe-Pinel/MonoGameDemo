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
	public class Lever : CollisionBox
	{
		public delegate void SwitchedHandler(Lever sender, EventArgs e);
		public event SwitchedHandler SwitchedOn;
		public event SwitchedHandler SwitchedOff;

		protected Sprite sprite;
		public bool State { get; protected set; }

		public Lever(Game game, Point location, bool state = false)
			: base(game, new Rectangle(location, new Point(32, 32)), false, false, false)
		{
			State = state;
			sprite = new Sprite(Game, Game.Content.Load<Texture2D>("lever"));
			sprite.SourceRectangle.Width = 32;

			OnCollision += Lever_OnCollision;
		}

		private void Lever_OnCollision(CollisionBox sender, CollisionEventArgs e)
		{
			if ((!State) && (e.Box is Character))
			{
				State = true;
				SwitchedOn?.Invoke(this, EventArgs.Empty);
			}
		}

		public override void Draw(GameTime gameTime)
		{
			if (State)
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
