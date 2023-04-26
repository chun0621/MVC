using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Helpers;
using System.Web.UI.WebControls.WebParts;

namespace prjMvcDemo.Models
{
    public class CCustomerFactory
    {
        public void create(CCustomers p)
        {
            List<SqlParameter> paras = new List<SqlParameter>();
            string sql = "INSERT INTO tCustomer(";
            //當有輸入值才需要做存儲
            if (!string.IsNullOrEmpty(p.fName))
                sql += " fName,";
            if (!string.IsNullOrEmpty(p.fPhone))
                sql += " fPhone,";
            if (!string.IsNullOrEmpty(p.fEmail))
                sql += " fEmail,";
            if (!string.IsNullOrEmpty(p.fAddress))
                sql += " fAddress,";
            if (!string.IsNullOrEmpty(p.fPassword))
                sql += " fPassword,";
            //最後一個要去掉,
            if (sql.Trim().Substring(sql.Trim().Length - 1, 1) == ",")
                sql = sql.Trim().Substring(0, sql.Trim().Length - 1);
            sql += " )VALUES(";
            //當有輸入值才需要做存儲
            if (!string.IsNullOrEmpty(p.fName))
            {
                sql += " @K_FNAME,";
                paras.Add(new SqlParameter("K_FNAME", (object)p.fName));
            }
            if (!string.IsNullOrEmpty(p.fPhone))
            {
                sql += " @K_FPHONE,";
                paras.Add(new SqlParameter("K_FPHONE", (object)p.fPhone));
            }
            if (!string.IsNullOrEmpty(p.fEmail))
            {
                sql += " @K_FEMAIL,";
                paras.Add(new SqlParameter("K_FEMAIL", (object)p.fEmail));
            }
            if (!string.IsNullOrEmpty(p.fAddress))
            {
                sql += " @K_FADDRESS,";
                paras.Add(new SqlParameter("K_FADDRESS", (object)p.fAddress));
            }
            if (!string.IsNullOrEmpty(p.fPassword))
            {
                sql += " @K_PASSWORD,";
                paras.Add(new SqlParameter("K_PASSWORD", (object)p.fPassword));
            }
            //最後一個要去掉,
            if (sql.Trim().Substring(sql.Trim().Length - 1, 1) == ",")
                sql = sql.Trim().Substring(0, sql.Trim().Length - 1);
            sql += ")";
            execulteSql(sql, paras);
        }

        public void delete(int fId)
        {
            string sql = "DELETE FROM tCustomer WHERE fId=@K_FID ";
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("K_FID", (object)fId));
            execulteSql(sql, paras);
        }
         
        //將輸入值用參數代入，但值的數量不一定
        //所以用List
        private void execulteSql(string sql, List<SqlParameter> paras)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = @"Data Source=.;Initial Catalog=dbDemo;Integrated Security=True";
            con.Open();
            SqlCommand cmd = new SqlCommand(sql, con);
            if (paras != null)
                cmd.Parameters.AddRange(paras.ToArray());
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void update(CCustomers p)
        {
            List<SqlParameter> paras = new List<SqlParameter>();
            string sql = "UPDATE tCustomer SET";
            if (!string.IsNullOrEmpty(p.fName))
            {
                sql += " fName = @K_FNAME, ";
                paras.Add(new SqlParameter("K_FNAME", (object)p.fName));
            }
            if (!string.IsNullOrEmpty(p.fPhone))
            {
                sql += " fPhone = @K_FPHONE,";
                paras.Add(new SqlParameter("K_FPHONE", (object)p.fPhone));
            }
            if (!string.IsNullOrEmpty(p.fEmail))
            {
                sql += " fEmail = @K_FEMAIL, ";
                paras.Add(new SqlParameter("K_FEMAIL", (object)p.fEmail));
            }
            if (!string.IsNullOrEmpty(p.fAddress))
            {
                sql += " fAddress = @K_FADDRESS, ";
                paras.Add(new SqlParameter("K_FADDRESS", (object)p.fAddress));
            }
            if (!string.IsNullOrEmpty(p.fPassword))
            {
                sql += " fPassword = @K_PASSWORD, ";
                paras.Add(new SqlParameter("K_PASSWORD", (object)p.fPassword));
            }
            //最後一個要去掉,
            if (sql.Trim().Substring(sql.Trim().Length - 1, 1) == ",")
                sql = sql.Trim().Substring(0, sql.Trim().Length - 1);
            sql += " WHERE fId= @K_FID";
            paras.Add(new SqlParameter("K_FID", (object)p.fId));
            execulteSql(sql, paras);
        }
        //查詢單一
        public CCustomers queryByFId(int fId)
        {
            string sql = "SELECT * FROM tCustomer WHERE fId=@K_FID";
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("K_FID", (object)fId));
            List<CCustomers> list = queryBySql(sql,paras);
            if(list.Count==0)
                return null;
            return list[0];
        }
        //查詢所有資料
        public List<CCustomers> queryAll()
        {
            return queryBySql("SELECT * FROM tCustomer", null);
        }

        //共用的查詢方法
        private List<CCustomers> queryBySql(string sql, List<SqlParameter> paras)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = @"Data Source=.;Initial Catalog=dbDemo;Integrated Security=True";
            con.Open();
            SqlCommand cmd = new SqlCommand(sql, con);
            if (paras != null)
                cmd.Parameters.AddRange(paras.ToArray());
            SqlDataReader reader = cmd.ExecuteReader();
            List<CCustomers> list = new List<CCustomers>();
            while (reader.Read())
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
                list.Add(x);
            }
            con.Close();
            return list;
        }

        internal List<CCustomers> queryByKeyword(string keyword)
        {
            string sql = "SELECT * FROM tCustomer WHERE fName LIKE @K_KEYWORD"; //LIKE為模糊查詢
            sql += " OR fPhone LIKE @K_KEYWORD ";
            sql += " OR fEmail LIKE @K_KEYWORD ";
            sql += " OR fAddress LIKE @K_KEYWORD ";
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("K_KEYWORD", "%"+(object)keyword+"%")); //模糊查詢要加 %{}%        
            return queryBySql(sql,paras);
        }

        internal CCustomers queryByEmail(string email)
        {
            string sql = "SELECT * FROM tCustomer WHERE fEmail=@K_FEMAIL";
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("K_FEMAIL", (object)email));
            List<CCustomers> list = queryBySql(sql, paras);
            if (list.Count == 0)
                return null;
            return list[0];
        }
    }

}