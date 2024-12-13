using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Cart
    {
        public int BookId { get; set; }
        public Book Book { get; set; }
        public string StudentId { get; set; }
        public Student Student { get; set; }

    }
}
