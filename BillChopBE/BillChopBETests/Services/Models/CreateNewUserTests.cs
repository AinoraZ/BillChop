using BillChopBE.Services.Models;
using NUnit.Framework;
using System.ComponentModel.DataAnnotations;
using Bogus;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;
using System.Collections.Generic;
using System.Linq;

namespace BillChopBETests.Services.Models
{
    public class CreateNewUserTests
    {
        [Test]
        [TestCase("@test.com")]
        [TestCase("john@gmailom")]
        [TestCase("alicyahoo.com")]
        public void WhenEmailIsWrong_ShouldThrow(string email)
        {
            //Arrange
            var faker = new Faker();
            var user = new CreateNewUser
            {
                Name = faker.Person.FullName,
                Email = email,
                Password = "test123!",
            };

            //Act & Assert
            Assert.Throws<ValidationException>(() => Validator.ValidateObject(user, new ValidationContext(user), true));
        }

        [Test]
        [TestCase("test12!", Description = "Length Less Than 8")]
        [TestCase("Random1234", Description = "No Special Character")]
        [TestCase("TestPassword!@!@", Description = "No Number")]
        [TestCase("123456!@", Description = "No Letter")]
        [TestCase("NotLeGaL123.", Description = "Illegal Symbol")]
        public void IncorrectPassword_ShouldThrow(string password)
        {
            //Arrange
            var faker = new Faker();
            var user = new CreateNewUser
            {
                Name = faker.Person.FullName,
                Email = faker.Person.Email,
                Password = password,
            };

            //Act & Assert
            Assert.Throws<ValidationException>(() => Validator.ValidateObject(user, new ValidationContext(user), true));
        }

        [Test]
        [TestCase("test123!")]
        [TestCase("@TEST954")]
        [TestCase("Ran&dom1")]
        public void MinimalLegalPassword_ShouldNotThrow(string password)
        {
            //Arrange
            var faker = new Faker();
            var user = new CreateNewUser
            {
                Name = faker.Person.FullName,
                Email = faker.Person.Email,
                Password = password,
            };

            //Act & Assert
            Assert.DoesNotThrow(() => Validator.ValidateObject(user, new ValidationContext(user), true));
        }

        [Test]
        public void PasswordWithAllLegalSymbols_ShouldNotThrow()
        {
            //Arrange
            var faker = new Faker();

            var legalUpperCase = new List<char>();
            for (char letter = 'A'; letter <= 'Z'; letter++)
            {
                legalUpperCase.Add(letter);
            }

            var legalLowerCase = new List<char>();
            for (char letter = 'A'; letter <= 'Z'; letter++)
            {
                legalLowerCase.Add(letter);
            }

            var legalNumbers = "0123456789";
            var legalSpecialSymbols = "@$!%*#?&";
            var legalSymbols = legalUpperCase.Concat(legalLowerCase).Concat(legalNumbers).Concat(legalSpecialSymbols);
            var password = string.Join("", legalSymbols);

            var user = new CreateNewUser
            {
                Name = faker.Person.FullName,
                Email = faker.Person.Email,
                Password = password,
            };

            //Act & Assert
            Assert.DoesNotThrow(() => Validator.ValidateObject(user, new ValidationContext(user), true));
        }
    }
}
