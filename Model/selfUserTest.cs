using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WhatsappWebAPI.Model
{
    public class selfUserTest
    {


        [Required]
        public int userID { get; set; }
        [Required]
        public int AssessmentMasterID { get; set; }
        [Required]
        public int AssessmentOptionID { get; set; }
        
        public string userSelfTestDescription { get; set; }
        [Required]
        public string selfTestDate { get; set; }
       

    }



}


