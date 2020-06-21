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
    public class DeliverController : BaseController
    {
        PurchaseManager PM = new PurchaseManager();
        SettingManager SM = new SettingManager();
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
        public IActionResult GetData(string start_time, string end_time, string company_name, string purchase_index,
            string material_name, string deliver_index, string category)
        {
            ViewBag.start_time = start_time;
            ViewBag.end_time = end_time;

            ViewBag.company_name = company_name;
            ViewBag.purchase_index = purchase_index;
            ViewBag.material_name = material_name;
            ViewBag.deliver_index = deliver_index;
            ViewBag.category = category;

            if (start_time == null)
            {
                start_time = "0001-01-01";
            }
            if (end_time == null)
            {
                end_time = "2222-01-01";
            }

            var objList = PM.SelectDeliverAll(start_time, end_time, company_name, purchase_index, material_name, deliver_index, category);
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
                if (id > 0)
                {
                    Purchase purchase = PM.SelectById(id);
                    return View(purchase);
                }
                else
                {
                    List<Purchase> listPurchase = PM.SelectTodayForPurchaseIndex(DateTime.Now.ToLocalTime().AddHours(8).ToString("yyyy-MM"));
                    string count_num = "";
                    int num = listPurchase.Count + 1;
                    if (num < 10)
                    {
                        count_num = "000" + num;
                    }
                    else if (num < 100)
                    {
                        count_num = "00" + num;
                    }
                    else if (num < 1000)
                    {
                        count_num = "0" + num;
                    }
                    else
                    {
                        count_num = "" + num;
                    }

                    ViewBag.supplier_id = 0;

                    ViewBag.purchase_index = SM.SelectConfigList(4)[0].config_list + DateTime.Now.ToLocalTime().AddHours(8).ToString("yyyy-MM").Replace("-", "") + count_num;

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
            //验证账户是否存在
            int id = 0;

            int supplier_id = Convert.ToInt32(HttpContext.Request.Form["supplier_id"]);
            string purchase_index = Convert.ToString(HttpContext.Request.Form["purchase_index"]);
            string category = Convert.ToString(HttpContext.Request.Form["category"]);

            string[] material_name = Convert.ToString(HttpContext.Request.Form["material_name"]).Split(',');
            string[] material_spec = Convert.ToString(HttpContext.Request.Form["material_spec"]).Split(',');
            string[] material_unit = Convert.ToString(HttpContext.Request.Form["material_unit"]).Split(',');
            string[] material_num = Convert.ToString(HttpContext.Request.Form["material_num"]).Split(',');
            string[] material_price = Convert.ToString(HttpContext.Request.Form["material_price"]).Split(',');
            string[] material_all_price = Convert.ToString(HttpContext.Request.Form["material_all_price"]).Split(',');
            int rowLength = Convert.ToInt32(HttpContext.Request.Form["rowLength"]);

            //int money_onoff = Convert.ToInt32(HttpContext.Request.Form["money_onoff"]);
            int money_way = Convert.ToInt32(HttpContext.Request.Form["money_way"]);
            DateTime deliver_time = Convert.ToDateTime(HttpContext.Request.Form["deliver_time"]);
            
            DateTime confirm_time = Convert.ToDateTime(HttpContext.Request.Form["confirm_time"]);
            string deliver_index = Convert.ToString(HttpContext.Request.Form["deliver_index"]);

            Purchase objPurchase = new Purchase();

            objPurchase.supplier_id = supplier_id;
            objPurchase.purchase_index = purchase_index;
            objPurchase.category = category;

            objPurchase.deliver_index = deliver_index;
            objPurchase.deliver_time = deliver_time;
            //objPurchase.money_onoff = money_onoff;
            objPurchase.money_way = money_way;
            if (material_name[0] != "")
            {
                //若为现金，则直接进入到历史
                if (money_way == 2)
                {
                    objPurchase.money_onoff = 1;
                    objPurchase.status = 1;
                    objPurchase.confirm_time = confirm_time;
                }
                for (int i = 0; i < material_name.Length; i++)
                {
                    int count = 0;
                    objPurchase.material_name = material_name[i];
                    objPurchase.material_spec = material_spec[i];
                    objPurchase.material_unit = material_unit[i];
                    objPurchase.material_num = Convert.ToDouble(material_num[i]);
                    objPurchase.material_price = Convert.ToDouble(material_price[i]);
                    objPurchase.material_all_price = Convert.ToDouble(material_all_price[i]);
                    objPurchase.purchase_time = DateTime.Now.ToLocalTime().AddHours(8);
                    count = PM.Insert(objPurchase);
                    if (count < 1)
                    {
                        return Json("Fail");
                    }
                }
                return Json("Success");
            }
            else
            {
                return Json("Fail");
            }

        }

        public IActionResult EditDetail()
        {
            try
            {
                int id = Convert.ToInt32(Request.Query["id"]);
                Purchase purchase = PM.SelectById(id);
                return View(purchase);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult EditHandleDetail(Purchase purchase)
        {
            Purchase newPurchase = purchase;
            int count1 = PM.UpdateDeliver(newPurchase);
            if (purchase.money_way==2) {
                purchase.status = 1;
            }
            int count2 = PM.UpdateCommonDeliver(newPurchase);//更新公共部分
            if (count1 > 0 && count2 > 0)
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
                int count = PM.Del(id);
                ViewBag.Comfirm = 0;
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

        public IActionResult GetUnits()
        {
            List<Setting> settings = SM.SelectConfigList(1);
            return Json(settings);
        }
        public IActionResult GetMaterials()
        {
            List<Setting> settings = SM.SelectConfigList(10);
            return Json(settings);
        }

        /// <summary>
        /// 获取材料规格
        /// </summary>
        /// <returns></returns>
        public IActionResult GetProductPrice(int id, double material_num, double material_price)
        {
            ViewBag.material_all_price = material_price * material_num;
            ViewBag.id = "material_all_price" + id;
            return View();
        }


        public IActionResult SupplierIndex()
        {
            return View();
        }
        public IActionResult GetSupplierData(string start_time, string end_time, string company_name, string purchase_index,
            string material_name, string deliver_index, string category)
        {
            ViewBag.start_time = start_time;
            ViewBag.end_time = end_time;

            ViewBag.company_name = company_name;
            ViewBag.purchase_index = purchase_index;
            ViewBag.material_name = material_name;
            ViewBag.deliver_index = deliver_index;
            ViewBag.category = category;

            if (start_time == null)
            {
                start_time = "0001-01-01";
            }
            if (end_time == null)
            {
                end_time = "2222-01-01";
            }

            var objList = PM.SelectDeliverAll(start_time, end_time, company_name, purchase_index, material_name, deliver_index, category);
            return Json(objList);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <returns></returns>
        public IActionResult SupplierConfirm()
        {
            try
            {
                int id = Convert.ToInt32(Request.Query["id"]);
                Purchase purchase = PM.SelectById(id);
                int status = 0;

                DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
                dtFormat.ShortDatePattern = "yyyy/M/d H:mm:ss";
                DateTime dt = Convert.ToDateTime("0001/1/1 0:00:00", dtFormat);

                if (purchase.status == 1)
                {
                    status = 0;
                    purchase.money_onoff = 0;
                    dt = Convert.ToDateTime("0001/1/1 0:00:00");
                }
                else
                {
                    status = 1;
                    purchase.money_onoff = 1;
                    dt = Convert.ToDateTime(Request.Query["comfirm_time"]);
                }
                purchase.status = status;
                purchase.confirm_time = dt;
                int count = PM.UpdateDeliverStatus(purchase);
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


        public IActionResult HistoryIndex()
        {
            return View();
        }

        public IActionResult GetHistoryData(string start_time, string end_time, string confirm_start_time, string confirm_end_time, string company_name, string purchase_index,
            string material_name, string deliver_index, string category, string day, string month, string year)
        {
            ViewBag.start_time = start_time;
            ViewBag.end_time = end_time;

            ViewBag.confirm_start_time = confirm_start_time;
            ViewBag.confirm_end_time = confirm_end_time;

            ViewBag.company_name = company_name;
            ViewBag.purchase_index = purchase_index;
            ViewBag.material_name = material_name;
            ViewBag.deliver_index = deliver_index;
            ViewBag.category = category;

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

            DateTime dt = DateTime.Now.AddHours(8);
            DateTime dt2 = dt.AddMonths(1);

            if (day == "1")
            {
                confirm_start_time = DateTime.Now.ToLocalTime().AddHours(8).ToString("yyyy-MM-dd");
                confirm_end_time = DateTime.Now.ToLocalTime().AddHours(8).ToString("yyyy-MM-dd");
                ViewBag.day = "1";
            }
            if (month == "1")
            {
                confirm_start_time = dt.AddDays(-(dt.Day) + 1).ToString("yyyy-MM-dd");
                confirm_end_time = dt2.AddDays(-dt.Day).ToString("yyyy-MM-dd");
                ViewBag.month = "1";
            }
            if (year == "1")
            {
                confirm_start_time = dt.AddMonths(-dt.Month + 1).AddDays(-dt.Day + 1).ToString("yyyy-MM-dd");
                confirm_end_time = new DateTime(DateTime.Now.AddHours(8).Year, 12, 31).ToString("yyyy-MM-dd");
                ViewBag.year = "1";
            }

            var objList = PM.SelectHistory(start_time, end_time, confirm_start_time, confirm_end_time, company_name, purchase_index, material_name, deliver_index, category);
            return Json(objList);
        }
    }
}