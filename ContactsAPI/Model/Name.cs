using System;
using System.Collections.Generic;

namespace ContactsAPI.Model
{
    public partial class Name
    {
        public long IName { get; set; }
        public string First { get; set; }
        public string Middle { get; set; }
        public string Last { get; set; }
    }
}
