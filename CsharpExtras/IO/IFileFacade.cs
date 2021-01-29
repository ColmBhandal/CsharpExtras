using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpExtras.IO
{
    public interface IFileFacade
    {
        void Create(string path);

        string[] ReadAllLines(string path);

        string ReadAllText(string path);

        void WriteAllLines(string path, IEnumerable<string> contents);

        void WriteAllText(string path, string contents);

        /// <summary>
        /// Determines whether the specified file exists.
        /// </summary>
        /// <param name="path">The path to check.</param>
        /// <returns>True if the path is valid, the file is found, and the user has read access to the file. False otherwise.</returns>
        bool Exists(string path);
    }
}
