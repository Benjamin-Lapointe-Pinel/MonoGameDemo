using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
		public static SpriteFont SpriteFont { get; private set; }

		public static void Init(GraphicsDevice GraphicsDevice, ContentManager Content)
		{
			Pixel = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
			Pixel.SetData(new[] { Color.White });

			SpriteFont = Content.Load<SpriteFont>("default_spritefont");
		}

		public static void DrawText(SpriteBatch spriteBatch, string text, Vector2 position, Color color)
		{
			spriteBatch.DrawString(SpriteFont, text, position, color);
		}

		public static void DrawRectangle(SpriteBatch spriteBatch, Rectangle rectangle, Color color)
		{
			spriteBatch.Draw(Pixel, rectangle, color);
		}

		public static void DrawOutline(SpriteBatch spriteBatch, Rectangle rectangle, Color color)
		{
			spriteBatch.Draw(Pixel, new Rectangle(rectangle.Left, rectangle.Top, rectangle.Width, 1), color);
			spriteBatch.Draw(Pixel, new Rectangle(rectangle.Left, rectangle.Top, 1, rectangle.Height), color);
			spriteBatch.Draw(Pixel, new Rectangle(rectangle.Left, rectangle.Bottom - 1, rectangle.Width, 1), color);
			spriteBatch.Draw(Pixel, new Rectangle(rectangle.Right - 1, rectangle.Top, 1, rectangle.Height), color);
		}

		//https://gamedev.stackexchange.com/a/44016
		public static void DrawLine(SpriteBatch spriteBatch, Point start, Point end, Color color)
		{
			Vector2 edge = end.ToVector2() - start.ToVector2();
			float angle = (float)Math.Atan2(edge.Y, edge.X);
			Rectangle line = new Rectangle(start.X, start.Y, (int)edge.Length(), 1);

			spriteBatch.Draw(Pixel, line, null, color, angle, new Vector2(0, 0), SpriteEffects.None, 0);
		}
	}
}
