using System;
using System.Collections.Generic;
using System.Text;

namespace SByteStream.Renamer.Model
{
	public class Position
	{
		public Position()
		{
		}

		public Position(int start, int length)
		{
			Start = start;
			Length = length;
		}

		public int Start { get; set; }
		public int Length { get; set; }
	}
}
