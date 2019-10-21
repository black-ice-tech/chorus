using Chorus.Core;
using NUnit.Framework;

namespace Chorus.Specs
{
    public class StringExtensionsSpecs
    {
        [Test]
        public void Should_convert_pascal_to_kebab_case()
        {
            var kebabCase = "MyTestString".PascalToKebabCase();

            Assert.That(kebabCase, Is.EqualTo("my-test-string"));
        }

        [Test]
        public void Should_handle_empty_string()
        {
            Assert.That(string.Empty.PascalToKebabCase(), Is.EqualTo(string.Empty));
        }

        [Test]
        public void Should_return_null_if_null()
        {
            string nullString = null;
            Assert.That(nullString.PascalToKebabCase(), Is.Null);
        }
    }
}