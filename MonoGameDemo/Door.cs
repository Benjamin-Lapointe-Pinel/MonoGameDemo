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
	public class Door : AABB
	{
		Sprite sprite;

		public Door(Game game, Point location)
			: base(game, new Rectangle(location, new Point(64, 64)), true)
		{
			sprite = new Sprite(Game, Game.Content.Load<Texture2D>("door"));
			sprite.DestinationRectangle.Width = 64;
			sprite.SourceRectangle.Width = 64;
			Close(this);
		}

		public void Open(object sender)
		{
			Solid = false;
			Width = 53;
			sprite.SourceRectangle.X = 64;
		}

		public void Close(object sender)
		{
			Solid = true;
			Width = 4;
			sprite.SourceRectangle.X = 0;
		}

		public override void Draw(GameTime gameTime)
		{
			sprite.DestinationRectangle.Location = Rectangle.Location;
			sprite.Draw(gameTime);
		}
	}
}
