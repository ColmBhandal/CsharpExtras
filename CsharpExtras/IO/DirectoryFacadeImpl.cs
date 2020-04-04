using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CsharpExtras.Log.StaticLogManager;

namespace IO.Impl
{
    class DirectoryFacadeImplIDirectoryFacade
    {
        public DirectoryInfo CreateDirectory(string path)
        {
            return Directory.CreateDirectory(path);
        }

        public void CreateIfNonExistent(string path)
        {
            if (!Exists(path))
            {
                Logger.DebugFormat("Creating directory {0}", path);
                CreateDirectory(path);
            }
            Logger.DebugFormat("Directory {0} already exists. Create did nothing.", path);
        }

        public bool Exists(string path)
        {
            return Directory.Exists(path);
        }

        public string GetParentDirectoryOrThisDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                return path;
            }
            else
            {
                return Path.GetDirectoryName(path);
            }
        }

        public string GetRelativeDifferenceBetweenPaths(string pathToRootDir, string pathToNestedDir)
        {
            if (pathToNestedDir.StartsWith(pathToRootDir))
            {
                string relativePath = pathToNestedDir.Remove(0, pathToRootDir.Length);
                if (relativePath[0] == Path.DirectorySeparatorChar)
                {
                    relativePath = relativePath.Remove(0, 1);
                }
                return relativePath;
            }
            else
            {
                throw new ArgumentException(string.Format(
                    "The directory {0} does not appear to be nested under the directory {1}",
                    pathToRootDir, pathToNestedDir));
            }
        }
    }
}
