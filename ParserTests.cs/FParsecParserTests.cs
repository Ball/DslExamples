using NUnit.Framework;
using ParserCombinatorBased;

namespace ParserTests.cs
{
    [TestFixture]
    public class FParsecParserTests:BaseParserTest
    {
        [SetUp]
        public void SetUp()
        {
            _parser = new FParsecParser();
        }
    }
}