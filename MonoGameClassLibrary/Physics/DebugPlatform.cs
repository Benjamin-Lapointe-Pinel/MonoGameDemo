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
	public class DebugPlatform : EntityManager.Drawable
	{
		public AxisAlignedBoundingBox box { get; protected set; }

		public DebugPlatform(Rectangle rectangle)
		{
			this.box = new AxisAlignedBoundingBox(rectangle, true);
		}

		public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
		{
			spriteBatch.Draw(DrawHelper.Pixel, box.Rectangle, Color.Magenta);
		}
	}
}
