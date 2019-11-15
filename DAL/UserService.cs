using System;
using System.Collections.Generic;
using System.Text;
using Model;
using Model.Helper;

namespace DAL
{
    public class UserService
    {
        SharedService SS = new SharedService();
        /// <summary>
        /// 执行插入人员信息方法
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>影响行数</returns>
        public int Insert(User obj)
        {
            try
            {
                User newUser = new User();
                newUser.user_name = obj.user_name;
                newUser.level = obj.level;
                newUser.level_name = obj.level_name;
                newUser.pass_word = SS.Base64Encryption(Encoding.UTF8, obj.pass_word);
                newUser.createtime = DateTime.Now.ToLocalTime();
                int count = PostgreHelper.InsertSingleEntity<User>("jinchen.user_info", newUser);
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// 查询用户信息列表
        /// </summary>
        /// <param name="user_name">根据用户名查询，可为空</param>
        /// <returns></returns>
        public List<User> SelectAll(string user_name)
        {
            try
            {
                List<User> objList = new List<User>();
                string sql = null;
                if (string.IsNullOrEmpty(user_name))
                {
                    sql = "select * from jinchen.user_info order by level_name,level desc";
                }
                else
                {
                    sql = "select * jinchen.user_info where user_name ~* '{0}' order by user_name,level desc";
                    sql = string.Format(sql, user_name);
                }
                objList = PostgreHelper.GetEntityList<User>(sql);

                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 查询单笔数
        /// </summary>
        /// <param name="user_name">用户名</param>
        /// <returns></returns>
        public User SelectSingle(string user_name)
        {
            try
            {
                User obj = new User();
                string sql = "select * from jinchen.user_info where user_name='{0}' order by user_name,level desc";
                sql = string.Format(sql, user_name);
                obj = PostgreHelper.GetSingleEntity<User>(sql);
                if (obj == null)
                {
                }
                else
                {
                    obj.pass_word = SS.Base64Decrypt(Encoding.UTF8, obj.pass_word);
                }
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 更新人员信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Update(User obj)
        {
            try
            {
                int count = 0;
                string sql = "update jinchen.user_info set user_name='{0}',level={1},level_name='{2}' where id={3}";
                sql = string.Format(sql, obj.user_name, obj.level,obj.level_name, obj.id);
                count = PostgreHelper.ExecuteNonQuery(sql);
                return count;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="user_name">账户</param>
        /// <param name="pass_word">密码</param>
        /// <returns></returns>
        public int UpdatePassWord(string user_name, string pass_word)
        {
            try
            {
                pass_word = SS.Base64Encryption(Encoding.UTF8, pass_word);
                string sql = "update jinchen.user_info set pass_word='{0}' where user_name='{1}'";
                sql = string.Format(sql, pass_word, user_name);
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="user_name"></param>
        /// <returns></returns>
        public int ResetPassWord(string user_name)
        {
            try
            {
                string pass_word = SS.Base64Encryption(Encoding.UTF8, "111111");
                string sql = "update jinchen.user_info set pass_word='{0}' where user_name='{1}'";
                sql = string.Format(sql, pass_word, user_name);
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Del(int id)
        {
            try
            {
                string sql = "delete from jinchen.user_info where id={0}";
                sql = string.Format(sql, id);
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="user_name">用户名称</param>
        /// <param name="pass_word">用户密码</param>
        /// <returns></returns>
        public bool Login(string user_name, string pass_word, ref int level)
        {
            try
            {
                bool result = false;
                string sql = "select * from jinchen.user_info where user_name='{0}' and pass_word='{1}'";
                sql = string.Format(sql, user_name, SS.Base64Encryption(Encoding.UTF8, pass_word));
                //sql = string.Format(sql, user_name,  pass_word);
                User u = PostgreHelper.GetSingleEntity<User>(sql);
                if (u != null)
                {
                    result = true;
                    level = u.level;
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
