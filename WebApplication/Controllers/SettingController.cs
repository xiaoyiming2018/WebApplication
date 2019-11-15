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
    public class SettingController : BaseController
    {
        SettingManager SM = new SettingManager();
        /// <summary>
        /// 用户列表首页
        /// </summary>
        /// <param name="page">分页页码</param>
        /// <param name="size">每页显示数量</param>
        /// <returns></returns>
        public IActionResult Index()
        {
            
            Setting settingOrder = SM.SelectInUse(3);
            Setting settingPurchase = SM.SelectInUse(4);
            Setting settingDeliver = SM.SelectInUse(5);
            Setting settingReturn = SM.SelectInUse(7);
            //订单开头
            ViewBag.order_index_begin = settingOrder.index_begin;
            ViewBag.order_index_view = settingOrder.index_begin + "20190101001";
            //采购单开头
            ViewBag.purchase_index_begin = settingPurchase.index_begin;
            ViewBag.purchase_index_view = settingPurchase.index_begin + "20190101001";
            //出货单开头
            ViewBag.deliver_index_begin = settingDeliver.index_begin;
            ViewBag.deliver_index_view = settingDeliver.index_begin + "20190101001";
            //退货单开头
            ViewBag.return_index_begin = settingReturn.index_begin;
            ViewBag.return_index_view = settingReturn.index_begin + "20190101001";
            return View();
            
        }

        /// <summary>
        /// 插入更新页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Edit(string material_unit,string category, string order_index_begin, string purchase_index_begin, 
            string deliver_index_begin, string deliver_company_head,string return_index_begin)
        {
            try
            {
                Setting res = new Setting();
                if (!string.IsNullOrEmpty(material_unit))
                {
                    res.config_list = material_unit;
                    res.all_type = 1;
                    int count=SM.Insert(res);
                    if (count>0)
                    {
                        return Json("Success");
                    }
                    else
                    {
                        return Json("Fail");
                    }
                    
                }
                else if (!string.IsNullOrEmpty(category))
                {
                    res.config_list = category;
                    res.all_type = 2;
                    int count = SM.Insert(res);
                    if (count > 0)
                    {
                        return Json("Success");
                    }
                    else
                    {
                        return Json("Fail");
                    }
                }
                else if (!string.IsNullOrEmpty(order_index_begin))
                {
                    res.index_begin = order_index_begin;
                    res.all_type = 3;
                    res.in_use = 1;
                    int count = SM.Update(res);
                    if (count > 0)
                    {
                        return Json("Success");
                    }
                    else
                    {
                        return Json("Fail");
                    }
                }
                else if (!string.IsNullOrEmpty(purchase_index_begin))
                {
                    res.index_begin = purchase_index_begin;
                    res.all_type = 4;
                    res.in_use = 1;
                    int count = SM.Update(res);
                    if (count > 0)
                    {
                        return Json("Success");
                    }
                    else
                    {
                        return Json("Fail");
                    }
                }
                else if (!string.IsNullOrEmpty(deliver_index_begin))
                {
                    res.index_begin = deliver_index_begin;
                    res.all_type = 5;
                    res.in_use = 1;
                    int count = SM.Update(res);
                    if (count > 0)
                    {
                        return Json("Success");
                    }
                    else
                    {
                        return Json("Fail");
                    }
                }
                else if(!string.IsNullOrEmpty(deliver_company_head))
                {
                    res.config_list = deliver_company_head;
                    res.all_type = 6;
                    int count = SM.Insert(res);
                    if (count > 0)
                    {
                        return Json("Success");
                    }
                    else
                    {
                        return Json("Fail");
                    }
                }
                else
                {
                    res.index_begin = return_index_begin;
                    res.all_type = 7;
                    res.in_use = 1;
                    int count = SM.Update(res);
                    if (count > 0)
                    {
                        return Json("Success");
                    }
                    else
                    {
                        return Json("Fail");
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}