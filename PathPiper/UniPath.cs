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
        public static UniPath Parse(string path, PathStyle pathStyle)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            char[] invalidFileNameChars = { '\"', '<', '>', '|', '\0', (Char)1, (Char)2, (Char)3, (Char)4, (Char)5, (Char)6, (Char)7, (Char)8, (Char)9, (Char)10, (Char)11, (Char)12, (Char)13, (Char)14, (Char)15, (Char)16, (Char)17, (Char)18, (Char)19, (Char)20, (Char)21, (Char)22, (Char)23, (Char)24, (Char)25, (Char)26, (Char)27, (Char)28, (Char)29, (Char)30, (Char)31, ':', '*', '?', '\\', '/' };


            char directorySeperator = GetDirectorySeperator(pathStyle);
            var parts = path.Split(new[] { directorySeperator });
            var items = new List<string>();

            for (var i = 0; i < parts.Length; i++)
            {
                var part = parts[i];
                Trace.WriteLine("Parse() part: " + part);

                //check windows drive letter
                var isValidDriveSpecifier = (i == 0 && pathStyle == PathStyle.Windows && part.Length == 2 && part[1] == ':' && Char.IsLetter(part[0]));

                //check invalid chars
                if (!isValidDriveSpecifier)
                {
                    for (int j = 0; j < part.Length; j++)
                    {
                        var ch = part[j];
                        if (invalidFileNameChars.Contains(ch))
                            throw new FormatException(String.Format("Found illegal char '{0}'", ch));
                    }
                }

                //check if this thing is empty
                if (part.Length == 0)
                    continue;

                //if everything is fine, add
                items.Add(part);
            }

            return new UniPath(new ReadOnlyCollection<string>(items));
        }

        private static char GetDirectorySeperator(PathStyle pathStyle)
        {
            switch (pathStyle)
            {
                case PathStyle.Windows:
                    return '\\';
                case PathStyle.Unix:
                    return '/';
                default:
                    throw new PlatformNotSupportedException();
            }
        }

        public bool HasExtension
        {
            get
            {
                Debug.Assert(_directories != null);
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
                return Path.GetFileName(last);
            }
        }

        public UniPath Normalize()
        {
            throw new NotImplementedException();
        }

        public UniPath Append(string directoryOrFile)
        {
            if (directoryOrFile == null)
                return this; //TODO: Is this okay?
            return Append(directoryOrFile, EnvironmentPathStyle);
        }

        public UniPath Append(string directoryOrFile, PathStyle pathStyle)
        {
            if (directoryOrFile == null)
                return this; //TODO: Is this okay?

            var seperator = GetDirectorySeperator(pathStyle);
            if (directoryOrFile.StartsWith(seperator.ToString()))
            {
                if (directoryOrFile.Length > 1 && directoryOrFile[1] != seperator || directoryOrFile.Length == 1)
                {
                    // If not \\abc or //abc, remove first / or \
                    directoryOrFile = directoryOrFile.Substring(1);
                }
            }

            var parsed = Parse(directoryOrFile, pathStyle);
            return Append(parsed);
        }

        public UniPath Append(UniPath path)
        {
            // TODO: Unit tests for this
            if (path == null)
                throw new ArgumentNullException("path");
            var currentDirs = _directories;
            var appendedDirs = path._directories;

            var combined = currentDirs.Concat(appendedDirs).ToArray();
            return new UniPath(new ReadOnlyCollection<string>(combined));
        }

        /// <summary>Changes the extension of a path.</summary>
        /// <param name="newExtensionWithDot">The new extension (with or without a leading period). Specify null to remove an existing extension from path. </param>
        /// <returns></returns>
        public UniPath ChangeExtension(string newExtensionWithDot)
        {
            Debug.Assert(_directories != null);
            var dirs = _directories.ToArray();
            if (dirs.Length == 0)
                return new UniPath(new ReadOnlyCollection<string>(new[] { newExtensionWithDot }));
            var last = dirs[dirs.Length - 1];
            dirs[dirs.Length - 1] = Path.ChangeExtension(last, newExtensionWithDot);
            return new UniPath(new ReadOnlyCollection<string>(dirs));
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
        public static char EnvironmentDirectorySeperator { get { return GetDirectorySeperator(EnvironmentPathStyle); } }

        public static string CurrentDirectory { get { return _currentDirectory; } }

        public static string ParentDirectory { get { return _parentDirectory; } }

        public override string ToString()
        {
            return ToString(EnvironmentPathStyle);
        }

        public string ToString(PathStyle pathStyle)
        {
            var seperator = GetDirectorySeperator(pathStyle);
            return string.Join(seperator.ToString(), _directories);
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
