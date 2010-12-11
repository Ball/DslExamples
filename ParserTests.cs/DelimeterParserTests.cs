using DelimiterDirected;
using NUnit.Framework;

namespace ParserTests.cs
{
    [TestFixture]
    public class DelimeterParserTests:BaseParserTest
    {
        [SetUp]
        public void SetUp()
        {
            _parser = new DelimiterParser();
        }
    }
}