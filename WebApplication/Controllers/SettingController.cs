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
            
            List<Setting> settingOrder = SM.SelectConfigList(3);
            List<Setting> settingPurchase = SM.SelectConfigList(4);
            List<Setting> settingDeliver = SM.SelectConfigList(5);
            List<Setting> settingReturn = SM.SelectConfigList(7);
            List<Setting> settingDz = SM.SelectConfigList(8);
            List<Setting> settingInvoiceHead = SM.SelectConfigList(9);
            //订单开头
            ViewBag.order_index_begin = settingOrder[0].config_list;
            ViewBag.order_index_id = settingOrder[0].id;
            ViewBag.order_index_view = settingOrder[0].config_list + "2019010001";
            //采购单开头
            ViewBag.purchase_index_begin = settingPurchase[0].config_list;
            ViewBag.purchase_index_id = settingPurchase[0].id;
            ViewBag.purchase_index_view = settingPurchase[0].config_list + "2019010001";
            //出货单开头
            ViewBag.deliver_index_begin = settingDeliver[0].config_list;
            ViewBag.deliver_index_id = settingDeliver[0].id;
            ViewBag.deliver_index_view = settingDeliver[0].config_list + "2019010001";
            //退货单开头
            ViewBag.return_index_begin = settingReturn[0].config_list;
            ViewBag.return_index_id = settingReturn[0].id;
            ViewBag.return_index_view = settingReturn[0].config_list + "2019010001";
            //对账单号开头
            ViewBag.dz_index_begin = settingDz[0].config_list;
            ViewBag.dz_index_id = settingDz[0].id;
            ViewBag.dz_index_view = settingDz[0].config_list + "2019010001";
            return View();
            
        }

        /// <summary>
        /// 插入更新页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Edit(int id,string name,int type)
        {
            try
            {
                Setting res = new Setting();
                List<Setting> settings = SM.SelectConfigList(type);
                //修改
                if (id > 0)
                {
                    var result = settings.Where(s => s.id != id &&s.config_list==name).ToList();//查找是否存在重名
                    if (result.Count > 0)
                    {
                        return Json("Repeat");
                    }
                    else {
                        res.id = id;
                        res.config_list = name;
                        res.all_type = type;
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
                //新增
                else {
                    var result = settings.Where(s => s.config_list == name).ToList();
                    if (result.Count > 0)
                    {
                        return Json("Repeat");
                    }
                    else {
                        res.config_list = name;
                        res.all_type = type;
                        res.in_use = 1;
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
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult Delete(int id) {
            int count = SM.Delete(id);
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
}