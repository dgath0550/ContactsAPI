using ContactsAPI;
using ContactsAPI.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;


namespace XUnitTestContactAPI
{
    public class UnitTest1 : IDisposable
    {
        #region Constructors/Destructors

        private TestServer _server;
        public IRepository repository;

        public HttpClient Client { get; private set; }

        public UnitTest1()
        {
            SetUpClient();
        }

        public void Dispose()
        { }

        #endregion

        #region Test Definitions

        //// TEST NAME - checkNonExistentApi
        //// TEST DESCRIPTION - It should check if an API exists
        [Fact]
        public async Task CheckInvalidPath()
        {
            // Return with 404 if no API path exists 
            var response = await GetResponseAsync("/Contact/test");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        // TEST NAME - GetAllContacts
        // TEST DESCRIPTION - It finds all the Contacts
        [Fact]
        public void GetAllContactsTest()
        {
            // Get All Contacts 
            var response = GetResponseAsync("/Contacts").Result;

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var jsonresult = JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result);
                var expectedresult = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(repository.GetContact(0)));
                Assert.Equal(jsonresult, expectedresult);
            }
        }

        // TEST NAME - GetSingleContact
        // TEST DESCRIPTION - Gets a single contact
        [Theory]
        [InlineData(2)]
        public void GetContactTest(int id)
        {
            // Get All Contacts 
            var response = GetResponseAsync("Contacts/" + id).Result;

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var jsonresult = JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result);
                var expectedresult = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(repository.GetContact(id)));
                Assert.Equal(jsonresult, expectedresult);
            }
        }

        // TEST NAME - CreateContact
        // TEST DESCRIPTION - A new entry should be created
        [Theory]
        [InlineData("{\'id\':0,\'name\':{\'first\':\'Jim\',\'middle\':\'Francis\',\'last\':\'test\'},\'address\':{\'street\':\'8 High Autumn Row\',\'city\':\'Cannon\',\'state\':\'Delaware\',\'zip\':\'19797\'},\'phone\':[{\'number\':\'302-611-9148\',\'type\':\'home\'},{\'number\':\'302-532-9427\',\'type\':\'mobile\'}],\'email\':\'jim.test@yahoo.com\'}")]
        public async void CreateContactTest(string json)
        {
            var content = new StringContent(
              json,
              System.Text.Encoding.UTF8,
              "application/json"
              );
            var response = await Client.PostAsync("/Contacts", content);
            var result = response.Content.ReadAsStringAsync().Result;
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            //Assert.Equal(HttpStatusCode.Created, result);
        }

        // TEST NAME - updateEntry
        // TEST DESCRIPTION - Update Contact details
        [Theory]
        [InlineData(32, "{\'id\':32,\'name\':{\'first\':\'Joe\',\'middle\':\'Francis\',\'last\':\'test\'},\'address\':{\'street\':\'8 High Summmer Row\',\'city\':\'Cannon\',\'state\':\'Delaware\',\'zip\':\'19797\'},\'phone\':[{\'number\':\'302-611-9148\',\'type\':\'home\'},{\'number\':\'347-532-9427\',\'type\':\'mobile\'}],\'email\':\'joe.test@yahoo.com\'}")]
        public async void UpdateContactTest(int id, string json)
        {
            var content = new StringContent(
                         json,
                         System.Text.Encoding.UTF8,
                         "application/json"
                         );
            var response = await Client.PutAsync("/Contacts/" + id, content);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("Contact Updated", response.Content.ReadAsStringAsync().Result);
        }

        // TEST NAME - deleteEntry
        // TEST DESCRIPTION - Delete a Contact by id
        [Theory]
        [InlineData(32)]
        public async void DeleteContactTest(int id)
        {
            var response = await Client.DeleteAsync("/Contacts/" + id);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("Contact deleted", response.Content.ReadAsStringAsync().Result);
        }

        #endregion

        #region Helper Methods

        public async Task<HttpResponseMessage> GetResponseAsync(string uri)
        {
            return await Client.GetAsync(uri);
        }

        public string getreponse(string url)
        {
            //Create a WebRequest to get the file
            HttpWebRequest fileReq = (HttpWebRequest)HttpWebRequest.Create(url);

            //Create a response for this request
            HttpWebResponse fileResp = (HttpWebResponse)fileReq.GetResponse();

            if (fileReq.ContentLength > 0)
                fileResp.ContentLength = fileReq.ContentLength;

            //Get the Stream returned from the response
            Stream stream = fileResp.GetResponseStream();
            byte[] buffer;
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                buffer = ms.ToArray();
            }
            return Encoding.UTF8.GetString(buffer);
        }

        public string getpostresponse(string url, string parameter)
        {
            using (var wb = new WebClient())
            {
                var data = new NameValueCollection();
                data["r"] = parameter;

                var response = wb.UploadValues(url, "POST", data);
                return Encoding.UTF8.GetString(response);
            }
        }


        private void SetUpClient()
        {
            ContactsAPI.Globals globals = new ContactsAPI.Globals();
            globals.ConnectionString = "DataSource=D:\\tfs\\web\\Contacts API\\ContactsAPI\\Contacts.db";
            
            var builder = new WebHostBuilder()
                .UseStartup<ContactsAPI.Startup>()
                .ConfigureServices(services =>
                {
                    var context = new contactsContext(new DbContextOptionsBuilder<contactsContext>()
                        //.UseSqlite("DataSource=:memory:")
                        .UseSqlite("DataSource=D:\\tfs\\web\\Contacts API\\ContactsAPI\\Contacts.db")
                        .EnableSensitiveDataLogging()
                        .Options, globals);

                    services.RemoveAll(typeof(contactsContext));
                    services.AddSingleton(context);

                    context.Database.OpenConnection();
                    context.Database.EnsureCreated();

                    context.SaveChanges();

                    // Clear local context cache
                    foreach (var entity in context.ChangeTracker.Entries().ToList())
                    {
                        entity.State = EntityState.Detached;
                    }
                    repository = new Repository(context);
                });

            _server = new TestServer(builder);

            Client = _server.CreateClient();
 
        }

        #endregion
    }
}
