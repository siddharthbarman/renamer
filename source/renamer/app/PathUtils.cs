using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;

namespace SByteStream.Samples.Grpc.Stocks.Common {
	public class PathUtils {
		public static string GetAppPath(string path) {
			return Path.Combine(
				Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), 
				path);
		}
	}
}
