﻿using System;
using System.Dynamic;

namespace PathPiper
{
    public class UniPath
    {
        public static UniPath Parse(string path)
        {
            return Parse(path, PathStyle.Current);
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

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public static PathStyle CurrentPathStyle
        {
            get
            {
                return PathStyle.Windows; // TODO
            }
        }

        public override string ToString()
        {
            return ToString(CurrentPathStyle);
        }

        public string ToString(PathStyle style)
        {
            throw new NotImplementedException();
        }
    }
}
