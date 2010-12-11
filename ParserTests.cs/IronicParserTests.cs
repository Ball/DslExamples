using IronyBased;
using NUnit.Framework;

namespace ParserTests.cs
{
    [TestFixture]
    public class IronicParserTests:BaseParserTest
    {
        [SetUp]
        public void SetUp()
        {
            _parser = new IronicParser();
        }
    }
}