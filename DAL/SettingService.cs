using Model;
using Model.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class SettingService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Setting> SelectConfigList(int all_type)
        {
            try
            {
                List<Setting> objList = new List<Setting>();
                string sql = null;
                sql = "select id,config_list from jinchen.setting where all_type={0} order by config_list";
                sql = string.Format(sql, all_type);

                objList = PostgreHelper.GetEntityList<Setting>(sql);

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
        public Setting SelectInUse(int all_type)
        {
            try
            {
                Setting obj = new Setting();
                string sql = "select * from jinchen.setting where in_use=1 and all_type={0}";
                sql = string.Format(sql, all_type);
                obj = PostgreHelper.GetSingleEntity<Setting>(sql);
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
        public int Insert(Setting obj)
        {
            try
            {
                Setting setting = new Setting();
                setting.config_list = obj.config_list;
                setting.index_begin = obj.index_begin;
                setting.all_type = obj.all_type;
                setting.in_use = obj.in_use;
                int count = PostgreHelper.InsertSingleEntity<Setting>("jinchen.setting", setting);
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 更新使用哪一个
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Update(Setting obj)
        {
            try
            {
                int count = 0;
                string sql = "update jinchen.setting set index_begin='{0}',in_use='{1}' where all_type={2}";
                sql = string.Format(sql, obj.index_begin, obj.in_use, obj.all_type);
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
