using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameClassLibrary.Animation
{
	public class Frame
	{
		public TimeSpan TimeSpan;

		public Frame(TimeSpan timeSpan)
		{
			this.TimeSpan = timeSpan;
		}
	}
}
