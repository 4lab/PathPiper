using System;
using NUnit.Framework;

namespace PathPiper.Tests
{
    [TestFixture]
    public class UniPathTests
    {
        [Test]
        public void HasExtension()
        {
            UniPath path = UniPath.Parse("some_file.ext");
            var expected = true;
            Assert.That(path.HasExtension, Is.EqualTo(expected));

            path = UniPath.Parse("some_file.multiple.dots");
            expected = true;
            Assert.That(path.HasExtension, Is.EqualTo(expected));

            path = UniPath.Parse(".gitignore");
            expected = true;
            Assert.That(path.HasExtension, Is.EqualTo(expected));

            path = UniPath.Parse("some_file");
            expected = false;
            Assert.That(path.HasExtension, Is.EqualTo(expected));
        }

        [Test]
        public void Extension()
        {
            UniPath path = UniPath.Parse("some_file.ext");
            var expected = ".ext";
            Assert.That(path.Extension, Is.EqualTo(expected));

            path = UniPath.Parse("some_file.multiple.dots");
            expected = ".dots";
            Assert.That(path.Extension, Is.EqualTo(expected));

            path = UniPath.Parse(".gitignore");
            expected = ".gitignore";
            Assert.That(path.Extension, Is.EqualTo(expected));

            path = UniPath.Parse("some_file");
            expected = "";
            Assert.That(path.Extension, Is.EqualTo(expected));

            path = UniPath.Parse("some_file/some_file2", PathStyle.Unix);
            expected = "";
            Assert.That(path.Extension, Is.EqualTo(expected));

            path = UniPath.Parse("some_file/", PathStyle.Unix);
            expected = "";
            Assert.That(path.Extension, Is.EqualTo(expected));

            path = UniPath.Parse("some_file.ext/", PathStyle.Unix);
            expected = ".ext"; // Intended, since trailing / will be ignored
            Assert.That(path.Extension, Is.EqualTo(expected));
        }

        [Test]
        public void Name()
        {
            UniPath path = UniPath.Parse("some_file.ext");
            var expected = "some_file.ext";
            Assert.That(path.Name, Is.EqualTo(expected));

            path = UniPath.Parse("some_file.multiple.dots");
            expected = "some_file.multiple.dots";
            Assert.That(path.Name, Is.EqualTo(expected));

            path = UniPath.Parse(".gitignore");
            expected = ".gitignore";
            Assert.That(path.Name, Is.EqualTo(expected));

            path = UniPath.Parse("some_file");
            expected = "some_file";
            Assert.That(path.Name, Is.EqualTo(expected));

            path = UniPath.Parse("some_file/some_file2", PathStyle.Unix);
            expected = "some_file2";
            Assert.That(path.Name, Is.EqualTo(expected));

            path = UniPath.Parse("some_file/", PathStyle.Unix);
            expected = "some_file";
            Assert.That(path.Name, Is.EqualTo(expected));

            path = UniPath.Parse("some_file.ext/", PathStyle.Unix);
            expected = "some_file.ext"; // Intended, since trailing / will be ignored
            Assert.That(path.Name, Is.EqualTo(expected));
        }

        [Test]
        public void Normalize()
        {
            UniPath path = UniPath.Parse("some/path/../lol.ext", PathStyle.Unix);
            var expected = @"some/lol.ext";
            var normalized = path.Normalize();
            Assert.That(normalized.ToString(PathStyle.Unix), Is.EqualTo(expected));

            path = UniPath.Parse("some/path/../lol.ext", PathStyle.Unix);
            expected = @"some\lol.ext";
            normalized = path.Normalize();
            Assert.That(normalized.ToString(PathStyle.Windows), Is.EqualTo(expected));

            path = UniPath.Parse("some/path/../../lol.ext", PathStyle.Unix);
            expected = @"lol.ext";
            normalized = path.Normalize();
            Assert.That(normalized.ToString(PathStyle.Unix), Is.EqualTo(expected));

            path = UniPath.Parse("some/path/../../lol.ext", PathStyle.Unix);
            expected = @"lol.ext";
            normalized = path.Normalize();
            Assert.That(normalized.ToString(PathStyle.Windows), Is.EqualTo(expected));

            path = UniPath.Parse("../lol.ext", PathStyle.Unix);
            expected = @"..\lol.ext";
            normalized = path.Normalize();
            Assert.That(normalized.ToString(PathStyle.Windows), Is.EqualTo(expected));

            path = UniPath.Parse("lol.ext");
            expected = @"lol.ext";
            normalized = path.Normalize();
            Assert.That(normalized.ToString(PathStyle.Windows), Is.EqualTo(expected));

            path = UniPath.Parse("dir/lol.ext", PathStyle.Unix);
            expected = @"dir\lol.ext";
            normalized = path.Normalize();
            Assert.That(normalized.ToString(PathStyle.Windows), Is.EqualTo(expected));

            path = UniPath.Parse("some/path/../../path/../../../lol.ext", PathStyle.Unix);
            expected = @"../../lol.ext";
            normalized = path.Normalize();
            Assert.That(normalized.ToString(PathStyle.Unix), Is.EqualTo(expected));
        }

