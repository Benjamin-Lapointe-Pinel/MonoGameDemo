using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameClassLibrary.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameClassLibrary.Physics
{
	public class DebugPlatform : Box
	{
		public Color Color { get; protected set; }

		public DebugPlatform(Game game, Rectangle rectangle, Color color)
			: base(game, rectangle, true)
		{
			this.Color = color;
		}

		public override void Draw(GameTime gameTime)
		{
			SpriteBatch spriteBatch = Game.Services.GetService<SpriteBatch>();
			DrawHelper.DrawRectangle(spriteBatch, Rectangle, Color);
		}
	}
}
