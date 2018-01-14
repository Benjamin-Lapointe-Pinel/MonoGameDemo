using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameClassLibrary
{
	public class DrawHelper
	{
		public static Texture2D Pixel { get; private set; }

		public DrawHelper(GraphicsDevice graphicsDevice)
		{
			if (Pixel == null)
			{
				Pixel = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
				Pixel.SetData(new[] { Color.White });
			}
		}
	}
}
