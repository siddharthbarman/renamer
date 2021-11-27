using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace SByteStream.Renamer.Model
{
	public class What
	{
		public What() 
		{ 
		}

		public What(WhatTypes type, string value)
		{
			this.Type = type;
			this.Value = value;
		}

		[JsonConverter(typeof(StringEnumConverter))]
		public WhatTypes Type { get; set; }
		
		public string Value { get; set; }

		public Position Position { get; set; }
	}
}
