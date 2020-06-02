using Model;
using Model.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class DuiZhangService
    {
        /// <summary>
        /// 用于销售管理：模糊查询，查询结果为一笔或多笔数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<DuiZhang> SelectForInvoice( string dz_start_time, string dz_end_time, string deliver_start_time, string deliver_end_time,
            string deliver_index, string deliver_company_head, string order_name, string dz_index)
        {
            try
            {
                List<DuiZhang> objList = new List<DuiZhang>();
                string sql = null;
                if (!string.IsNullOrEmpty(deliver_company_head))
                {
                    deliver_company_head = deliver_company_head.Replace("(", "\\(").Replace(")", "\\)");
                }
                sql = "select a.id, a.dz_index,a.dui_time,a.deliver_time, a.company_order_index,b.deliver_index,a.company_name,a.dui_num,a.dui_price,a.dui_all_price," +
                    "a.order_name,a.unit  from jinchen.duizhang_info a, jinchen.sale_info b " +
                      "where a.sale_id= b.id and deliver_index ~*'{0}' and deliver_company_head ~*'{1}' " +
                      "and to_char(a.dui_time,'yyyy-MM-dd')>='{2}' and to_char(a.dui_time,'yyyy-MM-dd')<='{3}' and " +
                      "order_name ~* '{4}' and dz_index ~* '{5}' and to_char(a.deliver_time,'yyyy-MM-dd')>='{6}' and to_char(a.deliver_time,'yyyy-MM-dd')<='{7}' and a.invoice_time is null order by a.dui_time desc, deliver_index desc,deliver_company_head ";
                sql = string.Format(sql, deliver_index, deliver_company_head,dz_start_time, dz_end_time, order_name, dz_index, deliver_start_time, deliver_end_time);

                objList = PostgreHelper.GetEntityList<DuiZhang>(sql);

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
        public List<DuiZhang> SelectHistory(string deliver_start_time, string deliver_end_time, string dz_start_time, string dz_end_time, 
            string company_order_index, string company_name, string order_name,string dz_index,string invoice_index,string deliver_index)
        {
            try
            {
                List<DuiZhang> objList = new List<DuiZhang>();
                string sql = null;
                if (!string.IsNullOrEmpty(company_name))
                {
                    company_name = company_name.Replace("(", "\\(").Replace(")", "\\)");
                }
                sql = "select a.company_order_index,a.company_name,a.dui_num,a.dui_price,a.dui_all_price," +
                    "a.order_name,a.unit,a.dz_index,a.invoice_index,a.deliver_time,dui_time,b.deliver_index " +
                    "from (select * " +
                    " from jinchen.duizhang_info a " +
                      "where company_order_index ~*'{0}' and company_name ~*'{1}' and to_char(a.deliver_time,'yyyy-MM-dd')>='{2}' and " +
                      "to_char(a.deliver_time,'yyyy-MM-dd')<='{3}' and to_char(a.dui_time,'yyyy-MM-dd')>='{4}' and to_char(a.dui_time,'yyyy-MM-dd')<='{5}' and " +
                      "order_name ~* '{6}' and dz_index ~* '{7}' and invoice_index ~* '{8}' ) a " +
                      "left join (select * from jinchen.sale_info) b on a.sale_id=b.id and deliver_index ~* '{9}' " +
                      "order by a.dui_time desc, b.deliver_index desc,a.company_name";
                sql = string.Format(sql, company_order_index, company_name, deliver_start_time, deliver_end_time, dz_start_time, dz_end_time,
                    order_name, dz_index, invoice_index, deliver_index);

                objList = PostgreHelper.GetEntityList<DuiZhang>(sql);

                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 单笔查询
        /// </summary>
        /// <returns></returns>
        public DuiZhang SelectById(int id)
        {
            try
            {
                DuiZhang obj = new DuiZhang();
                string sql = null;
                sql = "select a.id,a.dz_index,a.dui_num,a.dui_price,a.dui_time,a.company_name,a.order_name,a.unit,a.dui_all_price,a.deliver_time,a.company_order_index,b.deliver_index " +
                    "from jinchen.duizhang_info a,jinchen.sale_info b where a.sale_id=b.id and a.id={0} ";
                sql = string.Format(sql, id);
                obj = PostgreHelper.GetSingleEntity<DuiZhang>(sql);

                return obj;
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
        public int Insert(DuiZhang obj)
        {

            try
            {
                string sql = "insert into jinchen.duizhang_info(sale_id,dz_index,dui_num,dui_price,dui_time,company_name," +
                    "order_name,unit,deliver_time,company_order_index,dui_all_price,invoice_index) values({0},'{1}',{2},{3},'{4}','{5}','{6}','{7}','{8}','{9}',{10},'{11}')";
                sql = string.Format(sql, obj.sale_id, obj.dz_index, obj.dui_num,obj.dui_price, obj.dui_time,
                    obj.company_name,obj.order_name,obj.unit,obj.deliver_time,obj.company_order_index,obj.dui_all_price, obj.invoice_index);
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 关联发票更新
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Update(DuiZhang obj)
        {
            try
            {
                int count = 0;
                string sql = "update jinchen.duizhang_info set invoice_index='{0}',invoice_time='{1}' where id={2}";
                sql = string.Format(sql, obj.invoice_index, obj.invoice_time, obj.id);
                count = PostgreHelper.ExecuteNonQuery(sql);
                return count;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
