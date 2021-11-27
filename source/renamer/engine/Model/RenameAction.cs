using System;
using System.Collections.Generic;
using System.Text;

namespace SByteStream.Renamer.Model
{
	public class RenameAction
	{
		public ActionTypes Type { get; set; }
		public What What { get; set; }
		public With With { get; set; }
		public int? At { get; set; }
	}
}
