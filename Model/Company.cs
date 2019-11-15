using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Company
    {
        //自增id
        public int id { get; set; }
        //公司名称
        public string company_name { get; set; }
        //税号
        public string tax_num { get; set; }
        //账号
        public string account { get; set; }
        //开户行
        public string bank { get; set; }
        //地址
        public string address { get; set; }
        //客户0、供应商1
        public int company_type { get; set; }

        //联系人
        public string name { get; set; }
        //职位
        public string position { get; set; }
        //手机号
        public string telephone { get; set; }
        //邮箱
        public string email { get; set; }
    }
}
