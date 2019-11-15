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
        public IActionResult Index(string start_time, string end_time, string company_name, string purchase_index, string material_name,string deliver_index, int pageindex = 1, int pagesize = 8)
        {
            

            ViewBag.start_time = start_time;
            ViewBag.end_time = end_time;

            ViewBag.company_name = company_name;
            ViewBag.purchase_index = purchase_index;
            ViewBag.material_name = material_name;
            ViewBag.deliver_index = deliver_index;

            if (start_time == null)
            {
                start_time = "2001-01-01";
            }
            if (end_time == null)
            {
                end_time = "2222-01-01";
            }

            var objList = PM.SelectDeliverAll(start_time, end_time, company_name, purchase_index, material_name,deliver_index);
            var pagedList = PagedList<Purchase>.PageList(pageindex, pagesize, objList);
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
                    Purchase purchase = PM.SelectById(id);
                    return View(purchase);
                }
                else
                {
                    List<Purchase> listPurchase = PM.SelectTodayForPurchaseIndex(DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd"));
                    string count_num = "";
                    int num = listPurchase.Count + 1;
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

                    ViewBag.supplier_id = 0;

                    ViewBag.purchase_index = SM.SelectInUse(4).index_begin + DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd").Replace("-", "") + count_num;

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
        public IActionResult EditHandle(Purchase purchase)
        {

            //验证账户是否存在
            int id = purchase.id;
            int supplier_id = purchase.supplier_id;

            string purchase_index = purchase.purchase_index;
            string category = purchase.category;
            string material_name = purchase.material_name;
            string material_spec = purchase.material_spec;
            string material_unit = purchase.material_unit;

            int material_num = purchase.material_num;

            double material_price = purchase.material_price;
            double material_all_price = purchase.material_all_price;

            string deliver_index = purchase.deliver_index;
            DateTime deliver_time = purchase.deliver_time;
            int money_onoff = purchase.money_onoff;
            int money_way = purchase.money_way;

            

            int count = 0;
            Purchase oldPurchase = PM.SelectById(id);//获取现有的入库量

            Purchase objPurchase = new Purchase();

            objPurchase.supplier_id = supplier_id;
            objPurchase.purchase_index = purchase_index;
            objPurchase.category = category;
            objPurchase.material_name = material_name;
            objPurchase.material_spec = material_spec;
            objPurchase.material_unit = material_unit;
            objPurchase.material_num = material_num;
            objPurchase.material_price = material_price;
            objPurchase.material_all_price = material_all_price;
            objPurchase.purchase_time = DateTime.Now.ToLocalTime();

            objPurchase.deliver_index = deliver_index;
            objPurchase.deliver_time = deliver_time;
            objPurchase.money_onoff = money_onoff;
            objPurchase.money_way = money_way;
            if (id > 0)
            {
                objPurchase.id = id;
                count = PM.UpdateDeliver(objPurchase);
            }
            else
            {
                count = PM.Insert(objPurchase);
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

        /// <summary>
        /// 获取材料规格
        /// </summary>
        /// <returns></returns>
        public IActionResult getProductPrice(int material_num, double material_price)
        {
            ViewBag.material_all_price = material_price * material_num;
            return View();
        }


        public IActionResult SupplierIndex(string start_time, string end_time, string company_name, string purchase_index, string material_name, string deliver_index, int pageindex = 1, int pagesize = 8)
        {
            
            ViewBag.start_time = start_time;
            ViewBag.end_time = end_time;

            ViewBag.company_name = company_name;
            ViewBag.purchase_index = purchase_index;
            ViewBag.material_name = material_name;
            ViewBag.deliver_index = deliver_index;

            if (start_time == null)
            {
                start_time = "2001-01-01";
            }
            if (end_time == null)
            {
                end_time = "2222-01-01";
            }

            var objList = PM.SelectDeliverAll(start_time, end_time, company_name, purchase_index, material_name, deliver_index);
            var pagedList = PagedList<Purchase>.PageList(pageindex, pagesize, objList);
            ViewBag.model = pagedList.Item2;
            return View(pagedList.Item1);
            
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
                    dt = Convert.ToDateTime("0001/1/1 0:00:00");
                }
                else
                {
                    status = 1;
                    dt= DateTime.Now.ToLocalTime();
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


        public IActionResult HistoryIndex(string start_time, string end_time, string company_name, string purchase_index, string material_name, string deliver_index, int pageindex = 1, int pagesize = 8)
        {
            
            ViewBag.start_time = start_time;
            ViewBag.end_time = end_time;

            ViewBag.company_name = company_name;
            ViewBag.purchase_index = purchase_index;
            ViewBag.material_name = material_name;
            ViewBag.deliver_index = deliver_index;

            if (start_time == null)
            {
                start_time = "2001-01-01";
            }
            if (end_time == null)
            {
                end_time = "2222-01-01";
            }

            var objList = PM.SelectHistory(start_time, end_time, company_name,purchase_index, material_name, deliver_index);
            var pagedList = PagedList<Purchase>.PageList(pageindex, pagesize, objList);
            ViewBag.model = pagedList.Item2;
            return View(pagedList.Item1);
            
        }
    }
}