using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public class InOutManager
    {
        public InOutService MS = new InOutService();

        public List<InOut> SelectAll()
        {
            List<InOut> objList = MS.SelectAll();
            return objList;
        }

        public List<InOut> SelectInventoryInfo() {
            List<InOut> objList = MS.SelectInventoryInfo();
            return objList;
        }

        public List<InOut> SelectInventoryTrendDay()
        {
            List<InOut> objList = MS.SelectInventoryTrendDay();
            return objList;
        }

        public List<InOut> SelectInventoryTrendMonth()
        {
            List<InOut> objList = MS.SelectInventoryTrendMonth();
            return objList;
        }

        public List<InOut> SelectInventoryTrendYear()
        {
            List<InOut> objList = MS.SelectInventoryTrendYear();
            return objList;
        }

        /// <summary>
        /// 根据ID查询，查询结果为一笔数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public InOut SelectSingle(int id)
        {
            InOut obj = MS.SelectSingle(id);
            return obj;
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Insert(InOut obj)
        {
            int count = MS.Insert(obj);
            return count;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Update(InOut obj)
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
