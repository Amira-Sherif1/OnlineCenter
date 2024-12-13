using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public interface IStripeService
    {
        Task<Stripe.Checkout.Session> GetSessionAsync(string sessionId);
        Task<Stripe.Checkout.Session> CreateCheckoutSessionAsync(int lectureId, string userId, decimal price, string title);
    }
}
