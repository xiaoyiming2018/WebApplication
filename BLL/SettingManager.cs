using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using Model;

namespace BLL
{
    public class SettingManager
    {
        public SettingService SS = new SettingService();
        /// <summary>
        /// 模糊查询结果为一笔或多笔数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Setting> SelectConfigList(int all_type)
        {
            List<Setting> objList = SS.SelectConfigList(all_type);
            return objList;
        }

        /// 模糊查询结果为一笔或多笔数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Setting SelectInUse(int all_type)
        {
            Setting obj = SS.SelectInUse(all_type);
            return obj;
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Insert(Setting obj)
        {
            int count = SS.Insert(obj);
            return count;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Update(Setting obj)
        {
            int count = SS.Update(obj);
            return count;
        }
    }
}
