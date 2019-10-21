using Chorus.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chorus.Specs.Core
{
    [TestFixture]
    public class ObjectExtensionsSpecs
    {
        [Test]
        public void Should_not_throw_argument_null_exception_if_argument_is_not_null()
        {
            object myObj = new { abc = "123" };
            Assert.DoesNotThrow(() => myObj.ThrowIfNull(nameof(myObj)));
        }

        [Test]
        public void Should_throw_argument_null_exception_if_argument_is_null()
        {
            object myObj = null;
            Assert.Throws<ArgumentNullException>(() => myObj.ThrowIfNull(nameof(myObj)));
        }
    }
}
