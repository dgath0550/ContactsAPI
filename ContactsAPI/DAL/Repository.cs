using ContactsAPI.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace ContactsAPI
{
    /// <summary>
    /// Inherits IRepository interface
    /// </summary>
    public class Repository : IRepository
    {
        #region Globals/Constructors

        private readonly contactsContext _context;

        public Repository(contactsContext context)
        {
            _context = context;
        }

        #endregion

        #region SQLLite Entity CRUD operations

        /// <inheritdoc />
        public List<ContactJson> GetContact(long id)
        {
            List<ContactJson> cl = new List<ContactJson>();
            IQueryable<Contact> temp = (id == 0) ? _context.Contact : _context.Contact.Where(c => c.Id == id);

            foreach (Contact c in temp)
            {
                ContactJson cc = new ContactJson(c.Id,
                    _context.Name.First(n => n.IName == c.IName),
                    _context.Address.First(a => a.IAddress == c.IAddress),
                    _context.Phone.Where(p => p.ContactId == c.Id).ToList(),
                    c.Email);
                cl.Add(cc);
            }

            return cl;
        }

        /// <inheritdoc />
        public async Task<ContactJson> AddContact(ContactJson Contact)
        {
            if (!_context.Name.Any(n => n.First == Contact.name.First && n.Last == Contact.name.Last))
            {
                Contact.name = _context.Name.Add(Contact.name).Entity;
                Contact.address = _context.Address.Add(Contact.address).Entity;
                await _context.SaveChangesAsync();

                Contact contact = new Contact(Contact.name.IName, Contact.address.IAddress, Contact.email);
                contact = _context.Contact.Add(contact).Entity;
                await _context.SaveChangesAsync();

                foreach (Phone phone in Contact.phone)
                {
                    phone.ContactId = contact.Id;
                    _context.Phone.Add(phone);
                }

                Contact.Id = contact.Id;
                await _context.SaveChangesAsync();
            }
            return Contact;
        }

        /// <inheritdoc />
        public async Task<dynamic> UpdateContact(long id, ContactJson Contact)
        {
            //Get contact object
            Contact contact = _context.Contact.Where(c => c.Id == id).FirstOrDefault();

            //Update if exist
            if (contact != null)
            {
                //Update from contact table first
                contact.Email = Contact.email;
                contact = _context.Contact.Update(contact).Entity;

                //Update name table
                Name name = _context.Name.First(n => n.IName == contact.IName);
                name.First = Contact.name.First;
                name.Last = Contact.name.Last;
                name.Middle = Contact.name.Middle;
                _context.Name.Update(name);

                //Update address table
                Address address = _context.Address.First(a => a.IAddress == contact.IAddress);
                address.Street = Contact.address.Street;
                address.City = Contact.address.City;
                address.State = Contact.address.State;
                address.Zip = Contact.address.Zip;
                _context.Address.Update(address);

                //Get the right Phone List object based on contact id (foreign key)
                List<Phone> phonelist = _context.Phone.Where(p => p.ContactId == contact.Id).ToList();
                //Delete existing phones since primary key (iPhone) is not being passed
                foreach (Phone phone in phonelist)
                {
                    _context.Phone.Remove(phone);
                }
                //Add Phone list from input 
                foreach (Phone phone in Contact.phone)
                {
                    phone.ContactId = contact.Id;
                    _context.Phone.Add(phone);
                }

                await _context.SaveChangesAsync();    //commit changes

                return "Contact Updated";
            }
            else
                return "Contact not found";
        }

        /// <inheritdoc />
        public async Task<string> DeleteContact(long id)
        {
            //Get contact object
            Contact contact = _context.Contact.Where(c => c.Id == id).FirstOrDefault();

            //delete if exist
            if (contact != null)
            {
                //Delete from contact table first
                contact = _context.Contact.Remove(contact).Entity;
                _context.SaveChanges();   //commit changes

                //Delete dependencies
                Name name = _context.Name.First(n => n.IName == contact.IName);
                _context.Name.Remove(name);

                Address address = _context.Address.First(a => a.IAddress == contact.IAddress);
                _context.Address.Remove(address);

                //Get the right Phone List object based on contact id (foreign key)
                List<Phone> phonelist = _context.Phone.Where(p => p.ContactId == contact.Id).ToList();
                foreach (Phone phone in phonelist)
                {
                    _context.Phone.Remove(phone);
                }

                await _context.SaveChangesAsync();    //commit changes

                return "Contact deleted";
            }
            else
                return "Contact not found";
        }

        ///<inheritdoc/>
        public long GetLastContactId()
        {
            return _context.Contact.Max(c => c.Id);
        }

        #endregion
    }
}
