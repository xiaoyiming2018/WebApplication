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
        public IActionResult Index(string start_time, string end_time, string deliver_index, string deliver_company_head,
            int pageindex = 1, int pagesize = 20)
        {

            ViewBag.start_time = start_time;
            ViewBag.end_time = end_time;

            ViewBag.deliver_index = deliver_index;
            ViewBag.deliver_company_head = deliver_company_head;

            if (start_time == null)
            {
                start_time = "0001-01-01";
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
        public IActionResult Edit(string order_index,string order_name)
        {
            try
            {        
                if (Request.Query["deliver_index"] !="new")
                {
                    string deliver_index = Convert.ToString(Request.Query["deliver_index"]);
                    Sale sale=SM.SelectDeliverByDeliverIndex(deliver_index);
                    if (sale != null)
                    {
                        ViewBag.deliver_index = deliver_index;
                        ViewBag.deliver_company_head = sale.deliver_company_head;
                        ViewBag.address = sale.address;
                        ViewBag.order_index = order_index;
                        ViewBag.order_name = order_name;

                        List<Sale> saleList = SM.SelectSeqByDeliverIndex(deliver_index, order_index, order_name);

                        return View(saleList);
                    }
                    else
                    {
                        return RedirectToAction("Index","Sale");
                    }  

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
            ViewBag.deliver_index = deliver_index;
            string deliver_company_head = Convert.ToString(HttpContext.Request.Form["deliver_company_head"]);

            string[] seq_id = Convert.ToString(HttpContext.Request.Form["Seq_Id"]).Split(',');
            string[] real_num = Convert.ToString(HttpContext.Request.Form["Real_Num"]).Split(',');
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

                if (real_num[i] != "" &&  deliver_price[i] == "")
                {
                    return Json("Price");
                }
                else
                {
                    objSale.deliver_price = Convert.ToDouble(deliver_price[i]);
                }

                if (real_num[i] != "" && deliver_all_price[i] == "")
                {
                    return Json("Price");
                }
                else
                {
                    objSale.deliver_all_price = Convert.ToDouble(deliver_all_price[i]);
                }
                Order order=OM.SelectByOrderSeqId(objSale.seq_id);
                objOrder.remain_num = order.remain_num - objOrder.open_num;
                objOrder.open_num = order.open_num + objOrder.open_num;
                objOrder.tui_num = order.tui_num;
                objOrder.seq_id = objSale.seq_id;
                //更新OrderSeq
                OM.UpdateSeqNum(objOrder);

                Order orderHistory = OM.SelectByOrderSeqId(Convert.ToInt32(seq_id[i]));
                
                if (orderHistory.remain_num == 0)
                {
                    Order res = new Order();
                    res.seq_id = Convert.ToInt32(seq_id[i]);
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
        public IActionResult GetOrderList(int order_id,string deliver_company_head)
        {
            List<Order> order = OM.SelectOrderSeqList(order_id, deliver_company_head);
            return View(order);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult GetOrderSelect(int order_id)
        {
            List<Order> order = OM.SelectAll(0,"0001-01-01","2222-01-01", "0001-01-01", "2222-01-01");
            return View(order);
        }

        /// <summary>
        /// 获取价格
        /// </summary>
        /// <returns></returns>
        public IActionResult GetProductPrice(int id, int real_num, double deliver_price)
        {
            ViewBag.deliver_all_price = real_num * deliver_price;
            ViewBag.id = "order_all_price" + id;
            return View();
        }

        /// <summary>
        /// 用户列表首页
        /// </summary>
        /// <param name="page">分页页码</param>
        /// <param name="size">每页显示数量</param>
        /// <returns></returns>
        public IActionResult MoneyIndex(string start_time, string end_time, string deliver_index, string deliver_company_head,
            string order_name,int pageindex = 1, int pagesize = 20)
        {
            ViewBag.start_time = start_time;
            ViewBag.end_time = end_time;

            ViewBag.deliver_index = deliver_index;

            ViewBag.deliver_company_head = deliver_company_head;
            ViewBag.order_name = order_name;

            if (start_time == null)
            {
                start_time = "0001-01-01";
            }
            if (end_time == null)
            {
                end_time = "2222-01-01";
            }

            List<Sale> sales = SM.SelectForDzIndex();
            string count_num = "";
            int num = sales.Count;
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
            ViewBag.dz_index = SettingM.SelectInUse(8).index_begin + count_num;

            var objList = SM.SelectMoneyAll(0, start_time, end_time, "0001-01-01", "2222-01-01", deliver_index, deliver_company_head,order_name);
            var pagedList = PagedList<Sale>.PageList(pageindex, pagesize, objList);
            ViewBag.model = pagedList.Item2;
            return View(pagedList.Item1);
        }

        /// <summary>
        /// 用户列表首页
        /// </summary>
        /// <param name="page">分页页码</param>
        /// <param name="size">每页显示数量</param>
        /// <returns></returns>
        public IActionResult MoneyHistoryIndex(string start_time, string end_time, string confirm_start_time, string confirm_end_time, string deliver_index, 
            string deliver_company_head,string order_name,string dz_index,string day, string month, string year, int pageindex = 1, int pagesize = 20)
        {
            ViewBag.start_time = start_time;
            ViewBag.end_time = end_time;

            ViewBag.confirm_start_time = confirm_start_time;
            ViewBag.confirm_end_time = confirm_end_time;

            ViewBag.deliver_index = deliver_index;
            ViewBag.order_name = order_name;
            ViewBag.dz_index = dz_index;

            ViewBag.deliver_company_head = deliver_company_head;

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

            var objList = SM.SelectMoneyAll(1, start_time, end_time, confirm_start_time, confirm_end_time, deliver_index, deliver_company_head,order_name,dz_index);
            var pagedList = PagedList<Sale>.PageList(pageindex, pagesize, objList);
            ViewBag.model = pagedList.Item2;
            return View(pagedList.Item1);
        }


        /// <summary>
        /// 用户列表首页
        /// </summary>
        /// <param name="page">分页页码</param>
        /// <param name="size">每页显示数量</param>
        /// <returns></returns>
        public IActionResult MoneyFinalIndex(string start_time, string end_time, string confirm_start_time, string confirm_end_time, string deliver_index,
            string deliver_company_head, string order_name, string dz_index,string invoice_index, string day, string month, string year, int pageindex = 1, int pagesize = 20)
        {
            ViewBag.start_time = start_time;
            ViewBag.end_time = end_time;

            ViewBag.confirm_start_time = confirm_start_time;
            ViewBag.confirm_end_time = confirm_end_time;

            ViewBag.deliver_index = deliver_index;
            ViewBag.order_name = order_name;
            ViewBag.dz_index = dz_index;
            ViewBag.invoice_index = invoice_index;

            ViewBag.deliver_company_head = deliver_company_head;

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

            var objList = SM.SelectMoneyAll(2, start_time, end_time, confirm_start_time, confirm_end_time, deliver_index, deliver_company_head, 
                order_name, dz_index, invoice_index);
            var pagedList = PagedList<Sale>.PageList(pageindex, pagesize, objList);
            ViewBag.model = pagedList.Item2;
            return View(pagedList.Item1);
        }
        /// <summary>
        /// 更新结款方式
        /// </summary>
        /// <returns></returns>
        public IActionResult MoneyEdit()
        {
            try
            {
                string deliver_index = Convert.ToString(HttpContext.Request.Form["deliver_index"]);
                int seq_id = Convert.ToInt32(HttpContext.Request.Form["seq_id"]);
                int money_onoff = Convert.ToInt32(HttpContext.Request.Form["money_onoff"]);
                int money_way = Convert.ToInt32(HttpContext.Request.Form["money_way"]);

                Sale sale = new Sale();
                sale.deliver_index = deliver_index;
                sale.seq_id = seq_id;
                sale.money_onoff = money_onoff;
                sale.money_way = money_way;
                int count = SM.UpdateMoney(sale);

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

        public IActionResult MoneySeqIndex()
        {
            string deliver_index = Convert.ToString(Request.Query["deliver_index"]);
            int seq_id = Convert.ToInt32(Request.Query["seq_id"]);

            Sale sale = SM.SelectSingleBySeqIndex(seq_id,deliver_index);
            return View(sale);
        }

        public IActionResult GetPickerList()
        {
            try
            {
                string[] duizhang = Convert.ToString(HttpContext.Request.Form["Dui_Zhang"]).Split(',');
                List<Sale> saleList = new List<Sale>();
                if (duizhang.Length <= 1 && duizhang[0]=="")
                {
                    return View(saleList);
                }

                for (int i = 0; i < duizhang.Length; i++)
                {
                    string[] detail = duizhang[i].Split('+');
                    string deliver_index = detail[0];
                    int seq_id = Convert.ToInt32(detail[1]);

                    Sale sale=SM.SelectSingleBySeqIndex(seq_id, deliver_index);
                    saleList.Add(sale);
                }
                return View(saleList);

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
        public IActionResult DuiZhang()
        {
            try
            {
                int count = 0;
                bool flag = true;
                string[] duizhang = Convert.ToString(HttpContext.Request.Form["Dui_Zhang"]).Split(',');
                string dz_index = Convert.ToString(HttpContext.Request.Form["dz_index"]);
                Sale sale = new Sale();
                if (duizhang.Length<1)
                {
                    return Json("Fail");
                }

                for (int i=0;i<duizhang.Length;i++)
                {
                    string[] detail= duizhang[i].Split('+');
                    string deliver_index = detail[0];
                    int seq_id = Convert.ToInt32(detail[1]);
                    sale.deliver_index = deliver_index;
                    sale.seq_id = seq_id;
                    sale.deliver_status = 1;
                    sale.confirm_time = DateTime.Now.ToLocalTime();
                    count = SM.UpdateDeliverStatus(sale);
                    SM.UpdateDzIndex(dz_index, deliver_index, seq_id);
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
        public IActionResult PutInvoice()
        {
            try
            {
                int count = 0;
                bool flag = true;
                string[] duizhang = Convert.ToString(HttpContext.Request.Form["Dui_Zhang"]).Split(',');
                string invoice_index = Convert.ToString(HttpContext.Request.Form["invoice_index"]);
                Sale sale = new Sale();
                if (duizhang.Length < 1)
                {
                    return Json("Fail");
                }

                for (int i = 0; i < duizhang.Length; i++)
                {
                    string[] detail = duizhang[i].Split('+');
                    string deliver_index = detail[0];
                    int seq_id = Convert.ToInt32(detail[1]);
                    sale.deliver_index = deliver_index;
                    sale.seq_id = seq_id;
                    sale.deliver_status = 2;
                    sale.real_time = DateTime.Now.ToLocalTime();//记录绑定发票的日期
                    count = SM.UpdateDeliverStatusInvoice(sale);
                    SM.UpdateInvoiceIndex(invoice_index, deliver_index, seq_id);
                    if (count < 1)
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
    }
}