using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameClassLibrary.Animation
{
	public class AnimationSheet : Sprite
	{
		protected int cycleIndex;
		public int CycleIndex
		{
			get
			{
				return cycleIndex;
			}
			set
			{
				CurrentCycle.IsOver = false;
				cycleIndex = value % cycles.Length;
			}
		}
		protected Cycle[] cycles;
		public Cycle CurrentCycle { get { return cycles[CycleIndex]; } }
		public int FrameIndex { get { return CurrentCycle.FrameIndex; } }

		public AnimationSheet(Game game, Texture2D texture2D, Rectangle sourceRectangle, Cycle[] cycles, int cycleIndex = 0)
			: base(game, texture2D)
		{
			this.DestinationRectangle = sourceRectangle;
			this.SourceRectangle = sourceRectangle;
			this.cycles = cycles;

			CycleIndex = cycleIndex;
		}

		public override void Update(GameTime gameTime)
		{
			CurrentCycle.Update(gameTime);

			SourceRectangle.Y = CycleIndex * SourceRectangle.Height;
			SourceRectangle.X = CurrentCycle.FrameIndex * SourceRectangle.Width;
		}
	}
}
