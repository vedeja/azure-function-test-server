using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading.Tasks;

namespace DemoFunction.Test
{
    [TestClass]
    public class Function1Tests
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            var functionPath = new FileInfo(typeof(Function1).Assembly.Location).Directory.Parent.FullName;
            Directory.SetCurrentDirectory(functionPath);

            var server = CreateServer(functionPath);
            var client = server.CreateClient();
            var response = await client.GetAsync("Function1");
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
        }

        private static TestServer CreateServer(string functionPath) =>
            new TestServer(WebHost
                .CreateDefaultBuilder()
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    config
                        .SetBasePath(functionPath)
                        .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables();
                })
                .UseStartup<TestStartup>()); //Use the startup class of your WebApi project.
    }
}
