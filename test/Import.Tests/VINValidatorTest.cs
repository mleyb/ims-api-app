using Xunit;

namespace Import.Tests
{
    public class VINValidatorTest
    {
        [Theory]
        // https://vingenerator.org/
        [InlineData("WDBJF65J1YB039105")]
        [InlineData("JH4DA9470MS008988")]
        [InlineData("3D7UT2CL0BG625027")]
        [InlineData("JH4DA9360NS008662")]
        [InlineData("WV2YB0253GG020574")]
        public void IsValid_ValidVIN_ReturnsTrue(string vin)
        {
            // Arrange

            var sut = new VINValidator();

            // Act

            bool valid = sut.IsValid(vin);

            // Assert

            Assert.True(valid);
        }

        [Theory]
        [InlineData("")]
        [InlineData("dfddd")]
        [InlineData("5534554")]
        [InlineData("JLKHFDKHFKJHFKJHFKJHFKJHFJKHFJHKF")]
        [InlineData("$%(&*^^%$(%)%&£$")]
        public void IsValid_InvalidVIN_ReturnsFalse(string vin)
        {
            // Arrange

            var sut = new VINValidator();

            // Act

            bool valid = sut.IsValid(vin);

            // Assert

            Assert.False(valid);
        }
    }
}
