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
        public IActionResult Index(string start_time, string end_time, string return_index, string order_name,int pageindex = 1, int pagesize = 20)
        {
            ViewBag.start_time = start_time;
            ViewBag.end_time = end_time;

            ViewBag.return_index = return_index;
            ViewBag.order_name = order_name;

            if (start_time == null)
            {
                start_time = "0001-01-01";
            }
            if (end_time == null)
            {
                end_time = "2222-01-01";
            }

            var objList = SM.SelectAll(0, start_time, end_time, return_index, order_name);
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
                if (Request.Query["return_index"] != "new")
                {
                    string return_index = Convert.ToString(Request.Query["return_index"]);
                    List<SaleReturn> saleReturnList = SM.SelectAllByReturnIndex(return_index);

                    //和History共用一个edit,用于隐藏可提交的部分
                    if (!string.IsNullOrEmpty(Request.Query["flag"]))
                    {
                        ViewBag.flag = 1;
                    }

                    if (saleReturnList != null)
                    {
                        ViewBag.return_index = return_index;
                        return View(saleReturnList);
                    }
                    else
                    {
                        return RedirectToAction("Index", "SaleReturn");
                    }

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
            string[] return_price = Convert.ToString(HttpContext.Request.Form["Return_Price"]).Split(',');
            string[] return_all_price = Convert.ToString(HttpContext.Request.Form["Return_All_Price"]).Split(',');
            string[] remark = Convert.ToString(HttpContext.Request.Form["Remark"]).Split(',');

            bool flag = true;
            SaleReturn objSaleReturn = new SaleReturn();
            objSaleReturn.return_index = return_index;
            objSaleReturn.deliver_index = deliver_index;
            objSaleReturn.insert_time = DateTime.Now.ToLocalTime();

            Order objOrder = new Order();
            for (int i = 0; i < inputNum; i++)
            {
                objSaleReturn.seq_id = Convert.ToInt32(seq_id[i]);
                objSaleReturn.remark = remark[i];

                if (return_num[i] == "已退货")
                {
                    continue;
                }
                else
                {
                    if (return_num[i] == "")
                    {
                        continue;
                    }
                    else
                    {
                        objSaleReturn.return_num = Convert.ToInt32(return_num[i]);
                        objOrder.tui_num = Convert.ToInt32(return_num[i]);
                    }

                }

                if (return_price[i] == "" || return_num[i] == "" || return_num[i] == "0")
                {
                    continue;
                }
                else
                {
                    objSaleReturn.return_price = Convert.ToDouble(return_price[i]);
                }

                if (return_all_price[i] == "" || return_num[i] == "" || return_num[i] == "0")
                {
                    continue;
                }
                else
                {
                    objSaleReturn.return_all_price = Convert.ToDouble(return_all_price[i]);
                }

                objSaleReturn.seq_id = objSaleReturn.seq_id;
                int count = SM.Insert(objSaleReturn);

                SaleM.UpdateReturnFlag(deliver_index, objSaleReturn.seq_id);
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
        /// 获取价格
        /// </summary>
        /// <returns></returns>
        public IActionResult GetProductPrice(int id, int return_num, double return_price)
        {
            ViewBag.return_all_price = return_num * return_price;
            ViewBag.id = "return_all_price" + id;
            return View();
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
                string return_index = Convert.ToString(HttpContext.Request.Form["return_index"]);
                int seq_id = Convert.ToInt32(HttpContext.Request.Form["seq_id"]);
                //更新退单的状态
                int count = SM.UpdateReturnStatus(return_index, seq_id);

                //将退单的数量加到对应订单中的剩余数量中，开单数量不变
                SaleReturn saleReturn = SM.SelectSingleBySeqReturnIndex(seq_id,return_index);
                Order orderOld = OM.SelectByOrderSeqId(seq_id);

                Order orderNew = new Order();
                orderNew.seq_id = seq_id;
                orderNew.remain_num = orderOld.remain_num+ saleReturn.return_num;
                orderNew.open_num = orderOld.open_num;
                orderNew.tui_num = orderOld.tui_num + saleReturn.return_num;
                OM.UpdateSeqNum(orderNew);

                //退单确认后更新订单的状态（不管原来订单是否完成，全部改为0）
                Order order = new Order();
                order.seq_id = seq_id;
                order.order_status = 0;
                OM.UpdateOrderStatus(order);

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
        public IActionResult ReturnHistoryIndex(string start_time, string end_time, string return_index,string order_name, string day, string month, string year, 
            int pageindex = 1, int pagesize = 20)
        {
            ViewBag.start_time = start_time;
            ViewBag.end_time = end_time;

            ViewBag.return_index = return_index;
            ViewBag.order_name = order_name;
            

            if (start_time == null)
            {
                start_time = "0001-01-01";
            }
            if (end_time == null)
            {
                end_time = "2222-01-01";
            }

            ViewBag.flag = 1;

            DateTime dt = DateTime.Now;
            DateTime dt2 = dt.AddMonths(1);

            if (day == "1")
            {
                start_time = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");
                end_time = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");
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
                end_time = new DateTime(DateTime.Now.Year, 12, 31).ToString("yyyy-MM-dd");
                ViewBag.year = "1";
            }

            var objList = SM.SelectAll(1, start_time, end_time,  return_index, order_name);
            var pagedList = PagedList<SaleReturn>.PageList(pageindex, pagesize, objList);
            ViewBag.model = pagedList.Item2;
            return View(pagedList.Item1);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult GetDeliverList(string deliver_index,string order_index)
        {
            List<Sale> sale = SaleM.SelectSeqByDeliverIndex(deliver_index);
            return View(sale);
        }

    }
}