using prjMvcDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjMvcDemo.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult List()
        {   
            //加入關鍵字跟全查詢兩種方式
            string keyword = Request.Form["txtKeyword"];

            List<CCustomers> datas = null;
            if (string.IsNullOrEmpty(keyword))
                datas = (new CCustomerFactory()).queryAll();
            else
                datas = (new CCustomerFactory()).queryByKeyword(keyword); ;

            ////宣告客戶參數，並指定為所有客戶資料
            //List<CCustomers> datas = (new CCustomerFactory()).queryAll();
            return View(datas);
        }

        public ActionResult Create()
        {       
            return View();
        }

        public ActionResult Save()
        {
            //新增客戶物件並實體化
            CCustomers x = new CCustomers();
            //客戶屬性代入參數
            x.fName = Request.Form["txtName"];
            x.fPhone = Request.Form["txtPhone"];
            x.fEmail = Request.Form["txtEmail"];
            x.fAddress = Request.Form["txtAddress"];
            x.fPassword = Request.Form["txtPassword"];
            //透過客戶的方法來新增資料
            (new CCustomerFactory()).create(x);
            //回到客戶資料列表
            return RedirectToAction("List");
        }

        public ActionResult Delete(int? id)
        {
            if (id != null) 
                (new CCustomerFactory()).delete((int)id);
                
            //回到客戶資料列表
            return RedirectToAction("List");
        }

        //edit , get ；預設為HttpGet
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("List");
            CCustomers x = (new CCustomerFactory()).queryByFId((int)id); 
                      
            return View(x);
        }

        //因為有兩個Edit方法，html無法辨識，所以要說明是post還是get
        [HttpPost]
        public ActionResult Edit(CCustomers x)
        {
            //如果不想new，可以直接變成參數↑，就可以省略不寫            
            //CCustomers x = new CCustomers();
            //因為name的名稱與屬性一樣，所以可以省略不寫，系統會自動帶入
            //x.fName = Request.Form["txtName"];
            //x.fPhone = Request.Form["txtPhone"];
            //x.fEmail = Request.Form["txtEmail"];
            //x.fAddress = Request.Form["txtAddress"];
            //x.fPassword = Request.Form["txtPassword"];
            //因此程式碼可以簡化如下
            (new CCustomerFactory()).update(x);

            return RedirectToAction("List");
        }
    }
}