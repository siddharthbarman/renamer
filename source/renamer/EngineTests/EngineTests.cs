using NUnit.Framework;
using SByteStream.Renamer;
using SByteStream.Renamer.Model;
using System.Collections.Generic;

namespace EngineTests
{
	public class Tests
	{
		[SetUp]
		public void Setup()
		{
		}

		[Test]
		public void ReplaceSingleLiternalWithLiteral()
		{
			string config = @"[
			{
				""Type"":""Replace"",
				""What"":{
					""Type"":""Literal"",
					""Value"":""foo""
				},
				""With"":{
					""Type"":""Literal"",
					""Value"":""bar""
				}
			}]";

			IList<RenameAction> actions = Parser.ParseJson(config);
			Engine engine = new Engine(actions, true);
			Assert.AreEqual ("bar.txt", engine.Rename(@"foo.txt").Item1);
		}

		[Test]
		public void ReplaceSingleLiternalWithPositional()
		{
			string config = @"[
			{
				""Type"":""Replace"",
				""What"":{
					""Type"":""Literal"",
					""Value"":""foo""
				},
				""With"":{
					""Type"":""Positional"",
					""Position"": { 
						""Start"": ""1"",
						""Length"": ""1""
					}
				}
			}]";

			IList<RenameAction> actions = Parser.ParseJson(config);
			Engine engine = new Engine(actions, true);
			Assert.AreEqual("o.txt", engine.Rename(@"foo.txt").Item1);
		}

		[Test]
		public void ReplaceSingleLiternalWithUCase()
		{
			string config = @"[
			{
				""Type"":""Replace"",
				""What"":{
					""Type"":""Literal"",
					""Value"":""foo""
				},
				""With"":{
					""Type"":""Transform"",
					""Value"":""ucase""
				}
			}]";

			IList<RenameAction> actions = Parser.ParseJson(config);
			Engine engine = new Engine(actions, true);
			Assert.AreEqual("FOO.txt", engine.Rename(@"foo.txt").Item1);
		}

		[Test]
		public void ReplaceMultipleLiternalWithLiteral()
		{
			string config = @"[
			{
				""Type"":""Replace"",
				""What"":{
					""Type"":""Literal"",
					""Value"":""moon""
				},
				""With"":{
					""Type"":""Literal"",
					""Value"":""sun""
				}
			},
			{
				""Type"":""Replace"",
				""What"":{
					""Type"":""Literal"",
					""Value"":""over""
				},
				""With"":{
					""Type"":""Literal"",
					""Value"":""under""
				}
			}
			]";

			IList<RenameAction> actions = Parser.ParseJson(config);
			Engine engine = new Engine(actions, true);
			Assert.AreEqual("under the sun.txt", engine.Rename(@"over the moon.txt").Item1);
		}

		[Test]
		public void ReplaceSinglePositionalWithLiteral()
		{
			string config = @"[
			{
				""Type"":""Replace"",
				""What"":{
					""Type"":""Positional"",
					""Position"": { 
						""Start"": ""0"",
						""Length"": ""1""
					}
				},
				""With"":{
					""Type"":""Literal"",
					""Value"":""b""
				}
			}]";

			IList<RenameAction> actions = Parser.ParseJson(config);
			Engine engine = new Engine(actions, true);
			Assert.AreEqual("boo.txt", engine.Rename(@"foo.txt").Item1);
		}

		[Test]
		public void ReplaceSinglePositionalWithPositional()
		{
			string config = @"[
			{
				""Type"":""Replace"",
				""What"":{
					""Type"":""Positional"",
					""Position"": { 
						""Start"": ""0"",
						""Length"": ""1""
					}
				},
				""With"":{
					""Type"":""Positional"",
					""Position"": { 
						""Start"": ""2"",
						""Length"": ""1""
					}
				}
			}]";

			IList<RenameAction> actions = Parser.ParseJson(config);
			Engine engine = new Engine(actions, true);
			Assert.AreEqual("rar.txt", engine.Rename(@"far.txt").Item1);
		}

		[Test]
		public void ReplaceSinglePositionalWithUCase()
		{
			string config = @"[
			{
				""Type"":""Replace"",
				""What"":{
					""Type"":""Positional"",
					""Position"": { 
						""Start"": ""1"",
						""Length"": ""1""
					}
				},
				""With"":{
					""Type"":""Transform"",					
					""Value"": ""ucase""					
				}
			}]";

			IList<RenameAction> actions = Parser.ParseJson(config);
			Engine engine = new Engine(actions, true);
			Assert.AreEqual("fOo.txt", engine.Rename(@"foo.txt").Item1);
		}

		[Test]
		public void ReplaceSinglePositionalFromEndWithLiteral()
		{
			string config = @"[
			{
				""Type"":""Replace"",
				""What"":{
					""Type"":""Positional"",
					""Position"": { 
						""Start"": ""-5"",
						""Length"": ""1""
					}
				},
				""With"":{
					""Type"":""Literal"",
					""Value"":""r""
				}
			}]";

			IList<RenameAction> actions = Parser.ParseJson(config);
			Engine engine = new Engine(actions, true, true);
			Assert.AreEqual("for.txt", engine.Rename(@"foo.txt").Item1);
		}

		[Test]
		public void ReplaceMultiplePositionalWithLiteral()
		{
			string config = @"[
				{
					""Type"":""Replace"",
					""What"":{
						""Type"":""Positional"",
						""Position"": { 
							""Start"": ""0"",
							""Length"": ""1""
						}
					},
					""With"":{
						""Type"":""Literal"",
						""Value"":""b""
					}
				},
				{
					""Type"":""Replace"",
					""What"":{
						""Type"":""Positional"",
						""Position"": { 
							""Start"": ""2"",
							""Length"": ""1""
						}
					},
					""With"":{
						""Type"":""Literal"",
						""Value"":""z""
					}
				}
			]";

			IList<RenameAction> actions = Parser.ParseJson(config);
			Engine engine = new Engine(actions, true);
			Assert.AreEqual("boz.txt", engine.Rename(@"foo.txt").Item1);
		}

		[Test]
		public void InsertSingleLiteralAtStart()
		{
			string config = @"[
			{
				""Type"":""Insert"",
				""At"": ""0"",
				""What"":{
					""Type"":""Literal"",
					""Value"":""foo""
				}
			}]";

			IList<RenameAction> actions = Parser.ParseJson(config);
			Engine engine = new Engine(actions, true);
			Assert.AreEqual("foobar.txt", engine.Rename(@"bar.txt").Item1);
		}

		[Test]
		public void AppendSingleLiteralAtEnd()
		{
			string config = @"[
			{
				""Type"":""Append"",				
				""What"":{
					""Type"":""Literal"",
					""Value"":""bar""
				}
			}]";

			IList<RenameAction> actions = Parser.ParseJson(config);
			Engine engine = new Engine(actions, true);
			Assert.AreEqual("foobar.txt", engine.Rename(@"foo.txt").Item1);

			engine.ConsiderExtension = true;
			Assert.AreEqual("foo.txtbar", engine.Rename(@"foo.txt").Item1);
		}

		[Test]
		public void AppendSinglePositionalAtEnd()
		{
			string config = @"[
			{
				""Type"":""Append"",				
				""What"":{
					""Type"" : ""Positional"",
					""Position"": { 
						""Start"" : ""1"",
						""Length"": ""2""
					}
				}
			}]";

			IList<RenameAction> actions = Parser.ParseJson(config);
			Engine engine = new Engine(actions, true);
			Assert.AreEqual("foooo.txt", engine.Rename(@"foo.txt").Item1);

			engine.ConsiderExtension = true;
			Assert.AreEqual("foo.txtoo", engine.Rename(@"foo.txt").Item1);
		}
	}
}