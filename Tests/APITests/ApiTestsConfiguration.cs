using HTTPRequestsLibrary;
using HTTPRequestsLibrary.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.APITests
{
    public abstract class ApiTestsConfiguration
    {
        protected ApiClient Client { get; private set; }

        [SetUp]
        public void SetUp()
        {
            Client = new ApiClient("http://localhost:5211");
        }

        [TearDown]
        public void TearDown()
        {
            Client?.Dispose();
        }
    }
}
