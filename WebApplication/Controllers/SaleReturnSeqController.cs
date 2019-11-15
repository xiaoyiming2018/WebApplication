using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace WebApplication.Controllers
{
    public class SaleReturnSeqController : BaseController
    {
        SaleReturnManager SM = new SaleReturnManager();
        SaleManager SaleM = new SaleManager();
        OrderManager OM = new OrderManager();
        /// <summary>
        /// 用户列表首页
        /// </summary>
        /// <param name="page">分页页码</param>
        /// <param name="size">每页显示数量</param>
        /// <returns></returns>
        public IActionResult Index(string start_time, string end_time, string order_name, string unit, int pageindex = 1, int pagesize = 8)
        {           
            string return_index = Convert.ToString(Request.Query["return_index"]);
            //退单和历史退单共用一个Index，用于修改菜单栏的选中状态
            if (!string.IsNullOrEmpty(Request.Query["confirm_time"]))
            {
                ViewBag.flag = "History";
            }
            else
            {
                ViewBag.flag = "Return";
            }
            SaleReturn saleReturn = SM.SelectByReturnIndex(return_index);
            ViewBag.return_index = return_index;
            ViewBag.deliver_index = saleReturn.deliver_index;
            ViewBag.order_index = saleReturn.order_index;
            ViewBag.company_name = saleReturn.company_name;
            ViewBag.company_order_index = saleReturn.company_order_index;

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

            var objList = SM.SelectSeqByReturnIndex(return_index);
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
                string return_index = Convert.ToString(Request.Query["return_index"]);
                ViewBag.return_index = return_index;
                SaleReturn sale = SM.SelectByReturnIndex(return_index);
                ViewBag.deliver_index = sale.deliver_index;
                ViewBag.order_index = sale.order_index;
                ViewBag.order_id = sale.order_id;
                ViewBag.company_name = sale.company_name;
                ViewBag.company_order_index = sale.company_order_index;

                List<SaleReturn> res = SM.SelectSeqByReturnIndex(return_index);

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
            string return_index = Convert.ToString(HttpContext.Request.Form["return_index"]);
            ViewBag.return_index = return_index;
            int order_id = Convert.ToInt32(HttpContext.Request.Form["order_id"]);
            string deliver_index = Convert.ToString(HttpContext.Request.Form["deliver_index"]);


            string[] seq_id = Convert.ToString(HttpContext.Request.Form["Seq_Id"]).Split(',');
            string[] return_num = Convert.ToString(HttpContext.Request.Form["Return_Num"]).Split(',');
            string[] return_time = Convert.ToString(HttpContext.Request.Form["Return_Time"]).Split(',');
            string[] return_price = Convert.ToString(HttpContext.Request.Form["Return_Price"]).Split(',');
            string[] return_all_price = Convert.ToString(HttpContext.Request.Form["Return_All_Price"]).Split(',');


            bool flag = true;
            SaleReturn objSale = new SaleReturn();
            objSale.return_index = return_index;
            objSale.order_id = order_id;
            objSale.deliver_index = deliver_index;

            objSale.insert_time = DateTime.Now.ToLocalTime();

            Order objOrder = new Order();
            for (int i = 0; i < inputNum; i++)
            {
                objSale.seq_id = Convert.ToInt32(seq_id[i]);
                SaleReturn saleOld = SM.SelectById(objSale.seq_id, return_index);//获取原来的退货数量

                if (return_num[i] == "已退货")
                {
                    continue;
                }
                else
                {
                    if (return_num[i] == "")
                    {
                        objSale.return_num = 0;
                    }
                    else
                    {
                        objSale.return_num = Convert.ToInt32(return_num[i]);
                    }

                }

                if (return_time[i] == "" || return_num[i] == "" || return_num[i] == "0")
                {
                    objSale.return_time = Convert.ToDateTime("0001/01/01 0:00:00");
                }
                else
                {
                    objSale.return_time = Convert.ToDateTime(return_time[i]);
                }

                if (return_price[i] == "" || return_num[i] == "" || return_num[i] == "0")
                {
                    objSale.return_price = 0;
                }
                else
                {
                    objSale.return_price = Convert.ToDouble(return_price[i]);
                }

                if (return_all_price[i] == "" || return_num[i] == "" || return_num[i] == "0")
                {
                    objSale.return_all_price = 0;
                }
                else
                {
                    objSale.return_all_price = Convert.ToDouble(return_all_price[i]);
                }             

                //Order order = OM.SelectByOrderSeqId(order_id, objSale.seq_id);
                ////开单数量和剩余数量不变，只修改退单数量（累加）
                //objOrder.remain_num = order.remain_num;
                //objOrder.open_num = order.open_num;

                //objOrder.tui_num = order.tui_num-saleOld.return_num+ objSale.return_num;
                //objOrder.seq_id = objSale.seq_id;
                ////更新OrderSeq
                //OM.UpdateSeqNum(objOrder);

                //更新Sale
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
                    ViewBag.Message = "删除成功！";
                    return Json("Success");
                }
                else
                {
                    ViewBag.Message = "删除失败！";
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