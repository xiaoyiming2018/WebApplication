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
        public List<Invoice> SelectAll(int status, string start_time, string end_time, string confirm_start_time, string confirm_end_time, 
            string invoice_index, string company_name)
        {
            try
            {
                List<Invoice> objList = new List<Invoice>();
                string sql = null;
                sql = "select a.id,a.company_name,a.invoice_type,a.invoice_index," +
                    "a.invoice_time,a.invoice_price,a.invoice_ratio,a.invoice_all_price,a.pay_type,a.remark,confirm_time " +
                    "from jinchen.invoice_info a " +
                    "where a.invoice_index ~* '{0}' and a.company_name ~* '{1}' and a.status={2} and to_char(invoice_time,'yyyy-MM-dd')>='{3}' " +
                    "and to_char(invoice_time,'yyyy-MM-dd')<='{4}' and to_char(confirm_time,'yyyy-MM-dd')>='{5}' and to_char(confirm_time,'yyyy-MM-dd')<='{6}' " +
                    "order by a.invoice_time desc";
                sql = string.Format(sql, invoice_index, company_name, status,start_time,end_time, confirm_start_time, confirm_end_time);

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
                sql = "select * from jinchen.invoice_info a where a.id={0}";
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
                invoice.company_name = obj.company_name;
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
                    "invoice_price={3},invoice_ratio={4},invoice_all_price={5},pay_type={6},remark='{7}',company_name='{8}' where id={9}";
                sql = string.Format(sql, obj.invoice_type, obj.invoice_index, obj.invoice_time, obj.invoice_price, 
                    obj.invoice_ratio, obj.invoice_all_price, obj.pay_type, obj.remark,obj.company_name, obj.id);
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
        public int UpdateStatus(int id,int pay_type)
        {
            try
            {
                int count = 0;
                string sql = "update jinchen.invoice_info set status=1,confirm_time='{0}',pay_type={1} where id={2}";
                sql = string.Format(sql,DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss"),pay_type, id);
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
