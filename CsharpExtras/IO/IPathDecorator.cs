using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpExtras.IO
{
    //TODO: Add decoration logic. Right now this interface/class just forwards from System.IO.Path.
    /// <summary>
    /// Provides extra methods on top of the methods in System.IO.Path.
    /// Also provides alternative error-handling to System.IO.Path.
    /// </summary>
    public interface IPathDecorator
    {
        string GetExtension(string path);

        string GetFileName(string path);

        string GetDirectoryName(string path);

        string GetFileNameWithoutExtension(string path);
    }
}
