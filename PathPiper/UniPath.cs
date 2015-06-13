using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace PathPiper
{
    public class UniPath {
        private const string _currentDirectory = ".";
        private const string _parentDirectory = "..";

        private readonly IReadOnlyCollection<string> _directories;

        protected UniPath(IReadOnlyCollection<string> directories)
        {
            _directories = directories ?? new ReadOnlyCollection<string>(new string[0]);
        }

        public static UniPath Parse(string path)
        {
            return Parse(path, EnvironmentPathStyle);
        }
        public static UniPath Parse(string path, PathStyle pathStyle)
        {
            throw new NotImplementedException();
        }

        public bool HasExtension
        {
            get { throw new NotImplementedException(); }
        }

        public bool Extension
        {
            get { throw new NotImplementedException(); }
        }

        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        public UniPath Normalize()
        {
            throw new NotImplementedException();
        }

        /// <summary>Changes the extension of a path.</summary>
        /// <param name="newExtensionWithDot">The new extension (with or without a leading period). Specify null to remove an existing extension from path. </param>
        /// <returns></returns>
        public UniPath ChangeExtension(string newExtensionWithDot)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public static PathStyle EnvironmentPathStyle
        {
            get
            {
                var platform = Environment.OSVersion.Platform;
                switch (platform)
                {
                    case PlatformID.Win32NT:
                    case PlatformID.Win32S:
                    case PlatformID.Win32Windows:
                    case PlatformID.WinCE:
                    case PlatformID.Xbox:
                        return PathStyle.Windows;
                    case PlatformID.Unix:
                    case PlatformID.MacOSX:
                        return PathStyle.Unix;
                    default:
                        //it was at this moment we knew... you fucked up.
                        throw new PlatformNotSupportedException();
                }
            }
        }

        //todo: char or string?
        public static char EnvironmentDirectorySeperator
        {
            get {
                return Path.DirectorySeparatorChar;
            }
        }

        public static string CurrentDirectory
        {
            get
            {
                return _currentDirectory;
            }
        }

        public static string ParentDirectory
        {
            get
            {
                return _parentDirectory;
            }
        }

        public override string ToString()
        {
            return ToString(EnvironmentPathStyle);
        }

        public string ToString(PathStyle style)
        {
            throw new NotImplementedException();
        }

        public static bool operator ==(UniPath a, UniPath b)
        {
            // TODO: Casing? Windows doesn't care about the casing.

            if (object.ReferenceEquals(a, b))
                return true;
            if (((object)a == null) || ((object)b == null))
                return false;

            Debug.Assert(a._directories != null);
            Debug.Assert(b._directories != null);

            var aNorm = a.Normalize();
            var bNorm = b.Normalize();

            if (aNorm._directories.Count != bNorm._directories.Count)
                return false;

            var aArr = a._directories.ToArray();
            var bArr = b._directories.ToArray();

            for (int i = 0; i < aArr.Length; ++i)
            {
                if (aArr[i] != bArr[i])
                    return false;
            }
            return true;
        }

        public static bool operator !=(UniPath a, UniPath b)
        {
            return !(a == b);
        }
    }
}
