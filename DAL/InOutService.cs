using Model;
using Model.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class InOutService
    {
        /// <summary>
        /// 出入库流水
        /// </summary>
        /// <returns></returns>
        public List<InOut> SelectAll()
        {
            try
            {
                List<InOut> objList = new List<InOut>();
                string sql = null;

                sql = "select a.id,a.material_id,a.inout_price,a.inout_all_price,a.inout_num,a.create_time,a.remarks,a.flag,b.material_name,b.price,b.picture " +
                    "from jinchen.inout_info a,jinchen.material_info b where a.material_id=b.id order by create_time desc";
                sql = string.Format(sql);

                objList = PostgreHelper.GetEntityList<InOut>(sql);

                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 返回品名在库存中的结存数量和结存总金额
        /// </summary>
        /// <returns></returns>
        public List<InOut> SelectInventoryInfo()
        {
            try
            {
                List<InOut> objList = new List<InOut>();
                string sql = null;

                sql = "SELECT material_name, sum(inout_num * flag) as inout_num,sum(inout_all_price * flag) as inout_all_price,b.picture " +
                      " FROM jinchen.inout_info a, jinchen.material_info b " +
                    " where a.material_id = b.id " +
                    " group by b.material_name, b.picture " +
                    " order by b.material_name";
                sql = string.Format(sql);

                objList = PostgreHelper.GetEntityList<InOut>(sql);

                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 品名在库存中的结存数量和结存总金额日趋势
        /// </summary>
        /// <returns></returns>
        public List<InOut> SelectInventoryTrendDay()
        {
            try
            {
                List<InOut> objList = new List<InOut>();
                string sql = null;

                sql = "select material_name,sum(inout_num*flag) as inout_num,sum(inout_all_price*flag) as inout_all_price,to_date(a.date,'yyyy-MM-dd') as create_time "+
                       " from (select material_id, inout_num, inout_price, flag, inout_all_price, to_char(create_time, 'yyyy-MM-dd') as date " +
                       " from jinchen.inout_info) a,jinchen.material_info b " +
                       " where a.material_id = b.id " +
                       " group by b.material_name,a.date " +
                       " order by a.date";
                sql = string.Format(sql);

                objList = PostgreHelper.GetEntityList<InOut>(sql);

                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 品名在库存中的结存数量和结存总金额月趋势
        /// </summary>
        /// <returns></returns>
        public List<InOut> SelectInventoryTrendMonth()
        {
            try
            {
                List<InOut> objList = new List<InOut>();
                string sql = null;

                sql = "select material_name,sum(inout_num*flag) as inout_num,sum(inout_all_price*flag) as inout_all_price,to_date(a.date,'yyyy-MM') as create_time " +
                       " from(select material_id, inout_num, inout_price, flag, inout_all_price, to_char(create_time, 'yyyy-MM') as date " +
                       " from jinchen.inout_info) a,jinchen.material_info b " +
                       " where a.material_id = b.id " +
                       " group by b.material_name,a.date " +
                       " order by a.date";
                sql = string.Format(sql);

                objList = PostgreHelper.GetEntityList<InOut>(sql);

                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 品名在库存中的结存数量和结存总金额年趋势
        /// </summary>
        /// <returns></returns>
        public List<InOut> SelectInventoryTrendYear()
        {
            try
            {
                List<InOut> objList = new List<InOut>();
                string sql = null;

                sql = "select material_name,sum(inout_num*flag) as inout_num,sum(inout_all_price*flag) as inout_all_price,to_date(a.date,'yyyy') as create_time " +
                       " from(select material_id, inout_num, inout_price, flag, inout_all_price, to_char(create_time, 'yyyy') as date " +
                       " from jinchen.inout_info) a,jinchen.material_info b " +
                       " where a.material_id = b.id " +
                       " group by b.material_name,a.date " +
                       " order by a.date";
                sql = string.Format(sql);

                objList = PostgreHelper.GetEntityList<InOut>(sql);

                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// 根据ID查询，查询结果为一笔数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public InOut SelectSingle(int id)
        {
            try
            {
                InOut obj = new InOut();
                string sql = "select a.id,a.material_id,a.inout_price,a.inout_all_price,a.inout_num,a.create_time,a.remarks,a.flag,b.material_num,b.price,b.picture " +
                    "from jinchen.inout_info a,jinchen.material_info b where a.material_id=b.id and a.id={0}";
                sql = string.Format(sql, id);
                obj = PostgreHelper.GetSingleEntity<InOut>(sql);
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
        public int Insert(InOut obj)
        {
            try
            {
                int count = 0;
                string sql = "insert into jinchen.inout_info (material_id, inout_num, inout_price, create_time, remarks, flag, inout_all_price) " +
                    "VALUES({0}, {1}, {2}, '{3}', '{4}', {5}, {6}); ";
                sql = string.Format(sql, obj.material_id, obj.inout_num, obj.inout_price, obj.create_time, obj.remarks, obj.flag, obj.inout_all_price);
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
        public int Update(InOut obj)
        {
            try
            {
                int count = 0;
                string sql = "update jinchen.inout_info set material_id={0},inout_num={1},inout_price={2},inout_all_price={3},flag={4} " +
                    ",remarks='{5}' where id={6}";
                sql = string.Format(sql, obj.material_id, obj.inout_num, obj.inout_price, obj.inout_all_price, obj.flag, obj.remarks, obj.id);
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
                string sql = "delete from jinchen.inout_info where id={0}";
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
