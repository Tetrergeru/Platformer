using NUnit.Framework;

namespace viper_script.Tests
{
    [TestFixture]
    internal class TokenLineTest
    {
        [Test]
        public void EmptyLine()
        {
            var res = new TokenLine("");
            Assert.AreEqual(res, new string[] { });
        }

        [Test]
        public void OnlySpaces()
        {
            var res = new TokenLine("           ");
            Assert.AreEqual(res, new string[] { });
        }

        [Test]
        [TestCase("var", new[] { "var" })]
        [TestCase("(", new[] { "(" })]
        [TestCase(".", new[] { "." })]
        [TestCase("123_123", new[] { "123_123" })]
        [TestCase("++", new[] { "++" })]
        public void SingleWord(string line, string[] tokens)
        {
            var res = new TokenLine(line);
            Assert.AreEqual(res, tokens);
        }

        [Test]
        [TestCase("var x", new[] { "var", "x" })]
        [TestCase("var (", new[] { "var", "(" })]
        [TestCase(". (", new[] { ".", "(" })]
        public void TwoWordsDividedBySpace(string line, string[] tokens)
        {
            var res = new TokenLine(line);
            Assert.AreEqual(res, tokens);
        }

        [Test]
        [TestCase("var(", new[] { "var", "(" })]
        [TestCase("var.", new[] { "var", "." })]
        [TestCase("[(", new[] { "[", "(" })]
        public void TwoWordsClose(string line, string[] tokens)
        {
            var res = new TokenLine(line);
            Assert.AreEqual(res, tokens);
        }

        [Test]
        [TestCase("    +     ", new[] { "+" })]
        [TestCase("    (     ", new[] { "(" })]
        [TestCase("    (    result", new[] { "(", "result" })]
        [TestCase("    (   ) +    var  ", new[] { "(", ")", "+", "var" })]
        public void SpacesDoNotAffect(string line, string[] tokens)
        {
            var res = new TokenLine(line);
            Assert.AreEqual(res, tokens);
        }

        [Test]
        [TestCase("'w o r l d' 'world'", new[] { "'w o r l d'", "'world'" })]
        [TestCase("   '. () ++'  ", new[] { "'. () ++'" })]
        public void TestScreenedByQuote(string line, string[] tokens)
        {
            var res = new TokenLine(line);
            Assert.AreEqual(res, tokens);
        }

        [Test]
        [TestCase("Assert.AreEqual(res, tokens);", new[] { "Assert", ".", "AreEqual", "(", "res", ",", "tokens", ")",";" })]
        [TestCase("  ( 'print' test &&& $$ +string+", new[] {"(", "'print'", "test", "&&&", "$$", "+", "string", "+"})]
        public void LongLine(string line, string[] tokens)
        {
            var res = new TokenLine(line);
            Assert.AreEqual(res, tokens);
        }
    }
}
