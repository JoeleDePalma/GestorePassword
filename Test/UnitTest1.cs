using NUnit.Framework;
using GestorePassword;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class Tests
    {
        private SendRequest client;

        [SetUp]
        public void Setup()
        {
            client = new SendRequest();
        }

        [Test]
        [Category("API")]
        [TestCase("TestApp", "ok")]
        [TestCase("TestApp2", "ok2")]
        public async Task AddPassword(string app, string StringPassword)
        {
            var NewPassword = new Password
            {
                App = app,
                EncryptedPassword = Encoding.UTF8.GetBytes(StringPassword)
            };

            var Response = await client.AddPassword(NewPassword);
            Assert.That(Response.IsSuccessStatusCode);
        }

        [Test]
        [Category("API")]
        [Repeat(5)]
        public async Task GetListOfPasswords()
        {
            var Response = await client.GetPasswords();
            Assert.That(Response, Is.Not.Null);
        }

        [Ignore("To build yet")]
        public async Task UpdatePassword()
        {

        }
    }
}
