using Model;
using Model.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class SaleReturnService
    {
        /// <summary>
        /// 退单统计数据（待确认和已确认）
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<SaleReturn> SelectAll(int return_status,string start_time, string end_time, string return_index,string order_name)
        {
            try
            {
                List<SaleReturn> objList = new List<SaleReturn>();
                string sql = null;

                sql = "select a.return_index,a.return_num,a.return_price,a.return_all_price,a.insert_time,b.deliver_index,c.order_name,c.unit," +
                    "d.company_order_index,b.deliver_company_head,a.seq_id,a.confirm_time " +
                    "from jinchen.salereturn_info a,jinchen.sale_info b,jinchen.orderseq_info c,jinchen.order_info d " +
                    " where a.deliver_index=b.deliver_index and a.seq_id=b.seq_id " +
                    "and b.seq_id=c.seq_id and b.order_id=c.order_id and c.order_id=d.id and " +
                    " return_index ~*'{0}' and to_char(a.insert_time,'yyyy-MM-dd') >='{1}' and " +
                    "to_char(a.insert_time,'yyyy-MM-dd') <='{2}' and return_status={3} and c.order_name ~* '{4}' " +
                    "order by return_index desc";
                sql = string.Format(sql, return_index, start_time, end_time,return_status, order_name);

                objList = PostgreHelper.GetEntityList<SaleReturn>(sql);

                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询单笔退货信息
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns></returns>
        public SaleReturn SelectSingleBySeqReturnIndex(int seq_id, string return_index)
        {
            try
            {
                SaleReturn obj = new SaleReturn();
                string sql = "select a.deliver_index,a.deliver_company_head,b.order_index,d.company_name,b.company_order_index,a.id," +
                            "c.order_time,c.order_name,c.order_num,c.order_price,c.purchase_person,c.unit,a.real_num,a.deliver_price,a.deliver_all_price," +
                            "e.return_num,e.return_time,e.return_price,e.return_all_price,e.remark,e.seq_id,c.open_num,c.tui_num " +
                            "from jinchen.salereturn_info e, jinchen.sale_info a, jinchen.order_info b, jinchen.orderseq_info c, jinchen.company_info d " +
                            "where a.order_id = b.id and b.id = c.order_id and a.seq_id = c.seq_id and e.seq_id=a.seq_id and b.customer_id = d.id and a.seq_id ={0} and e.return_index='{1}' ";
                sql = string.Format(sql, seq_id, return_index);
                obj = PostgreHelper.GetSingleEntity<SaleReturn>(sql);
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询退单号下的所有退货详情
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns></returns>
        public List<SaleReturn> SelectAllByReturnIndex(string return_index)
        {
            try
            {
                List<SaleReturn> objList = new List<SaleReturn>();
                string sql = null;
                sql = "select c.return_index, a.deliver_index,d.order_index,c.return_num,c.return_price,c.return_all_price," +
                    "b.seq_id,b.order_name,b.unit,b.purchase_person,c.remark,d.company_order_index " +
                    " from jinchen.sale_info a,jinchen.orderseq_info b,jinchen.salereturn_info c,jinchen.order_info d " +
                "where a.order_id = b.order_id and a.seq_id = b.seq_id and b.seq_id = c.seq_id and a.deliver_index = c.deliver_index " +
                "and c.return_index ='{0}' and a.order_id=d.id " +
                "order by c.return_index,b.order_name";
                sql = string.Format(sql, return_index);

                objList = PostgreHelper.GetEntityList<SaleReturn>(sql);

                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询退货单下的序号，用于自动生成退货单号
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns></returns>
        public List<SaleReturn> SelectTodayForReturnIndex(string insert_time)
        {
            try
            {
                List<SaleReturn> objList = new List<SaleReturn>();
                string sql = null;
                sql = sql = "select a.return_index " +
                    "from jinchen.salereturn_info a " +
                    "where to_char(a.insert_time,'yyyy-MM')='{0}' group by a.return_index ";
                sql = string.Format(sql, insert_time);

                objList = PostgreHelper.GetEntityList<SaleReturn>(sql);

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
        public int Insert(SaleReturn obj)
        {

            try
            {
                string sql = "insert into jinchen.salereturn_info(seq_id,return_index,return_num," +
                    "return_time,return_price,return_all_price,insert_time,deliver_index,remark) values({0},'{1}',{2},'{3}',{4},{5},'{6}','{7}','{8}')";
                sql = string.Format(sql,obj.seq_id, obj.return_index,
                    obj.return_num, obj.return_time, obj.return_price, obj.return_all_price, obj.insert_time,obj.deliver_index,obj.remark);
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
        public int Update(SaleReturn obj)
        {
            try
            {
                int count = 0;
                string sql = "update jinchen.salereturn_info set return_num={0},return_price={1},return_all_price={2},remark='{3}' where seq_id={4} and return_index='{5}' ";
                sql = string.Format(sql, obj.return_num,obj.return_price,obj.return_all_price,obj.remark,obj.seq_id,obj.return_index);
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
        public int UpdateReturnStatus(string return_index,int seq_id)
        {
            try
            {
                int count = 0;
                string sql = "update jinchen.salereturn_info set return_status=1,confirm_time='{0}' where return_index='{1}' and seq_id={2} ";
                sql = string.Format(sql,DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd HH:ss:mm"), return_index, seq_id);
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
        public int Del(int seq_id,string return_index)
        {
            try
            {
                string sql = "delete from jinchen.salereturn_info where seq_id={0} and return_index='{1}'";
                sql = string.Format(sql, seq_id, return_index);
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
