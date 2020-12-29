using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Models.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Xunit;

namespace BillsToPay.Test
{
    public class BillsControllerIntegrationTest
    {
        public IConfiguration _configuration { get; set; }

        private readonly HttpClient _client;

        public BillsControllerIntegrationTest()
        {
            var builder = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                            .AddEnvironmentVariables();

            _configuration = builder.Build();

            _client = new TestServer(
                    new WebHostBuilder()
                    .UseEnvironment("Development")
                    .UseStartup<Startup>()
                    .UseConfiguration(_configuration)
                ).CreateClient();

        }

        [Fact]
        public async void Get_Response200_Test()
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod("GET"), "/api/Bills");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }


        [Fact]
        public async void Post_Response200_Test()
        {
            // Arrange
            var setupApi = new ViewModelBillInsert
            {
                Name = "Conta1",
                DueDate = DateTime.Now,
                PaymentDate = DateTime.Now,
                ValueOriginal = 100.0m
            };

            // Act
            var dataAsString = JsonConvert.SerializeObject(setupApi);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await _client.PostAsync("/api/Bills", content);

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async void Put_Response200_Test()
        {
            // Arrange
            var setupApi = new ViewModelBillUpdate
            {
                Id = 1,
                Name = "Conta1_1",
                DueDate = DateTime.Now,
                PaymentDate = DateTime.Now,
                ValueOriginal = 150.0m
            };

            // Act
            var dataAsString = JsonConvert.SerializeObject(setupApi);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await _client.PutAsync("/api/Bills", content);

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async void Get_View_Response200_Test()
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod("GET"), "/api/Bills");

            // Act
            var response = await _client.SendAsync(request);

            var data = await response.Content.ReadAsStringAsync();

            var view = JsonConvert.DeserializeObject<IEnumerable<ViewModelBillView>>(data);

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.True(view.Any());
        }

        [Fact]
        public async void Delete_Response200_Test()
        {
            // Arrange
            var setupApi = new ViewModelBillDelete
            {
                Id = 1,
                Name = "Conta1",
                DueDate = DateTime.Now,
                PaymentDate = DateTime.Now,
                ValueOriginal = 100.0m
            };

            // Act
            var dataAsString = JsonConvert.SerializeObject(setupApi);

            var request = new HttpRequestMessage(new HttpMethod("DELETE"), "/api/Bills");
            request.Content = new StringContent(dataAsString, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async void Get_ViewNothing_Response200_Test()
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod("GET"), "/api/Bills");

            // Act
            var response = await _client.SendAsync(request);

            var data = await response.Content.ReadAsStringAsync();

            var view = JsonConvert.DeserializeObject<IEnumerable<ViewModelBillView>>(data);

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.False(view.Any());
        }

    }
}
