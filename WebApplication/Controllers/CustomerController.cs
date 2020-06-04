using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreMvcPager;
using BLL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Newtonsoft.Json.Linq;

namespace WebApplication.Controllers
{
    public class CustomerController : BaseController
    {
        CompanyManager UM = new CompanyManager();
        ContactManager CM = new ContactManager();
        OrderManager OM = new OrderManager();
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
        public IActionResult GetData(string company_name, string bank)
        {
            ViewBag.company_name = company_name;
            ViewBag.bank = bank;
            var objList = UM.SelectAll(type: 0, company_name: company_name, bank: bank);
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
                    var obj = UM.SelectSingle(id);
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
        public IActionResult EditHandle(Company company)
        {

            //验证账户是否存在
            int id = 0;
            bool verify = false;

            string company_name = company.company_name;
            string tax_num = company.tax_num;
            string address = company.address;
            string account = company.account;
            string bank = company.bank;

            string name = company.name;
            string position = company.position;
            string telephone = company.telephone;
            string email = company.email;


            if (!string.IsNullOrEmpty(HttpContext.Request.Form["id"]))
            {
                id = Convert.ToInt32(HttpContext.Request.Form["id"]);
            }
            Company obj = UM.SelectSingleByName(company_name, 0);

            if (id <= 0)
            {
                //新增
                if (obj == null)
                {
                    verify = true;
                }
            }
            else
            {
                //新增
                if (obj == null)
                {
                    verify = true;
                }
                else
                {
                    //更新
                    if (id == obj.id)
                    {
                        verify = true;
                    }
                }
            }
            //若验证通过则开始更新或插入操作
            if (verify)
            {
                int count = 0;
                Company objCompany = new Company();
                objCompany.company_name = company_name;
                objCompany.tax_num = tax_num;
                objCompany.address = address;
                objCompany.account = account;
                objCompany.bank = bank;
                objCompany.company_type = 0;
                if (id > 0)
                {
                    objCompany.id = id;
                    count = UM.Update(objCompany);
                }
                else
                {
                    count = UM.Insert(objCompany);
                    if (count>0)
                    {
                        Company res = UM.SelectSingleByName(company_name, 0);
                        Contact res1 = new Contact();
                        res1.customer_id = res.id;
                        res1.name = name;
                        res1.position = position;
                        res1.telephone = telephone;
                        res1.email = email;
                        CM.Insert(res1);

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
            else
            {
                JObject result = new JObject(); //创建一个json对象

                result.Add("company_name", obj.company_name);
                result.Add("tax_num", obj.tax_num);
                result.Add("address", obj.address);
                result.Add("account", obj.account);
                result.Add("bank", obj.bank);
                result.ToString(); //输出为json字符串

                return Json(result);
            }
        }


        /// <summary>
        /// 编辑处理页面
        /// </summary>
        /// <returns></returns>
        public IActionResult EditHandleList()
        {
            int check = 0;
            string[] allList = Convert.ToString(HttpContext.Request.Form["AllList"]).Split(',');
            int rowLength = Convert.ToInt32(HttpContext.Request.Form["num"])-1;

            int count = 0;
            string company_name = "";//用于管控多个联系人插入
            for (int i = 0; i < rowLength; i++)
            {
                string[] res = new string[10];
                Array.Copy(allList, i * 10, res, 0, 10);
                if (res[1] != "")
                {
                    company_name = res[1];

                    Company objCompany = new Company();

                    objCompany.company_name = res[1];
                    objCompany.tax_num = res[2];
                    objCompany.address = res[3];
                    objCompany.account = res[4];
                    objCompany.bank = res[5];
                    objCompany.company_type = 0;

                    Company obj = UM.SelectSingleByName(res[1],0);
                    if (obj != null)
                    {
                        return Json(obj.company_name);
                    }

                    count = UM.Insert(objCompany);
                    if (count <= 0)
                    {
                        check++;
                        return Json("Fail");
                    }
                    else
                    {
                        Company company = UM.SelectSingleByName(res[1],0);
                        Contact contact = new Contact();
                        contact.customer_id = company.id;
                        if (res[6] == "")
                        {
                            continue;
                        }
                        contact.name = res[6];
                        contact.position = res[7];
                        contact.telephone = res[8];
                        contact.email = res[9];
                        CM.Insert(contact);
                    }
                }
                else
                {
                    Company company = UM.SelectSingleByName(company_name,0);
                    Contact contact = new Contact();

                    contact.customer_id = company.id;                    
                    if (res[6]=="")
                    {
                        continue;
                    }
                    contact.name = res[6];
                    contact.position = res[7];
                    contact.telephone = res[8];
                    contact.email = res[9];
                    CM.Insert(contact);
                }                             
            }

            if (check==0)
            {
                return Json("Success");
            }
            else {
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
                List<Order> order = OM.SelectByCustomerId(id);
                if (order.Count > 0)
                {
                    return Json("Existence");
                }
                else
                {
                    int count = UM.Del(id);
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