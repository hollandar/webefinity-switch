using Microsoft.VisualStudio.TestTools.UnitTesting;
using Webefinity.Switch;

namespace Switch.Tests
{
    [TestClass]
    public class SwitchTests
    {
        [TestMethod]
        public void SimpleString()
        {
            var builder = new ArgumentsBuilder();
            builder.SetArguments("--string", "astring");
            builder.Add("string").AcceptString("default");
            builder.Add("nostring").AcceptString("default");
            var handler = builder.Build();
            Assert.AreEqual("astring", handler.GetValue<string>("string"));
            Assert.AreEqual("default", handler.GetValue<string>("nostring"));

        }

        enum Numbers { one, two, three};
        
        [TestMethod]
        public void SimpleEnum()
        {
            var builder = new ArgumentsBuilder();
            builder.SetArguments("--enum", "one");
            builder.Add("enum").AcceptEnum<Numbers>(Numbers.two);
            builder.Add("noenum").AcceptEnum<Numbers>(Numbers.two);
            var handler = builder.Build();
            Assert.AreEqual(Numbers.one, handler.GetValue<Numbers>("enum"));
            Assert.AreEqual(Numbers.two, handler.GetValue<Numbers>("noenum"));
        }
        
        [TestMethod]
        public void SimpleFile()
        {
            var builder = new ArgumentsBuilder();
            builder.SetArguments("--file", "./Switch.Tests.dll");
            builder.Add("file").AcceptFilename(true);
            builder.Add("nofile").AcceptFilename(true, "./Switch.Tests.dll");
            builder.Add("idont").AcceptFilename(false, "./Nope_Not_Here.dll");
            var handler = builder.Build();
            Assert.AreEqual("./Switch.Tests.dll", handler.GetValue<string>("file"));
            Assert.AreEqual("./Switch.Tests.dll", handler.GetValue<string>("nofile"));
            Assert.AreEqual("./Nope_Not_Here.dll", handler.GetValue<string>("idont"));
        }
        
        [TestMethod]
        public void SimpleFileFail()
        {
            var builder = new ArgumentsBuilder();
            builder.SetArguments("--file", "./Switch.DoesNotExist.dll");
            builder.Add("file").AcceptFilename(true);
            var handler = builder.Build();
            Assert.IsFalse(handler.IsValid);
        }

        [TestMethod]
        public void SimpleDirectory()
        {
            var builder = new ArgumentsBuilder();
            builder.SetArguments("--dir", "./");
            builder.Add("dir").AcceptDirectory(true);
            builder.Add("nodir").AcceptDirectory(true, "./");
            builder.Add("idont").AcceptDirectory(false, "./Nope/");
            var handler = builder.Build();
            Assert.AreEqual("./", handler.GetValue<string>("dir"));
            Assert.AreEqual("./", handler.GetValue<string>("nodir"));
            Assert.AreEqual("./Nope/", handler.GetValue<string>("idont"));
        }

        [TestMethod]
        public void SimpleDirectoryFail()
        {
            var builder = new ArgumentsBuilder();
            builder.SetArguments("--dir", "./DoesNotExist");
            builder.Add("dir").AcceptDirectory(true);
            var handler = builder.Build();
            Assert.IsFalse(handler.IsValid);
        }

        [TestMethod]
        public void SimpleTrueFlag()
        {
            var builder = new ArgumentsBuilder();
            builder.SetArguments("--flag", "true");
            builder.Add("flag").AcceptFlag();
            var handler = builder.Build();
            Assert.AreEqual(true, handler.GetValue<bool>("flag"));
        }

        [TestMethod]
        public void SimpleFlag()
        {
            var builder = new ArgumentsBuilder();
            builder.SetArguments("--flag");
            builder.Add("flag").AcceptFlag();
            var handler = builder.Build();
            Assert.AreEqual(true, handler.GetValue<bool>("flag"));
        }

        [TestMethod]
        public void SimpleFlagCompound()
        {
            var builder = new ArgumentsBuilder();
            builder.SetArguments("--flag","--banner");
            builder.Add("flag").AcceptFlag();
            builder.Add("banner").AcceptFlag();
            builder.Add("last").AcceptFlag();
            var handler = builder.Build();
            Assert.AreEqual(true, handler.GetValue<bool>("flag"));
            Assert.AreEqual(true, handler.GetValue<bool>("banner"));
            Assert.AreEqual(false, handler.GetValue<bool>("last"));
        }

