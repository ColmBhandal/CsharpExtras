using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpExtras.IO
{
    public interface IPathFacade
    {
        string GetExtension(string path);

        string GetFileName(string path);

        string GetDirectoryName(string path);

        string GetFileNameWithoutExtension(string path);
    }
}
