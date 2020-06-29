using Model;
using Model.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class PurchaseService
    {

        /// <summary>
        /// 用于自动生成采购单号
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Purchase> SelectTodayForPurchaseIndex(string purchase_time)
        {
            try
            {
                List<Purchase> objList = new List<Purchase>();
                string sql = null;
                sql = "SELECT a.purchase_index " +
                 "FROM jinchen.purchase_info a, jinchen.company_info b " +
                   " where a.supplier_id = b.id and to_char(a.purchase_time,'yyyy-MM')='{0}' group by a.purchase_index ";
                sql = string.Format(sql, purchase_time);

                objList = PostgreHelper.GetEntityList<Purchase>(sql);

                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 用于自动生成采购单号
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Purchase> SelectBySupplierId(int supplier_id)
        {
            try
            {
                List<Purchase> objList = new List<Purchase>();
                string sql = null;
                sql = "SELECT * " +
                 "FROM jinchen.purchase_info a, jinchen.company_info b " +
                   " where a.supplier_id = b.id and flag=0 and supplier_id={0} ";
                sql = string.Format(sql, supplier_id);

                objList = PostgreHelper.GetEntityList<Purchase>(sql);

                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据id查询单笔数据
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns></returns>
        public Purchase SelectById(int id)
        {
            try
            {
                Purchase obj = new Purchase();
                string sql = "SELECT a.id,a.supplier_id, b.company_name, a.purchase_index, a.category, a.material_name, a.material_spec, " +
                "a.material_num, a.material_unit, a.material_price, a.material_all_price,deliver_index,deliver_time,money_onoff,money_way,confirm_time,status " +
                 "FROM jinchen.purchase_info a, jinchen.company_info b " +
                   " where a.supplier_id = b.id and a.id={0} ";
                sql = string.Format(sql, id);
                obj = PostgreHelper.GetSingleEntity<Purchase>(sql);
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询入库单中尚未确认结款的单子（物料购入流水账和供应商结款共用一个）
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Purchase> SelectDeliverAll(string start_time, string end_time,string company_name, string purchase_index, string material_name,string deliver_index,string category)
        {
            try
            {
                List<Purchase> objList = new List<Purchase>();
                string sql = null;
                if (!string.IsNullOrEmpty(company_name))
                {
                    company_name = company_name.Replace("(", "\\(").Replace(")", "\\)");
                }
                sql = "SELECT a.id, b.company_name, a.purchase_index, a.category,a.deliver_index,a.deliver_time, a.material_name, a.material_spec, " +
                "a.material_num, a.material_unit, a.material_price, a.material_all_price,money_onoff,money_way,status,confirm_time " +
                 "FROM jinchen.purchase_info a, jinchen.company_info b " +
                   " where a.supplier_id = b.id and a.purchase_index ~* '{0}' and b.company_name ~* '{1}' and a.material_name ~* '{2}' and flag=0 and " +
                   "a.deliver_index ~* '{3}' and a.status =0 and to_char(a.deliver_time,'yyyy-MM-dd')>='{4}' and to_char(a.deliver_time,'yyyy-MM-dd')<='{5}' and a.category ~* '{6}' " +
                   "order by a.purchase_index desc ";
                sql = string.Format(sql, purchase_index, company_name, material_name, deliver_index, start_time, end_time, category);

                objList = PostgreHelper.GetEntityList<Purchase>(sql);

                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 历史购入清单：已确认结款的
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Purchase> SelectHistory(string start_time, string end_time, string confirm_start_time, string confirm_end_time, string company_name, string purchase_index,
            string material_name, string deliver_index,string category)
        {
            try
            {
                List<Purchase> objList = new List<Purchase>();
                string sql = null;
                if (!string.IsNullOrEmpty(company_name))
                {
                    company_name = company_name.Replace("(", "\\(").Replace(")", "\\)");
                }
                sql = "SELECT a.id, b.company_name, a.purchase_index, a.category,a.deliver_index,a.deliver_time, a.material_name, a.material_spec, " +
                "a.material_num, a.material_unit, a.material_price, a.material_all_price,money_onoff,money_way,confirm_time " +
                 "FROM jinchen.purchase_info a, jinchen.company_info b " +
                   " where a.supplier_id = b.id and a.purchase_index ~* '{0}' and b.company_name ~* '{1}' and a.material_name ~* '{2}' and flag=0 and " +
                   "a.deliver_index ~* '{3}' and to_char(a.deliver_time,'yyyy-MM-dd')>='{4}' and to_char(a.deliver_time,'yyyy-MM-dd')<='{5}' and a.status=1 " +
                   "and to_char(a.confirm_time,'yyyy-MM-dd')>='{6}' and to_char(a.confirm_time,'yyyy-MM-dd')<='{7}' and a.category ~* '{8}' " +
                   "order by a.purchase_index desc ";
                sql = string.Format(sql, purchase_index, company_name, material_name, deliver_index, start_time, end_time, confirm_start_time, confirm_end_time, category);

                objList = PostgreHelper.GetEntityList<Purchase>(sql);

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
        public int Insert(Purchase obj)
        {

            try
            {
                string sql = "INSERT INTO jinchen.purchase_info(supplier_id, purchase_index, category, material_name, " +
                    "material_spec, material_num, material_unit, material_price, material_all_price,purchase_time," +
                    "deliver_index,deliver_time,money_onoff,money_way,status,confirm_time) " +
                    "values({0},'{1}','{2}','{3}','{4}',{5},'{6}',{7},{8},'{9}','{10}','{11}',{12},{13},{14},'{15}') ";
                sql = string.Format(sql, obj.supplier_id, obj.purchase_index, obj.category, obj.material_name, obj.material_spec, obj.material_num,
                    obj.material_unit, obj.material_price, obj.material_all_price, obj.purchase_time,obj.deliver_index,obj.deliver_time,
                    obj.money_onoff,obj.money_way,obj.status,obj.confirm_time);
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 更新入库信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int UpdateDeliver(Purchase obj)
        {
            try
            {
                int count = 0;
                string sql = "update jinchen.purchase_info set material_name='{0}', material_spec='{1}',material_num={2}," +
                    "material_unit='{3}',material_price={4},material_all_price={5} where id={6}";
                sql = string.Format(sql, obj.material_name, obj.material_spec, obj.material_num, obj.material_unit, 
                    obj.material_price, obj.material_all_price, obj.id);
                count = PostgreHelper.ExecuteNonQuery(sql);
                return count;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 更新公共信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int UpdateCommonDeliver(Purchase obj)
        {
            try
            {
                int count = 0;
                string sql = "update jinchen.purchase_info set category='{0}',supplier_id={1}, deliver_time='{2}', " +
                    "money_onoff={3},money_way={4},confirm_time='{5}',status={6} where purchase_index='{7}'";
                sql = string.Format(sql, obj.category, obj.supplier_id, obj.deliver_time, obj.money_onoff, obj.money_way, obj.confirm_time, 
                    obj.status,obj.purchase_index);
                count = PostgreHelper.ExecuteNonQuery(sql);
                return count;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 更新入库单的状态（用于区分是否结款）
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int UpdateDeliverStatus(Purchase obj)
        {
            try
            {
                int count = 0;
                string sql = "update jinchen.purchase_info set status={0},confirm_time='{1}',money_onoff={2} where id={3}";
                sql = string.Format(sql, obj.status,obj.confirm_time,obj.money_onoff,obj.id);
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
                string sql = "update jinchen.purchase_info set flag=1 where id={0}";
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
