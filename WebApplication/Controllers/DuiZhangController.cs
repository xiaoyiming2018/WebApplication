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
    public class DuiZhangController : BaseController
    {
        DuiZhangManager DZM = new DuiZhangManager();
        SaleManager SM = new SaleManager();
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

        public IActionResult GetData(string dz_start_time, string dz_end_time, string deliver_company_head, string dz_index, string day, string month, string year)
        {

            ViewBag.dz_start_time = dz_start_time;
            ViewBag.dz_end_time = dz_end_time;
            ViewBag.dz_index = dz_index;

            ViewBag.deliver_company_head = deliver_company_head;

            if (dz_start_time == null)
            {
                dz_start_time = "0001-01-01";
            }
            if (dz_end_time == null)
            {
                dz_end_time = "2222-01-01";
            }

            DateTime dt = DateTime.Now.AddHours(8);
            DateTime dt2 = dt.AddMonths(1);

            if (day == "1")
            {
                dz_start_time = DateTime.Now.ToLocalTime().AddHours(8).ToString("yyyy-MM-dd");
                dz_end_time = DateTime.Now.ToLocalTime().AddHours(8).ToString("yyyy-MM-dd");
                ViewBag.day = "1";
            }
            if (month == "1")
            {
                dz_start_time = dt.AddDays(-(dt.Day) + 1).ToString("yyyy-MM-dd");
                dz_end_time = dt2.AddDays(-dt.Day).ToString("yyyy-MM-dd");
                ViewBag.month = "1";
            }
            if (year == "1")
            {
                dz_start_time = dt.AddMonths(-dt.Month + 1).AddDays(-dt.Day + 1).ToString("yyyy-MM-dd");
                dz_end_time = new DateTime(DateTime.Now.AddHours(8).Year, 12, 31).ToString("yyyy-MM-dd");
                ViewBag.year = "1";
            }

            var objList = DZM.SelectForInvoice(dz_start_time, dz_end_time, deliver_company_head, dz_index);
            return Json(objList);
        }

        public IActionResult GetPickerList(string DZ_Index)
        {
            string[] duizhangIndex = DZ_Index.Split(',');
            List<DuiZhang> duiZhangs = new List<DuiZhang>();
            if (duizhangIndex.Length <= 1 && duizhangIndex[0] == "")
            {
                return View(duiZhangs);
            }

            for (int i = 0; i < duizhangIndex.Length; i++)
            {
                List<DuiZhang> duiZhang = DZM.SelectByDzIndex(duizhangIndex[i]);
                duiZhangs.AddRange(duiZhang);
            }

            return Json(duiZhangs);


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
                string[] dzId = Convert.ToString(HttpContext.Request.Form["DZ_Id"]).Split(',');
                string invoice_index = Convert.ToString(HttpContext.Request.Form["invoice_index"]);
                DuiZhang dz = new DuiZhang();
                if (dzId.Length <= 1 && dzId[0] == "")
                {
                    return Json("Fail");
                }

                for (int i = 0; i < dzId.Length; i++)
                {
                    dz.id = Convert.ToInt32(dzId[i]);
                    dz.invoice_index = invoice_index;
                    dz.invoice_time = DateTime.Now.ToLocalTime().AddHours(8);
                    count = DZM.Update(dz);
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



        /// <summary>
        /// 用户列表首页
        /// </summary>
        /// <param name="page">分页页码</param>
        /// <param name="size">每页显示数量</param>
        /// <returns></returns>
        public IActionResult HistoryIndex()
        {
            return View();
        }

        public IActionResult GetHistoryData(string deliver_start_time, string deliver_end_time, string dz_start_time, string dz_end_time, string company_order_index,
            string company_name, string order_name, string dz_index, string invoice_index, string deliver_index, string day, string month, string year)
        {
            ViewBag.deliver_start_time = deliver_start_time;
            ViewBag.deliver_end_time = deliver_end_time;

            ViewBag.dz_start_time = dz_start_time;
            ViewBag.dz_end_time = dz_end_time;

            ViewBag.company_order_index = company_order_index;
            ViewBag.order_name = order_name;
            ViewBag.dz_index = dz_index;
            ViewBag.invoice_index = invoice_index;
            ViewBag.deliver_index = deliver_index;

            ViewBag.company_name = company_name;

            if (deliver_start_time == null)
            {
                deliver_start_time = "0001-01-01";
            }
            if (deliver_end_time == null)
            {
                deliver_end_time = "2222-01-01";
            }

            if (dz_start_time == null)
            {
                dz_start_time = "0001-01-01";
            }
            if (dz_end_time == null)
            {
                dz_end_time = "2222-01-01";
            }

            DateTime dt = DateTime.Now.AddHours(8);
            DateTime dt2 = dt.AddMonths(1);

            if (day == "1")
            {
                dz_start_time = DateTime.Now.ToLocalTime().AddHours(8).ToString("yyyy-MM-dd");
                dz_end_time = DateTime.Now.ToLocalTime().AddHours(8).ToString("yyyy-MM-dd");
                ViewBag.day = "1";
            }
            if (month == "1")
            {
                dz_start_time = dt.AddDays(-(dt.Day) + 1).ToString("yyyy-MM-dd");
                dz_end_time = dt2.AddDays(-dt.Day).ToString("yyyy-MM-dd");
                ViewBag.month = "1";
            }
            if (year == "1")
            {
                dz_start_time = dt.AddMonths(-dt.Month + 1).AddDays(-dt.Day + 1).ToString("yyyy-MM-dd");
                dz_end_time = new DateTime(DateTime.Now.AddHours(8).Year, 12, 31).ToString("yyyy-MM-dd");
                ViewBag.year = "1";
            }

            var objList = DZM.SelectHistory(deliver_start_time, deliver_end_time, dz_start_time, dz_end_time, company_order_index, company_name,
                order_name, dz_index, invoice_index, deliver_index);
            return Json(objList);
        }

        /// <summary>
        /// 编辑处理Excel导入的批量信息
        /// </summary>
        /// <returns></returns>
        public IActionResult EditHandleList()
        {
            int check = 0;
            string[] allList = Convert.ToString(HttpContext.Request.Form["AllList"]).Split(',');
            int rowLength = Convert.ToInt32(HttpContext.Request.Form["num"]) - 1;

            int all = 0;

            for (int i = 0; i < rowLength; i++)
            {
                string[] res = new string[11];
                Array.Copy(allList, i * 11, res, 0, 11);

                if (res[1] != "")
                {

                    DuiZhang duiZhang = new DuiZhang();

                    duiZhang.deliver_time = Convert.ToDateTime(res[1]);
                    duiZhang.deliver_index = res[2];
                    duiZhang.company_name = res[3];
                    duiZhang.order_index = res[4];
                    duiZhang.order_name = res[5] + res[6];
                    duiZhang.unit = res[7];
                    duiZhang.dui_num = Convert.ToDouble(res[8]);
                    duiZhang.dui_price = Convert.ToDouble(res[9]);
                    duiZhang.dui_all_price = Convert.ToDouble(res[10]);
                    duiZhang.invoice_index = " ";

                    int count = DZM.Insert(duiZhang);
                    if (count > 0)
                    {
                        check++;
                    }
                    all++;
                }
            }
            string result = "共" + all + "笔数据，导入成功" + check + "笔数据";
            return Json(result);
        }

    }
}