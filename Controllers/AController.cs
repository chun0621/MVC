using prjMvcDemo.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Versioning;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace prjMvcDemo.Controllers
{
    public class AController : Controller
    {
        static int count = 0;

        public ActionResult showCountByCookie()
        {
            int count = 0;
            HttpCookie x = Request.Cookies["COUNT"];
            if (x != null)
                count = Convert.ToInt32(x.Value);
            count++;
            x = new HttpCookie("COUNT");
            x.Value = count.ToString();
            x.Expires = DateTime.Now.AddSeconds(20);//設定cookie過期時間(20秒)
            Response.Cookies.Add(x);

            ViewBag.COUNT = count;
            return View();
        }


        //在不同瀏覽器時，次數會分別計算
        //獲得session時，還有當下時間，若下次session變更>20分則會重置
        public ActionResult showCountBySession()
        {
            //這邊不使用全域變數
            int count = 0;
            if (Session["COUNT"] != null)
                count = (int)Session["COUNT"];
            count++;
            Session["COUNT"] = count;
            ViewBag.COUNT = count;
            return View();
        }

        //次數會累積計算
        public ActionResult showCount()
        {
            count++;
            ViewBag.COUNT = count;
            return View();
        }

        public string testingInsert()
        {
            CCustomers x = new CCustomers()
            {
                fName = "12345",
                //fPhone = "0998765432",
                fEmail = "12345@gmail.com",
                fAddress = "556678",
                fPassword = "1234",
            };

            (new CCustomerFactory()).create(x);
            return "新增資料成功";
        }

        public string testingDelete(int? id)
        {
            if (id == null)
                return "沒有指定id";
            (new CCustomerFactory()).delete((int)id);
            return "刪除資料成功";
        }

        public string testingUpdate()
        {
            CCustomers x = new CCustomers()
            {
                fId=4,
                //fName = "12345",
                fPhone = "0998765432",
                //fEmail = "12345@gmail.com",
                //fAddress = "556678",
                //fPassword = "1234",
            };

            (new CCustomerFactory()).update(x);
            return "修改資料成功";
        }

        public string testingQuery()
        {
            return "目前客戶數 :" + (new CCustomerFactory()).queryAll().Count.ToString();
        }

        public string sayHello()
        {
            return "Hello ASP.NET MVC";
        }

        [NonAction]
        //往下無論行數離最近的方法無法被網址使用
        public string lotto() 
        {
            return (new CLottoGen()).getNumber();
            
            //Random x = new Random();
            ////int[] numbers = new int[6];
            //List<int> numbers = new List<int>();

            //for (int i = 0; i < 6; i++)
            //{
            //    int n;
            //    //當有重複值，重新取值
            //    do
            //    {
            //     n = x.Next(1, 50);
            //    }
            //    while (numbers.Contains(n));//對比是否有跟陣列內的值相同

            //    //numbers[i] = n;   
            //    numbers.Add(n);

            //}
            ////小到大排序
            ////Array.Sort(numbers);
            //numbers.Sort();

            //string result = "";
            //foreach (int n in numbers)
            //{
            //    result += n.ToString()+ " ";
            //}

            //return result;
        }

        //透過網址傳遞訊息給server,選項內容用 ? 
        public string demoRequest()
        {
            string id = Request.QueryString["pid"];
            if (id == "0")
                return "XBox加入購物車成功";
            else if (id == "1")
                return "PS5加入購物車成功";
            else if (id == "2")
                return "Nintendo Switch加入購物車成功";
            //else
            return "找不到開發產品資料";
        }

        //透過網址的參數傳遞訊息給server,選項內容用 ? 
        //但不填pid會發生null的錯誤
        public string demoParameter(int pid)
        {
            //string id = Request.QueryString["pid"];
            if (pid == 0)
                return "XBox加入購物車成功";
            else if (pid == 1)
                return "PS5加入購物車成功";
            else if (pid == 2)
                return "Nintendo Switch加入購物車成功";
            //else
            return "找不到開發產品資料";
        }

        //可以透過?來不填,獲得未找到的參數回應
        //當參數為單一且名稱為id 可以省略 ?id/ 不寫 , 直接用參數值回應
        public string addToCart(int? productId)
        {
            if (productId == 125)
                return "XBox 加入購物車成功";
            return "找不到該產品資料";
        }

        //取得網頁實體目錄
        //「.」：網頁目前路徑
        //「..」：網頁的上一層路徑
        //「~」：根目錄路徑
        public string demoServer()
        {
            return "目前伺服器上的實體位置：" + Server.MapPath(".");
        }

        //使用 Response 實作檔案下載
        public string demoResponse()
        {
            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.Filter.Close();
            Response.WriteFile(@"C:\note\01.jpg");
            Response.End();
            return "";
        }

        //用ADO.NET連server進行資料比對
        public string queryByFid(int? id)
        {
            if (id == null)
                return "沒有指定id";

            SqlConnection con = new SqlConnection();
            con.ConnectionString = @"Data Source=.;Initial Catalog=dbDemo;Integrated Security=True";
            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM tCustomer WHERE fid=" + id.ToString(),con);
            SqlDataReader reader =cmd.ExecuteReader();
            string s = "沒有符合條件的資料";
            //換行要注意改用html的寫法
            if (reader.Read())
            {
                s = reader["fName"].ToString() + "<br>" + reader["fPhone"].ToString();
            }
               
            con.Close();
            return s;
        }

        //檢視名稱必須跟方法名稱一樣，否則功能網頁會不正常
        public ActionResult showByFid(int? id)
        {
       
            if (id != null)
            { 
                SqlConnection con = new SqlConnection();
                con.ConnectionString = @"Data Source=.;Initial Catalog=dbDemo;Integrated Security=True";
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM tCustomer WHERE fId=" + id.ToString(), con);
                SqlDataReader reader = cmd.ExecuteReader();
                             
                if (reader.Read())
                {
                    CCustomers x = new CCustomers()
                    {
                        fId = (int)reader["fId"],
                        fName = reader["fName"].ToString(),
                        fPhone = reader["fPhone"].ToString(),
                        fEmail = reader["fEmail"].ToString(),
                        fAddress = reader["fAddress"].ToString(),
                        fPassword = reader["fPassword"].ToString(),
                    };
                    ViewBag.KK = x;
            }

                con.Close();
            }
            return View();
        }

        public ActionResult bindingByFid(int? id)
        {
            CCustomers x = null;
            if (id != null)
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = @"Data Source=.;Initial Catalog=dbDemo;Integrated Security=True";
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM tCustomer WHERE fId=" + id.ToString(), con);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    x = new CCustomers()
                    {
                        fId = (int)reader["fId"],
                        fName = reader["fName"].ToString(),
                        fPhone = reader["fPhone"].ToString(),
                    };
                   
                }

                con.Close();
            }
            return View(x);
        }

        public ActionResult demoForm()
        {
            ViewBag.ANS = "?";
            //ViewBag.A = "";
            //ViewBag.B = "";
            //ViewBag.C = "";
            if (!string.IsNullOrEmpty(Request.Form["txtA"]))
            {
                double a = Convert.ToDouble(Request.Form["txtA"]);
                double b = Convert.ToDouble(Request.Form["txtB"]);
                double c = Convert.ToDouble(Request.Form["txtC"]);
                ViewBag.A = a;
                ViewBag.B = b;
                ViewBag.C = c;
                double d = System.Math.Sqrt(b*b-4*a*c);
                ViewBag.ANS = ((-b + d) / (2 * a)).ToString("0.0#") + //#是1~9，即0.80會顯示為0.8
                    " Or X=" + ((-b - d) / (2 * a)).ToString("0.0#");
            }

            return View();
        }


            // GET: A
            public ActionResult Index()
        {
            return View();
        }
    }
}