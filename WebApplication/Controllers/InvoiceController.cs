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
        public IActionResult Index(string start_time, string end_time,string invoice_index, string company_name, int pageindex = 1, int pagesize = 8)
        {
            ViewBag.start_time = start_time;
            ViewBag.end_time = end_time;
            ViewBag.invoice_index = invoice_index;
            ViewBag.company_name = company_name;
            
            if (start_time == null)
            {
                start_time = "2001-01-01";
            }
            if (end_time == null)
            {
                end_time = "2222-01-01";
            }
            string confirm_start_time = "0001-01-01";
            string confirm_end_time = "2222-01-01";

            var objList = IM.SelectAll(0, start_time, end_time, confirm_start_time, confirm_end_time, invoice_index, company_name);
            var pagedList = PagedList<Invoice>.PageList(pageindex, pagesize, objList);
            ViewBag.model = pagedList.Item2;
            return View(pagedList.Item1);
            
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
            int invoice_type = invoice.invoice_type;
            string invoice_index = invoice.invoice_index;
            DateTime invoice_time = invoice.invoice_time;
            double invoice_price = invoice.invoice_price;
            double invoice_ratio = invoice.invoice_ratio;
            double invoice_all_price = invoice.invoice_all_price;
            int pay_type = invoice.pay_type;
            string remark = invoice.remark;
            string qq = Convert.ToString(HttpContext.Request.Form["id"]);

            Invoice objInvoice = new Invoice();
            objInvoice.company_name = company_name;
            objInvoice.invoice_type = invoice_type;
            objInvoice.invoice_index = invoice_index;
            objInvoice.invoice_time = invoice_time;
            objInvoice.invoice_price = invoice_price;
            objInvoice.invoice_ratio = invoice_ratio;
            objInvoice.invoice_all_price = invoice_all_price;
            objInvoice.pay_type = pay_type;
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

                int count = IM.UpdateStatus(id);
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
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult GetPriceList(double invoice_price,double invoice_ratio)
        {
            ViewBag.invoice_all_price = invoice_price + invoice_ratio;
            return View();
        }

        /// <summary>
        /// 用户列表首页
        /// </summary>
        /// <param name="page">分页页码</param>
        /// <param name="size">每页显示数量</param>
        /// <returns></returns>
        public IActionResult HistoryIndex(string start_time, string end_time, string confirm_start_time, string confirm_end_time, string invoice_index, string company_name, 
            string day, string month, string year, int pageindex = 1, int pagesize = 8)
        {

            ViewBag.start_time = start_time;
            ViewBag.end_time = end_time;
            ViewBag.confirm_start_time = confirm_start_time;
            ViewBag.confirm_end_time = confirm_end_time;
            ViewBag.invoice_index = invoice_index;
            ViewBag.company_name = company_name;

            if (start_time == null)
            {
                start_time = "2001-01-01";
            }
            if (end_time == null)
            {
                end_time = "2222-01-01";
            }

            if (confirm_start_time == null)
            {
                confirm_start_time = "2001-01-01";
            }
            if (confirm_end_time == null)
            {
                confirm_end_time = "2222-01-01";
            }

            DateTime dt = DateTime.Now;
            DateTime dt2 = dt.AddMonths(1);

            if (day == "1")
            {
                confirm_start_time = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");
                confirm_end_time = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");
                ViewBag.day = "1";
            }
            if (month == "1")
            {
                confirm_start_time = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");
                confirm_end_time = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");
                ViewBag.month = "1";
            }
            if (year == "1")
            {
                confirm_start_time = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");
                confirm_end_time = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");
                ViewBag.year = "1";
            }

            var objList = IM.SelectAll(1, start_time,end_time,confirm_start_time, confirm_end_time, invoice_index, company_name);
            var pagedList = PagedList<Invoice>.PageList(pageindex, pagesize, objList);
            ViewBag.model = pagedList.Item2;
            return View(pagedList.Item1);

        }

    }
}