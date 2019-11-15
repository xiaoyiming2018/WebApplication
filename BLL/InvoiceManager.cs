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
        public List<Invoice> SelectAll(string invoice_index = "", string company_name = "", string order_index="", string company_order_index="") { 
            List<Invoice> objList = IS.SelectAll(invoice_index, company_name, order_index, company_order_index);
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
        /// 插入
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Update(Invoice obj)
        {
            int count = IS.Update(obj);
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
