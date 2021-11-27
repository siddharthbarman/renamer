using System;
using System.Text;
using System.Linq;

namespace SByteStream.Renamer
{
	public static class StringHelpers
	{
		public static string ReplacePositionally(this string str, string stringToFind, int replaceWithStart, int replaceWithLength)
		{
			if (replaceWithLength == 0)
			{
				throw new ArgumentOutOfRangeException("replaceWithLength must be greater than zero");
			}

			if (replaceWithStart > str.Length - 1)
			{
				throw new ArgumentOutOfRangeException("replaceWithStart must not be greater than or equal to the length of the string");
			}

			if (replaceWithStart < 0)
			{
				if (replaceWithStart < (str.Length * -1))
				{
					throw new ArgumentOutOfRangeException(String.Format("Invalid value for replaceWithStart: {0}", replaceWithStart));
				}
				else
				{
					replaceWithStart = str.Length + replaceWithStart;
				}
			}

			string replaceWith = str.Substring(replaceWithStart, replaceWithLength);
			return str.Replace(stringToFind, replaceWith);
		}				

		public static string ReplacePositionally(this string str, int start, int length, int replaceWithStart, int replaceWithLength)
		{
			if (replaceWithLength == 0)
			{
				throw new ArgumentOutOfRangeException("replaceWithLength must be greater than zero");
			}

			replaceWithStart = GetCustomStartIndex(str, replaceWithStart);			
			string replaceWith = str.Substring(replaceWithStart, replaceWithLength);
			
			return ReplacePositionally(str, start, length, replaceWith);
		}

		public static string ReplacePositionally(this string str, int start, int length, string replaceWith)
		{
			if (length == 0)
			{
				throw new ArgumentOutOfRangeException("length must be greater than zero");
			}

			start = GetCustomStartIndex(str, start);			
			StringBuilder sb = new StringBuilder(str.Length + replaceWith.Length);

			int skipCount = 0;
			bool skip = false;
			for(int n = 0; n < str.Length; n++)
			{
				if (n == start)
				{
					skip = true;
					sb.Append(replaceWith);
				}

				if (skip == true)
				{
					skipCount++;
					if (skipCount > length)
					{
						skip = false;
					}
				}

				if (!skip)
				{
					sb.Append(str[n]);
				}
			}

			return sb.ToString();
		}

		public static string ReplacePositionally(this string str, int start, int length, Func<string, string> transform)
		{
			if (length == 0)
			{
				throw new ArgumentOutOfRangeException("length must be greater than zero");
			}

			start = GetCustomStartIndex(str, start);
			StringBuilder sb = new StringBuilder(str.Length);
			StringBuilder skippedPart = new StringBuilder();

			int skipCount = 0;
			bool skip = false;
			for (int n = 0; n < str.Length; n++)
			{
				if (n == start)
				{
					skip = true;					
				}

				if (skip == true)
				{					
					skipCount++;
					if (skipCount > length)
					{
						skip = false;
						string t = transform(skippedPart.ToString());
						sb.Append(t);
					}
					else
					{
						skippedPart.Append(str[n]);
					}
				}

				if (!skip)
				{
					sb.Append(str[n]);
				}
			}

			return sb.ToString();
		}

		public static int GetCustomStartIndex(this string str, int start)
		{
			if (start > str.Length - 1)
			{
				throw new ArgumentOutOfRangeException("start must not be greater than or equal to the length of the string");
			}

			if (start < 0)
			{
				if (start < (str.Length * -1))
				{
					throw new ArgumentOutOfRangeException(String.Format("Invalid value for start: {0}", start));
				}
				else
				{
					start = str.Length + start;
				}
			}

			return start;
		}

		public static string ToSentenceCase(this string str, char sentenceEnder = '.')
		{
			StringBuilder sb = new StringBuilder();
			bool nextIsStart = true;
			for (int n = 0; n < str.Length; n++)
			{
				if (str[n] == sentenceEnder)
				{
					sb.Append(str[n]);
					nextIsStart = true;
					continue;
				}

				if (nextIsStart)
				{
					sb.Append(str.Substring(n, 1).ToUpper());
					nextIsStart = false;
				}
				else
				{
					sb.Append(str[n]);
				}
			}
			return sb.ToString();
		}

		public static string ToTitleCase(this string str, char[] wordDelimiters = null)
		{
			if (wordDelimiters == null)
			{
				wordDelimiters = DEFAULT_WORD_DELIMITERS;
			}

			StringBuilder sb = new StringBuilder();
			bool nextIsStart = true;
			
			for (int n = 0; n < str.Length; n++)
			{
				if (wordDelimiters.Any(d => d == str[n]))
				{
					sb.Append(str[n]);
					nextIsStart = true;
					continue;
				}

				if (nextIsStart)
				{
					sb.Append(str.Substring(n, 1).ToUpper());
					nextIsStart = false;
				}
				else
				{
					sb.Append(str[n]);
				}
			}
			return sb.ToString();
		}

		public static readonly char[] DEFAULT_WORD_DELIMITERS = new char[] { ' ', '_', '.', ',', ';', ':' };
	}
}
