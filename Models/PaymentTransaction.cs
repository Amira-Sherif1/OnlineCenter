using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class PaymentTransaction
    {
        public int Id { get; set; }
        public int LectureId { get; set; }
        public string UserId { get; set; }
        public string StripeSessionId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual Lecture Lecture { get; set; }
    }
}
