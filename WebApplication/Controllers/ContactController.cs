using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using AspNetCoreMvcPager;
using Model;
using DAL;
using BLL;

namespace WebApplication.Controllers
{
    public class ContactController : BaseController
    {
        CompanyService CS = new CompanyService();
        ContactManager CM = new ContactManager();
        /// <summary>
        /// 用户列表首页
        /// </summary>
        /// <param name="page">分页页码</param>
        /// <param name="size">每页显示数量</param>
        /// <returns></returns>
        public IActionResult Index(string name,int pageindex = 1, int pagesize = 5)
        {          
            int type = Convert.ToInt32(Request.Query["type"]);

            int customer_id = 0;                
            if (!string.IsNullOrEmpty(Request.Query["customer_id"]))
            {
                customer_id = Convert.ToInt32(Request.Query["customer_id"]);
            }
            else
            {
                customer_id = Convert.ToInt32(HttpContext.Request.Form["customer_id"]);
            }
                
            Company obj=CS.SelectSingle(customer_id);
            ViewBag.customer_id = customer_id;
            ViewBag.company_name = obj.company_name;
            ViewBag.tax_num = obj.tax_num;
            ViewBag.address = obj.address;
            ViewBag.account = obj.account;
            ViewBag.bank = obj.bank;

            ViewBag.type = type;
            ViewBag.name = name;
            //var objList = CM.SelectAll(customer_id,name: name);
            return View();
           
        }

        public IActionResult GetData(int customer_id, string name)
        {
            var objList = CM.SelectAll(customer_id,name);
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
                int type = Convert.ToInt32(Request.Query["type"]);
                int customer_id = Convert.ToInt32(Request.Query["customer_id"]);
                ViewBag.customer_id = customer_id;
                Company ob = CS.SelectSingle(customer_id);
                ViewBag.customer_id = customer_id;
                ViewBag.company_name = ob.company_name;
                ViewBag.tax_num = ob.tax_num;
                ViewBag.address = ob.address;
                ViewBag.account = ob.account;
                ViewBag.bank = ob.bank;

                ViewBag.type = type;
                if (id > 0)
                {
                    var obj = CM.SelectById(id);
                    return View(obj);
                }
                else
                {
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

            string name = Convert.ToString(HttpContext.Request.Form["name"]);
            string position = Convert.ToString(HttpContext.Request.Form["position"]);
            string telephone = Convert.ToString(HttpContext.Request.Form["telephone"]);
            string email = Convert.ToString(HttpContext.Request.Form["email"]);
            int customer_id = Convert.ToInt32(HttpContext.Request.Form["customer_id"]);
            int type = Convert.ToInt32(HttpContext.Request.Form["type"]);
            ViewBag.customer_id = customer_id;
            ViewBag.type = type;
            if (!string.IsNullOrEmpty(HttpContext.Request.Form["id"]))
            {
                id = Convert.ToInt32(HttpContext.Request.Form["id"]);
            }
            
                int count = 0;
                Contact objContact = new Contact();
                objContact.name = name;
                objContact.position = position;
                objContact.telephone = telephone;
                objContact.email = email;
                objContact.customer_id = customer_id;
                if (id > 0)
                {
                    objContact.id = id;
                    count = CM.Update(objContact);
                }
                else
                {
                    count = CM.Insert(objContact);
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
                ViewBag.customer_id = Convert.ToInt32(Request.Query["customer_id"]);
                ViewBag.type = Convert.ToInt32(Request.Query["type"]);
                int count = CM.Del(id);
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
    }
}