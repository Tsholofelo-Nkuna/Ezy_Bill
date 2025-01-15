using Question1;

namespace ISS_Assememnt.UnitTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [
            TestCase("nest", "best", false),
            TestCase("file", "life", true),
            TestCase("Thread Bare", "The Red Arab", true),
            TestCase("zbBz bzZzzB BzBbbzz zZZZz", "z Z BzbzZzbb ZzzBz Zzbbbzz", true),
            TestCase("abBa baAaaB BaBbbaa aAAAa", "Ab aAba bA AbBb aAa baa Bab", false)
        ]
        public void Anagram_Test(string s1, string s2, bool expected)
        {
            var actual =  AnagramsClass.Anagrams(s1, s2);
            Assert.That(actual, Is.EqualTo(expected));
           
        }
    }
}