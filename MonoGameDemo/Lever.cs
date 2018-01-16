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
		public delegate void ActionHandler();
		public event ActionHandler OnAction;

		protected Sprite sprite;

		public Lever(Game game, Point location)
			: base(game, new Rectangle(location, new Point(32, 32)), false, false, false)
		{
			sprite = new Sprite(Game, Game.Content.Load<Texture2D>("lever"));
			sprite.SourceRectangle.Width = 32;

			OnCollision += Lever_OnCollision;
		}

		private void Lever_OnCollision(CollisionBox sender, CollisionEventArgs e)
		{
			if (e.Box is Character)
			{
				sprite.SourceRectangle.X = 32;

				OnAction?.Invoke();
			}
		}

		public override void Draw(GameTime gameTime)
		{
			sprite.DestinationRectangle = Rectangle;
			sprite.Draw(gameTime);
		}
	}
}
