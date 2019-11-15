using System;
using System.Collections.Generic;
using System.Text;
using Model;
using Model.Helper;

namespace DAL
{
    public class CompanyService
    {
        /// <summary>
        /// 根据名称、税号、账户和开户行对公司模糊查询，查询结果为一笔或多笔数据
        /// </summary>
        /// <param name="company_name"></param>
        /// <param name="tax_number"></param>
        /// <param name="account"></param>
        /// <param name="bank"></param>
        /// <returns></returns>
        public List<Company> SelectAll(string company_name, string tax_number, string account, string bank,int type)
        {
            try
            {
                List<Company> objList = new List<Company>();
                string sql = null;
                sql = "select * from jinchen.company_info where company_name ~* '{0}' and tax_num ~* '{1}' and account ~* '{2}' " +
                    "and bank ~* '{3}' and company_type={4} order by company_name";
                sql = string.Format(sql, company_name, tax_number, account, bank, type);

                objList = PostgreHelper.GetEntityList<Company>(sql);

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
        public Company SelectSingle(int id)
        {
            try
            {
                Company obj = new Company();
                string sql = "select * from jinchen.company_info where id={0}";
                sql = string.Format(sql, id);
                obj = PostgreHelper.GetSingleEntity<Company>(sql);
                return obj;
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
        public Company SelectSingleByName(string company_name)
        {
            try
            {
                Company obj = new Company();
                string sql = "select * from jinchen.company_info where company_name='{0}'";
                sql = string.Format(sql, company_name);
                obj = PostgreHelper.GetSingleEntity<Company>(sql);
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
        public int Insert(Company obj)
        {
            try
            {
                Company company = new Company();
                company.company_name = obj.company_name;
                company.tax_num = obj.tax_num;
                company.account = obj.account;
                company.bank = obj.bank;
                company.address = obj.address;
                company.company_type = obj.company_type;
                int count = PostgreHelper.InsertSingleEntity<Company>("jinchen.company_info", company);
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
        public int Update(Company obj)
        {
            try
            {
                int count = 0;
                string sql = "update jinchen.company_info set company_name='{0}',tax_num='{1}',account='{2}',bank='{3}',address='{4}' where id={5}";
                sql = string.Format(sql, obj.company_name, obj.tax_num, obj.account, obj.bank,obj.address, obj.id);
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
                string sql = "delete from jinchen.company_info where id={0}";
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
