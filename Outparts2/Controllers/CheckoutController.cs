using Microsoft.AspNetCore.Mvc;
using Outparts2.Helpers;
using Outparts2.Models;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Outparts2.Controllers
{
    public class CheckoutController : Controller
    {
        /*
         * Note that TotalAmount property is decorated with TempData attribute
         * This attribute ensures that the TotalAMount is persistent across posts to anohter action method
         * which will be known as "Processing action method
         * 
         * The IndexAction method creates and assigns a cart to the dynamic ViewBag oject. 
         * It then gets the total price amount of products in the cart in dollar value
         * The dollar value is internally converted to cents and rounded up to 2 places of decimal
         * This is necesarry as the Stripe Gateway deals in cents only for calculations
         * The rounded up total price in cents is then converted to long data type (As stripe considers amount to be of long)
         * 
         */
        [TempData]
        public string TotalAmount { get; set; }
        public IActionResult Index()
        {
            var cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            ViewBag.cart = cart;
            ViewBag.DollarAmount = cart.Sum(item => item.Product.Price * item.Quantity);
            ViewBag.total = Math.Round(ViewBag.DollarAmount, 3) * 100;
            ViewBag.total = Convert.ToInt64(ViewBag.total);
            long total = ViewBag.total;
            TotalAmount = total.ToString();
            TotalAmount = String.Format("{0:#.00}", TotalAmount);
            return View();
        }

        /*
         * Action Method called Processing 
         * 
         * This method is responsible for processing the checkout and is decorated with an HttpPost as it is of Post type
         * Here the customer object is created with options to fill the customer details such as:
         * Email, Name, Phone... in the CustomerCreateOptions object. 
         * These objects are then passed to a CustomerService type through the latter's Create method to create the customer
         * 
         * Similarly a charge object is created with options to fill the charge details such as: Amount, Currency, Description, Source, Receipt email...
         * in the ChargeCreateOPtions object. These options are passed to a ChargeService type through the latter's Create method to create the charge
         * 
         * Now how to process when the charge (payment) succeeds? 
         * The method we use in this project is to show properties such as the Customer name, Amount paid etc. 
         * in the Processing View when the charge status is "succeeded" as the response of the payment request
         */
        [HttpPost]
        public IActionResult Processing(string stripeToken, string stripeEmail)
        {
            var optionsCust = new CustomerCreateOptions
            {
                Email = stripeEmail,
                Name = "Jim",
                Phone = "04-234567"

            };
            var serviceCust = new CustomerService();
            Customer customer = serviceCust.Create(optionsCust);
            var optionsCharge = new ChargeCreateOptions
            {
                /*Amount = HttpContext.Session.GetLong("Amount")*/
                Amount = Convert.ToInt64(TempData["TotalAmount"]),
                Currency = "USD",
                Description = "Buying Flowers",
                Source = stripeToken,
                ReceiptEmail = stripeEmail,

            };
            var service = new ChargeService();
            Charge charge = service.Create(optionsCharge);
            if (charge.Status == "succeeded")
            {
                string BalanceTransactionId = charge.BalanceTransactionId;
                ViewBag.AmountPaid = Convert.ToDecimal(charge.Amount) % 100 / 100 + (charge.Amount) / 100;
                ViewBag.BalanceTxId = BalanceTransactionId;
                ViewBag.Customer = customer.Name;
                //return View();
            }

            return View();
        }





    }
}
