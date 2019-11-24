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
    public class SaleController : BaseController
    {
        SaleManager SM = new SaleManager();
        OrderManager OM = new OrderManager();
        SettingManager SettingM = new SettingManager();
        /// <summary>
        /// 用户列表首页
        /// </summary>
        /// <param name="page">分页页码</param>
        /// <param name="size">每页显示数量</param>
        /// <returns></returns>
        public IActionResult Index(string start_time, string end_time, string deliver_index, string deliver_company_head,int pageindex = 1, int pagesize = 8)
        {

            ViewBag.start_time = start_time;
            ViewBag.end_time = end_time;

            ViewBag.deliver_index = deliver_index;
            ViewBag.deliver_company_head = deliver_company_head;

            if (start_time == null)
            {
                start_time = "2001-01-01";
            }
            if (end_time == null)
            {
                end_time = "2222-01-01";
            }

            var objList = SM.SelectAll(start_time, end_time, deliver_index, deliver_company_head);
            var pagedList = PagedList<Sale>.PageList(pageindex, pagesize, objList);
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
                if (!string.IsNullOrEmpty(HttpContext.Request.Form["deliver_index"]))
                {
                    string deliver_index = Convert.ToString(HttpContext.Request.Form["deliver_index"]);
                    Sale sale=SM.SelectDeliverByDeliverIndex(deliver_index);
                    ViewBag.deliver_index = deliver_index;
                    ViewBag.deliver_company_head = sale.deliver_company_head;

                    List<Sale> saleList = SM.SelectSeqByDeliverIndex(deliver_index);

                    return View(saleList);

                }
                else
                {
                    List<Sale> listSale = SM.SelectTodayForDeliverIndex(DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd"));
                    string count_num = "";
                    int num = listSale.Count + 1;
                    if (num < 10)
                    {
                        count_num = "00" + num;
                    }
                    else if (num < 100)
                    {
                        count_num = "0" + num;
                    }
                    else
                    {
                        count_num = "" + num;
                    }
                    ViewBag.deliver_index = SettingM.SelectInUse(5).index_begin + DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd").Replace("-", "") + count_num;
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
            string[] remark = Convert.ToString(HttpContext.Request.Form["Remark"]).Split(',');


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
                objSale.remark = Convert.ToString(remark[i]);

                if (real_num[i] == "" || real_num[i] == "出货已完成")
                {
                    continue;
                }
                else
                {
                    objSale.real_num = Convert.ToInt32(real_num[i]);
                    objOrder.open_num = Convert.ToInt32(real_num[i]);
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
                Order order=OM.SelectByOrderSeqId(order_id, objSale.seq_id);
                objOrder.remain_num = order.remain_num - objOrder.open_num;
                objOrder.open_num = order.open_num + objOrder.open_num;
                objOrder.tui_num = order.tui_num;
                objOrder.seq_id = objSale.seq_id;
                //更新OrderSeq
                OM.UpdateSeqNum(objOrder);

                Order orderHistory = OM.SelectForHistoryOrder(order_id);
                
                if (orderHistory.remain_num == 0)
                {
                    Order res = new Order();
                    res.id = order_id;
                    res.order_status = 1;
                    OM.UpdateOrderStatus(res);
                }

                //插入Sale中
                objSale.seq_id = objSale.seq_id;
                int count=SM.Insert(objSale);

                if (count<=0)
                {
                    flag = false;
                    return Json("Fail");
                }

            }

            if (flag ==true)
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
                int seq_id = Convert.ToInt32(Request.Query["seq_id"]);
                string deliver_index = Convert.ToString(Request.Query["deliver_index"]);
                int count = SM.Del(seq_id, deliver_index);
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
        public IActionResult GetOrderList(int order_id)
        {
            List<Order> order = OM.SelectOrderSeqList(order_id);
            return View(order);
        }

    }
}