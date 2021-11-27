using SByteStream.Renamer;
using SByteStream.Utils;
using System;
using System.IO;

namespace app
{
	class Program
	{
		static void Help()
		{
			Console.WriteLine(@"
  _____                                      
 |  __ \                                     
 | |__) |___ _ __   __ _ _ __ ___   ___ _ __ 
 |  _  // _ \ '_ \ / _` | '_ ` _ \ / _ \ '__|
 | | \ \  __/ | | | (_| | | | | | |  __/ |   
 |_|  \_\___|_| |_|\__,_|_| |_| |_|\___|_|  v1
(c) 2021, Siddharth Barman			
			");
			Console.WriteLine("Rename files with ease!");
			Console.WriteLine("Syntax:");
			Console.WriteLine("renamer [-t] [-r] [-e] -c <rules.json> <filepattern>");
			Console.WriteLine("renamer -c trailing-spaces.json c:\\temp\\*.png");
			Console.WriteLine("-t    : Optional, runs the utility in test mode. Files are not renamed. Its a good way to check the effectiveness of the renaming rules.");
			Console.WriteLine("-r    : Optional. Use it to recursively rename files in subfolders.");
			Console.WriteLine("-e    : Optional, specifies rename to consider filename + extension. Default is false");
			Console.WriteLine("-c    : Required, specifies the rule json file.");			
			Console.WriteLine("filepattern specifies the files that need to be renamed e.g. c:\\temp\\myfolder\\*.doc. It is not necessary that all files that match the pattern will be renamed, that depends on the reanme rules.");
		}

		static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				Help();
				return;
			}

			CmdLine cmd = new CmdLine(args);
			
			string conf = cmd.GetFlagValue(FLAG_CONFIG);
			if (conf == null)
			{
				Console.WriteLine("Configuration file not specified. Use the -c option.");
				return;
			}

			string pattern = cmd.GetPositionalArgument(0);
			if (pattern == null)
			{
				Console.WriteLine("File/pattern not specified. Use the -p option.");
				return;
			}

			bool testMode = cmd.IsFlagPresent(FLAG_TEST);
			bool recurse = cmd.IsFlagPresent(FLAG_RECURSE);
			bool extension = cmd.IsFlagPresent(FLAG_EXTENSION);

			Engine engine = new Engine(Parser.ParseJson(File.ReadAllText(conf)), testMode, extension);
			string directory = Path.GetDirectoryName(pattern);
			string patternOnly = Path.GetFileName(pattern);

			if (testMode)
			{
				Console.WriteLine("Running in TEST mode, no rename operations will be performed.");
			}
			else
			{
				Console.WriteLine("Not running in TEST mode, hope you know what you are doing.");
			}

			Walk(directory, patternOnly, recurse, engine);
		}

		private static void Walk(string directory, string pattern, bool recurse, Engine engine)
		{
			foreach(string file in Directory.EnumerateFiles(directory, pattern))
			{	
				var result = engine.Rename(file);
				if (result.Item2 != EngineResult.NoChange)
				{
					Console.WriteLine("{0} -> {1} [{2}]", file, result.Item1, result.Item2);
				}
			}

			if (recurse)
			{
				foreach(string folder in Directory.EnumerateDirectories(directory, pattern))
				{
					Walk(folder, pattern, recurse, engine);
				}
			}
		}

		const string FLAG_CONFIG = "c";		
		const string FLAG_TEST = "t";
		const string FLAG_RECURSE = "r";
		const string FLAG_EXTENSION = "n";
	}
}
