using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhatsappWebAPI.Model
{
    public class UserReport
    {
        public int msgMasterId { get; set; }
        public string reportName { get; set; }

        public string mobileNo { get; set; }
        public DateTime createdDate { get; set; }
        public string msgGroup { get; set; }
    }
}
