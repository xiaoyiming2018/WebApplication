using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreMvcPager;
using BLL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace WebApplication.Controllers
{
    public class SaleReturnController : BaseController
    {
        SaleReturnManager SM = new SaleReturnManager();
        OrderManager OM = new OrderManager();
        SaleManager SaleM = new SaleManager();
        SettingManager SettingM = new SettingManager();
        /// <summary>
        /// 用户列表首页
        /// </summary>
        /// <param name="page">分页页码</param>
        /// <param name="size">每页显示数量</param>
        /// <returns></returns>
        public IActionResult Index(string start_time, string end_time, string return_index, string deliver_index, string order_index, string company_name, string company_order_index,
            int pageindex = 1, int pagesize = 8)
        {
            ViewBag.start_time = start_time;
            ViewBag.end_time = end_time;

            ViewBag.return_index = return_index;
            ViewBag.deliver_index = deliver_index;
            ViewBag.order_index = order_index;
            ViewBag.company_name = company_name;
            ViewBag.company_order_index = company_order_index;

            if (start_time == null)
            {
                start_time = "2001-01-01";
            }
            if (end_time == null)
            {
                end_time = "2222-01-01";
            }

            var objList = SM.SelectAll(0, start_time, end_time, return_index,deliver_index, order_index, company_name, company_order_index);
            var pagedList = PagedList<SaleReturn>.PageList(pageindex, pagesize, objList);
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
                int id = Convert.ToInt32(Request.Query["id"]);
                if (id > 0)
                {
                    //此处可忽略，修改动作在出货详情中
                    Sale sale = new Sale();
                    //sale = SM.SelectById(id);

                    return View(sale);

                }
                else
                {
                    List<SaleReturn> listSale = SM.SelectTodayForReturnIndex(DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd"));
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
                    ViewBag.return_index = SettingM.SelectInUse(7).index_begin + DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd").Replace("-", "") + count_num;
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
            string return_index = Convert.ToString(HttpContext.Request.Form["return_index"]);
            string deliver_index = Convert.ToString(HttpContext.Request.Form["deliver_index"]);

            string[] seq_id = Convert.ToString(HttpContext.Request.Form["Seq_Id"]).Split(',');
            string[] return_num = Convert.ToString(HttpContext.Request.Form["Return_Num"]).Split(',');
            string[] return_time = Convert.ToString(HttpContext.Request.Form["Return_Time"]).Split(',');
            string[] return_price = Convert.ToString(HttpContext.Request.Form["Return_Price"]).Split(',');
            string[] return_all_price = Convert.ToString(HttpContext.Request.Form["Return_All_Price"]).Split(',');


            bool flag = true;
            SaleReturn objSaleReturn = new SaleReturn();
            objSaleReturn.return_index = return_index;
            objSaleReturn.deliver_index = deliver_index;
            objSaleReturn.insert_time = DateTime.Now.ToLocalTime();

            Order objOrder = new Order();
            for (int i = 0; i < inputNum; i++)
            {
                objSaleReturn.seq_id = Convert.ToInt32(seq_id[i]);

                if (return_num[i] == "已退货")
                {
                    continue;
                }
                else
                {
                    if (return_num[i] == "")
                    {
                        objSaleReturn.return_num = 0;
                        objOrder.tui_num = 0;
                    }
                    else
                    {
                        objSaleReturn.return_num = Convert.ToInt32(return_num[i]);
                        objOrder.tui_num = Convert.ToInt32(return_num[i]);
                    }
                    
                }

                if (return_time[i] == "" || return_num[i] == "" || return_num[i] == "0")
                {
                    objSaleReturn.return_time = Convert.ToDateTime("0001/01/01 0:00:00");
                }
                else
                {
                    objSaleReturn.return_time = Convert.ToDateTime(return_time[i]);
                }

                if (return_price[i] == "" || return_num[i] == "" || return_num[i] == "0")
                {
                    objSaleReturn.return_price = 0;
                }
                else
                {
                    objSaleReturn.return_price = Convert.ToDouble(return_price[i]);
                }

                if (return_all_price[i] == "" || return_num[i] == "" || return_num[i] == "0")
                {
                    objSaleReturn.return_all_price = 0 ;
                }
                else
                {
                    objSaleReturn.return_all_price = Convert.ToDouble(return_all_price[i]);
                }

                //Order order = OM.SelectByOrderSeqId(order_id, objSaleReturn.seq_id);
                //objOrder.remain_num = order.remain_num;
                //objOrder.open_num = order.open_num;
                //objOrder.tui_num = order.tui_num + objOrder.tui_num;
                //objOrder.seq_id = objSaleReturn.seq_id;
                ////更新OrderSeq
                //OM.UpdateSeqNum(objOrder);

                //插入Sale中
                objSaleReturn.seq_id = objSaleReturn.seq_id;
                int count = SM.Insert(objSaleReturn);

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
                int seq_id = Convert.ToInt32(Request.Query["seq_id"]);
                string return_index = Convert.ToString(Request.Query["return_index"]);
                int count = SM.Del(seq_id,return_index);
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
        /// 确认数据
        /// </summary>
        /// <returns></returns>
        public IActionResult Confirm()
        {
            try
            {
                //将退单状态改为1，保存到退单历史
                string return_index = Convert.ToString(Request.Query["return_index"]);
                int count = SM.UpdateReturnStatus(return_index);

                //为了考虑已进入历史订单中的订单，将订单状态改为0
                SaleReturn saleReturn = SM.SelectByReturnIndex(return_index);
                Order orderInfo = new Order();
                orderInfo.id = saleReturn.order_id;
                orderInfo.order_status = 0;
                OM.UpdateOrderStatus(orderInfo);

                //将退单的数量加到对应订单中的剩余数量中，开单数量不变
                List<SaleReturn> saleReturnList = SM.SelectSeqByReturnIndex(return_index);
                for (int i=0; i<saleReturnList.Count;i++)
                {
                    Order orderOld = OM.SelectByOrderSeqId(saleReturnList[i].order_id, saleReturnList[i].seq_id);

                    Order orderNew = new Order();
                    orderNew.seq_id = saleReturnList[i].seq_id;
                    orderNew.remain_num = orderOld.remain_num+saleReturnList[i].return_num;
                    orderNew.open_num = orderOld.open_num;
                    orderNew.tui_num = orderOld.tui_num + saleReturnList[i].return_num;
                    OM.UpdateSeqNum(orderNew);
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
        /// 用户列表首页
        /// </summary>
        /// <param name="page">分页页码</param>
        /// <param name="size">每页显示数量</param>
        /// <returns></returns>
        public IActionResult ReturnHistoryIndex(string start_time, string end_time, string return_index, string deliver_index, string order_index, string company_name, string company_order_index,
            int pageindex = 1, int pagesize = 8)
        {
            ViewBag.start_time = start_time;
            ViewBag.end_time = end_time;

            ViewBag.return_index = return_index;
            ViewBag.deliver_index = deliver_index;
            ViewBag.order_index = order_index;
            ViewBag.company_name = company_name;
            ViewBag.company_order_index = company_order_index;

            if (start_time == null)
            {
                start_time = "2001-01-01";
            }
            if (end_time == null)
            {
                end_time = "2222-01-01";
            }

            var objList = SM.SelectReturnHistory(1, start_time, end_time,  return_index, deliver_index, order_index, company_name, company_order_index);
            var pagedList = PagedList<SaleReturn>.PageList(pageindex, pagesize, objList);
            ViewBag.model = pagedList.Item2;
            return View(pagedList.Item1);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult GetDeliverList(string deliver_index)
        {
            List<Sale> sale = SaleM.SelectSeqByDeliverIndex(deliver_index);
            Sale saleView = SaleM.SelectDeliverByDeliverIndex(deliver_index);
            ViewBag.order_id = saleView.order_id;
            ViewBag.order_index = saleView.order_index;
            ViewBag.company_name = saleView.company_name;
            ViewBag.company_order_index = saleView.company_order_index;
            return View(sale);
        }

        /// <summary>
        /// 用户列表首页
        /// </summary>
        /// <param name="page">分页页码</param>
        /// <param name="size">每页显示数量</param>
        /// <returns></returns>
        public IActionResult MoneyIndex(string start_time, string end_time, string deliver_index, string deliver_company_head, string order_index, string company_name, string company_order_index,
            int pageindex = 1, int pagesize = 8)
        {
            ViewBag.start_time = start_time;
            ViewBag.end_time = end_time;

            ViewBag.deliver_index = deliver_index;

            ViewBag.deliver_company_head = deliver_company_head;
            ViewBag.order_index = order_index;
            ViewBag.company_name = company_name;
            ViewBag.company_order_index = company_order_index;

            if (start_time == null)
            {
                start_time = "2001-01-01";
            }
            if (end_time == null)
            {
                end_time = "2222-01-01";
            }


            var objList = SM.SelectMoneyAll(start_time, end_time,deliver_index, deliver_company_head, order_index, company_name, company_order_index);
            var pagedList = PagedList<SaleReturn>.PageList(pageindex, pagesize, objList);
            ViewBag.model = pagedList.Item2;
            return View(pagedList.Item1);   
        }

        /// <summary>
        /// 用户列表首页
        /// </summary>
        /// <param name="page">分页页码</param>
        /// <param name="size">每页显示数量</param>
        /// <returns></returns>
        public IActionResult MoneyHistoryIndex(string start_time, string end_time, string deliver_index, string deliver_company_head, string order_index, string company_name, string company_order_index,
            int pageindex = 1, int pagesize = 8)
        {
            ViewBag.start_time = start_time;
            ViewBag.end_time = end_time;

            ViewBag.deliver_index = deliver_index;

            ViewBag.deliver_company_head = deliver_company_head;
            ViewBag.order_index = order_index;
            ViewBag.company_name = company_name;
            ViewBag.company_order_index = company_order_index;

            if (start_time == null)
            {
                start_time = "2001-01-01";
            }
            if (end_time == null)
            {
                end_time = "2222-01-01";
            }


            var objList = SM.SelectMoneyHistoryAll(start_time, end_time,deliver_index, deliver_company_head, order_index, company_name, company_order_index);
            var pagedList = PagedList<SaleReturn>.PageList(pageindex, pagesize, objList);
            ViewBag.model = pagedList.Item2;
            return View(pagedList.Item1);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <returns></returns>
        public IActionResult DeliverConfirm()
        {
            try
            {
                bool flag = true;
                string deliver_index = Convert.ToString(Request.Query["deliver_index"]);
                List<Sale> saleList = SaleM.SelectSeqByDeliverIndex(deliver_index);
                

                DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
                dtFormat.ShortDatePattern = "yyyy/M/d H:mm:ss";
                DateTime dt = Convert.ToDateTime("0001/1/1 0:00:00", dtFormat);

                for (int i=0;i< saleList.Count;i++)
                {
                    int status = 0;
                    if (saleList[i].deliver_status == 0)
                    {
                        status = 1;
                        dt = DateTime.Now.ToLocalTime();
                    }
                    else
                    {
                        status = 0;
                        dt = Convert.ToDateTime("0001/1/1 0:00:00");
                    }

                    Sale sale = new Sale();
                    sale.deliver_status = status;
                    sale.confirm_time = dt;
                    sale.deliver_index = saleList[i].deliver_index;
                    sale.seq_id = saleList[i].seq_id;

                    int count=SaleM.UpdateDeliverStatus(sale);
                    if (count<1)
                    {
                        flag = false;
                    }
                }
                if (flag)
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
        /// 插入更新页面
        /// </summary>
        /// <returns></returns>
        public IActionResult MoneyEdit()
        {
            try
            {
                string deliver_index = Convert.ToString(HttpContext.Request.Form["deliver_index"]);
                int money_onoff = Convert.ToInt32(HttpContext.Request.Form["money_onoff"]);
                int money_way = Convert.ToInt32(HttpContext.Request.Form["money_way"]);

                Sale sale = new Sale();
                sale.deliver_index = deliver_index;
                sale.money_onoff = money_onoff;
                sale.money_way = money_way;
                int count=SaleM.UpdateMoney(sale);

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

        public IActionResult MoneySeqIndex(string start_time, string end_time, string order_name, string unit, int pageindex = 1, int pagesize = 8)
        {
            
            string deliver_index = Convert.ToString(Request.Query["deliver_index"]);

            if (!string.IsNullOrEmpty(Request.Query["deliver_status"]))
            {
                if (Convert.ToInt32(Request.Query["deliver_status"]) == 0)
                {
                    ViewBag.flag = "Return";
                }
                else
                {
                    ViewBag.flag = "History";
                }
            }

            Sale sale = SaleM.SelectDeliverByDeliverIndex(deliver_index);
            ViewBag.money_onoff = sale.money_onoff;
            ViewBag.money_way = sale.money_way;

            List<SaleReturn> saleReturnList = SM.SelectByDeliverIndex(deliver_index);

            Int64 returnNum = 0;
            Int64 deliverNum = 0;
            double returnPrice = 0;
            double deliverPrice = 0;
            if (saleReturnList.Count>0)
            {
                for (int i = 0; i < saleReturnList.Count; i++)
                {
                    returnNum += saleReturnList[i].return_num;
                    returnPrice += saleReturnList[i].return_all_price;
                }
            }
                

            ViewBag.saleReturn = saleReturnList;

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

            List<Sale> objList = SaleM.SelectSeqByDeliverIndex(deliver_index);
            for (int i = 0; i < objList.Count; i++)
            {
                deliverNum += objList[i].real_num;
                deliverPrice += objList[i].deliver_all_price;
            }

            ViewBag.returnNum = returnNum;
            ViewBag.deliverNum = deliverNum;
            ViewBag.returnPrice = returnPrice;
            ViewBag.deliverPrice = deliverPrice;
            return View(objList);
            
        }

    }
}