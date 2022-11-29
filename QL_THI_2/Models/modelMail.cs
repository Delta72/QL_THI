using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QL_THI_2.Models
{
    public class modelMail
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        // public IFormFile Attachment { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
