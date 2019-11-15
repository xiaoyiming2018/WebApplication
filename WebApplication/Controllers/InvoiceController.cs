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
        public IActionResult Index(string invoice_index, string company_name, string order_index, string company_order_index, int pageindex = 1, int pagesize = 8)
        {
            
            ViewBag.invoice_index = invoice_index;
            ViewBag.company_name = company_name;
            ViewBag.order_index = order_index;
            ViewBag.company_order_index = company_order_index;

            var objList = IM.SelectAll(invoice_index, company_name, order_index, company_order_index);
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
                    ViewBag.order_id = invoice.order_id;
                    Order order = OM.SelectById(invoice.order_id);
                    ViewBag.order_index = order.order_index;
                    ViewBag.company_name = order.company_name;
                    ViewBag.company_order_index = order.company_order_index;

                    List<Order> orderList = OM.SelectForInvoiceOrderList(invoice.order_id);
                    ViewBag.orderList = orderList;

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
            int id = 0;
            int order_id = invoice.order_id;
            int invoice_type = invoice.invoice_type;
            string invoice_index = invoice.invoice_index;
            DateTime invoice_time = invoice.invoice_time;
            double invoice_price = invoice.invoice_price;
            double invoice_ratio = invoice.invoice_ratio;
            double invoice_all_price = invoice.invoice_all_price;
            int pay_type = invoice.pay_type;
            string remark = invoice.remark;
            if (!string.IsNullOrEmpty(HttpContext.Request.Form["id"]))
            {
                id = Convert.ToInt32(HttpContext.Request.Form["id"]);
            }

            Invoice objInvoice = new Invoice();
            objInvoice.order_id = order_id;
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
                //修改订单的invoice_status为1
                Order order = new Order();
                order.invoice_status = 1;
                order.id = order_id;
                OM.UpdateInvoiceStatus(order);
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
        /// 删除数据
        /// </summary>
        /// <returns></returns>
        public IActionResult Del()
        {
            try
            {
                int id = Convert.ToInt32(Request.Query["id"]);
                Invoice invoice = IM.SelectById(id);
                
                //修改订单的invoice_status为0
                Order order = new Order();
                order.invoice_status = 0;
                order.id = invoice.order_id;
                OM.UpdateInvoiceStatus(order);

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
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult GetOrderList(int order_id)
        {
            Order order = OM.SelectById(order_id);
            ViewBag.company_name = order.company_name;
            ViewBag.company_order_index = order.company_order_index;

            List<Order> orderList = OM.SelectForInvoiceOrderList(order_id);
            return View(orderList);
        }
    }
}