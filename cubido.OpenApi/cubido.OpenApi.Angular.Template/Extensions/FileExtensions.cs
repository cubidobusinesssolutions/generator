using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cubido.OpenApi.Angular.Template.Extensions
{
    public static class FileExtensions
    {
        /// <summary>Creates a new file, writes the specified string to the file, and then closes the file. If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="filePath">The file to write to.</param>
        /// <param name="contents">The string to write to the file.</param>
        /// <returns></returns>
        public static async Task WriteAllTextAsync(string path, string contents)
        {
            using (var writer = new StreamWriter(path))
            {
                await writer.WriteAsync(contents);
            };
        }

        /// <summary>
        /// Copies an existing file to a new file. Overwriting a file of the same name is allowed.
        /// </summary>
        /// <param name="sourceFileName">The file to copy.</param>
        /// <param name="destFileName">The name of the destination file. This cannot be a directory.</param>
        /// <param name="overwrite"></param><c>true</c> if the destination file can be overwritten; otherwise, <c>false</c>.
        public static async Task CopyAsync(string sourceFileName, string destFileName, bool overwrite)
        {
            using (var source = File.OpenRead(sourceFileName))
            using (var destination = File.Open(destFileName, overwrite ? FileMode.Create: FileMode.CreateNew, FileAccess.Write))
            {
                await source.CopyToAsync(destination);
            }
        }
    }
}
