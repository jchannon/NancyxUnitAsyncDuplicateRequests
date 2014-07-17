namespace Tests
{
    using System;
    using Xunit;
    using System.Threading.Tasks;
    using Microsoft.Owin.Testing;
    using NancyxUnitAsyncDuplicateRequests;
    using System.Net.Http;

	public class StartupTests
	{
		private const string BaseURL = "http://localhost";

		[Fact]
		public async Task Test1()
		{
			//Given
			var client = CreateHttpClient();

			//When
			var response = await client.GetAsync(BaseURL);

			//Then
			Assert.Equal(200, (int)response.StatusCode);
		}

        [Fact]
        public async Task Test2()
        {
            //Given
            var client = CreateHttpClient();

            //When
            var response = await client.GetAsync(BaseURL);

            //Then
            Assert.Equal(200, (int)response.StatusCode);
        }

        [Fact]
        public async Task Test3()
        {
            //Given
            var client = CreateHttpClient();

            //When
            var response = await client.GetAsync(BaseURL);

            //Then
            Assert.Equal(200, (int)response.StatusCode);
        }

		private HttpClient CreateHttpClient()
		{
			var client = TestServer.Create(builder =>
				new Startup()
				.Configuration(builder))
				.HttpClient;

			client.DefaultRequestHeaders.Add("Accept", "application/json");
		
			return client;
		}
	}
}

