using System;
using System.Collections.Generic;

namespace ContactsAPI.Model
{
    public partial class Phone
    {
        public long IPhone { get; set; }
        public string Number { get; set; }
        public string type { get; set; }
        public long ContactId { get; set; }
    }
}
