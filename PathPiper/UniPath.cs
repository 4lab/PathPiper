using System;

namespace PathPiper
{
    public class UniPath
    {
        public static UniPath Parse(string path)
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

        public override bool Equals(object obj) {
            throw new NotImplementedException();
        }

        public override int GetHashCode() {
            throw new NotImplementedException();
        }

        public override string ToString() {
            var currentPathStyle = PathStyle.Windows; //todo:
            return ToString(currentPathStyle);
        }

        public string ToString(PathStyle style) {
            throw new NotImplementedException();
        }
    }
}
