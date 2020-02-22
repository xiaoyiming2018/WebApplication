using Model;
using Model.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class SaleService
    {
        /// <summary>
        /// 用于销售管理：模糊查询，查询结果为一笔或多笔数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Sale> SelectAll(string start_time, string end_time, string deliver_index, string deliver_company_head)
        {
            try
            {
                List<Sale> objList = new List<Sale>();
                string sql = null;
                sql = "select deliver_index,deliver_company_head,sum(real_num) as real_num,sum(deliver_all_price) as deliver_all_price,max(insert_time) as insert_time " +
                      "from jinchen.sale_info a "+
                      "where deliver_index ~*'{0}' and deliver_company_head ~*'{1}' and to_char(insert_time,'yyyy-MM-dd')>='{2}' and to_char(insert_time,'yyyy-MM-dd')<='{3}' " +
                      "group by deliver_index,deliver_company_head order by deliver_index desc";
                sql = string.Format(sql, deliver_index, deliver_company_head,start_time,end_time);

                objList = PostgreHelper.GetEntityList<Sale>(sql);

                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询出货单对应的出货抬头、结款与否、结款方式
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Sale SelectDeliverByDeliverIndex(string deliver_index)
        {
            try
            {
                Sale objList = new Sale();
                string sql = null;
                sql = "select a.deliver_index,a.deliver_company_head,b.address,max(money_onoff) as money_onoff," +
                    "max(money_way) as money_way,max(a.insert_time) as insert_time " +
                        "from jinchen.sale_info a,jinchen.company_info b " +
                        "where a.deliver_index='{0}' and a.deliver_company_head=b.company_name " +
                        "group by a.deliver_index,a.deliver_company_head,b.address";
                sql = string.Format(sql, deliver_index);

                objList = PostgreHelper.GetSingleEntity<Sale>(sql);

                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 用于查看某一出货单下的出货详情
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Sale> SelectSeqByDeliverIndex(string deliver_index,string order_index,string order_name)
        {
            try
            {
                List<Sale> objList = new List<Sale>();
                string sql = null;
                sql = "select a.deliver_index,a.deliver_company_head,a.order_id,a.seq_id,b.order_index,d.company_name,b.company_order_index," +
                        "a.real_num,a.real_time,a.deliver_price,a.deliver_all_price,c.order_price,c.order_all_price," +
                        "c.order_num,c.remain_num,c.unit,c.purchase_person,c.deliver_time,c.order_name,c.order_time,a.deliver_status,a.confirm_time,a.remark " +
                        "from jinchen.sale_info a,jinchen.order_info b, jinchen.orderseq_info c, jinchen.company_info d " +
                        "where a.order_id = b.id and b.id = c.order_id and a.seq_id = c.seq_id and b.customer_id = d.id and a.deliver_index= '{0}' and " +
                        "b.order_index ~*'{1}' and c.order_name ~*'{2}' " +
                        "order by c.order_name";
                sql = string.Format(sql, deliver_index, order_index, order_name);

                objList = PostgreHelper.GetEntityList<Sale>(sql);

                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 用于查看某一出货单下的出货详情
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Sale> SelectSaleForReturn(string deliver_index,string order_index)
        {
            try
            {
                List<Sale> objList = new List<Sale>();
                string sql = null;
                sql = "select c.order_name,c.unit,c.purchase_person,a.real_num,a.deliver_price,a.deliver_all_price " +
                    "from jinchen.sale_info a,jinchen.order_info b,jinchen.orderseq_info c where a.order_id=b.id and b.id=c.order_id and a.seq_id=c.seq_id " +
                    "and a.deliver_index='{0}' and b.order_index='{1}'";
                sql = string.Format(sql, deliver_index);

                objList = PostgreHelper.GetEntityList<Sale>(sql);

                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 用于查看某一出货单下的出货详情
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Sale> SelectByOrderId(int order_id)
        {
            try
            {
                List<Sale> objList = new List<Sale>();
                string sql = null;
                sql = "select * from jinchen.sale_info a,jinchen.orderseq_info b "+
                       " where a.order_id = b.order_id and a.seq_id = b.seq_id and a.order_id ={0} "+
                       " order by deliver_index";
                sql = string.Format(sql, order_id);

                objList = PostgreHelper.GetEntityList<Sale>(sql);

                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询某一送货单号下对应的一个订单
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns></returns>
        public Sale SelectSingleBySeqIndex(int seq_id,string deliver_index)
        {
            try
            {
                Sale obj = new Sale();
                string sql = "select a.deliver_index,a.deliver_company_head,b.order_index,d.company_name,b.company_order_index,a.id,"+
                            "c.order_time,c.order_name,c.order_num,c.order_price,c.purchase_person,c.unit,c.open_num,c.tui_num," +
                            "c.remain_num,a.real_num,a.real_time,a.deliver_price,a.deliver_all_price,a.remark,a.money_way,a.seq_id " +
                            "from jinchen.sale_info a, jinchen.order_info b, jinchen.orderseq_info c, jinchen.company_info d " +
                            "where a.order_id = b.id and b.id = c.order_id and a.seq_id = c.seq_id and b.customer_id = d.id and a.seq_id ={0} and a.deliver_index='{1}' ";
                sql = string.Format(sql, seq_id, deliver_index);
                obj = PostgreHelper.GetSingleEntity<Sale>(sql);
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
        public List<Sale> SelectTodayForDeliverIndex(string insert_time)
        {
            try
            {
                List<Sale> objList = new List<Sale>();
                string sql = null;
                sql = sql = "select a.deliver_index " +
                    "from jinchen.sale_info a " +
                    "where to_char(a.insert_time,'yyyy-MM-dd')='{0}' group by a.deliver_index ";
                sql = string.Format(sql, insert_time);

                objList = PostgreHelper.GetEntityList<Sale>(sql);

                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 用于查看所有送货单号（筛选）
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Sale> SelectDeliverList()
        {
            try
            {
                List<Sale> objList = new List<Sale>();
                string sql = null;
                sql = "select a.deliver_index " +
                        "from jinchen.sale_info a " +
                        "group by a.deliver_index " +
                        "order by a.deliver_index";
                sql = string.Format(sql);

                objList = PostgreHelper.GetEntityList<Sale>(sql);

                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 用于查看某一出货单下的出货详情
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Sale> SelectForReturnIndex()
        {
            try
            {
                List<Sale> objList = new List<Sale>();
                string sql = null;
                sql = "select a.deliver_index " +
                        "from jinchen.sale_info a " +
                        "group by a.deliver_index " +
                        "order by a.deliver_index";
                sql = string.Format(sql);

                objList = PostgreHelper.GetEntityList<Sale>(sql);

                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 用于销售结款和历史销售结款
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Sale> SelectMoneyAll(int deliver_status,string start_time, string end_time, string confirm_start_time, string confirm_end_time, 
            string deliver_index, string deliver_company_head,string order_name,string dz_index,string invoice_index)
        {
            try
            {
                List<Sale> objList = new List<Sale>();
                string sql = null;
                sql = "select b.order_index,b.company_order_index,a.deliver_index,a.seq_id,a.deliver_company_head,real_num,deliver_price,deliver_all_price," +
                    "a.money_way,deliver_status,remark,order_name,unit,purchase_person,a.insert_time,a.seq_id,a.money_onoff,a.confirm_time,a.dz_index,a.invoice_index " +
                    " from jinchen.sale_info a,jinchen.order_info b,jinchen.orderseq_info c " +
                      "where a.order_id=b.id and a.seq_id=c.seq_id and a.order_id=c.order_id and deliver_index ~*'{0}' and deliver_company_head ~*'{1}' and to_char(a.insert_time,'yyyy-MM-dd')>='{2}' and " +
                      "to_char(a.insert_time,'yyyy-MM-dd')<='{3}' and deliver_status={4} and to_char(a.confirm_time,'yyyy-MM-dd')>='{5}' and to_char(a.confirm_time,'yyyy-MM-dd')<='{6}' and " +
                      "order_name ~* '{7}' and dz_index ~* '{8}' and invoice_index ~* '{9}' " +
                      "order by deliver_index desc,deliver_company_head";
                sql = string.Format(sql, deliver_index, deliver_company_head, start_time, end_time, deliver_status, confirm_start_time, confirm_end_time,
                    order_name, dz_index, invoice_index);

                objList = PostgreHelper.GetEntityList<Sale>(sql);

                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询目前对账单的个数
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns></returns>
        public List<Sale> SelectForDzIndex()
        {
            try
            {
                List<Sale> objList = new List<Sale>();
                string sql = null;
                sql = "select a.dz_index " +
                    "from jinchen.sale_info a where dz_index !=' ' group by dz_index ";
                sql = string.Format(sql);

                objList = PostgreHelper.GetEntityList<Sale>(sql);

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
        public int Insert(Sale obj)
        {

            try
            {
                string sql = "insert into jinchen.sale_info(order_id,seq_id,deliver_index,deliver_company_head,real_num," +
                    "real_time,deliver_price,deliver_all_price,insert_time,remark) values({0},{1},'{2}','{3}',{4},'{5}',{6},{7},'{8}','{9}')";
                sql = string.Format(sql, obj.order_id, obj.seq_id, obj.deliver_index,
                    obj.deliver_company_head, obj.real_num, obj.real_time,obj.deliver_price,obj.deliver_all_price,obj.insert_time,obj.remark);
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
        public int UpdateDeliverDetails(Sale obj)
        {
            try
            {
                int count = 0;
                string sql = "update jinchen.sale_info set real_num={0},remark='{1}',deliver_price={2},deliver_all_price={3}, deliver_company_head='{4}',insert_time='{5}' where seq_id={6} and deliver_index='{7}'";
                sql = string.Format(sql, obj.real_num, obj.remark, obj.deliver_price, obj.deliver_all_price,obj.deliver_company_head,obj.insert_time, obj.seq_id,obj.deliver_index);
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
        public int UpdateDeliverHead(string deliver_company_head,string deliver_index)
        {
            try
            {
                int count = 0;
                string sql = "update jinchen.sale_info set deliver_company_head='{0}' where deliver_index='{1}'";
                sql = string.Format(sql, deliver_company_head,deliver_index);
                count = PostgreHelper.ExecuteNonQuery(sql);
                return count;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 更新对账单号
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int UpdateDzIndex(string dz_index,string deliver_index, int seq_id)
        {
            try
            {
                int count = 0;
                string sql = "update jinchen.sale_info set dz_index='{0}' where deliver_index='{1}' and seq_id='{2}'";
                sql = string.Format(sql, dz_index, deliver_index, seq_id);
                count = PostgreHelper.ExecuteNonQuery(sql);
                return count;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 更新对账单号
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int UpdateInvoiceIndex(string invoice_index, string deliver_index, int seq_id)
        {
            try
            {
                int count = 0;
                string sql = "update jinchen.sale_info set invoice_index='{0}' where deliver_index='{1}' and seq_id='{2}'";
                sql = string.Format(sql, invoice_index, deliver_index, seq_id);
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
        public int UpdateDeliverStatus(Sale obj)
        {
            try
            {
                int count = 0;
                string sql = "update jinchen.sale_info set deliver_status={0},confirm_time='{1}' where seq_id={2} and deliver_index='{3}'";
                sql = string.Format(sql, obj.deliver_status, obj.confirm_time, obj.seq_id, obj.deliver_index);
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
        public int UpdateDeliverStatusInvoice(Sale obj)
        {
            try
            {
                int count = 0;
                string sql = "update jinchen.sale_info set deliver_status={0},real_time='{1}',invoice_index='{2}' where seq_id={3} and deliver_index='{4}'";
                sql = string.Format(sql, obj.deliver_status, obj.real_time, obj.invoice_index, obj.seq_id, obj.deliver_index);
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
        public int UpdateMoney(Sale obj)
        {
            try
            {
                int count = 0;
                string sql = "update jinchen.sale_info set money_onoff={0},money_way={1} where deliver_index='{2}' and seq_id={3} ";
                sql = string.Format(sql, obj.money_onoff, obj.money_way, obj.deliver_index,obj.seq_id);
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
        public int Del(int seq_id, string deliver_index)
        {
            try
            {
                string sql = "delete from jinchen.sale_info where seq_id={0} and deliver_index='{1}'";
                sql = string.Format(sql, seq_id, deliver_index);
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
