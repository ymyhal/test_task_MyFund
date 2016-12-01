using MyFund.Infrastructure.Converters;
using NUnit.Framework;

namespace MyFund.Infrastructure.Tests.Converters
{
    [TestFixture]
    public class BoolToContentConverterTests
    {
        private BoolToContentConverter _converter;

        [SetUp]
        public void SetUp()
        {
            _converter = new BoolToContentConverter();
        }

        [Test]
        public void ConvertObjectToNullContentSuccess()
        {
            Assert.That(_converter.Convert(new object(), typeof(object), null, null), Is.EqualTo(_converter.NullContent));
        }

        [Test]
        public void ConvertTrueToTruelContentSuccess()
        {
            Assert.That(_converter.Convert(true, typeof(object), null, null), Is.EqualTo(_converter.TrueContent));
        }

        [Test]
        public void ConvertFalseToFalselContentSuccess()
        {
            Assert.That(_converter.Convert(false, typeof(object), null, null), Is.EqualTo(_converter.FalseContent));
        }
    }
}
