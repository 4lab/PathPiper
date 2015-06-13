using System;
using NUnit.Framework;

namespace PathPiper.Tests
{
    [TestFixture]
    public class UniPathTests
    {
        [Test]
        public void TestMethod1()
        {
        }

        public void ReferenceTest()
        {
            // TODO
        }

        [Test]
        public void HasExtension()
        {
            UniPath path = UniPath.Parse("some_file.ext");
            var expected = true;
            Assert.That(path.HasExtension, Is.EqualTo(expected));

            path = UniPath.Parse("some_file.multiple.dots");
            expected = true;
            Assert.That(path.HasExtension, Is.EqualTo(expected));

            path = UniPath.Parse("some_file");
            expected = false;
            Assert.That(path.HasExtension, Is.EqualTo(expected));
        }
    }
}
