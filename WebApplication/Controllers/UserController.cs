using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Http;
using AspNetCoreMvcPager;
using Model;
using DAL;
using BLL;

namespace WebApplication.Controllers
{
    public class UserController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        UserManager UM = new UserManager();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public UserController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        /// <summary>
        /// 用户列表首页
        /// </summary>
        /// <param name="page">分页页码</param>
        /// <param name="size">每页显示数量</param>
        /// <returns></returns>
        public IActionResult Index(int pageindex = 1, int pagesize = 20)
        {
            string user_name = Request.Query["user_name"];
            if (!string.IsNullOrEmpty(user_name))
            {
                var objList = UM.SelectAll(user_name);
                var pagedList = PagedList<User>.PageList(pageindex, pagesize, objList);
                ViewBag.model = pagedList.Item2;
                return View(pagedList.Item1);
            }
            else
            {
                var objList = UM.SelectAll();
                var pagedList = PagedList<User>.PageList(pageindex, pagesize, objList);
                ViewBag.model = pagedList.Item2;
                return View(pagedList.Item1);
            }
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }
        /// <summary>
        /// 登录逻辑处理
        /// </summary>
        /// <returns></returns>
        public ActionResult LoginHandle()
        {
            //获取登录的账户密码
            string user_name = HttpContext.Request.Form["userName"];
            string pass_word = HttpContext.Request.Form["passWord"];
            //获取是否将session延迟为2小时
            bool remember = Convert.ToBoolean(HttpContext.Request.Form["remember"]);
            bool result = UM.Login(user_name, pass_word, remember);
            if (result)
            {
                return Json("Success");
            }
            else
            {
                return Json("Fail");
            }
        }

        /// <summary>
        /// 注销用户
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            HttpContext.Session.Remove("user_name");
            HttpContext.Session.Remove("user_level");           
            ViewBag.Message = "退出登录成功！";
            return View("Views/User/logout.cshtml");
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <returns></returns>
        public ActionResult Del()
        {         
            try
            {
                int id = Convert.ToInt32(Request.Query["id"]);
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <returns></returns>
        public ActionResult Reset()
        {
            
            try
            {
                string user_name = Request.Query["user_name"];

                int count = UM.ResetPassWord(user_name);
                if (count > 0)
                {
                    ViewBag.Message = "重置密码成功！";
                    return View("Views/User/alert.cshtml");
                }
                else
                {
                    ViewBag.Message = "重置密码失败！";
                    return View("Views/User/alert.cshtml");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }

        /// <summary>
        /// 编辑页面(添加，修改)
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit()
        {
            try
            {
                string user_name = Request.Query["user_name"];
                if (string.IsNullOrEmpty(user_name))
                {
                    return View();
                }
                else
                {
                    int id = Convert.ToInt32(Request.Query["id"]);
                    ViewBag.id = id;
                    var obj = UM.SelectSingle(user_name);
                    return View(obj);
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
        public ActionResult EditHandle()
        {
            
            //验证账户是否存在
            int id = 0;
            bool verify = false;
            string user_name = HttpContext.Request.Form["user_name"];
            string level = HttpContext.Request.Form["level_name"];
            
            
            if (!string.IsNullOrEmpty(HttpContext.Request.Form["id"]))
            {
                id = Convert.ToInt32(HttpContext.Request.Form["id"]);
            }
            User obj = UM.SelectSingle(user_name);

            if (id <= 0)
            {
                if (obj == null)
                {
                    verify = true;
                }
            }
            else
            {
                if (obj == null)
                {
                    verify = true;
                }
                else
                {
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
                User objUser = new User();
                objUser.user_name = user_name;


                if (string.IsNullOrEmpty(level) == false)
                {
                    if (level == UserLevelEnum.Operator.ToString())
                    {
                        objUser.level = (int)UserLevelEnum.Operator;
                    }
                    else if (level == UserLevelEnum.ProductManager.ToString())
                    {
                        objUser.level = (int)UserLevelEnum.ProductManager;
                    }
                    else if (level == UserLevelEnum.Boss.ToString())
                    {
                        objUser.level = (int)UserLevelEnum.Boss;
                    }
                    else if (level == UserLevelEnum.Developer.ToString())
                    {
                        objUser.level = (int)UserLevelEnum.Developer;
                    }

                }
                objUser.level_name = level;
                objUser.pass_word = HttpContext.Request.Form["pass_word"];
                
                if (id > 0)
                {
                    objUser.id = id;
                    count = UM.Update(objUser);
                }
                else
                {
                    count = UM.Insert(objUser);
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
                return Json("Existence");
            }
           
            
            
        }

        /// <summary>
        /// 更新密码
        /// </summary>
        /// <returns></returns>
        public ActionResult EditPassWord()
        {
            if (HttpContext.Session.GetString("user_name") == null)
            {
                ViewBag.Message = "登录超时，请重新登录！";
                return View("Views/User/logout.cshtml");
            }
            else
            {
                return View();
            }
        }

        public ActionResult EditPassWordHandle()
        {
            try
            {
                //获取登录的账户密码
                string user_name = HttpContext.Request.Form["user_name"];
                string oldpassword = HttpContext.Request.Form["oldpassword"];
                string newpassword = HttpContext.Request.Form["newpassword"];
                bool result = UM.Login(user_name, oldpassword);
                if (result)
                {
                    int count = UM.UpdatePassWord(user_name, newpassword);
                    if (count > 0)
                    {
                        HttpContext.Session.Remove("user_name");//Session.Remove("user_name");
                        HttpContext.Session.Remove("user_level");//Session.Remove("user_level");
                        ViewBag.Message = "密码修改成功，请重新登录！";
                        return Json("Success");
                    }
                    else
                    {
                        return Json("Fail");
                    }
                }
                else
                {
                    return Json("Error");
                }       
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult Test()
        {
            return View();
        }

    }
}