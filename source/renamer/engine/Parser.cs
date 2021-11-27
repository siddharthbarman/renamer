using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using SByteStream.Renamer.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace SByteStream.Renamer
{
	public class Parser
	{
		public static string ToJson(IEnumerable<RenameAction> actions)
		{
			return JsonConvert.SerializeObject(actions, new StringEnumConverter());
		}

		public static IList<RenameAction> ParseJson(string json)
		{
			return JsonConvert.DeserializeObject<List<RenameAction>>(json);
		}
	}
}
