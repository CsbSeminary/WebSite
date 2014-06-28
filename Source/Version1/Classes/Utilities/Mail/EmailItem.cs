using System;

namespace Csbs.Utilities
{
    public class EmailItem
    {
        public String From { get; set; }
        public String To { get; set; }
        public String Cc { get; set; }
        public String Bcc { get; set; }
        public String Subject { get; set; }
        public String Body { get; set; }
    }
}