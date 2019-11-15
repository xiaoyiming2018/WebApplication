using Model;
using Model.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class OrderService
    {
        /// <summary>
        /// 用于订单管理：根据联系人名模糊查询，查询结果为一笔或多笔数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Order> SelectAll(int order_status, string start_time, string end_time, string deliver_start_time, string deliver_end_time,  string company_name,
            string order_index,string company_order_index)
        {
            try
            {
                List<Order> objList = new List<Order>();
                string sql = null;
                sql = "select c.id,c.company_order_index,c.order_index,b.company_name,a.order_time,min(a.deliver_time) as deliver_time,max(a.deliver_time) as insert_time," +
                    " sum(order_num) as order_num,sum(order_all_price) as order_all_price, " +
                    "sum(tui_num) as tui_num, sum(open_num) as open_num, sum(remain_num) as remain_num " +
                    "from jinchen.orderseq_info a,jinchen.company_info b,jinchen.order_info c where a.order_id=c.id and c.customer_id=b.id and " +
                    " b.company_name ~* '{0}' and c.order_index ~* '{1}' and c.company_order_index ~*'{2}' and to_char(a.order_time,'yyyy-MM-dd')>='{3}' and to_char(a.order_time,'yyyy-MM-dd')<='{4}' and order_status={5} " +
                    " and to_char(a.deliver_time,'yyyy-MM-dd')>='{6}' and to_char(a.deliver_time,'yyyy-MM-dd')<='{7}' " +
                    " group by c.id,c.order_index,b.company_name,c.company_order_index,a.order_time " +
                    " order by a.order_time desc,c.order_index desc";
                sql = string.Format(sql, company_name, order_index, company_order_index,start_time,end_time, order_status, deliver_start_time, deliver_end_time);

                objList = PostgreHelper.GetEntityList<Order>(sql);

                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询订单下的序号
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns></returns>
        public List<Order> SelectSeqByOrderId(int order_id, string start_time, string end_time,  string order_name, string unit)
        {
            try
            {
                List<Order> objList = new List<Order>();
                string sql = null;
                sql = "select a.id,a.order_index,b.company_name, c.seq_id,c.order_time,c.order_name,c.unit,c.deliver_time," +
                    "c.order_num,c.open_num,c.tui_num,c.remain_num,c.order_price,c.order_all_price,c.purchase_person,c.order_picture " +
                    "from jinchen.order_info a,jinchen.company_info b,jinchen.orderseq_info c " +
                    "where a.customer_id=b.id and a.id=c.order_id and a.id={0} and to_char(c.deliver_time,'yyyy-MM-dd')>='{1}' and to_char(c.deliver_time,'yyyy-MM-dd')<='{2}' and c.order_name ~* '{3}' and " +
                    "c.unit ~* '{4}' order by c.order_name";
                sql = string.Format(sql, order_id, start_time, end_time, order_name, unit);

                objList = PostgreHelper.GetEntityList<Order>(sql);

                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询订单下的序号，只用于判断该订单下的序号有多少个
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns></returns>
        public List<Order> SelectOrderSeqList(int order_id)
        {
            try
            {
                List<Order> objList = new List<Order>();
                string sql = null;
                sql =  "select a.id,a.order_index,b.company_name, c.seq_id,c.order_time,c.order_name,c.unit,c.deliver_time," +
                    "c.order_num,c.open_num,c.tui_num,c.remain_num,c.order_price,c.order_all_price,c.purchase_person " +
                    "from jinchen.order_info a,jinchen.company_info b,jinchen.orderseq_info c " +
                    "where a.customer_id=b.id and a.id=c.order_id and a.id={0} order by c.order_name";
                sql = string.Format(sql, order_id);

                objList = PostgreHelper.GetEntityList<Order>(sql);

                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 用于订单管理：根据联系人名模糊查询，查询结果为一笔或多笔数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Order> SelectForSaleAll(int order_status)
        {
            try
            {
                List<Order> objList = new List<Order>();
                string sql = null;
                sql = "select * " +
                    "from jinchen.order_info a where a.order_status={0} " +
                    " order by a.order_index";
                sql = string.Format(sql, order_status);

                objList = PostgreHelper.GetEntityList<Order>(sql);

                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 查询某一订单号
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns></returns>
        public Order SelectById(int id)
        {
            try
            {
                Order obj = new Order();
                string sql = null;
                sql = "select * from jinchen.order_info a,jinchen.company_info b where a.customer_id=b.id and a.id={0}";
                sql = string.Format(sql, id);

                obj = PostgreHelper.GetSingleEntity<Order>(sql);

                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询某一订单号
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns></returns>
        public Order SelectByOrderIndex(string order_index)
        {
            try
            {
                Order obj = new Order();
                string sql = null;
                sql = "select * from jinchen.order_info a where a.order_index='{0}'";
                sql = string.Format(sql, order_index);

                obj = PostgreHelper.GetSingleEntity<Order>(sql);

                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询某一订单下的某个序号
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns></returns>
        public Order SelectByOrderSeqId(int order_id, int seq_id)
        {
            try
            {
                Order obj = new Order();
                string sql = null;
                sql = "select * from jinchen.orderseq_info a where a.order_id={0} and a.seq_id={1}";
                sql = string.Format(sql, order_id, seq_id);

                obj = PostgreHelper.GetSingleEntity<Order>(sql);

                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询订单下的序号
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns></returns>
        public List<Order> SelectTodayForOrderIndex(string start_time)
        {
            try
            {
                List<Order> objList = new List<Order>();
                string sql = null;
                sql = "select a.id,a.order_index " +
                    "from jinchen.order_info a where to_char(a.insert_time,'yyyy-MM-dd')='{0}' ";
                sql = string.Format(sql, start_time);

                objList = PostgreHelper.GetEntityList<Order>(sql);

                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询订单下的序号
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns></returns>
        public Order SelectForHistoryOrder(int order_id)
        {
            try
            {
                Order obj = new Order();
                string sql = null;
                sql = "select b.order_id, a.order_index, sum(b.remain_num) as remain_num " +
                    "from jinchen.order_info a, jinchen.orderseq_info b where a.id = b.order_id and b.order_id={0} group by b.order_id, a.order_index ";
                sql = string.Format(sql,order_id);

                obj = PostgreHelper.GetSingleEntity<Order>(sql);

                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询订单下的序号
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns></returns>
        public List<Order> SelectByInvoiceStatus(int invoice_status)
        {
            try
            {
                List<Order> objList = new List<Order>();
                string sql = null;
                sql = "select * from jinchen.order_info where invoice_status={0} and order_status=1";
                sql = string.Format(sql, invoice_status);

                objList = PostgreHelper.GetEntityList<Order>(sql);

                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询订单下的序号
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns></returns>
        public List<Order> SelectForInvoiceOrderList(int order_id)
        {
            try
            {
                List<Order> objList = new List<Order>();
                string sql = null;
                sql = "	select res1.order_name,res1.order_time,res1.order_num,res1.tui_num,res1.order_price,res1.order_all_price,res2.deliver_all_price,"+
                    "res1.unit,res1.purchase_person " +
                    "from(select * from jinchen.orderseq_info where order_id = '{0}') res1 " +
                     "left join " +
                     "(select seq_id, sum(a.deliver_all_price) as deliver_all_price " +
                      "from jinchen.sale_info a where order_id = {0} group by seq_id) res2 " +
                       " on res1.seq_id = res2.seq_id";
                sql = string.Format(sql, order_id);

                objList = PostgreHelper.GetEntityList<Order>(sql);

                return objList;
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
        public int InsertOrder(Order obj)
        {

            try
            {
                string sql = "insert into jinchen.order_info(customer_id,order_index,company_order_index,insert_time) values({0},'{1}','{2}','{3}')";
                sql = string.Format(sql, obj.customer_id, obj.order_index,obj.company_order_index,DateTime.Now.ToLocalTime());//插入数据时，订单数量与剩余数量一致
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
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
        public int InsertOrderSeq(Order obj)
        {

            try
            {
                string sql = "insert into jinchen.orderseq_info(order_id,order_time,order_name,order_num," +
                    "order_price,order_all_price,remain_num,deliver_time,unit,purchase_person,order_picture) values({0},'{1}','{2}',{3},{4},{5},{6},'{7}','{8}','{9}','{10}')";
                sql = string.Format(sql, obj.order_id, obj.order_time,obj.order_name, obj.order_num, 
                    obj.order_price, obj.order_all_price, obj.order_num, obj.deliver_time, obj.unit, obj.purchase_person, obj.order_picture);//插入数据时，订单数量与剩余数量一致
                int count = PostgreHelper.ExecuteNonQuery(sql);
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
        public int UpdateOrder(Order obj)
        {
            try
            {
                int count = 0;
                string sql = "update jinchen.order_info set customer_id={0},order_index='{1}',company_order_index='{2}' where id={3}";
                sql = string.Format(sql, obj.customer_id, obj.order_index, obj.company_order_index, obj.order_id);
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
        public int UpdateOrderSeq(Order obj)
        {
            try
            {
                int count = 0;
                string sql = "update jinchen.orderseq_info set order_name='{0}',deliver_time='{1}',unit='{2}',order_price={3},order_all_price={4},order_picture='{5}' where seq_id={6}";
                sql = string.Format(sql, obj.order_name, obj.deliver_time, obj.unit,obj.order_price, obj.order_all_price,obj.order_picture, obj.seq_id);
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
        public int UpdateSeqNum(Order obj)
        {
            try
            {
                int count = 0;
                string sql = "update jinchen.orderseq_info set open_num={0},tui_num={1},remain_num={2} where seq_id={3}";
                sql = string.Format(sql, obj.open_num, obj.tui_num, obj.remain_num,obj.seq_id,obj.order_id);
                count = PostgreHelper.ExecuteNonQuery(sql);
                return count;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 更新订单的状态，若status为1则
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int UpdateOrderStatus(Order obj)
        {
            try
            {
                int count = 0;
                string sql = "update jinchen.order_info set order_status={0} where id={1}";
                sql = string.Format(sql, obj.order_status, obj.id);
                count = PostgreHelper.ExecuteNonQuery(sql);
                return count;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 更新订单的状态，若status为1则
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int UpdateInvoiceStatus(Order obj)
        {
            try
            {
                int count = 0;
                string sql = "update jinchen.order_info set invoice_status={0} where id={1}";
                sql = string.Format(sql, obj.invoice_status, obj.id);
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
        public int DelOrder(int id)
        {
            try
            {
                string sql = "delete from jinchen.order_info where id={0}";
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
        /// 删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DelOrderSeq(int id)
        {
            try
            {
                string sql = "delete from jinchen.orderseq_info where seq_id={0}";
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
