using System.Collections.Generic;
using System.IO;
using syp.biz.Core.Extensions;

namespace syp.biz.Core.Helpers
{
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
	}
}
