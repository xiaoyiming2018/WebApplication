using System;
using System.Collections.Generic;
using System.Text;
using Model;
using Model.Helper;

namespace DAL
{
    public class ContactService
    {
        /// <summary>
        /// 根据联系人名模糊查询，查询结果为一笔或多笔数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Contact> SelectAll(int customer_id,string name)
        {
            try
            {
                List<Contact> objList = new List<Contact>();
                string sql = null;
                sql = "select * from jinchen.customer_info where name ~* '{0}' and customer_id={1} order by name";
                sql = string.Format(sql, name, customer_id);

                objList = PostgreHelper.GetEntityList<Contact>(sql);

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
        /// <param name="id">用户id</param>
        /// <returns></returns>
        public Contact SelectById(int id)
        {
            try
            {
                Contact obj = new Contact();
                string sql = "select * from jinchen.customer_info where id={0}";
                sql = string.Format(sql, id);
                obj = PostgreHelper.GetSingleEntity<Contact>(sql);
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Insert(Contact obj)
        {
            try
            {
                Contact customer = new Contact();
                customer.name = obj.name;
                customer.position = obj.position;
                customer.telephone = obj.telephone;
                customer.email = obj.email;
                customer.customer_id = obj.customer_id;
                int count = PostgreHelper.InsertSingleEntity<Contact>("jinchen.customer_info", customer);
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Update(Contact obj)
        {
            try
            {
                int count = 0;
                string sql = "update jinchen.customer_info set name='{0}',position='{1}',telephone='{2}',email='{3}' where id={4}";
                sql = string.Format(sql, obj.name, obj.position, obj.telephone,obj.email, obj.id);
                count = PostgreHelper.ExecuteNonQuery(sql);
                return count;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 单笔删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Del(int id)
        {
            try
            {
                string sql = "delete from jinchen.customer_info where id={0}";
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
        /// 批量删除，当删除供应商或客户时，会将该下面的所有联系人全部删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DelByCustomerID(int customer_id)
        {
            try
            {
                string sql = "delete from jinchen.customer_info where customer_id={0}";
                sql = string.Format(sql, customer_id);
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
    }
}
