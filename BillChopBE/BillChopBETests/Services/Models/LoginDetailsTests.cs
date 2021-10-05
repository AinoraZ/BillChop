using BillChopBE.Services.Models;
using Bogus;
using NUnit.Framework;
using System.ComponentModel.DataAnnotations;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

namespace BillChopBETests.Services.Models
{
    public class LoginDetailsTests
    {
        [Test]
        [TestCase("@test.com")]
        [TestCase("john@gmailom")]
        [TestCase("alicyahoo.com")]
        public void WhenEmailIsWrong_ShouldThrow(string email)
        {
            //Arrange
            var user = new LoginDetails
            {
                Email = email,
                Password = "test123!",
            };

            //Act & Assert
            Assert.Throws<ValidationException>(() => Validator.ValidateObject(user, new ValidationContext(user), true));
        }

        [Test]
        public void WhenEmailIsCorrect_ShouldNotThrow()
        {
            //Arrange
            var faker = new Faker();
            var user = new LoginDetails
            {
                Email = faker.Person.Email,
                Password = "test123!",
            };

            //Act & Assert
            Assert.DoesNotThrow(() => Validator.ValidateObject(user, new ValidationContext(user), true));
        }
    }
}
