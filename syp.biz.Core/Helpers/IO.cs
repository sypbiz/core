using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using syp.biz.Core.Extensions;

namespace syp.biz.Core.Helpers
{
	/// <summary>
	/// A collection of I/O related methods.
	/// </summary>
	public static class IO
	{
		/// <summary>
		/// Returns the names of files (including their paths) in the specified directory and its subdirectories.
		/// </summary>
		/// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> of the full names (including paths) of files in the specified path and its subdirectories.</returns>
		public static IEnumerable<string> GetFilesRecursive(string path)
		{
			var files = new List<string>(Directory.GetFiles(path));
			Directory.GetDirectories(path).ForEach(dir => files.AddRange(GetFilesRecursive(dir)));
			return files;
		}

		/// <summary>
		/// Returns the names of files (including their paths) in the specified directory and its subdirectories, unless defined in the ignore file.
		/// </summary>
		/// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
		/// <param name="ignoreFileName">The name of the ignore file to look for in each directory.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> of the full names (including paths) of files in the specified path and its subdirectories.</returns>
		public static IEnumerable<string> GetFilesRecursive(string path, string ignoreFileName)
		{
			var ignores = GetIgnoreRegexes(Path.Combine(path, ignoreFileName));
			var files = Directory.GetFiles(path)
				.Where(f => !File.Exists($"{f}{ignoreFileName}"))
				.Where(f => ignores.All(i => !i.IsMatch(f)))
				.ToList();
			Directory.GetDirectories(path)
				.Where(d => !File.Exists($"{d}{ignoreFileName}"))
				.Where(d => ignores.All(i => !i.IsMatch(d)))
				.ForEach(dir => files.AddRange(GetFilesRecursive(dir, ignoreFileName).Where(f => ignores.All(i => !i.IsMatch(f)))));
			return files;
		}

		private static IEnumerable<Regex> GetIgnoreRegexes(string ignoreFilePath)
		{
			try
			{
				if (!File.Exists(ignoreFilePath)) return new Regex[0];
				var lines = File.ReadAllLines(ignoreFilePath);
				var regexes = lines
					.Where(l => !l.IsNullOrWhiteSpace())
					.Select(l => Try.Catch<Exception, Regex>(() => new Regex(l), ex => { Debug.WriteLine($"{nameof(GetIgnoreRegexes)}: {ex}"); return null; }))
					.NotNulls();
				return new HashSet<Regex>(regexes);
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"{nameof(GetIgnoreRegexes)}: {ex}");
				return new Regex[0];
			}
		}
	}
}
