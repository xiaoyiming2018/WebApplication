using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreMvcPager;
using BLL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace WebApplication.Controllers
{
    public class InvoiceController : BaseController
    {
        InvoiceManager IM = new InvoiceManager();
        OrderManager OM = new OrderManager();
        /// <summary>
        /// 用户列表首页
        /// </summary>
        /// <param name="page">分页页码</param>
        /// <param name="size">每页显示数量</param>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();  
        }
        public IActionResult GetData(string start_time, string end_time, string invoice_index,string invoice_company, string company_name,
            string day, string month, string year) {
            ViewBag.start_time = start_time;
            ViewBag.end_time = end_time;
            ViewBag.invoice_index = invoice_index;
            ViewBag.company_name = company_name;
            ViewBag.invoice_company = invoice_company;
            if (start_time == null)
            {
                start_time = "0001-01-01";
            }
            if (end_time == null)
            {
                end_time = "2222-01-01";
            }

            DateTime dt = DateTime.Now.AddHours(8);
            DateTime dt2 = dt.AddMonths(1);

            if (day == "1")
            {
                start_time = DateTime.Now.ToLocalTime().AddHours(8).ToString("yyyy-MM-dd");
                end_time = DateTime.Now.ToLocalTime().AddHours(8).ToString("yyyy-MM-dd");
                ViewBag.day = "1";
            }
            if (month == "1")
            {
                start_time = dt.AddDays(-(dt.Day) + 1).ToString("yyyy-MM-dd");
                end_time = dt2.AddDays(-dt.Day).ToString("yyyy-MM-dd");
                ViewBag.month = "1";
            }
            if (year == "1")
            {
                start_time = dt.AddMonths(-dt.Month + 1).AddDays(-dt.Day + 1).ToString("yyyy-MM-dd");
                end_time = new DateTime(DateTime.Now.AddHours(8).Year, 12, 31).ToString("yyyy-MM-dd");
                ViewBag.year = "1";
            }
            string confirm_start_time = "0001-01-01";
            string confirm_end_time = "2222-01-01";

            List<Invoice> objList = new List<Invoice>();
            if (company_name==null)
            {
                List<Invoice> res = IM.SelectAll(0, start_time, end_time, confirm_start_time, confirm_end_time, invoice_index, "", invoice_company);
                for (int i = 0; i < res.Count; i++)
                {
                    objList.Add(res[i]);
                }
            }
            else
            {
                string[] company_nameRes = company_name.Split(",");
                for (int i = 0; i < company_nameRes.Length; i++)
                {
                    List<Invoice> res = IM.SelectAll(0, start_time, end_time, confirm_start_time, confirm_end_time, invoice_index, company_nameRes[i], invoice_company);
                    for (int j = 0; j < res.Count; j++)
                    {
                        objList.Add(res[j]);
                    }
                }
            }
            List<Invoice> resList = objList.Distinct<Invoice>(new ListComparer<Invoice>((x, y) => x.id.Equals(y.id))).ToList();
            return Json(resList);
        }
        /// <summary>
        /// 插入更新页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Edit()
        {
            try
            {
                int id= Convert.ToInt32(Request.Query["id"]);
                if (id>0)
                {
                    Invoice invoice = IM.SelectById(id);
                    ViewBag.company_name = invoice.company_name;
                    return View(invoice);
                }
                else
                {
                    return View();
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 编辑处理页面
        /// </summary>
        /// <returns></returns>
        public IActionResult EditHandle(Invoice invoice)
        {
            int count = 0;
            int id = invoice.id;
            string company_name = invoice.company_name;
            string invoice_company = invoice.invoice_company;
            int invoice_type = invoice.invoice_type;
            string invoice_index = invoice.invoice_index;
            DateTime invoice_time = invoice.invoice_time;
            double invoice_price = invoice.invoice_price;
            double invoice_real_price = invoice.invoice_real_price;
            double invoice_prepay = invoice.invoice_prepay;
            string remark = invoice.remark;
            string qq = Convert.ToString(HttpContext.Request.Form["id"]);

            Invoice objInvoice = new Invoice();
            objInvoice.company_name = company_name;
            objInvoice.invoice_company = invoice_company;
            objInvoice.invoice_type = invoice_type;
            objInvoice.invoice_index = invoice_index;
            objInvoice.invoice_time = invoice_time;
            objInvoice.invoice_price = invoice_price;
            objInvoice.invoice_real_price = invoice_real_price;
            objInvoice.invoice_prepay = invoice_prepay;
            objInvoice.pay_type = 0;
            objInvoice.remark = remark;
            if (id>0)
            {
                objInvoice.id = id;
                count = IM.Update(objInvoice);
                
            }
            else
            {
                count = IM.Insert(objInvoice);
            }

            if (count > 0)
            {
                return Json("Success");
            }
            else
            {
                return Json("Fail");
            }
        }

        /// <summary>
        /// 结款
        /// </summary>
        /// <returns></returns>
        public IActionResult Confirm()
        {
            try
            {
                int id = Convert.ToInt32(Request.Query["id"]);
                int pay_type = Convert.ToInt32(Request.Query["pay_type"]);
                string confirm_time = Convert.ToDateTime(HttpContext.Request.Form["confirm_time"]).ToString("yyyy-MM-dd HH:mm:ss");
                int count = IM.UpdateStatus(id, confirm_time, pay_type);
                if (count>0)
                {
                    return Json("Success");
                }
                else
                {
                    return Json("Fail");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <returns></returns>
        public IActionResult Del()
        {
            try
            {
                int id = Convert.ToInt32(Request.Query["id"]);
                Invoice invoice = IM.SelectById(id);                

                int count = IM.Del(id);
                if (count > 0)
                {
                    return Json("Success");
                }
                else
                {
                    return Json("Fail");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 用户列表首页
        /// </summary>
        /// <param name="page">分页页码</param>
        /// <param name="size">每页显示数量</param>
        /// <returns></returns>
        public IActionResult HistoryIndex()
        {
            return View();

        }
        public IActionResult GetHistoryData(string start_time, string end_time, string confirm_start_time, string confirm_end_time, string invoice_index, string company_name,
            string day, string month, string year,string invoice_company) {
            ViewBag.start_time = start_time;
            ViewBag.end_time = end_time;
            ViewBag.confirm_start_time = confirm_start_time;
            ViewBag.confirm_end_time = confirm_end_time;
            ViewBag.invoice_index = invoice_index;
            ViewBag.company_name = company_name;
            ViewBag.invoice_company = invoice_company;

            
            if (start_time == null)
            {
                start_time = "0001-01-01";
            }
            if (end_time == null)
            {
                end_time = "2222-01-01";
            }

            if (confirm_start_time == null)
            {
                confirm_start_time = "0001-01-01";
            }
            if (confirm_end_time == null)
            {
                confirm_end_time = "2222-01-01";
            }

            DateTime dt = DateTime.Now.AddHours(8);
            DateTime dt2 = dt.AddMonths(1);

            if (day == "1")
            {
                confirm_start_time = DateTime.Now.ToLocalTime().AddHours(8).ToString("yyyy-MM-dd");
                confirm_end_time = DateTime.Now.ToLocalTime().AddHours(8).ToString("yyyy-MM-dd");
                ViewBag.day = "1";
            }
            if (month == "1")
            {
                confirm_start_time = dt.AddDays(-(dt.Day) + 1).ToString("yyyy-MM-dd");
                confirm_end_time = dt2.AddDays(-dt.Day).ToString("yyyy-MM-dd");
                ViewBag.month = "1";
            }
            if (year == "1")
            {
                confirm_start_time = dt.AddMonths(-dt.Month + 1).AddDays(-dt.Day + 1).ToString("yyyy-MM-dd");
                confirm_end_time = new DateTime(DateTime.Now.AddHours(8).Year, 12, 31).ToString("yyyy-MM-dd");
                ViewBag.year = "1";
            }

            List<Invoice> objList = new List<Invoice>();
            if (company_name==null)
            {
                List<Invoice> res = IM.SelectAll(1, start_time, end_time, confirm_start_time, confirm_end_time, invoice_index, "", invoice_company);
                for (int i = 0; i < res.Count; i++)
                {
                    objList.Add(res[i]);
                }
            }
            else
            {
                string[] company_nameRes = company_name.Split(",");
                for (int i = 0; i < company_nameRes.Length; i++)
                {
                    List<Invoice> res = IM.SelectAll(1, start_time, end_time, confirm_start_time, confirm_end_time, invoice_index, company_nameRes[i], invoice_company);
                    for (int j = 0; j < res.Count; j++)
                    {
                        objList.Add(res[j]);
                    }
                }
            }
            List<Invoice> resList = objList.Distinct<Invoice>(new ListComparer<Invoice>((x, y) => x.id.Equals(y.id))).ToList();
            return Json(resList);
        }
        /// <summary>
        /// 用委托实现IEqualityComparer<T>接口
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        public class ListComparer<T> : IEqualityComparer<T>
        {
            public Func<T, T, bool> EqualsFunc;
            public Func<T, int> GetHashCodeFunc;

            public ListComparer(Func<T, T, bool> Equals, Func<T, int> GetHashCode)
            {
                this.EqualsFunc = Equals;
                this.GetHashCodeFunc = GetHashCode;
            }

            public ListComparer(Func<T, T, bool> Equals) : this(Equals, t => 0)
            {

            }

            public bool Equals(T x, T y)
            {
                if (this.EqualsFunc != null)
                {
                    return this.EqualsFunc(x, y);
                }
                else
                {
                    return false;
                }
            }

            /// <summary>
            /// 获取目标对象的哈希值,只有返回相同的哈希值才能运行Equals方法
            /// </summary>
            /// <param name="obj">获取哈希值的目标类型对象</param>
            /// <returns>返回哈希值</returns>
            public int GetHashCode(T obj)
            {
                if (this.GetHashCodeFunc != null)
                {
                    return this.GetHashCodeFunc(obj);
                }
                else
                {
                    return 0;
                }
            }
        }

    }
}