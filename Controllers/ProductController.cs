using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjMvcDemo.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult List()
        {
            string keyword = Request.Form["txtKeyword"];
            dbDemoEntities db = new dbDemoEntities();
            //datas的形態可以透過List.cshtml去查看
            IEnumerable<tProduct> datas = null;
            //當查不到關鍵字，顯示所有產品
            if (string.IsNullOrEmpty(keyword))
            {
                datas = from c in db.tProduct
                        select c;
            }
            //否則，顯示商品名稱符合關鍵字模糊查詢的所有商品
            else
                datas = db.tProduct.Where(p => p.fName.Contains(keyword));

            return View(datas);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Create(tProduct p)
        {
            dbDemoEntities db =new dbDemoEntities();
            db.tProduct.Add(p);
            db.SaveChanges();

            return RedirectToAction("List");
        }

        public ActionResult Delete(int?id)
        {
            dbDemoEntities db = new dbDemoEntities();
            tProduct prod = db.tProduct.FirstOrDefault(t=>t.fId==id);
            if (prod != null)
            {
                db.tProduct.Remove(prod);
                db.SaveChanges ();
            }

            return RedirectToAction("List");
        }

        public ActionResult Edit(int? id)
        {
            if(id==null)
                return RedirectToAction("List");

            dbDemoEntities db = new dbDemoEntities();
            tProduct prod = db.tProduct.FirstOrDefault(t => t.fId == id);
            if (prod == null)
                return RedirectToAction("List");

            return View(prod);
        }

        [HttpPost]

        public ActionResult Edit(tProduct p)
        {
            dbDemoEntities db = new dbDemoEntities();
            tProduct prod = db.tProduct.FirstOrDefault(t => t.fId == p.fId);
            if (prod != null)
            {
                prod.fName = p.fName;
                prod.fQty = p.fQty;
                prod.fCost = p.fCost;
                prod.fPrice = p.fPrice;
                db.SaveChanges();
            }

            return RedirectToAction("List");
        }
    }
}