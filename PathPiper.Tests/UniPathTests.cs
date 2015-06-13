﻿using System;
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

            path = UniPath.Parse("some_file/some_file2");
            expected = "";
            Assert.That(path.Extension, Is.EqualTo(expected));

            path = UniPath.Parse("some_file/");
            expected = "";
            Assert.That(path.Extension, Is.EqualTo(expected));

            path = UniPath.Parse("some_file.ext/");
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

            path = UniPath.Parse("some_file/some_file2");
            expected = "some_file2";
            Assert.That(path.Name, Is.EqualTo(expected));

            path = UniPath.Parse("some_file/");
            expected = "some_file";
            Assert.That(path.Name, Is.EqualTo(expected));

            path = UniPath.Parse("some_file.ext/");
            expected = "some_file.ext"; // Intended, since trailing / will be ignored
            Assert.That(path.Name, Is.EqualTo(expected));
        }

        [Test]
        public void Normalize()
        {
            UniPath path = UniPath.Parse("some/path/../lol.ext");
            var expected = @"some/lol.ext";
            var normalized = path.Normalize();
            Assert.That(normalized.ToString(PathStyle.Unix), Is.EqualTo(expected));

            path = UniPath.Parse("some/path/../lol.ext");
            expected = @"some\lol.ext";
            normalized = path.Normalize();
            Assert.That(normalized.ToString(PathStyle.Windows), Is.EqualTo(expected));

            path = UniPath.Parse("some/path/../../lol.ext");
            expected = @"lol.ext";
            normalized = path.Normalize();
            Assert.That(normalized.ToString(PathStyle.Unix), Is.EqualTo(expected));

            path = UniPath.Parse("some/path/../../lol.ext");
            expected = @"lol.ext";
            normalized = path.Normalize();
            Assert.That(normalized.ToString(PathStyle.Windows), Is.EqualTo(expected));

            // TODO: Use working directory or leave unchanged?
            path = UniPath.Parse("../lol.ext");
            expected = System.IO.Path.Combine(Environment.CurrentDirectory, "..", @"lol.ext");
            normalized = path.Normalize();
            Assert.That(normalized.ToString(PathStyle.Windows), Is.EqualTo(expected));

            path = UniPath.Parse("lol.ext");
            expected = @"lol.ext";
            normalized = path.Normalize();
            Assert.That(normalized.ToString(PathStyle.Windows), Is.EqualTo(expected));

            path = UniPath.Parse("dir/lol.ext");
            expected = @"dir\lol.ext";
            normalized = path.Normalize();
            Assert.That(normalized.ToString(PathStyle.Windows), Is.EqualTo(expected));
        }

        [Test]
        public void Parse()
        {
            UniPath path = UniPath.Parse(@"C:\user\docs\Letter.txt");
            var expected = @"C:\user\docs\Letter.txt";
            Assert.That(path.ToString(PathStyle.Windows), Is.EqualTo(expected));

            //todo: use working directory?
            path = UniPath.Parse("../lol.ext");
            expected = "../lol.ext";
            Assert.That(path.ToString(PathStyle.Windows), Is.EqualTo(expected));

            path = UniPath.Parse(@"C:\user\docs\somewhere\..\Letter.txt");
            expected = @"C:\user\docs\somewhere\..\Letter.txt";
            Assert.That(path.ToString(PathStyle.Windows), Is.EqualTo(expected));

            path = UniPath.Parse(@"\\Server01\user\docs\Letter.txt");
            expected = @"\\Server01\user\docs\Letter.txt";
            Assert.That(path.ToString(PathStyle.Windows), Is.EqualTo(expected));

            path = UniPath.Parse(@"/home/4lab/.config/fish/config.fish");
            expected = @"/home/4lab/.config/fish/config.fish";
            Assert.That(path.ToString(PathStyle.Unix), Is.EqualTo(expected));

            path = UniPath.Parse(@"/home/4lab/.config/fish/config.fish");
            expected = @"\home\4lab\.config\fish\config.fish";
            Assert.That(path.ToString(PathStyle.Windows), Is.EqualTo(expected));
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
            expected = "some_file.multiple.dots.lol";
            Assert.That(changed.Name, Is.EqualTo(expected));

            path = UniPath.Parse(".gitignore");
            changed = path.ChangeExtension(".lol");
            expected = ".lol";
            Assert.That(changed.Name, Is.EqualTo(expected));

            path = UniPath.Parse("some_file");
            changed = path.ChangeExtension(".lol");
            expected = "some_file.lol";
            Assert.That(changed.Name, Is.EqualTo(expected));

            path = UniPath.Parse("some_file/some_file2");
            changed = path.ChangeExtension(".lol");
            expected = "some_file/some_file2.lol";
            Assert.That(changed.Name, Is.EqualTo(expected));

            path = UniPath.Parse("some_file/");
            changed = path.ChangeExtension(".lol");
            expected = "some_file.lol"; // Intended, since trailing / will be ignored
            Assert.That(changed.Name, Is.EqualTo(expected));

            path = UniPath.Parse("some_file.ext/");
            changed = path.ChangeExtension(".lol");
            expected = "some_file.lol"; // Intended, since trailing / will be ignored
            Assert.That(changed.Name, Is.EqualTo(expected));
        }
    }
}
