using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Model;
using BLL;
using Microsoft.AspNetCore.Http;

namespace WebApplication.Controllers
{
    public class SaleSeqController : BaseController
    {
        SaleManager SM = new SaleManager();
        OrderManager OM = new OrderManager();
        /// <summary>
        /// 用户列表首页
        /// </summary>
        /// <param name="page">分页页码</param>
        /// <param name="size">每页显示数量</param>
        /// <returns></returns>
        public IActionResult Index(string start_time, string end_time, string order_name, string unit)
        {
            
            string deliver_index = Convert.ToString(Request.Query["deliver_index"]);
            Sale sale = SM.SelectByDeliverIndex(deliver_index);
            ViewBag.deliver_index = deliver_index;
            ViewBag.deliver_company_head = sale.deliver_company_head;
            ViewBag.order_index = sale.order_index;
            ViewBag.company_name = sale.company_name;
            ViewBag.company_order_index = sale.company_order_index;

            ViewBag.start_time = start_time;
            ViewBag.end_time = end_time;

            if (start_time == null)
            {
                start_time = "2001-01-01";
            }
            if (end_time == null)
            {
                end_time = "2222-01-01";
            }

            var objList = SM.SelectSeqByDeliverIndex(deliver_index);
            return View(objList);
            
        }

        /// <summary>
        /// 插入更新页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Edit()
        {
            try
            {
                string deliver_index = Convert.ToString(Request.Query["deliver_index"]);
                ViewBag.deliver_index = deliver_index;
                Sale sale=SM.SelectByDeliverIndex(deliver_index);
                ViewBag.deliver_company_head = sale.deliver_company_head;
                ViewBag.order_index = sale.order_index;
                ViewBag.order_id = sale.order_id;
                ViewBag.company_name = sale.company_name;
                ViewBag.company_order_index = sale.company_order_index;

                List<Sale> res = SM.SelectSeqByDeliverIndex(deliver_index);

                return View(res);

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
        public IActionResult EditHandle()
        {

            int inputNum = Convert.ToInt32(HttpContext.Request.Form["inputNum"]);
            int order_id = Convert.ToInt32(HttpContext.Request.Form["order_id"]);
            string deliver_index = Convert.ToString(HttpContext.Request.Form["deliver_index"]);
            string deliver_company_head = Convert.ToString(HttpContext.Request.Form["deliver_company_head"]);

            string[] seq_id = Convert.ToString(HttpContext.Request.Form["Seq_Id"]).Split(',');
            string[] real_num = Convert.ToString(HttpContext.Request.Form["Real_Num"]).Split(',');
            string[] real_time = Convert.ToString(HttpContext.Request.Form["Real_Time"]).Split(',');
            string[] deliver_price = Convert.ToString(HttpContext.Request.Form["Deliver_Price"]).Split(',');
            string[] deliver_all_price = Convert.ToString(HttpContext.Request.Form["Deliver_All_Price"]).Split(',');


            bool flag = true;
            Sale objSale = new Sale();
            objSale.order_id = order_id;
            objSale.deliver_index = deliver_index;
            objSale.deliver_company_head = deliver_company_head;
            objSale.insert_time = DateTime.Now.ToLocalTime();

            Order objOrder = new Order();
            for (int i = 0; i < inputNum; i++)
            {
                objSale.seq_id = Convert.ToInt32(seq_id[i]);
                Sale saleOld = SM.SelectById(objSale.seq_id,deliver_index);//获取原来的出货数量

                if (real_num[i] == "" || real_num[i] == "出货已完成")
                {
                    continue;
                }
                else
                {
                    objSale.real_num = Convert.ToInt32(real_num[i]);
                }

                if (real_time[i] == "")
                {
                    continue;
                }
                else
                {
                    objSale.real_time = Convert.ToDateTime(real_time[i]);
                }

                if (deliver_price[i] == "")
                {
                    continue;
                }
                else
                {
                    objSale.deliver_price = Convert.ToDouble(deliver_price[i]);
                }

                if (deliver_all_price[i] == "")
                {
                    continue;
                }
                else
                {
                    objSale.deliver_all_price = Convert.ToDouble(deliver_all_price[i]);
                }
                Order order = OM.SelectByOrderSeqId(order_id, objSale.seq_id);
                objOrder.remain_num = order.remain_num + saleOld.real_num - objSale.real_num;
                objOrder.open_num = order.open_num-saleOld.real_num + objSale.real_num;
                objOrder.tui_num = order.tui_num;
                objOrder.seq_id = objSale.seq_id;
                //更新OrderSeq
                OM.UpdateSeqNum(objOrder);

                //更新Sale
                objSale.seq_id = objSale.seq_id;
                int count = SM.Update(objSale);

                if (count <= 0)
                {
                    flag = false;
                    return Json("Fail");
                }

            }

            if (flag == true)
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
                int count = 0;
                int order_id = Convert.ToInt32(Request.Query["order_id"]);
                Sale order = new Sale();
                ViewBag.order_id = order_id;

                int seq_id = Convert.ToInt32(Request.Query["seq_id"]);
                //List<Sale> orderList = SM.SelectSaleSeqList(order_id);
                List<Sale> orderList = new List<Sale>();
                if (orderList.Count > 1)
                {
                    //count = SM.DelSaleSeq(seq_id);
                    ViewBag.key = "SaleSeq";

                }
                else
                {
                    //count = SM.DelSaleSeq(seq_id);
                    //SM.DelSale(order_id);
                    ViewBag.key = "Sale";
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取材料规格
        /// </summary>
        /// <returns></returns>
        public IActionResult GetProductPrice(int order_num, double order_price)
        {
            ViewBag.order_all_price = order_num * order_price;
            return View();
        }
    }
}