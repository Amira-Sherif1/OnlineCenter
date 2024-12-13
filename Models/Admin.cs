using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Admin
    {
        [ForeignKey("ApplicationUser")]
        public string ApplicationuserId { get; set; }
        public ApplicationUser Applicationuser { get; set; }

    }
}
