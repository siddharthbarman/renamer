using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace SByteStream.Renamer.Model
{
	public class With
	{
		public With()
		{
		}

		public With(WithTypes type, string value)
		{
			this.Type = type;
			this.Value = value;
		}

		[JsonConverter(typeof(StringEnumConverter))]
		public WithTypes Type { get; set; }
		
		public string Value { get; set; }

		public Position Position { get; set; }
	}
}
