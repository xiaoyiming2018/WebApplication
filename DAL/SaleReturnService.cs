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
        /// 用于销售管理：模糊查询，查询结果为一笔或多笔数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<SaleReturn> SelectAll(int return_status,string start_time, string end_time, string return_index, string deliver_index, string order_index,
            string company_name, string company_order_index)
        {
            try
            {
                List<SaleReturn> objList = new List<SaleReturn>();
                string sql = null;
                sql = "select res1.deliver_index,res1.order_index,res1.order_id,res1.company_name,res1.company_order_index,"+
                       " res0.order_num,res1.real_num,res2.return_num, res2.return_all_price,res2.insert_time,res2.return_index " +
                       " from" +
                        " (select b.order_index, d.company_name, b.company_order_index, sum(c.order_num) as order_num " +
                        " from(select a.order_id from jinchen.sale_info a group by a.order_id) a, jinchen.order_info b, jinchen.orderseq_info c, jinchen.company_info d " +
                        " where a.order_id = b.id and b.id = c.order_id and b.customer_id = d.id and b.order_index ~*'{2}' and d.company_name ~*'{3}' and " +
                        " b.company_order_index ~*'{4}' " +
                        " group by b.order_index, d.company_name, b.company_order_index " +
                        " order by b.order_index) res0 " +
                        "right join " +
                       "(select a.deliver_index, b.order_index, a.order_id,  d.company_name, b.company_order_index, " +
                        "sum(c.order_num) as order_num, sum(real_num) as real_num " +
                       " from jinchen.sale_info a, jinchen.order_info b, jinchen.orderseq_info c, jinchen.company_info d " +
                      "  where a.order_id = b.id and b.id = c.order_id and a.seq_id = c.seq_id and b.customer_id = d.id " +
                       " and a.deliver_index ~*'{0}' and b.order_index ~*'{1}' and d.company_name ~*'{2}' and b.company_order_index ~*'{3}' " +
                       " group by a.deliver_index, b.order_index, b.company_order_index, d.company_name, a.order_id) res1 " +
                       "on res0.order_index = res1.order_index and res0.company_name = res1.company_name and res0.company_order_index = res1.company_order_index" +
                       " right join " +
                       "  (select a.deliver_index, b.order_index, a.order_id, " +
                        "    d.company_name, b.company_order_index, " +
                          "  sum(e.return_num) as return_num, sum(e.return_all_price) as return_all_price, e.insert_time,e.return_index " +
                         "   from jinchen.salereturn_info e, jinchen.sale_info a, jinchen.order_info b, jinchen.orderseq_info c, jinchen.company_info d " +
                          "  where a.order_id = b.id and b.id = c.order_id and a.seq_id = c.seq_id and b.customer_id = d.id and e.seq_id = a.seq_id " +
                           " and a.deliver_index ~*'{0}' and b.order_index ~*'{1}' and d.company_name ~*'{2}' and b.company_order_index ~*'{3}' and " +
                            " e.return_index ~*'{4}' and e.deliver_index = a.deliver_index and to_char(e.insert_time,'yyyy-MM-dd')>='{5}' and " +
                            "to_char(e.insert_time,'yyyy-MM-dd')<='{6}' and e.return_status={7} " +
                           " group by a.deliver_index, b.order_index, b.company_order_index, d.company_name, e.insert_time, a.order_id,e.return_index) res2 " +
                                " on res1.deliver_index = res2.deliver_index and res1.order_index = res2.order_index and res1.order_id = res2.order_id and " +
                        "res1.company_name = res2.company_name and res1.company_order_index = res2.company_order_index order by res2.insert_time desc";
                sql = string.Format(sql, deliver_index, order_index, company_name, company_order_index,return_index, start_time, end_time, return_status);

                objList = PostgreHelper.GetEntityList<SaleReturn>(sql);

                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SaleReturn> SelectReturnHistory(int return_status,string start_time, string end_time, string deliver_index, string return_index, string order_index,
    string company_name, string company_order_index)
        {
            try
            {
                List<SaleReturn> objList = new List<SaleReturn>();
                string sql = null;
                sql = "select res1.deliver_index,res1.order_index,res1.order_id,res1.company_name,res1.company_order_index," +
                       " res0.order_num,res1.real_num,res2.return_num, res2.return_all_price,res2.return_index,res2.confirm_time " +
                       " from" +
                        " (select b.order_index, d.company_name, b.company_order_index, sum(c.order_num) as order_num " +
                        " from(select a.order_id from jinchen.sale_info a group by a.order_id) a, jinchen.order_info b, jinchen.orderseq_info c, jinchen.company_info d " +
                        " where a.order_id = b.id and b.id = c.order_id and b.customer_id = d.id and b.order_index ~*'{2}' and d.company_name ~*'{3}' and " +
                        " b.company_order_index ~*'{4}' " +
                        " group by b.order_index, d.company_name, b.company_order_index " +
                        " order by b.order_index) res0 " +
                        "right join " +
                       "(select a.deliver_index, b.order_index, a.order_id,  d.company_name, b.company_order_index, " +
                        "sum(c.order_num) as order_num, sum(real_num) as real_num " +
                       " from jinchen.sale_info a, jinchen.order_info b, jinchen.orderseq_info c, jinchen.company_info d " +
                      "  where a.order_id = b.id and b.id = c.order_id and a.seq_id = c.seq_id and b.customer_id = d.id " +
                       " and a.deliver_index ~*'{0}' and b.order_index ~*'{1}' and d.company_name ~*'{2}' and b.company_order_index ~*'{3}' " +
                       " group by a.deliver_index, b.order_index, b.company_order_index, d.company_name, a.order_id) res1 " +
                       "on res0.order_index = res1.order_index and res0.company_name = res1.company_name and res0.company_order_index = res1.company_order_index" +
                       " right join " +
                       "  (select a.deliver_index, b.order_index, a.order_id, " +
                        "    d.company_name, b.company_order_index, " +
                          "  sum(e.return_num) as return_num, sum(e.return_all_price) as return_all_price, e.confirm_time,e.return_index " +
                         "   from jinchen.salereturn_info e, jinchen.sale_info a, jinchen.order_info b, jinchen.orderseq_info c, jinchen.company_info d " +
                          "  where a.order_id = b.id and b.id = c.order_id and a.seq_id = c.seq_id and b.customer_id = d.id and e.seq_id = a.seq_id " +
                           " and a.deliver_index ~*'{0}' and b.order_index ~*'{1}' and d.company_name ~*'{2}' and b.company_order_index ~*'{3}' and " +
                            " e.return_index ~*'{4}' and e.deliver_index = a.deliver_index and to_char(e.confirm_time,'yyyy-MM-dd')>='{5}' and " +
                            "to_char(e.confirm_time,'yyyy-MM-dd')<='{6}' and e.return_status={7} " +
                           " group by a.deliver_index, b.order_index, b.company_order_index, d.company_name, e.confirm_time, a.order_id,e.return_index) res2 " +
                                " on res1.deliver_index = res2.deliver_index and res1.order_index = res2.order_index and res1.order_id = res2.order_id and " +
                        "res1.company_name = res2.company_name and res1.company_order_index = res2.company_order_index order by res2.confirm_time desc";
                sql = string.Format(sql, deliver_index, order_index, company_name, company_order_index, return_index, start_time, end_time, return_status);

                objList = PostgreHelper.GetEntityList<SaleReturn>(sql);

                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 用于查看某一退货单下的退货详情
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<SaleReturn> SelectSeqByReturnIndex(string return_index)
        {
            try
            {
                List<SaleReturn> objList = new List<SaleReturn>();
                string sql = null;
                sql = "select e.return_index, a.deliver_index,a.deliver_company_head,a.order_id,a.seq_id,b.order_index,d.company_name,b.company_order_index," +
                        "a.real_num,a.real_time,a.deliver_price,a.deliver_all_price,c.order_price,c.order_all_price," +
                        "c.order_num,c.remain_num,c.unit,c.purchase_person,c.deliver_time,c.order_name,c.order_time,e.return_num,e.return_time," +
                        "e.return_price,e.return_all_price " +
                        "from jinchen.salereturn_info e,jinchen.sale_info a,jinchen.order_info b, jinchen.orderseq_info c, jinchen.company_info d " +
                        "where a.order_id = b.id and b.id = c.order_id and a.seq_id = c.seq_id and e.seq_id=a.seq_id and b.customer_id = d.id and e.return_index= '{0}' and " +
                        "e.deliver_index=a.deliver_index " +
                        "order by c.order_name";
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
        /// 查询单笔数
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns></returns>
        public SaleReturn SelectById(int seq_id, string return_index)
        {
            try
            {
                SaleReturn obj = new SaleReturn();
                string sql = "select a.deliver_index,a.deliver_company_head,b.order_index,d.company_name,b.company_order_index,a.id," +
                            "c.order_time,c.order_name,c.order_num,c.order_price,c.purchase_person,c.unit,a.real_num,a.real_time,a.deliver_price,a.deliver_all_price," +
                            "e.return_num,e.return_time,e.return_price,e.return_all_price " +
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
        /// 用于销售管理：模糊查询，查询结果为一笔或多笔数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public SaleReturn SelectByReturnIndex(string return_index)
        {
            try
            {
                SaleReturn objList = new SaleReturn();
                string sql = null;
                sql = "select res.return_index,res1.order_index,res.order_id,res.deliver_index,res.deliver_company_head,"+
                       " res1.company_order_index,res2.company_name "+
                       " from(select a.order_id, e.return_index, a.deliver_index, a.deliver_company_head " +
                       " from jinchen.salereturn_info e, jinchen.sale_info a " +
                       " where e.return_index = '{0}' and e.deliver_index = a.deliver_index and a.seq_id = e.seq_id) res, " +
                       " jinchen.order_info res1, jinchen.company_info res2 " +
                        " where res1.id = res.order_id and res1.customer_id = res2.id " +
                        " limit 1";
                sql = string.Format(sql, return_index);

                objList = PostgreHelper.GetSingleEntity<SaleReturn>(sql);

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
        public List<SaleReturn> SelectTodayForReturnIndex(string insert_time)
        {
            try
            {
                List<SaleReturn> objList = new List<SaleReturn>();
                string sql = null;
                sql = sql = "select a.return_index " +
                    "from jinchen.salereturn_info a " +
                    "where to_char(a.insert_time,'yyyy-MM-dd')='{0}' group by a.return_index ";
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
        /// 查询订单下的序号
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns></returns>
        public List<SaleReturn> SelectByOrderIdForHistory(int order_id)
        {
            try
            {
                List<SaleReturn> objList = new List<SaleReturn>();
                string sql = null;
                sql = "select * from jinchen.sale_info a,jinchen.orderseq_info b,jinchen.salereturn_info c "+
                "where a.order_id = b.order_id and a.seq_id = b.seq_id and b.seq_id = c.seq_id and a.deliver_index = c.deliver_index and a.order_id ={0} "+
                "order by c.return_index";
                sql = string.Format(sql, order_id);

                objList = PostgreHelper.GetEntityList<SaleReturn>(sql);

                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询送货单号下所有的退单
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns></returns>
        public List<SaleReturn> SelectByDeliverIndex(string deliver_index)
        {
            try
            {
                List<SaleReturn> objList = new List<SaleReturn>();
                string sql = null;
                sql = sql = "select * " +
                    "from jinchen.salereturn_info a,jinchen.orderseq_info b " +
                    "where deliver_index='{0}' and a.seq_id=b.seq_id ";
                sql = string.Format(sql, deliver_index);

                objList = PostgreHelper.GetEntityList<SaleReturn>(sql);

                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 用于销售管理：模糊查询，查询结果为一笔或多笔数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<SaleReturn> SelectMoneyAll(string start_time, string end_time, string deliver_index, string deliver_company_head, string order_index, string company_name, string company_order_index)
        {
            try
            {
                List<SaleReturn> objList = new List<SaleReturn>();
                string sql = null;
                sql = "select res1.deliver_index,res1.deliver_company_head,res1.order_index,res1.company_name,res1.company_order_index," +
                       " res1.real_num,res1.deliver_all_price,res1.insert_time,res2.order_num,res1.money_onoff,res1.money_way,res1.deliver_status " +
                        "from " +
                       " (select a.deliver_index, a.deliver_company_head, b.order_index, d.company_name, b.company_order_index, " +
                       " sum(real_num) as real_num, sum(deliver_all_price) as deliver_all_price, a.insert_time,a.money_onoff,a.money_way,a.deliver_status " +
                       " from jinchen.sale_info a, jinchen.order_info b, jinchen.orderseq_info c, jinchen.company_info d " +
                       " where a.order_id = b.id and b.id = c.order_id and a.seq_id = c.seq_id and b.customer_id = d.id " +
                       " and a.deliver_index ~*'{0}' and a.deliver_company_head ~*'{1}' and b.order_index ~*'{2}' and d.company_name ~*'{3}' " +
                       " and b.company_order_index ~*'{4}' and to_char(a.insert_time,'yyyy-MM-dd')>='{5}' and to_char(a.insert_time,'yyyy-MM-dd')<='{6}' and a.deliver_status=0 " +
                       " group by a.deliver_index, a.deliver_company_head, b.order_index, b.company_order_index, " +
                       " d.company_name, a.insert_time,a.money_onoff,a.money_way,a.deliver_status order by a.insert_time desc, a.deliver_index) res1 " +
                        " left join " +
                        " (select b.order_index, d.company_name, b.company_order_index, sum(c.order_num) as order_num " +
                        " from(select a.order_id from jinchen.sale_info a group by a.order_id) a, jinchen.order_info b, jinchen.orderseq_info c, jinchen.company_info d " +
                        " where a.order_id = b.id and b.id = c.order_id and b.customer_id = d.id and b.order_index ~*'{2}' and d.company_name ~*'{3}' and " +
                        " b.company_order_index ~*'{4}' " +
                        " group by b.order_index, d.company_name, b.company_order_index " +
                        " order by b.order_index) res2 " +
                        " on res1.order_index = res2.order_index and res1.company_name = res2.company_name and res1.company_order_index = res2.company_order_index";
                sql = string.Format(sql, deliver_index, deliver_company_head, order_index, company_name, company_order_index,start_time, end_time);

                objList = PostgreHelper.GetEntityList<SaleReturn>(sql);

                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 用于销售管理：模糊查询，查询结果为一笔或多笔数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<SaleReturn> SelectMoneyHistoryAll(string start_time, string end_time, string deliver_index, string deliver_company_head, string order_index, string company_name, string company_order_index)
        {
            try
            {
                List<SaleReturn> objList = new List<SaleReturn>();
                string sql = null;
                sql = "select res1.deliver_index,res1.deliver_company_head,res1.order_index,res1.company_name,res1.company_order_index," +
                       " res1.real_num,res1.deliver_all_price,res2.order_num,res1.money_onoff,res1.money_way,res1.confirm_time,res1.deliver_status " +
                        "from " +
                       " (select a.deliver_index, a.deliver_company_head, b.order_index, d.company_name, b.company_order_index, " +
                       " sum(real_num) as real_num, sum(deliver_all_price) as deliver_all_price, a.confirm_time,a.money_onoff,a.money_way,a.deliver_status " +
                       " from jinchen.sale_info a, jinchen.order_info b, jinchen.orderseq_info c, jinchen.company_info d " +
                       " where a.order_id = b.id and b.id = c.order_id and a.seq_id = c.seq_id and b.customer_id = d.id " +
                       " and a.deliver_index ~*'{0}' and a.deliver_company_head ~*'{1}' and b.order_index ~*'{2}' and d.company_name ~*'{3}' " +
                       " and b.company_order_index ~*'{4}' and to_char(a.confirm_time,'yyyy-MM-dd')>='{5}' and to_char(a.confirm_time,'yyyy-MM-dd')<='{6}' and a.deliver_status=1 " +
                       " group by a.deliver_index, a.deliver_company_head, b.order_index, b.company_order_index, " +
                       " d.company_name, a.confirm_time,a.money_onoff,a.money_way,a.deliver_status order by a.confirm_time desc, a.deliver_index) res1 " +
                        " left join " +
                        " (select b.order_index, d.company_name, b.company_order_index, sum(c.order_num) as order_num " +
                        " from(select a.order_id from jinchen.sale_info a group by a.order_id) a, jinchen.order_info b, jinchen.orderseq_info c, jinchen.company_info d " +
                        " where a.order_id = b.id and b.id = c.order_id and b.customer_id = d.id and b.order_index ~*'{2}' and d.company_name ~*'{3}' and " +
                        " b.company_order_index ~*'{4}' " +
                        " group by b.order_index, d.company_name, b.company_order_index " +
                        " order by b.order_index) res2 " +
                        " on res1.order_index = res2.order_index and res1.company_name = res2.company_name and res1.company_order_index = res2.company_order_index";
                sql = string.Format(sql, deliver_index, deliver_company_head, order_index, company_name, company_order_index, start_time, end_time);

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
                    "return_time,return_price,return_all_price,insert_time,deliver_index) values({0},'{1}',{2},'{3}',{4},{5},'{6}','{7}')";
                sql = string.Format(sql,obj.seq_id, obj.return_index,
                    obj.return_num, obj.return_time, obj.return_price, obj.return_all_price, obj.insert_time,obj.deliver_index);
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
                string sql = "update jinchen.salereturn_info set return_num={0},return_time='{1}',return_price={2},return_all_price={3} where seq_id={4} and return_index='{5}' ";
                sql = string.Format(sql, obj.return_num,obj.return_time,obj.return_price,obj.return_all_price,obj.seq_id,obj.return_index);
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
        public int UpdateReturnStatus(string return_index)
        {
            try
            {
                int count = 0;
                string sql = "update jinchen.salereturn_info set return_status=1,confirm_time='{0}' where return_index='{1}'";
                sql = string.Format(sql,DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd HH:ss:mm"), return_index);
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
