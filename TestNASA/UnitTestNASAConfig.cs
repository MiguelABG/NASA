using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NASA.Client;
using NASA.Controllers;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace UnitTestNASA
{
    [TestClass]
    public class UnitTest_NASAClient
    {
        private string BaseAddress;
        private string BasePath;
        private string MEDIA_TYPE;
        private string APIKEY;
        public void LoadConfiguration()
        {
            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", true, true).Build();

            BaseAddress = config.GetSection("BaseAddress").Value;
            BasePath = config.GetSection("BasePath").Value;
            MEDIA_TYPE = config.GetSection("apiKey").Value;
            APIKEY = config.GetSection("mediaType").Value;
        }

        [TestMethod]
        public void Test_BaseAddressIsEmpy()
        {
            LoadConfiguration();

            string expectedException = "";
            BaseAddress = "";
            try
            {
                NASAClient client = new NASAClient(BaseAddress, BasePath, MEDIA_TYPE, APIKEY);
            }
            catch (Exception ex)
            {
                expectedException = ex.Message;
            }

            Assert.AreEqual(expectedException, "Invalid URI: The URI is empty.");
        }

        [TestMethod]
        public void Test_BaseAddressFormaIncorrect()
        {
            LoadConfiguration();

            string expectedException = "";
            BaseAddress = "ftp";
            try
            {
                NASAClient client = new NASAClient(BaseAddress, BasePath, MEDIA_TYPE, APIKEY);
            }
            catch (Exception ex)
            {
                expectedException = ex.Message;
            }

            Assert.AreEqual(expectedException, "Invalid URI: The format of the URI could not be determined.");
        }

        [TestMethod]
        public async Task Test_LoadApiNASA()
        {
            LoadConfiguration();
            NASAClient client = new NASAClient(BaseAddress, BasePath, MEDIA_TYPE, APIKEY); 
            var result = await client.GetStringAsync("https://api.nasa.gov/");            
        }
    }
}
