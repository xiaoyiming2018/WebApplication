using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public class PurchaseManager
    {
        public PurchaseService PS = new PurchaseService();

        /// <summary>
        /// 用于生成唯一的采购单号
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Purchase> SelectTodayForPurchaseIndex(string purchase_time)
        {
            List<Purchase> objList = PS.SelectTodayForPurchaseIndex(purchase_time);
            return objList;
        }

        /// <summary>
        /// 用于生成唯一的采购单号
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Purchase> SelectBySupplierId(int supplier_id)
        {
            List<Purchase> objList = PS.SelectBySupplierId(supplier_id);
            return objList;
        }

        /// <summary>
        /// 模糊查询结果为一笔或多笔数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Purchase> SelectDeliverAll(string start_time, string end_time, string company_name="", string purchase_index="", string material_name="", string deliver_index="")
        {
            List<Purchase> objList = PS.SelectDeliverAll(start_time,end_time,company_name, purchase_index, material_name, deliver_index);
            return objList;
        }


        /// <summary>
        /// 历史购入清单：已确认结款的
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Purchase> SelectHistory(string start_time, string end_time, string confirm_start_time, string confirm_end_time, string company_name="" , string purchase_index="",
            string material_name="", string deliver_index="")
        {
            List<Purchase> objList = PS.SelectHistory(start_time, end_time, confirm_start_time, confirm_end_time, company_name, purchase_index, material_name, deliver_index);
            return objList;
        }

        

        /// <summary>
        /// 根据id单笔查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Purchase SelectById(int id)
        {
            Purchase obj = PS.SelectById(id);
            return obj;
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Insert(Purchase obj)
        {
            int count = PS.Insert(obj);
            return count;
        }

        /// <summary>
        /// 更新，用于修改入库单中的信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int UpdateDeliver(Purchase obj)
        {
            int count = PS.UpdateDeliver(obj);
            return count;
        }

        /// <summary>
        /// 更新，用于确认和取消确认
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int UpdateDeliverStatus(Purchase obj)
        {
            int count = PS.UpdateDeliverStatus(obj);
            return count;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Del(int id)
        {
            int count = PS.Del(id);
            return count;
        }
    }
}
