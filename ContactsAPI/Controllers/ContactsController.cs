using ContactsAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace ContactsAPI.Controllers
{
    /// <summary>
    /// Performs CRUD operations on contacts DB
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ContactsController : ControllerBase
    {
        IRepository repository;

        /// <summary>
        /// Injected Repository service
        /// </summary>
        /// <param name="_repository"></param>
        public ContactsController(IRepository _repository)
        {
            repository = _repository;
        }

        /// <summary>
        /// Get Contact
        /// </summary>
        /// <param name="id">Contact ID(0 to get all contacts)</param>
        /// <returns>Single Contact JSON Object or all of Contacts if ID is equal to 0</returns>
        [HttpGet("{id?}")]
        public ActionResult<List<ContactJson>> Get(int id)
        {
            return repository.GetContact(id).ToList();
        }

        /// <summary>
        /// Adds a new Contact
        /// </summary>
        /// <param name="contact">Contact JSON Object from request body</param>
        /// <returns>HttpResponseMessage with custom status message</returns>
        [Consumes("application/json")]
        [HttpPost()]
        public ActionResult<HttpResponseMessage> Add([FromBody][Required] ContactJson contact)
        {
            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.Created);
            if (!ModelState.IsValid)
            {
                message.StatusCode = HttpStatusCode.InternalServerError;
                message.ReasonPhrase = "Invalid JSON parameter";
            }
            if(repository.AddContact(contact).Result.Id == 0)
            {
                message.StatusCode = HttpStatusCode.InternalServerError;
                message.ReasonPhrase = "Contact already exist";
            }
            
            return message;
        }

        /// <summary>
        /// Updates a Contact
        /// </summary>
        /// <param name="id">Contact ID</param>
        /// <param name="contact">Updated Contact JSON Object from request body</param>
        /// <returns>Status of the update operation</returns>
        [HttpPut("{id}")]
        public ActionResult<string> Update([Required]int id, [FromBody][Required] ContactJson contact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return repository.UpdateContact(id, contact).Result;
        }

        /// <summary>
        /// Deletes a Contact based on ID
        /// </summary>
        /// <param name="id">Contact ID</param>
        /// <returns>Status of the delete operation</returns>
        [HttpDelete("{id}")]
        public ActionResult<string> Delete([Required]int id)
        {
            return repository.DeleteContact(id).Result;
        }

    }
}
