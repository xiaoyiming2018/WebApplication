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
        DuiZhangManager DZM = new DuiZhangManager();
        OrderManager OM = new OrderManager();
        SettingManager SettingM = new SettingManager();
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

        public IActionResult GetData(string start_time, string end_time, string deliver_index, string deliver_company_head) {
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
            return Json(objList);
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
                    List<Sale> listSale = SM.SelectTodayForDeliverIndex(DateTime.Now.ToLocalTime().AddHours(8).ToString("yyyy-MM-dd"));
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
                    ViewBag.deliver_index = SettingM.SelectConfigList(5)[0].config_list + DateTime.Now.ToLocalTime().AddHours(8).ToString("yyyy-MM-dd").Replace("-", "") + count_num;
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
            
            string deliver_index = Convert.ToString(HttpContext.Request.Form["deliver_index"]);
            ViewBag.deliver_index = deliver_index;
            string deliver_company_head = Convert.ToString(HttpContext.Request.Form["deliver_company_head"]);

            string[] order_id = Convert.ToString(HttpContext.Request.Form["order_id"]).Split(',');
            string[] seq_id = Convert.ToString(HttpContext.Request.Form["Seq_Id"]).Split(',');
            string[] real_num = Convert.ToString(HttpContext.Request.Form["Real_Num"]).Split(',');
            string[] deliver_price = Convert.ToString(HttpContext.Request.Form["Deliver_Price"]).Split(',');
            string[] deliver_all_price = Convert.ToString(HttpContext.Request.Form["Deliver_All_Price"]).Split(',');
            string[] remark = Convert.ToString(HttpContext.Request.Form["Remark"]).Split(',');

            bool flag = true;
            Sale objSale = new Sale();
            objSale.deliver_index = deliver_index;
            objSale.deliver_company_head = deliver_company_head;
            objSale.insert_time = DateTime.Now.ToLocalTime().AddHours(8);

            Order objOrder = new Order();
            for (int i = 0; i < inputNum; i++)
            {
                objSale.order_id = Convert.ToInt32(order_id[i]);
                objSale.seq_id = Convert.ToInt32(seq_id[i]);
                objSale.remark = Convert.ToString(remark[i]);

                if (real_num[i] == "" || real_num[i] == "出货已完成")
                {
                    continue;
                }
                else
                {
                    objSale.real_num = Convert.ToDouble(real_num[i]);
                    objOrder.open_num = Convert.ToDouble(real_num[i]);
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
        public IActionResult GetOrderList(string deliver_company_head)
        {
            List<Order> order = OM.SelectOrderSeqList(0,deliver_company_head).OrderByDescending(s=>s.remain_num).ToList();
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
        public IActionResult GetProductPrice(int id, double real_num, double deliver_price)
        {
            ViewBag.deliver_all_price = real_num * deliver_price;
            ViewBag.id = "order_all_price" + id;
            return View();
        }

        /// <summary>
        /// 结款管理
        /// </summary>
        /// <param name="start_time"></param>
        /// <param name="end_time"></param>
        /// <param name="deliver_index"></param>
        /// <param name="deliver_company_head"></param>
        /// <param name="order_name"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public IActionResult Payment()
        {            
            return View();
        }

        public IActionResult GetPaymentData(string start_time, string end_time, string deliver_index, string deliver_company_head, string order_name) 
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

            var objList = SM.SelectMoneyAll(start_time, end_time, deliver_index, deliver_company_head, order_name);
            return Json(objList);
        }

        /// <summary>
        /// 用户列表首页
        /// </summary>
        /// <param name="page">分页页码</param>
        /// <param name="size">每页显示数量</param>
        /// <returns></returns>
        public IActionResult MoneyIndex()
        {
            string count_num = "";
            int num = DZM.SelectHistory("0000-01-01", "2222-01-01", "0000-01-01", "2222-01-01").GroupBy(s => s.dz_index).Select(s => s.Key).ToList().Count + 1;//从1开始
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
            ViewBag.dz_index = SettingM.SelectConfigList(8)[0].config_list + count_num;
            return View();
        }
        public IActionResult GetMoneyIndexData(string start_time, string end_time, string deliver_index, string deliver_company_head,string order_name) 
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

            

            var objList = SM.SelectMoneyAll(start_time, end_time, deliver_index, deliver_company_head, order_name).Where(s => s.real_num > s.dz_num).ToList();
            return Json(objList);
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
                string[] saleid = Convert.ToString(HttpContext.Request.Form["Sale_Id"]).Split(',');
                string[] duinum = Convert.ToString(HttpContext.Request.Form["Dui_Num"]).Split(',');
                string[] duiprice = Convert.ToString(HttpContext.Request.Form["Dui_Price"]).Split(',');
                string dz_index = Convert.ToString(HttpContext.Request.Form["dz_index"]);
                DuiZhang dz = new DuiZhang();
                if (saleid.Length<=1 && saleid[0]=="")
                {
                    return Json("Fail");
                }

                for (int i=0;i< saleid.Length;i++)
                {
                    dz.sale_id= Convert.ToInt32(saleid[i]);

                    Sale sale = SM.SelectById(Convert.ToInt32(saleid[i]));
                    dz.company_order_index = sale.company_order_index;
                    dz.company_name = sale.deliver_company_head;
                    dz.order_name = sale.order_name;
                    dz.unit = sale.unit;
                    dz.dui_num = Convert.ToDouble(duinum[i]);
                    dz.dui_price = sale.deliver_price;
                    dz.dui_all_price =Convert.ToDouble(duiprice[i]);
                    dz.deliver_time = sale.insert_time;
                    dz.dz_index = dz_index;
                    dz.dui_time = DateTime.Now.ToLocalTime().AddHours(8);
                    count = DZM.Insert(dz);
                    if (count < 1)
                    {
                        flag = false;
                    }
                    else 
                    {
                        double dz_num = sale.dz_num + Convert.ToDouble(duinum[i]);
                        int id = Convert.ToInt32(saleid[i]);
                        int updateRes = SM.UpdateDZNum(dz_num, id);
                        if (updateRes < 1) 
                        {
                            flag = false;
                        }
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