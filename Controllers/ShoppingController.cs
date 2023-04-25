using prjMvcDemo.Models;
using prjMvcDemo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Tokenizer.Symbols;

namespace prjMvcDemo.Controllers
{
    public class ShoppingController : Controller
    {
        public ActionResult CartView()
        {
            //新增購物車cart 用session紀錄
            List<CShoppingCartItem> cart = 
                Session[CDictionary.SK_PURCHASED_PRODUCTS_LIST] as List<CShoppingCartItem>;
            //如果購物車為空，則返回產品列表
            if (cart ==null)
                return RedirectToAction("List");
            return View(cart);
        }


        // GET: Shopping
        public ActionResult List()
        {
            dbDemoEntities db = new dbDemoEntities();

            var datas = from c in db.tProduct
                        select c;

            return View(datas);
        }

        public ActionResult AddToCart(int? id)
        {
            if (id == null)
                return RedirectToAction("List");

            ViewBag.FId = id;
            return View();
        }

        [HttpPost]
        public ActionResult AddToCart(CAddToCarViewModel vm)
        {
            dbDemoEntities db = new dbDemoEntities();
            tProduct prod = db.tProduct.FirstOrDefault(t => t.fId == vm.txtFId);
            if (prod != null)
            {
                tShoppingCart x =new tShoppingCart();
                x.fProductId = vm.txtFId;
                x.fPrice = prod.fPrice;
                x.fDate = DateTime.Now.ToString("yyyyMMddHHmmss");
                x.fCount = vm.txtCount;
                x.fCustomerId = 1;
                db.tShoppingCart.Add(x);
                db.SaveChanges();
            }

            return RedirectToAction("List");
        }

        public ActionResult AddToSession(int? id)
        {
            if (id == null)
                return RedirectToAction("List");

            ViewBag.FId = id;
            return View();
        }

        [HttpPost]
        public ActionResult AddToSession(CAddToCarViewModel vm)
        {
            dbDemoEntities db = new dbDemoEntities();
            tProduct prod = db.tProduct.FirstOrDefault(t => t.fId == vm.txtFId);
            if (prod != null)
            {
                List<CShoppingCartItem> cart = 
                    Session[CDictionary.SK_PURCHASED_PRODUCTS_LIST] as List<CShoppingCartItem>;  
                if (cart == null)
                {
                    cart = new List<CShoppingCartItem>();
                    Session[CDictionary.SK_PURCHASED_PRODUCTS_LIST] = cart;
                }
                CShoppingCartItem item = new CShoppingCartItem();
                item.price = (decimal)prod.fPrice;
                item.productId = vm.txtFId;
                item.count = vm.txtCount;
                item.product = prod;
                cart.Add(item);

            }

            return RedirectToAction("List");
        }

    }
}