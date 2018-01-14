using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameClassLibrary.Animation
{
	public class Cycle
	{
		public bool IsLooping;
		public bool IsOver;
		public int FrameIndex { get; protected set; }
		protected TimeSpan elapsedTime;
		protected Frame[] frames;
		public Frame CurrentFrame
		{
			get
			{
				return frames[FrameIndex];
			}
		}

		public Cycle(Frame[] frames, bool looping = true, int frameIndex = 0)
		{
			this.frames = frames;
			this.FrameIndex = frameIndex;
			this.IsLooping = looping;

			IsOver = false;
			elapsedTime = TimeSpan.Zero;
		}

		public void Update(GameTime gameTime)
		{
			if (!IsOver)
			{
				elapsedTime += gameTime.ElapsedGameTime;
				while (elapsedTime > CurrentFrame.TimeSpan)
				{
					elapsedTime -= CurrentFrame.TimeSpan;
					FrameIndex++;
					if (FrameIndex > frames.Length - 1)
					{
						if (IsLooping)
						{
							FrameIndex = 0;
						}
						else
						{
							FrameIndex = frames.Length - 1;
							IsOver = true;
						}
					}
				}
			}
		}
	}
}
