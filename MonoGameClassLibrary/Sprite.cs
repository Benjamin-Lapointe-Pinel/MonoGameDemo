using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameClassLibrary
{
	public class Sprite : EntityManager.Drawable
	{
		public static readonly int BACKGROUND = 0;
		public static readonly int FOREGROUND = int.MaxValue;

		public bool Visible;
		public Rectangle DestinationRectangle;
		public Rectangle SourceRectangle;
		public Color Color;
		public float Rotation;
		public Vector2 Origin;
		public SpriteEffects SpriteEffects;
		protected Texture2D texture2D;

		public Sprite(Texture2D texture2D)
		{
			this.texture2D = texture2D;
			Visible = true;
			DestinationRectangle = texture2D.Bounds;
			SourceRectangle = DestinationRectangle;
			Color = Color.White;
			Rotation = 0;
			Origin = new Vector2(0, 0);
			SpriteEffects = SpriteEffects.None;
			DrawOrder = 0;
		}

		public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
		{
			if (Visible)
			{
				spriteBatch.Draw(texture2D, DestinationRectangle, SourceRectangle, Color, Rotation, Origin, SpriteEffects, DrawOrder);
			}
		}
	}
}
