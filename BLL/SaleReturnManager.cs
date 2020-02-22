using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public class SaleReturnManager
    {
        public SaleReturnService SS = new SaleReturnService();
        /// <summary>
        /// 模糊查询结果为一笔或多笔数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<SaleReturn> SelectAll(int return_status,string start_time, string end_time, string return_index="", string order_name="")
        {
            List<SaleReturn> objList = SS.SelectAll(return_status,start_time, end_time, return_index, order_name);
            return objList;
        }

        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<SaleReturn> SelectAllByReturnIndex(string return_index)
        {
            List<SaleReturn> obj = SS.SelectAllByReturnIndex(return_index);
            return obj;
        }

        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SaleReturn SelectSingleBySeqReturnIndex(int seq_id, string return_index)
        {
            SaleReturn obj = SS.SelectSingleBySeqReturnIndex(seq_id, return_index);
            return obj;
        }



        /// <summary>
        /// 模糊查询结果为一笔或多笔数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<SaleReturn> SelectTodayForReturnIndex(string insert_time)
        {
            List<SaleReturn> objList = SS.SelectTodayForReturnIndex(insert_time);
            return objList;
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Insert(SaleReturn obj)
        {
            int count = SS.Insert(obj);
            return count;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Update(SaleReturn obj)
        {
            int count = SS.Update(obj);
            return count;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int UpdateReturnStatus(string return_index,int seq_id)
        {
            int count = SS.UpdateReturnStatus(return_index, seq_id);
            return count;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Del(int seq_id, string return_index)
        {
            int count = SS.Del(seq_id, return_index);
            return count;
        }
    }
}
