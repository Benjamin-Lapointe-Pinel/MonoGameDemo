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

		public DebugPlatform(Rectangle rectangle, Color color)
			: base(rectangle, true)
		{
			this.Color = color;
		}

		public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
		{
			DrawHelper.DrawRectangle(spriteBatch, Rectangle, Color);
		}
	}
}
