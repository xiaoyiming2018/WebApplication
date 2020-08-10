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
    public class OrderController : BaseController
    {
        OrderManager OM = new OrderManager();
        SettingManager SM = new SettingManager();
        ContactManager CM = new ContactManager();
        /// <summary>
        /// 用户列表首页
        /// </summary>
        /// <param name="page">分页页码</param>
        /// <param name="size">每页显示数量</param>
        /// <returns></returns>
        public IActionResult Index(string start_time, string end_time, string deliver_start_time, string deliver_end_time, string company_name,
            string order_index, string company_order_index, string purchase_person, string order_name)
        {
            ViewBag.company_name = company_name;
            ViewBag.order_index = order_index;
            ViewBag.company_order_index = company_order_index;
            ViewBag.purchase_person = purchase_person;
            ViewBag.order_name = order_name;

            ViewBag.start_time = start_time;
            ViewBag.end_time = end_time;
            ViewBag.deliver_start_time = deliver_start_time;
            ViewBag.deliver_end_time = deliver_end_time;
            return View();
        }

        public IActionResult GetData(string start_time, string end_time, string deliver_start_time, string deliver_end_time, string company_name,
            string order_index, string company_order_index, string purchase_person, string order_name) {

            if (start_time == null)
            {
                start_time = "0001-01-01";
            }
            if (end_time == null)
            {
                end_time = "2222-01-01";
            }

            if (deliver_start_time == null)
            {
                deliver_start_time = "0001-01-01";
            }
            if (deliver_end_time == null)
            {
                deliver_end_time = "2222-01-01";
            }


            var objList = OM.SelectAll(0, start_time, end_time, deliver_start_time, deliver_end_time, company_name, order_index, company_order_index, purchase_person, order_name);
            return Json(objList);
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
                List<string> contacts = CM.SelectAllContact().GroupBy(s=>s.name).Select(s=>s.Key).ToList();
                ViewBag.contacts = contacts;
                if (id > 0)
                {
                    
                    ViewBag.order_time = DateTime.Now.ToLocalTime().AddHours(8).ToString("yyyy-MM-dd").Replace("-", "");
                    var obj = OM.SelectById(id);//只允许修改order_info里面的字段
                    return View(obj);
                }
                else
                {
                    List<Order> listOrder =OM.SelectTodayForOrderIndex(DateTime.Now.ToLocalTime().AddHours(8).ToString("yyyy-MM"));
                    string count_num = "";
                    int num = listOrder.Count + 1;
                    if (num < 10)
                    {
                        count_num = "000" + num;
                    }
                    else if (num < 100)
                    {
                        count_num = "00" + num;
                    }
                    else if (num < 1000) {
                        count_num = "0" + num;
                    }
                    else
                    {
                        count_num = "" + num;
                    }                   

                    ViewBag.order_index = SM.SelectConfigList(3)[0].config_list + DateTime.Now.ToLocalTime().AddHours(8).ToString("yyyy-MM").Replace("-", "")+ count_num;
                    ViewBag.order_time = DateTime.Now.ToLocalTime().AddHours(8).ToString("yyyy-MM-dd");
                    return View();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult GetContacts()
        {
            List<string> contacts = CM.SelectAllContact().GroupBy(s=>s.name).Select(s=>s.Key).ToList();
            return Json(contacts);

        }

        /// <summary>
        /// 编辑处理页面
        /// </summary>
        /// <returns></returns>
        public IActionResult EditHandle()
        {
            //验证账户是否存在
            int id = 0;

            int count = 0;
            int customer_id = Convert.ToInt32(HttpContext.Request.Form["customer_id"]);
            string order_index = Convert.ToString(HttpContext.Request.Form["order_index"]);
            string company_order_index = Convert.ToString(HttpContext.Request.Form["company_order_index"]);

            string[] order_name = Convert.ToString(HttpContext.Request.Form["Order_Name"]).Split(',');
            string[] order_time = Convert.ToString(HttpContext.Request.Form["Order_Time"]).Split(',');
            string[] deliver_time = Convert.ToString(HttpContext.Request.Form["Deliver_Time"]).Split(',');
            string[] unit = Convert.ToString(HttpContext.Request.Form["Order_Unit"]).Split(',');
            string[] order_num = Convert.ToString(HttpContext.Request.Form["Order_Num"]).Split(',');
            string[] order_price = Convert.ToString(HttpContext.Request.Form["Order_Price"]).Split(',');
            string[] order_all_price = Convert.ToString(HttpContext.Request.Form["Order_All_Price"]).Split(',');
            string[] order_picture = Convert.ToString(HttpContext.Request.Form["Order_Picture"]).Split(',');
            string[] purchase_person = Convert.ToString(HttpContext.Request.Form["Purchase_Person"]).Split(',');
            string[] remark = Convert.ToString(HttpContext.Request.Form["Remark"]).Split(',');

            int rowLength = Convert.ToInt32(HttpContext.Request.Form["rowLength"]); 
            Order orderList = new Order();
            if (!string.IsNullOrEmpty(HttpContext.Request.Form["id"]))
            {
                id = Convert.ToInt32(HttpContext.Request.Form["id"]);
            }
            orderList.customer_id = customer_id;
            orderList.order_index = order_index;
            orderList.company_order_index = company_order_index;
            if (OM.SelectByOrderIndex(order_index) != null && id == 0)
            {
                return Json("Fail");
            }

            if (id > 0)
            {
                orderList.id = id;
                count = OM.UpdateOrder(orderList);
            }
            else
            {
                count = OM.InsertOrder(orderList);
                if (count > 0)
                {
                    for (int i = 0; i < rowLength; i++)
                    {
                        orderList.order_name = order_name[i];
                        orderList.order_time = Convert.ToDateTime(order_time[i]);
                        orderList.deliver_time = Convert.ToDateTime(deliver_time[i]);
                        orderList.unit = unit[i];
                        orderList.purchase_person = purchase_person[i];
                        orderList.remark = remark[i];
                        if (order_num[i] == "")
                        {
                            orderList.order_num = 0;
                        }
                        else
                        {
                            orderList.order_num = Convert.ToDouble(order_num[i]);
                        }

                        if (order_price[i] == "")
                        {
                            orderList.order_price = 0;
                        }
                        else
                        {
                            orderList.order_price = Convert.ToDouble(order_price[i]);
                        }

                        if (order_all_price[i] == "")
                        {
                            orderList.order_all_price = 0;
                        }
                        else
                        {
                            orderList.order_all_price = Convert.ToDouble(order_all_price[i]);
                        }


                        orderList.order_picture = order_picture[i];

                        Order or = OM.SelectByOrderIndex(order_index);
                        orderList.order_id = or.id;
                        OM.InsertOrderSeq(orderList);

                    }
                }
                               
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
        /// 获取价格
        /// </summary>
        /// <returns></returns>
        public IActionResult GetProductPrice(int id, double order_num, double order_price)
        {
            ViewBag.order_all_price = order_num * order_price;
            ViewBag.id = "order_all_price"+id;
            return View();
        }

        public IActionResult HistoryIndex()
        {  
            return View();
        }

        public IActionResult GetHistoryData(string start_time, string end_time, string deliver_start_time, string deliver_end_time,
            string company_name, string order_index, string company_order_index, string purchase_person, string order_name, string day, string month, string year) {
            ViewBag.company_name = company_name;
            ViewBag.order_index = order_index;
            ViewBag.company_order_index = company_order_index;
            ViewBag.purchase_person = purchase_person;
            ViewBag.order_name = order_name;

            ViewBag.start_time = start_time;
            ViewBag.end_time = end_time;
            ViewBag.deliver_start_time = deliver_start_time;
            ViewBag.deliver_end_time = deliver_end_time;

            if (start_time == null)
            {
                start_time = "0001-01-01";
            }
            if (end_time == null)
            {
                end_time = "2222-01-01";
            }

            if (deliver_start_time == null)
            {
                deliver_start_time = "0001-01-01";
            }
            if (deliver_end_time == null)
            {
                deliver_end_time = "2222-01-01";
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

            var objList = OM.SelectAll(1, start_time, end_time, deliver_start_time, deliver_end_time, company_name, order_index, company_order_index, purchase_person, order_name);
            return Json(objList);
        }

    }
}