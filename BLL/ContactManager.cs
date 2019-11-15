using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public class ContactManager
    {
        public ContactService CS = new ContactService();

        /// <summary>
        /// 根据用户名模糊查询结果为一笔或多笔数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Contact> SelectAll(int customer_id,string name = "")
        {
            List<Contact> objList = CS.SelectAll(customer_id,name);
            return objList;
        }

        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Contact SelectById(int id)
        {
            Contact obj = CS.SelectById(id);
            return obj;
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Insert(Contact obj)
        {
            int count = CS.Insert(obj);
            return count;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Update(Contact obj)
        {
            int count = CS.Update(obj);
            return count;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Del(int id)
        {
            int count = CS.Del(id);
            return count;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DelByCustomerID(int customer_id)
        {
            int count = CS.DelByCustomerID(customer_id);
            return count;
        }
    }
}
