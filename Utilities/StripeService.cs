using Microsoft.Extensions.Configuration;
using Stripe.Checkout;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    // Services/StripeService.cs
    public class StripeService : IStripeService
    {
        //private readonly IConfiguration _configuration;
        public IConfiguration _configuration { get; set; }

        public StripeService(IConfiguration configuration)
        {
            _configuration = configuration;
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
        }

        public async Task<Session> GetSessionAsync(string sessionId)
        {
            var service = new SessionService();
            return await service.GetAsync(sessionId);
        }

        public async Task<Session> CreateCheckoutSessionAsync(int lectureId, string userId, decimal price, string title)
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "egp",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = title,
                        },
                        UnitAmount = (long)(price * 100),
                    },
                    Quantity = 1,
                }
            },
                Mode = "payment",
                SuccessUrl = $"{_configuration["AppUrl"]}/checkout/success?session_id={{CHECKOUT_SESSION_ID}}&lecture_id={lectureId}",
                CancelUrl = $"{_configuration["AppUrl"]}/checkout/cancel?lecture_id={lectureId}",
                ClientReferenceId = userId,
            };

            var service = new SessionService();
            return await service.CreateAsync(options);
        }
    }

}
