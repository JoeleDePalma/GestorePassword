using Libreria.HTTPRequestsLibrary;
using Libreria.HTTPRequestsLibrary.Services;
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
            Client = new ApiClient("http://localhost:8080");
        }

        [TearDown]
        public void TearDown()
        {
            Client?.Dispose();
        }
    }
}