        [Test]
        public void Parse()
        {
            UniPath path = UniPath.Parse(@"C:\user\docs\Letter.txt", PathStyle.Windows);
            var expected = @"C:\user\docs\Letter.txt";
            Assert.That(path.ToString(PathStyle.Windows), Is.EqualTo(expected));

            //todo: use working directory?
            path = UniPath.Parse("../lol.ext", PathStyle.Unix);
            expected = @"..\lol.ext";
            Assert.That(path.ToString(PathStyle.Windows), Is.EqualTo(expected));
            expected = @"../lol.ext";
            Assert.That(path.ToString(PathStyle.Unix), Is.EqualTo(expected));

            path = UniPath.Parse(@"C:\user\docs\somewhere\..\Letter.txt", PathStyle.Windows);
            expected = @"C:\user\docs\somewhere\..\Letter.txt";
            Assert.That(path.ToString(PathStyle.Windows), Is.EqualTo(expected));

            path = UniPath.Parse(@"/home/4lab/.config/fish/config.fish", PathStyle.Unix);
            expected = @"/home/4lab/.config/fish/config.fish";
            Assert.That(path.ToString(PathStyle.Unix), Is.EqualTo(expected));

            path = UniPath.Parse(@"/home/4lab/.config/fish/config.fish", PathStyle.Unix);
            expected = @"\home\4lab\.config\fish\config.fish";
            Assert.That(path.ToString(PathStyle.Windows), Is.EqualTo(expected));

            path = UniPath.Parse(@"\\Server01\user\docs\Letter.txt", PathStyle.Windows);
            expected = @"\\Server01\user\docs\Letter.txt";
            Assert.That(path.ToString(PathStyle.Windows), Is.EqualTo(expected));
        }

        [Test]
        public void ParseExceptions()
        {
            Assert.That(() => UniPath.Parse(null), Throws.InstanceOf<ArgumentNullException>());
            Assert.That(() => UniPath.Parse(null, PathStyle.Unix), Throws.InstanceOf<ArgumentNullException>());
            Assert.That(() => UniPath.Parse(null, PathStyle.Windows), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void ChangeExtension()
        {
            UniPath path = UniPath.Parse("some_file.ext");
            var changed = path.ChangeExtension(".lol");
            var expected = "some_file.lol";
            Assert.That(changed.Name, Is.EqualTo(expected));

            path = UniPath.Parse("some_file.multiple.dots");
            changed = path.ChangeExtension(".lol");
            expected = "some_file.multiple.lol";
            Assert.That(changed.Name, Is.EqualTo(expected));

            path = UniPath.Parse(".gitignore");
            changed = path.ChangeExtension(".lol");
            expected = ".lol";
            Assert.That(changed.Name, Is.EqualTo(expected));

            path = UniPath.Parse("some_file");
            changed = path.ChangeExtension(".lol");
            expected = "some_file.lol";
            Assert.That(changed.Name, Is.EqualTo(expected));

            path = UniPath.Parse("some_file/some_file2", PathStyle.Unix);
            changed = path.ChangeExtension(".lol");
            expected = "some_file/some_file2.lol";
            Assert.That(changed.ToString(PathStyle.Unix), Is.EqualTo(expected));

            path = UniPath.Parse("some_file/", PathStyle.Unix);
            changed = path.ChangeExtension(".lol");
            expected = "some_file.lol"; // Intended, since trailing / will be ignored
            Assert.That(changed.ToString(PathStyle.Unix), Is.EqualTo(expected));

            path = UniPath.Parse("some_file.ext/", PathStyle.Unix);
            changed = path.ChangeExtension(".lol");
            expected = "some_file.lol"; // Intended, since trailing / will be ignored
            Assert.That(changed.ToString(PathStyle.Unix), Is.EqualTo(expected));
        }

        [Test]
        public void Append()
        {
            var actual = UniPath.Parse(@"C:\user\docs", PathStyle.Windows).Append(@"\Letter.txt", PathStyle.Windows);
            var expected = UniPath.Parse(@"C:\user\docs\Letter.txt", PathStyle.Windows);
            Assert.That(actual == expected);

            actual = UniPath.Parse(@"C:\user\docs", PathStyle.Windows).Append(@"Letter.txt");
            expected = UniPath.Parse(@"C:\user\docs\Letter.txt", PathStyle.Windows);
            //Assert.That(actual, Is.EqualTo(expected));
            Assert.That(actual == expected);

            actual = UniPath.Parse(@"C:\user\docs", PathStyle.Windows).Append(@"/subdir/Letter.txt", PathStyle.Unix);
            expected = UniPath.Parse(@"C:\user\docs\subdir\Letter.txt", PathStyle.Windows);
            //Assert.That(actual, Is.EqualTo(expected));
            Assert.That(actual == expected);

            var append = UniPath.Parse("picture.jpg");
            actual = UniPath.Parse(@"C:\user\pictures", PathStyle.Windows).Append(append);
            expected = UniPath.Parse(@"C:\user\pictures\picture.jpg", PathStyle.Windows);
            Assert.That(actual == expected);
        }

        [Test]
        public void AppendExceptions()
        {
            //var expected = UniPath.Parse(@"C:\user\docs\Letter.txt");
            //Assert.That(() => expected.Append((string)null), Throws.InstanceOf<ArgumentNullException>());
            //Assert.That(() => expected.Append((UniPath)null), Throws.InstanceOf<ArgumentNullException>());
        }
    }
}
