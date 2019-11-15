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
        public List<SaleReturn> SelectAll(int return_status,string start_time, string end_time, string return_index, string deliver_index,  string order_index, string company_name,
            string company_order_index)
        {
            List<SaleReturn> objList = SS.SelectAll(return_status,start_time, end_time, return_index, deliver_index,  order_index, company_name, company_order_index);
            return objList;
        }

        /// <summary>
        /// 模糊查询结果为一笔或多笔数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<SaleReturn> SelectReturnHistory(int return_status, string start_time, string end_time,  string deliver_index, string return_index, string order_index, string company_name,
            string company_order_index)
        {
            List<SaleReturn> objList = SS.SelectReturnHistory(return_status,start_time, end_time, deliver_index, return_index, order_index, company_name, company_order_index);
            return objList;
        }

        public List<SaleReturn> SelectMoneyAll(string start_time, string end_time, string deliver_index, string deliver_company_head, string order_index, string company_name, string company_order_index)
        {
            List<SaleReturn> objList = SS.SelectMoneyAll(start_time, end_time, deliver_index, deliver_company_head, order_index, company_name, company_order_index);
            return objList;
        }

        public List<SaleReturn> SelectMoneyHistoryAll(string start_time = "2001-01-01", string end_time = "2222-01-01", string deliver_index = "", string deliver_company_head = "", string order_index = "", string company_name = "", string company_order_index = "")
        {
            List<SaleReturn> objList = SS.SelectMoneyHistoryAll(start_time, end_time, deliver_index, deliver_company_head, order_index, company_name, company_order_index);
            return objList;
        }

        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SaleReturn SelectById(int seq_id, string deliver_index)
        {
            SaleReturn obj = SS.SelectById(seq_id, deliver_index);
            return obj;
        }

        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<SaleReturn> SelectSeqByReturnIndex(string return_index)
        {
            List<SaleReturn> objList = SS.SelectSeqByReturnIndex(return_index);
            return objList;
        }

        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<SaleReturn> SelectByOrderIdForHistory(int order_id)
        {
            List<SaleReturn> objList = SS.SelectByOrderIdForHistory(order_id);
            return objList;
        }

        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<SaleReturn> SelectByDeliverIndex(string deliver_index)
        {
            List<SaleReturn> objList = SS.SelectByDeliverIndex(deliver_index);
            return objList;
        }

        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SaleReturn SelectByReturnIndex(string return_index)
        {
            SaleReturn obj = SS.SelectByReturnIndex(return_index);
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
        public int UpdateReturnStatus(string return_index)
        {
            int count = SS.UpdateReturnStatus(return_index);
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
