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
	public class Door : Box
	{
		Sprite sprite;

		public Door(Game game, Point location)
			: base(game, new Rectangle(location, new Point(64, 64)), true, false)
		{
			sprite = new Sprite(Game, Game.Content.Load<Texture2D>("door"));
			sprite.SourceRectangle.Width = 64;
		}

		public void Open(object sender, EventArgs e)
		{
			Solid = false;
			sprite.SourceRectangle.X = 64;
		}

		public override void Draw(GameTime gameTime)
		{
			sprite.DestinationRectangle = Rectangle;
			sprite.Draw(gameTime);
		}
	}
}
