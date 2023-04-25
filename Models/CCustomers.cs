using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace prjMvcDemo.Models
{
    public class CCustomers
    {
        public int fId { get; set; }
        [Required(ErrorMessage = "姓名為必填欄位")]
        public string fName { get; set; }
        public string fPhone { get; set; }
        public string fEmail { get; set; }
        public string fAddress { get; set; }
        public string fPassword { get; set; }
    }
}