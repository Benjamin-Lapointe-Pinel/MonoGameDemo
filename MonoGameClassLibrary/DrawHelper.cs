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
		public static SpriteBatch DebugSpriteBatch { get; private set; }
		public static Texture2D Pixel { get; private set; }
		public static SpriteFont spriteFont { get; private set; }

		public static void Init(GraphicsDevice GraphicsDevice, ContentManager Content)
		{
			DebugSpriteBatch = new SpriteBatch(GraphicsDevice);

			Pixel = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
			Pixel.SetData(new[] { Color.White });

			spriteFont = Content.Load<SpriteFont>("default_spritefont");
		}

		public static void DrawDebugText(string text, Vector2 position, Color color)
		{
			DebugSpriteBatch.Begin();
			DrawRectangle(DebugSpriteBatch, new Rectangle(0, 0, 400, 100), new Color(Color.Gray, 0.25f));
			DebugSpriteBatch.DrawString(spriteFont, text, position, color);
			DebugSpriteBatch.End();
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

			spriteBatch.Draw(Pixel, line, null, Color.Red, angle, new Vector2(0, 0), SpriteEffects.None, 0);
		}
	}
}
