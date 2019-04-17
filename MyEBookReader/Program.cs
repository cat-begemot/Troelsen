using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;

namespace MyEBookReader
{
	class Program
	{
		private static String theEBook;

		static void Main(string[] args)
		{
			GetBook();
			Console.ReadLine();
		}

		internal static void GetBook()
		{
			WebClient wc = new WebClient();
			wc.DownloadStringCompleted += (s, eArgs) =>
				{
					theEBook = eArgs.Result;
					Console.WriteLine("Download complete.");
					GetStats();
				};

			wc.DownloadStringAsync(new Uri("http://www.gutenberg.org/files/74/74-0.txt"));
		}

		internal static void GetStats()
		{
			String[] words = theEBook.Split(new Char[] { ' ', '\u000A', ',', '.', ';', ':', '-', '?', '/' }, StringSplitOptions.RemoveEmptyEntries);

			String[] tenMostCommon = null;
			String longestWord = String.Empty;

			Parallel.Invoke(
				() =>
				{
					tenMostCommon = FindTenMostCommon(words);
				},
				() =>
				{
					longestWord = FindLongestWord(words);
				});

			StringBuilder bookStats = new StringBuilder("Ten Most Common Words are:\n");
			foreach (var s in tenMostCommon)
				bookStats.AppendLine(s);

			bookStats.AppendLine($"Longest word is: {longestWord}");
			bookStats.AppendLine();

			Console.WriteLine(bookStats.ToString(), "Book info");
		}

		private static String[] FindTenMostCommon(String[] words)
		{
			var result = from word in words
									 where word.Length > 6
									 group word by word into g
									 orderby g.Count() descending
									 select g.Key;
			string[] commonWords = (result.Take(10).ToArray());
			return commonWords;
		}

		private static String FindLongestWord(String[] words)
		{
			return (from w in words orderby w.Length descending select w).FirstOrDefault();
		}
	}
}
