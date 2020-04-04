using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO
{
    public interface IFileFacade
    {
        void Create(string path);

        string[] ReadAllLines(string path);

        string ReadAllText(string path);

        void WriteAllLines(string path, IEnumerable<string> contents);

        void WriteAllText(string path, string contents);
    }
}
