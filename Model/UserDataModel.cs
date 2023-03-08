using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImageUpload
{
    public class UserDataModel
    {
      

        [Required]
        public IFormFile ProfileImage { get; set; }
        
    }
}
