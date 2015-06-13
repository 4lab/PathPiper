using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace PathPiper
{
    public class UniPath
    {
        private const string _currentDirectory = ".";
        private const string _parentDirectory = "..";

        private readonly ReadOnlyCollection<string> _directories;

        protected UniPath(ReadOnlyCollection<string> directories)
        {
            _directories = directories ?? new ReadOnlyCollection<string>(new string[0]);
        }

        public static UniPath Parse(string path)
        {
            return Parse(path, EnvironmentPathStyle);
        }
        public static UniPath Parse(string path, PathStyle pathStyle) {
            char directorySeperator;
            char[] invalidFileNameChars = { '\"', '<', '>', '|', '\0', (Char)1, (Char)2, (Char)3, (Char)4, (Char)5, (Char)6, (Char)7, (Char)8, (Char)9, (Char)10, (Char)11, (Char)12, (Char)13, (Char)14, (Char)15, (Char)16, (Char)17, (Char)18, (Char)19, (Char)20, (Char)21, (Char)22, (Char)23, (Char)24, (Char)25, (Char)26, (Char)27, (Char)28, (Char)29, (Char)30, (Char)31, ':', '*', '?', '\\', '/' };

            switch (pathStyle) {
                case PathStyle.Windows:
                    directorySeperator = '\\';
                    break;
                case PathStyle.Unix:
                    directorySeperator = '/';
                    break;
                default:
                    throw new PlatformNotSupportedException();
            }

            var parts = path.Split(new[] {directorySeperator});
            var items = new List<string>();

            for (var i = 0; i < parts.Length; i++) {
                var part = parts[i];
                Trace.WriteLine("Parse() part: " + part);

                //check invalid chars
                for (int j = 0; j < part.Length; j++) {
                    var ch = part[j];
                    if (invalidFileNameChars.Contains(ch))
                        throw new FormatException(String.Format("Found illegal char '{0}'", ch));
                }

                //todo: check if this thing is empty

                //if everything is fine, add
                items.Add(part);
            }

            return new UniPath(new ReadOnlyCollection<string>(new List<string>()));
        }

        // private bool? _hasExtension;
        public bool HasExtension
        {
            get
            {
                Debug.Assert(_directories != null);
                /*
                if (!_hasExtension.HasValue)
                {
                    if (_directories.Count == 0)
                    {
                        _hasExtension = false;
                    }
                    else
                    {
                        var last = _directories[_directories.Count - 1];
                        _hasExtension = Path.HasExtension(last);
                    }
                }
                return _hasExtension.Value;
                */
                if (_directories.Count == 0)
                    return false;
                var last = _directories[_directories.Count - 1];
                return Path.HasExtension(last);
            }
        }

        public string Extension
        {
            get
            {
                Debug.Assert(_directories != null);
                if (_directories.Count == 0)
                    return null;
                var last = _directories[_directories.Count - 1];
                return Path.GetExtension(last);
            }
        }

        public string Name
        {
            get
            {
                Debug.Assert(_directories != null);
                if (_directories.Count == 0)
                    return null;
                var last = _directories[_directories.Count - 1];
                return Path.GetDirectoryName(last);
            }
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

        public override bool Equals(object other)
        {
            // If parameter cannot be cast to UniPath, return false:
            UniPath p = other as UniPath;
            if ((object)p == null)
                return false;
            return this == p;
        }
        public bool Equals(UniPath other)
        {
            // If parameter is null, return false:
            if ((object)other == null)
                return false;
            // Return true if the fields match:
            return this == other;
        }

        public override int GetHashCode()
        {
            return _directories.GetHashCode();
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
            get
            {
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
