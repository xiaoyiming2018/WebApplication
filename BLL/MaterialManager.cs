using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public class MaterialManager
    {
        public MaterialService MS = new MaterialService();

        public List<Material> SelectAll(string material_name = "")
        {
            List<Material> objList = MS.SelectAll(material_name);
            return objList;
        }

        /// <summary>
        /// 根据ID查询，查询结果为一笔数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Material SelectSingle(int id)
        {
            Material obj = MS.SelectSingle(id);
            return obj;
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Insert(Material obj)
        {
            int count = MS.Insert(obj);
            return count;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Update(Material obj)
        {
            int count = MS.Update(obj);
            return count;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Del(int id)
        {
            int count = MS.Del(id);
            return count;
        }
    }
}
