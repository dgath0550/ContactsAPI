using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ContactsAPI.Model
{
    /// <summary>
    /// Internal generated Entity Contact definitions with system ID's
    /// </summary>
    public partial class Contact
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public Contact() { }

        /// <summary>
        /// Custom constructor
        /// </summary>
        /// <param name="iName"></param>
        /// <param name="iAddress"></param>
        /// <param name="email"></param>
        public Contact(long iName, long iAddress ,string email)
        {
            IName = iName;
            IAddress = iAddress;
            Email = email;
        }

        public long Id { get; set; }
        public long? IName { get; set; }
        public long? IAddress { get; set; }
        public string Email { get; set; }
    }

    /// <summary>
    /// External JSON input/output.
    /// Example: [{\"id\":0,\"name\":{\"first\":\"Harold\",\"middle\":\"Francis\",\"last\":\"Gilkeys\"},\"address\":{\"street\":\"8 High Autumn Row\",\"city\":\"Cannon\",\"state\":\"Delaware\",\"zip\":\"19797\"},\"phone\":[{\"number\":\"929-532-9427\",\"type\":\"mobile\"},{\"number\":\"347-611-9148\",\"type\":\"home\"}],\"email\":\"harold.gilkeys@yahoo.com\"}]
    /// </summary>
   public class ContactJson
    {
        public ContactJson() {}

        public ContactJson(long _Id, Name _name, Address _address, List<Phone> _phones, string _email)
        {
            Id = _Id;
            name = _name;
            address = _address;
            phone = _phones;
            email = _email;
        }

        public long Id { get; set; }

        [Required]
        public Name name { get; set; }

        [Required]
        public Address address { get; set; }

  
        [Required] 
        public IEnumerable<Phone> phone { get; set; } 

        [Required]
        public string email { get; set; }

    }
}
