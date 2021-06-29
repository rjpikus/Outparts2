using Microsoft.AspNetCore.Mvc;
using Outparts2.Helpers;
using Outparts2.Models;
using Outparts2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Outparts2.Controllers
{
    public class CartController : Controller
    {  
        //The index action sets a cart object in the session with the object
        //from the Session Helper class using HttpContext
        //It then sets a dynamic ViewBag object with the sum total price
        //that it reads from the cart, rounding it to two decimal places and returns the view

        public IActionResult Index()
        {
            var cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            ViewBag.cart = cart;
            ViewBag.total = cart.Sum(item => item.Product.Price * item.Quantity);
            ViewBag.total = Math.Round(ViewBag.total, 2);
            ViewBag.total = String.Format("{0:#.00}", ViewBag.total);
            return View();
        }

        //The isExist method checks each object in the cart with a given id and if the object exists
        //returns its index otherwise it returns -1

        private int isExist(string id)
        {
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Product.Id.Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }

        public IActionResult Buy(string id)
        {
            ProductModel productModel = new ProductModel();
            if (SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart") == null)
            {
                List<Item> cart = new List<Item>();
                cart.Add(new Item { Product = productModel.find(id), Quantity = 1 });
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
                int index = isExist(id);
                if (index != -1)
                {
                    cart[index].Quantity++;
                }
                else
                {
                    cart.Add(new Item { Product = productModel.find(id), Quantity = 1 });
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Remove(string id)
        {
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            int index = isExist(id);
            cart.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("Index");

        }
    }
}
