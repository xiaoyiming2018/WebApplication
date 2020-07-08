using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public class OrderManager
    {
        public OrderService OS = new OrderService();

        /// <summary>
        /// 模糊查询结果为一笔或多笔数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Order> SelectAll(int order_status, string start_time, string end_time,string deliver_start_time, string deliver_end_time, 
             string company_name="", string order_index = "", string company_order_index = "", string purchase_person = "",string order_name="")
        {
            List<Order> objList = OS.SelectAll(order_status, start_time, end_time, deliver_start_time, deliver_end_time,  company_name, order_index, company_order_index, purchase_person, order_name);
            return objList;
        }

        /// <summary>
        /// 查询出可以开退货单的订单
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Order> SelectOrderForDropDown()
        {
            List<Order> objList = OS.SelectOrderForDropDown();
            return objList;
        }

        /// <summary>
        /// 查询出可以开退货单的订单
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Order> SelectOrderForSaleReturnAll()
        {
            List<Order> objList = OS.SelectOrderForSaleReturnAll();
            return objList;
        }


        /// <summary>
        /// 查询出可以开退货单的订单
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Order> SelectByCustomerId(int customer_id)
        {
            List<Order> objList = OS.SelectByCustomerId(customer_id);
            return objList;
        }


        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Order SelectById(int id)
        {
            Order obj = OS.SelectById(id);
            return obj;
        }

        /// <summary>
        /// order_index
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Order SelectByOrderIndex(string order_index)
        {
            Order obj = OS.SelectByOrderIndex(order_index);
            return obj;
        }

        /// <summary>
        /// 根据id和seq_id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Order SelectByOrderSeqId(int seq_id)
        {
            Order obj = OS.SelectByOrderSeqId( seq_id);
            return obj;
        }

        /// <summary>
        /// 根据order_id查询orderseq_info
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Order> SelectSeqByOrderId(int order_id, string start_time, string end_time,  string order_name, string unit)
        {
            List<Order> objList = OS.SelectSeqByOrderId(order_id, start_time, end_time, order_name, unit);
            return objList;
        }
        /// <summary>
        /// 根据order_id查询orderseq_info
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Order> SelectOrderSeqList(int order_id=0, string company_name="" )
        {
            List<Order> objList = OS.SelectOrderSeqList(order_id,company_name);
            return objList;
        }

        /// <summary>
        /// 用于自动生成Order Index
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Order> SelectTodayForOrderIndex(string start_time)
        {
            List<Order> objList = OS.SelectTodayForOrderIndex(start_time);
            return objList;
        }


        /// <summary>
        /// 插入订单
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int InsertOrder(Order obj)
        {
            int count = OS.InsertOrder(obj);
            return count;
        }

        /// <summary>
        /// 插入某订单下的序号
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int InsertOrderSeq(Order obj)
        {
            int count = OS.InsertOrderSeq(obj);
            return count;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int UpdateOrder(Order obj)
        {
            int count = OS.UpdateOrder(obj);
            return count;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int UpdateOrderSeq(Order obj)
        {
            int count = OS.UpdateOrderSeq(obj);
            return count;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int UpdateSeqNum(Order obj)
        {
            int count = OS.UpdateSeqNum(obj);
            return count;
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int UpdateOrderStatus(Order obj)
        {
            int count = OS.UpdateOrderStatus(obj);
            return count;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DelOrder(int id)
        {
            int count = OS.DelOrder(id);
            return count;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DelOrderSeq(int id)
        {
            int count = OS.DelOrderSeq(id);
            return count;
        }
    }
}
