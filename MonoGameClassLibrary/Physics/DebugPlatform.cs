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

		public DebugPlatform(Game game, float x, float y, float width, float height, Color color)
			: base(game, x, y, width, height, true)
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