        [TestMethod]
        public void SimpleNotProvidedFlag()
        {
            var builder = new ArgumentsBuilder();
            builder.SetArguments("--different");
            builder.Add("flag").AcceptFlag();
            var handler = builder.Build();
            Assert.AreEqual(false, handler.GetValue<bool>("flag"));
        }

        [TestMethod]
        public void SimpleFalseFlag()
        {
            var builder = new ArgumentsBuilder();
            builder.SetArguments("--flag", "false");
            builder.Add("flag").AcceptFlag();
            var handler = builder.Build();
            Assert.AreEqual(false, handler.GetValue<bool>("flag"));
        }

        [TestMethod]
        public void SimpleInteger()
        {
            var builder = new ArgumentsBuilder();
            builder.SetArguments("--int", "1234");
            builder.Add("int").AcceptInteger();
            builder.Add("def").AcceptInteger(332);
            builder.Add("no").AcceptInteger();
            var handler = builder.Build();
            Assert.AreEqual(1234, handler.GetValue<int>("int"));
            Assert.AreEqual(332, handler.GetValue<int>("def"));
            Assert.AreEqual(0, handler.GetValue<int>("no"));
        }

        [TestMethod]
        public void SimpleDecimal()
        {
            var builder = new ArgumentsBuilder();
            builder.SetArguments("--dec", "3.14");
            builder.Add("dec").AcceptDecimal();
            builder.Add("def").AcceptDecimal(2.98m);
            builder.Add("no").AcceptDecimal();
            var handler = builder.Build();
            Assert.AreEqual(3.14m, handler.GetValue<decimal>("dec"));
            Assert.AreEqual(2.98m, handler.GetValue<decimal>("def"));
            Assert.AreEqual(0m, handler.GetValue<decimal>("no"));
        }

        [TestMethod]
        public void FileDefaultDoesntExist()
        {
            Assert.ThrowsException<ArgumentException>(() => {
                new ArgumentFilenameProvider(true, "./DoesntExist.dll");
            });

        }

        [TestMethod]
        public void DirectoryDefaultDoesntExist()
        {
            Assert.ThrowsException<ArgumentException>(() => {
                new ArgumentDirectoryProvider(true, "./DoesntExist/");
            });

        }

        [TestMethod]
        public void CompoundTest()
        {
            var builder = new ArgumentsBuilder();
            builder.SetArguments("-d", "./", "--file", "./Switch.Tests.dll", "--count", "911", "-p", "3.14159", "--enum", "one", "--flag");
            builder.Add("directory", 'd').AcceptDirectory(true);
            builder.Add("file", 'f').AcceptFilename(true);
            builder.Add("count").AcceptInteger();
            builder.Add("pi", 'p').AcceptDecimal();
            builder.Add("enum").AcceptEnum<Numbers>();
            builder.Add("flag").AcceptFlag();
            var handler = builder.Build();
            Assert.AreEqual("./", handler.GetValue<string>("directory"));
            Assert.AreEqual("./Switch.Tests.dll", handler.GetValue<string>("file"));
            Assert.AreEqual(911, handler.GetValue<long>("count"));
            Assert.AreEqual(3.14159m, handler.GetValue<decimal>("pi"));
            Assert.AreEqual(Numbers.one, handler.GetValue<Numbers>("enum"));
            Assert.AreEqual(true, handler.GetValue<bool>("flag"));
        }

        [TestMethod]
        public void RepeatedFlags()
        {
            var builder = new ArgumentsBuilder();
            builder.Add("flag", 'f').AcceptFlag();

            Assert.ThrowsException<ArgumentException>(() =>
            {
                builder.Add("flag", 't').AcceptFlag();
            });
            Assert.ThrowsException<ArgumentException>(() =>
            {
                builder.Add("time", 'f').AcceptFlag();
            });

        }

        [TestMethod]
        public void Default()
        {
            var builder = new ArgumentsBuilder();
            builder.SetArguments("new", "--flag");
            builder.Add("command", 'c', true).AcceptString();
            builder.Add("flag", 'f').AcceptFlag();

            var handler = builder.Build();
            Assert.AreEqual("new", handler.GetValue<string>("command"));
            Assert.AreEqual(true, handler.GetValue<bool>("flag"));
        }

    }
}