using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public class StoreManager
    {
        public StoreService MS = new StoreService();

        public List<Store> SelectAll()
        {
            List<Store> objList = MS.SelectAll();
            return objList;
        }

        /// <summary>
        /// 根据ID查询，查询结果为一笔数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Store SelectSingle(int id)
        {
            Store obj = MS.SelectSingle(id);
            return obj;
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Insert(Store obj)
        {
            int count = MS.Insert(obj);
            return count;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Update(Store obj)
        {
            int count = MS.Update(obj);
            return count;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Del(int id)
        {
            int count = MS.Del(id);
            return count;
        }
    }
}
