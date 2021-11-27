using SByteStream.Renamer.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SByteStream.Renamer
{
	public class Engine
	{
		public Engine(IEnumerable<RenameAction> actions, bool reportMode = false, bool considerExtension = false)
		{
			m_actions = actions;
			ReportMode = reportMode;
			ConsiderExtension = considerExtension;
		}

		public bool ReportMode 
		{ 
			get => m_reportMode; 
			set => m_reportMode = value; 
		}

		public bool ConsiderExtension { get => m_considerExtension; set => m_considerExtension = value; }

		public Tuple<string, EngineResult> Rename(string filePath)
		{
			string fileName =  ConsiderExtension ? Path.GetFileName(filePath) : Path.GetFileNameWithoutExtension(filePath);
			string pathOnly = Path.GetDirectoryName(filePath);
			string newName = fileName;

			foreach(RenameAction action in m_actions)
			{
				if (action.Type == ActionTypes.Replace)
				{
					newName = GetReplace(newName, action);
				}
				else if (action.Type == ActionTypes.Insert)
				{
					newName = GetInsert(newName, action);
				}
				else if (action.Type == ActionTypes.Append)
				{
					newName = GetAppend(newName, action);
				}
			}

			string newFilePath = Path.Combine(pathOnly, newName);
			if (!ConsiderExtension)
            {
				newFilePath = newFilePath + Path.GetExtension(filePath);
            }
			
			if (newFilePath.Equals(filePath))
			{
				return new Tuple<string, EngineResult>(newFilePath, EngineResult.NoChange);
			}
			else if (File.Exists(newFilePath))
			{
				return new Tuple<string, EngineResult>(newFilePath, EngineResult.Exists);
			}
			else
			{
				if (!ReportMode)
				{
					File.Move(filePath, newFilePath);
				}
				return new Tuple<string, EngineResult>(newFilePath, EngineResult.Renamed);
			}
		}

		private string GetReplace(string filePath, RenameAction action)
		{
			if (action.What.Type == WhatTypes.Literal)
			{
				if (action.With.Type == WithTypes.Literal)
				{
					return filePath.Replace(action.What.Value, action.With.Value);
				}
				else if (action.With.Type == WithTypes.Positional)
				{
					if (action.With.Position == null)
					{
						throw new InvalidOperationException("Position not set when with type is Positional");
					}
					return filePath.ReplacePositionally(action.What.Value, action.With.Position.Start, action.With.Position.Length);
				}
				else if (action.With.Type == WithTypes.Transform)
				{
					if (!IsValidTransform(action.With.Value))
					{
						throw new InvalidOperationException(string.Format("Invalid transform value: {0}", action.With.Value));
					}
					return filePath.Replace(action.What.Value, GetTransformedString(action.What.Value, action.With.Value));
				}
			}
			else if (action.What.Type == WhatTypes.Positional)
			{
				if (action.What.Position == null)
				{
					throw new InvalidOperationException("Position not set when type is Positional");
				}
				
				if (action.With.Type == WithTypes.Literal)
				{
					return filePath.ReplacePositionally(action.What.Position.Start, action.What.Position.Length, action.With.Value);
				}
				else if (action.With.Type == WithTypes.Positional)
				{
					if (action.With.Position == null)
					{
						throw new InvalidOperationException("Position not set when with type is Positional");
					}
					return filePath.ReplacePositionally(action.What.Position.Start, action.What.Position.Length, action.With.Position.Start, action.With.Position.Length);
				}
				else if (action.With.Type == WithTypes.Transform)
				{
					if (!IsValidTransform(action.With.Value))
					{
						throw new InvalidOperationException(string.Format("Invalid transform value: {0}", action.With.Value));
					}
					return filePath.ReplacePositionally(action.What.Position.Start, action.What.Position.Length, s => GetTransformedString(s, action.With.Value));
				}
			}
			return filePath;
		}

		private string GetInsert(string filePath, RenameAction action)
		{
			if (!action.At.HasValue)
			{
				throw new InvalidOperationException("Insert operation requires 'At' to be specified");
			}

			if (action.What == null)
			{
				throw new InvalidOperationException("'What' has not been specified");
			}

			if (action.What.Type == WhatTypes.Literal)
			{				
				return filePath.Insert(filePath.GetCustomStartIndex(action.At.Value), action.What.Value);
			}
			else if (action.What.Type == WhatTypes.Positional)
			{
				int start = filePath.GetCustomStartIndex(action.What.Position.Start);
				string insertStr = filePath.Substring(start, action.What.Position.Length);
				return filePath.Insert(filePath.GetCustomStartIndex(action.At.Value), insertStr);
			}
			else
			{
				return filePath;
			}
		}

		private string GetAppend(string filePath, RenameAction action)
		{
			if (action.What.Type == WhatTypes.Literal)
			{
				return filePath + action.What.Value;
			}
			else if (action.What.Type == WhatTypes.Positional)
			{
				int start = filePath.GetCustomStartIndex(action.What.Position.Start);
				string appendStr = filePath.Substring(start, action.What.Position.Length);
				return filePath + appendStr;
			}
			else
			{
				return filePath;
			}
		}

		public bool IsValidTransform(string transform)
		{
			if (transform.Equals("ucase", StringComparison.CurrentCulture))
				return true;
			else if (transform.Equals("lcase", StringComparison.CurrentCulture))
				return true;
			else if (transform.Equals("scase", StringComparison.CurrentCulture))
				return true;
			else if (transform.Equals("tcase", StringComparison.CurrentCulture))
				return true;
			else
				return false;
		}

		public string GetTransformedString(string str, string transform)
		{
			if (transform.Equals("ucase", StringComparison.CurrentCulture))
				return str.ToUpper();
			else if (transform.Equals("lcase", StringComparison.CurrentCulture))
				return str.ToLower();
			else if (transform.Equals("scase", StringComparison.CurrentCulture))
				return str.ToSentenceCase();
			else if (transform.Equals("tcase", StringComparison.CurrentCulture))
				return str.ToTitleCase();
			else
				return str;
		}

		private IEnumerable<RenameAction> m_actions;
		private bool m_reportMode;
		private bool m_considerExtension;
    }
}
