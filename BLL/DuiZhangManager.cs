using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public class DuiZhangManager
    {
        public DuiZhangService DZS = new DuiZhangService();

        /// <summary>
        /// 用于绑定发票视图
        /// </summary>
        /// <returns></returns>
        public List<DuiZhang> SelectForInvoice(string dz_start_time, string dz_end_time, string deliver_company_head="", string dz_index="")
        {
            List<DuiZhang> objList = DZS.SelectForInvoice(dz_start_time, dz_end_time, deliver_company_head, dz_index);
            return objList;
        }

        /// <summary>
        /// 查看某一对账单号中的所有订单信息
        /// </summary>
        /// <returns></returns>
        public List<DuiZhang> SelectByDzIndex(string dz_index)
        {
            List<DuiZhang> objList = DZS.SelectByDzIndex(dz_index);
            return objList;
        }

        /// <summary>
        /// 历史数据
        /// </summary>
        /// <returns></returns>
        public List<DuiZhang> SelectHistory(string devliver_start_time, string deliver_end_time, string dz_start_time, string dz_end_time,
    string company_order_index = "", string company_name = "", string order_name = "", string dz_index = "",string invoice_index="",string deliver_index="")
        {
            List<DuiZhang> objList = DZS.SelectHistory(devliver_start_time, deliver_end_time,dz_start_time, dz_end_time, company_order_index,
                company_name, order_name, dz_index, invoice_index, deliver_index);
            return objList;
        }

        public DuiZhang SelectById(int id) {
            DuiZhang duiZhang = DZS.SelectById(id);
            return duiZhang;
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Insert(DuiZhang obj)
        {
            int count = DZS.Insert(obj);
            return count;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Update(DuiZhang duiZhang)
        {
            int count = DZS.Update(duiZhang);
            return count;
        }
    }
}
