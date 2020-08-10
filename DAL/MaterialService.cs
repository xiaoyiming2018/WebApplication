using Model;
using Model.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class MaterialService
    {
        public List<Material> SelectAll(string material_name)
        {
            try
            {
                List<Material> objList = new List<Material>();
                string sql = null;
                if (!string.IsNullOrEmpty(material_name))
                {
                    material_name = material_name.Replace("(", "\\(").Replace(")", "\\)");
                }
                sql = "select * from jinchen.material_info where Material_name ~* '{0}' order by Material_name";
                sql = string.Format(sql, material_name);

                objList = PostgreHelper.GetEntityList<Material>(sql);

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
        public Material SelectSingle(int id)
        {
            try
            {
                Material obj = new Material();
                string sql = "select * from jinchen.material_info where id={0}";
                sql = string.Format(sql, id);
                obj = PostgreHelper.GetSingleEntity<Material>(sql);
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
        public int Insert(Material obj)
        {
            try
            {
                int count = PostgreHelper.InsertSingleEntity<Material>("jinchen.material_info", obj);
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
        public int Update(Material obj)
        {
            try
            {
                int count = 0;
                string sql = "update jinchen.material_info set price='{0}',picture='{1}' where id={2}";
                sql = string.Format(sql, obj.price, obj.picture, obj.id);
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
                string sql = "delete from jinchen.material_info where id={0}";
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
