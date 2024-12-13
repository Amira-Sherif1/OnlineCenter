using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ApplicationUser :IdentityUser
    {
        public string Address { get; set; }
        public string Image { get; set; }

        public string AdminId { get; set; }
        public Admin Admin { get; set; }

        public string StudentId { get; set; }
        public Student Student { get; set; }

        public string TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public String AssistantId { get; set; }
        public Assistant Assistant { get; set; }

    }
}
