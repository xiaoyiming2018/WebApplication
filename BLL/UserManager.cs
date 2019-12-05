using DAL;
using Model;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace BLL
{using Microsoft.Extensions.DependencyInjection;
    public class UserManager
    {
        public UserService US = new UserService();

        public UserManager()//新的不含参数的类
        {
        }
      
        /// <summary>
        /// 执行插入人员信息方法
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>影响行数</returns>
        public int Insert(User obj)
        {
            int count = US.Insert(obj);
            return count;
        }

        /// <summary>
        /// 查询用户信息列表
        /// </summary>
        /// <param name="User_name">根据用户名查询，可为空</param>
        /// <returns></returns>
        public List<User> SelectAll(string User_name = null)
        {
            List<User> objList = US.SelectAll(User_name);
            return objList;
        }

        /// <summary>
        /// 查询单笔数
        /// </summary>
        /// <param name="User_name">用户名</param>
        /// <returns></returns>
        public User SelectSingle(string User_name)
        {
            User obj = US.SelectSingle(User_name);
            return obj;
        }

        /// <summary>
        /// 更新人员信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Update(User obj)
        {
            int count = US.Update(obj);
            return count;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="User_name">账户</param>
        /// <param name="pass_word">密码</param>
        /// <returns></returns>
        public int UpdatePassWord(string User_name, string pass_word)
        {
            int count = US.UpdatePassWord(User_name, pass_word);
            return count;
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="User_name"></param>
        /// <returns></returns>
        public int ResetPassWord(string User_name)
        {
            int count = US.ResetPassWord(User_name);
            return count;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Del(int id)
        {
            int count = US.Del(id);
            return count;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="user_name">用户名称</param>
        /// <param name="pass_word">用户密码</param>
        /// <param name="remember">是否需要长时间登录</param>
        /// <returns></returns>
        public bool Login(string user_name, string pass_word, bool remember = false)
        {
            int user_level = 0;
            bool result = US.Login(user_name, pass_word, ref user_level);
            if (result)
            {
                var _httpContextAccessor = ServiceLocator.Instance.GetService<IHttpContextAccessor>();
                var _session = _httpContextAccessor.HttpContext.Session;
                _session.SetString("user_name", user_name);
                _session.SetString("level_name", user_level.ToString());
                //假如选中记住用户名，那么Session保持30分钟
                if (remember)
                {

                }
            }
            return result;
        }


        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Get(string key)
        {
            var _httpContextAccessor = ServiceLocator.Instance.GetService<IHttpContextAccessor>();
            var _session = _httpContextAccessor.HttpContext.Session;
            return _session.GetString(key);
        }
        /// <summary>
        /// 获取当前session中的权限
        /// </summary>
        /// <returns></returns>
        public UserLevelEnum GetLevel
        {
            get
            {
                int level = 0;
                var _httpContextAccessor = ServiceLocator.Instance.GetService<IHttpContextAccessor>();
                var _session = _httpContextAccessor.HttpContext.Session;
                string tmp = _session.GetString("level_name");
                int.TryParse(tmp, out level);
                return (UserLevelEnum)level;
            }
        }

        /// <summary>
        /// 获取当前session中的权限
        /// </summary>
        /// <returns></returns>
        public string GetLoginName
        {
            get
            {
                int level = 0;
                var _httpContextAccessor = ServiceLocator.Instance.GetService<IHttpContextAccessor>();
                var _session = _httpContextAccessor.HttpContext.Session;
                string tmp = _session.GetString("user_name");
                return tmp;
            }
        }

        /// <summary>
        /// 当前用户
        /// </summary>
        public static UserManager Current
        {
            get
            {
                return new UserManager();
            }
        }
    }
}
