using ContactsAPI.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContactsAPI
{
    /// <summary>
    /// IRepository interface
    /// </summary>
    public interface IRepository
    {
        //int i { get; set; }
        //event EventHandler Datachanged;

        /// <summary>
        /// Gets contacts
        /// </summary>
        /// <returns>Gets contacts</returns>
        List<ContactJson> GetContact(long Id);

        /// <summary>
        /// Add new Contact to the database
        /// </summary>
        /// <param name="Contact">Contact to add</param>
        /// <returns>Added Contact</returns>
        Task<ContactJson> AddContact(ContactJson Contact);


        /// <summary>
        /// Saves updated Contact
        /// </summary>
        /// <param name="id">ID to update</param>
        /// <param name="Contact">User input Contact to save</param>
        /// <returns>Saved Contact</returns>
        Task<dynamic> UpdateContact(long id, ContactJson Contact);

        /// <summary>
        /// Deletes Contact by id
        /// </summary>
        /// <returns>Status of the delete request</returns>
        Task<string> DeleteContact(long id);
    }
}
