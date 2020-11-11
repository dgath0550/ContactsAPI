using System;
using System.Collections.Generic;

namespace ContactsAPI.Model
{
    public partial class Address
    {
        public Address()
        { }

        public long IAddress { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }

    }
}
