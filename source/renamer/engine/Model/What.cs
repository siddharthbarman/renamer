using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace SByteStream.Renamer.Model
{
	public class What
	{
		public What() 
		{
			CompareType = ComparisonType.CaseSensitive;
		}

		public What(WhatTypes type, string value) : base()
		{
			this.Type = type;
			this.Value = value;
		}

		public What(WhatTypes type, string value, ComparisonType comparisonType) : base()
		{
			this.Type = type;
			this.Value = value;
			this.CompareType = comparisonType;
		}

		[JsonConverter(typeof(StringEnumConverter))]
		public WhatTypes Type { get; set; }
		
		public string Value { get; set; }

		public Position Position { get; set; }

		[JsonConverter(typeof(StringEnumConverter))]
		public ComparisonType CompareType { get; set; }
	}
}
