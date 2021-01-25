using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public class SaleManager
    {
        public SaleService SS = new SaleService();
        /// <summary>
        /// 模糊查询结果为一笔或多笔数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Sale> SelectAll(string start_time, string end_time, string deliver_index="", string deliver_company_head="")
        {
            List<Sale> objList = SS.SelectAll(start_time,end_time,deliver_index, deliver_company_head);
            return objList;
        }

        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Sale SelectSingleBySeqIndex(int seq_id, string deliver_index)
        {
            Sale obj = SS.SelectSingleBySeqIndex(seq_id, deliver_index);
            return obj;
        }

        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Sale> SelectSeqByDeliverIndex(string deliver_index, string order_index="", string order_name="")
        {
            List<Sale> objList = SS.SelectSeqByDeliverIndex(deliver_index, order_index, order_name);
            return objList;
        }

        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Sale> SelectSaleForReturn(string deliver_index,string order_index)
        {
            List<Sale> objList = SS.SelectSaleForReturn(deliver_index, order_index);
            return objList;
        }
        

        /// <summary>
        /// 查询某一订单涉及的送货单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Sale> SelectByOrderId(int order_id)
        {
            List<Sale> objList = SS.SelectByOrderId(order_id);
            return objList;
        }

        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Sale SelectDeliverByDeliverIndex(string deliver_index)
        {
            Sale obj = SS.SelectDeliverByDeliverIndex(deliver_index);
            return obj;
        }

        /// <summary>
        /// 模糊查询结果为一笔或多笔数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Sale> SelectTodayForDeliverIndex(string insert_time )
        {
            List<Sale> objList = SS.SelectTodayForDeliverIndex(insert_time);
            return objList;
        }

        /// <summary>
        /// 模糊查询结果为一笔或多笔数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Sale> SelectDeliverList()
        {
            List<Sale> objList = SS.SelectDeliverList();
            return objList;
        }

        /// <summary>
        /// 模糊查询结果为一笔或多笔数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Sale> SelectForReturnIndex()
        {
            List<Sale> objList = SS.SelectForReturnIndex();
            return objList;
        }

        /// <summary>
        /// 用于销售对账和结款
        /// </summary>
        /// <returns></returns>
        public List<Sale> SelectMoneyAll(string start_time, string end_time,string deliver_index="", string deliver_company_head="", string order_name="",string purchase_person="")
        {
            List<Sale> objList = SS.SelectMoneyAll(start_time, end_time, deliver_index,deliver_company_head, order_name, purchase_person);
            return objList;
        }

        /// <summary>
        /// 单笔查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Sale SelectById(int id)
        {
            Sale obj= SS.SelectById(id);
            return obj;
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Insert(Sale obj)
        {
            int count = SS.Insert(obj);
            return count;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int UpdateDeliverDetails(Sale obj)
        {
            int count = SS.UpdateDeliverDetails(obj);
            return count;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int UpdateDeliverHead(string deliver_company_head, string deliver_index)
        {
            int count = SS.UpdateDeliverHead(deliver_company_head, deliver_index);
            return count;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int UpdateReturnFlag(string deliver_index, int seq_id)
        {
            int count = SS.UpdateReturnFlag(deliver_index, seq_id);
            return count;
        }


        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int UpdateDZNum( double dz_num,int id)
        {
            int count = SS.UpdateDZNum(dz_num, id);
            return count;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int UpdateInvoiceIndex(string invoice_index, string deliver_index, int seq_id)
        {
            int count = SS.UpdateInvoiceIndex(invoice_index, deliver_index, seq_id);
            return count;
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int UpdateMoney(Sale obj)
        {
            int count = SS.UpdateMoney(obj);
            return count;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Del(int id)
        {
            int count = SS.Del(id);
            return count;
        }
    }
}
