using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using Model;

namespace BLL
{
    public class CompanyManager
    {
        public CompanyService CS = new CompanyService();

        /// <summary>
        /// 根据名称、税号、账户和开户行对公司模糊查询，查询结果为一笔或多笔数据
        /// </summary>
        /// <param name="company_name"></param>
        /// <param name="tax_number"></param>
        /// <param name="account"></param>
        /// <param name="bank"></param>
        /// <returns></returns>
        public List<Company> SelectAll(int type,string company_name="", string tax_number = "", string account = "", string bank = "")
        {
            List<Company> objList = CS.SelectAll(company_name, tax_number, account, bank, type);
            return objList;
        }

        /// <summary>
        /// 根据ID查询，查询结果为一笔数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Company SelectSingle(int id)
        {
            Company obj = CS.SelectSingle(id);
            return obj;
        }

        /// <summary>
        /// 根据name查询，查询结果为一笔数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Company SelectSingleByName(string company_name)
        {
            Company obj = CS.SelectSingleByName(company_name);
            return obj;
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Insert(Company obj)
        {
            int count = CS.Insert(obj);
            return count;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Update(Company obj)
        {
            int count = CS.Update(obj);
            return count;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Del(int id)
        {
            int count = CS.Del(id);
            return count;
        }
    }
}
