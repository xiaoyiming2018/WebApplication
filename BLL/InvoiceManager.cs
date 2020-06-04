using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public class InvoiceManager
    {
        public InvoiceService IS = new InvoiceService();

        /// <summary>
        /// 模糊查询结果为一笔或多笔数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Invoice> SelectAll(int status, string start_time, string end_time, string confirm_start_time, string confirm_end_time, string invoice_index="", string company_name="",string invoice_company="") { 
            List<Invoice> objList = IS.SelectAll(status, start_time, end_time, confirm_start_time, confirm_end_time, invoice_index, company_name, invoice_company);
            return objList;
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Invoice> SelectAllInvoice()
        {
            List<Invoice> objList = IS.SelectAllInvoice();
            return objList;
        }

        /// <summary>
        /// 模糊查询结果为一笔或多笔数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Invoice SelectById(int id)
        {
            Invoice objList = IS.SelectById(id);
            return objList;
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Insert(Invoice obj)
        {
            int count = IS.Insert(obj);
            return count;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Update(Invoice obj)
        {
            int count = IS.Update(obj);
            return count;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int UpdateStatus(int id, string confirm_time, int pay_type)
        {
            int count = IS.UpdateStatus(id, confirm_time, pay_type);
            return count;
        }

        
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Del(int id)
        {
            int count = IS.Del(id);
            return count;
        }
    }
}
