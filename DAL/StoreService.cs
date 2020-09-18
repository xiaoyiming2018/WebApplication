using Model;
using Model.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class StoreService
    {
        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        public List<Store> SelectAll()
        {
            try
            {
                List<Store> objList = new List<Store>();
                string sql = null;

                sql = "select id, store_name from jinchen.store_info a " +
                       " order by a.store_name";
                sql = string.Format(sql);

                objList = PostgreHelper.GetEntityList<Store>(sql);

                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// 根据ID查询，查询结果为一笔数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Store SelectSingle(int id)
        {
            try
            {
                Store obj = new Store();
                string sql = "select a.id,a.store_name,a.create_time " +
                    "from jinchen.store_info a where a.id={0}";
                sql = string.Format(sql, id);
                obj = PostgreHelper.GetSingleEntity<Store>(sql);
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
        public int Insert(Store obj)
        {
            try
            {
                int count = 0;
                string sql = "insert into jinchen.store_info (store_name, create_time) " +
                    "VALUES('{0}', '{1}'); ";
                sql = string.Format(sql, obj.store_name, obj.create_time);
                count = PostgreHelper.ExecuteNonQuery(sql);
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
        public int Update(Store obj)
        {
            try
            {
                int count = 0;
                string sql = "update jinchen.store_info set store_name='{0}' where id={1}";
                sql = string.Format(sql, obj.store_name, obj.id);
                count = PostgreHelper.ExecuteNonQuery(sql);
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
                string sql = "delete from jinchen.store_info where id={0}";
                sql = string.Format(sql, id);
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
