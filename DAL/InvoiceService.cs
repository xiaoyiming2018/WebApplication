using Model;
using Model.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class InvoiceService
    {
        /// <summary>
        /// 根据名称、税号、账户和开户行对公司模糊查询，查询结果为一笔或多笔数据
        /// </summary>
        /// <param name="invoice_name"></param>
        /// <param name="company_name"></param>
        /// <returns></returns>
        public List<Invoice> SelectAll(string invoice_index, string company_name, string order_index, string company_order_index)
        {
            try
            {
                List<Invoice> objList = new List<Invoice>();
                string sql = null;
                sql = "select a.id,b.company_name,c.order_index,c.company_order_index,a.invoice_type,a.invoice_index," +
                    "a.invoice_time,a.invoice_price,a.invoice_ratio,a.invoice_all_price,a.pay_type,a.remark " +
                    "from jinchen.invoice_info a,jinchen.company_info b,jinchen.order_info c " +
                    "where c.customer_id=b.id and a.order_id=c.id and a.invoice_index ~* '{0}' and b.company_name ~* '{1}' and c.order_index ~*'{2}' and c.company_order_index ~* '{3}'" +
                    "order by a.invoice_time desc";
                sql = string.Format(sql, invoice_index, company_name, order_index, company_order_index);

                objList = PostgreHelper.GetEntityList<Invoice>(sql);

                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据名称、税号、账户和开户行对公司模糊查询，查询结果为一笔或多笔数据
        /// </summary>
        /// <param name="invoice_name"></param>
        /// <param name="company_name"></param>
        /// <returns></returns>
        public Invoice SelectById(int id)
        {
            try
            {
                Invoice objList = new Invoice();
                string sql = null;
                sql = "select * from jinchen.invoice_info a,jinchen.order_info b where a.order_id=b.id and a.id={0}";
                sql = string.Format(sql, id);

                objList = PostgreHelper.GetSingleEntity<Invoice>(sql);

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
        public int Insert(Invoice obj)
        {
            try
            {
                Invoice invoice = new Invoice();
                invoice.order_id = obj.order_id;
                invoice.invoice_type = obj.invoice_type;
                invoice.invoice_index = obj.invoice_index;
                invoice.invoice_time = obj.invoice_time;
                invoice.invoice_price = obj.invoice_price;
                invoice.invoice_ratio = obj.invoice_ratio;
                invoice.invoice_all_price = obj.invoice_all_price;
                invoice.pay_type = obj.pay_type;
                invoice.remark = obj.remark;
                int count = PostgreHelper.InsertSingleEntity<Invoice>("jinchen.invoice_info", invoice);
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
        public int Update(Invoice obj)
        {
            try
            {
                int count = 0;
                string sql = "update jinchen.invoice_info set invoice_type={0},invoice_index='{1}',invoice_time='{2}'," +
                    "invoice_price={3},invoice_ratio={4},invoice_all_price={5},pay_type={6},remark='{7}' where id={8}";
                sql = string.Format(sql, obj.invoice_type, obj.invoice_index, obj.invoice_time, obj.invoice_price, 
                    obj.invoice_ratio, obj.invoice_all_price, obj.pay_type, obj.remark, obj.id);
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
        public int Del(int id)
        {
            try
            {
                string sql = "delete from jinchen.invoice_info where id={0}";
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
