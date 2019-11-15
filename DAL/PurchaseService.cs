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
        /// 根据联系人名模糊查询，查询结果为一笔或多笔数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Purchase> SelectAll(string company_name, string purchase_index, string material_name)
        {
            try
            {
                List<Purchase> objList = new List<Purchase>();
                string sql = null;
                sql = "SELECT a.id, b.company_name, a.purchase_index, a.category, a.material_name, a.material_spec, "+
                "a.material_num, a.material_unit, a.material_price, a.material_all_price "+
                 "FROM jinchen.purchase_info a, jinchen.company_info b " +
                   " where a.supplier_id = b.id and a.purchase_index ~* '{0}' and b.company_name ~* '{1}' and a.material_name ~* '{2}' "+
                   "order by a.purchase_index,deliver_time,money_onoff desc ";
                sql = string.Format(sql, purchase_index, company_name, material_name);

                objList = PostgreHelper.GetEntityList<Purchase>(sql);

                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询当天的采购数量，用于自动生成采购单号
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Purchase> SelectTodayForPurchaseIndex(string purchase_time)
        {
            try
            {
                List<Purchase> objList = new List<Purchase>();
                string sql = null;
                sql = "SELECT * " +
                 "FROM jinchen.purchase_info a, jinchen.company_info b " +
                   " where a.supplier_id = b.id and to_char(a.purchase_time,'yyyy-MM-dd')='{0}' ";
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
        /// 查询单笔数
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns></returns>
        public Purchase SelectById(int id)
        {
            try
            {
                Purchase obj = new Purchase();
                string sql = "SELECT a.id,a.supplier_id, b.company_name, a.purchase_index, a.category, a.material_name, a.material_spec, " +
                "a.material_num, a.material_inventory_num,a.material_unit, a.material_price, a.material_all_price,deliver_index,deliver_time,money_onoff,money_way,confirm_time,status " +
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
        /// 用于入库单：模糊查询，查询结果为一笔或多笔数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Purchase> SelectDeliverAll(string start_time, string end_time,string company_name, string purchase_index, string material_name,string deliver_index)
        {
            try
            {
                List<Purchase> objList = new List<Purchase>();
                string sql = null;
                sql = "SELECT a.id, b.company_name, a.purchase_index, a.category,a.deliver_index,a.deliver_time, a.material_name, a.material_spec, " +
                "a.material_num,a.material_inventory_num, a.material_unit, a.material_price, a.material_all_price,money_onoff,money_way,status,confirm_time " +
                 "FROM jinchen.purchase_info a, jinchen.company_info b " +
                   " where a.supplier_id = b.id and a.purchase_index ~* '{0}' and b.company_name ~* '{1}' and a.material_name ~* '{2}' and " +
                   "a.deliver_index ~* '{3}' and a.status =0 and to_char(a.deliver_time,'yyyy-MM-dd')>='{4}' and to_char(a.deliver_time,'yyyy-MM-dd')<='{5}' " +
                   "order by a.purchase_index ";
                sql = string.Format(sql, purchase_index, company_name, material_name, deliver_index, start_time, end_time);

                objList = PostgreHelper.GetEntityList<Purchase>(sql);

                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Purchase> SelectDeliverAllForSupplier(int money_onoff,string start_time, string end_time, string company_name, string purchase_index,
            string material_name, string deliver_index)
        {
            try
            {
                List<Purchase> objList = new List<Purchase>();
                string sql = null;
                sql = "SELECT a.id, b.company_name, a.purchase_index, a.category,a.deliver_index,a.deliver_time, a.material_name, a.material_spec, " +
                "a.material_num,a.material_inventoy_num, a.material_unit, a.material_price, a.material_all_price,money_onoff,money_way,confirm_time " +
                 "FROM jinchen.purchase_info a, jinchen.company_info b " +
                   " where a.supplier_id = b.id and a.purchase_index ~* '{0}' and b.company_name ~* '{1}' and a.material_name ~* '{2}' and " +
                   "a.deliver_index ~* '{3}' and a.deliver_index !='no' and to_char(a.deliver_time,'yyyy-MM-dd')>='{4}' and to_char(a.deliver_time,'yyyy-MM-dd')<='{5}' " +
                   "order by a.purchase_index ";
                sql = string.Format(sql, company_name, purchase_index, material_name, deliver_index, start_time, end_time, money_onoff);

                objList = PostgreHelper.GetEntityList<Purchase>(sql);

                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 用于入库单：模糊查询，查询结果为一笔或多笔数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Purchase> SelectForDeliver()
        {
            try
            {
                List<Purchase> objList = new List<Purchase>();
                string sql = null;
                sql = "SELECT a.id, b.company_name, a.purchase_index, a.category,a.deliver_index,a.deliver_time, a.material_name, a.material_spec, " +
                "a.material_num, a.material_unit, a.material_price, a.material_all_price,money_onoff,money_way,confirm_time " +
                 "FROM jinchen.purchase_info a, jinchen.company_info b " +
                   " where a.supplier_id = b.id and a.deliver_index ='no' ";
                sql = string.Format(sql);

                objList = PostgreHelper.GetEntityList<Purchase>(sql);

                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 用于入库单：模糊查询，查询结果为一笔或多笔数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Purchase> SelectHistory(string start_time, string end_time, string company_name, string purchase_index,
            string material_name, string deliver_index)
        {
            try
            {
                List<Purchase> objList = new List<Purchase>();
                string sql = null;
                sql = "SELECT a.id, b.company_name, a.purchase_index, a.category,a.deliver_index,a.deliver_time, a.material_name, a.material_spec, " +
                "a.material_num, a.material_unit, a.material_price, a.material_all_price,money_onoff,money_way,confirm_time " +
                 "FROM jinchen.purchase_info a, jinchen.company_info b " +
                   " where a.supplier_id = b.id and a.purchase_index ~* '{0}' and b.company_name ~* '{1}' and a.material_name ~* '{2}' and " +
                   "a.deliver_index ~* '{3}' and a.deliver_index !='no' and to_char(a.deliver_time,'yyyy-MM-dd')>='{4}' and to_char(a.deliver_time,'yyyy-MM-dd')<='{5}' and a.status=1 " +
                   "order by a.purchase_index ";
                sql = string.Format(sql, purchase_index, company_name, material_name, deliver_index, start_time, end_time);

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
                    "deliver_index,deliver_time,money_onoff,money_way) " +
                    "values({0},'{1}','{2}','{3}','{4}',{5},'{6}',{7},{8},'{9}','{10}','{11}',{12},{13}) ";
                sql = string.Format(sql, obj.supplier_id, obj.purchase_index, obj.category, obj.material_name, obj.material_spec, obj.material_num,
                    obj.material_unit, obj.material_price, obj.material_all_price, obj.purchase_time,obj.deliver_index,obj.deliver_time,obj.money_onoff,obj.money_way);
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
        public int Update(Purchase obj)
        {
            try
            {
                int count = 0;
                string sql = "update jinchen.purchase_info set category='{0}',material_name='{1}', material_spec='{2}',material_num={3}," +
                    "material_unit='{4}',material_price={5},material_all_price={6},supplier_id={7} where id={8}";
                sql = string.Format(sql, obj.category, obj.material_name, obj.material_spec, obj.material_num,obj.material_unit,obj.material_price,obj.material_all_price, obj.supplier_id, obj.id);
                count = PostgreHelper.ExecuteNonQuery(sql);
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
                string sql = "update jinchen.purchase_info set category='{0}',material_name='{1}', material_spec='{2}',material_num={3}," +
                    "material_unit='{4}',material_price={5},material_all_price={6},supplier_id={7}, deliver_time='{8}', " +
                    "money_onoff={9},money_way={10} where id={11}";
                sql = string.Format(sql, obj.category, obj.material_name, obj.material_spec, obj.material_num, obj.material_unit, 
                    obj.material_price, obj.material_all_price, obj.supplier_id,obj.deliver_time, obj.money_onoff, obj.money_way, obj.id);
                count = PostgreHelper.ExecuteNonQuery(sql);
                return count;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 更新deliver_time/inventory_status/deliver_status
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int UpdateDeliverStatus(Purchase obj)
        {
            try
            {
                int count = 0;
                string sql = "update jinchen.purchase_info set status={0},confirm_time='{1}' where id={2}";
                sql = string.Format(sql, obj.status,obj.confirm_time,obj.id);
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
                string sql = "delete from jinchen.purchase_info where id={0}";
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
