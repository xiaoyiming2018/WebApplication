using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Invoice
    {
        //自增id
        public int id { get; set; }

        //公司名称
        public string company_name { get; set; }

        //开票公司
        public string invoice_company { get; set; }

        //发票类型
        public int invoice_type { get; set; }
        //发票号
        public string invoice_index { get; set; }
        //开票时间
        public DateTime invoice_time { get; set; }
        //应付金额（发票金额）
        public double invoice_price { get; set; }
        //实付金额
        public double invoice_real_price { get; set; }
        //客户预付款
        public double invoice_prepay { get; set; }
        //付款方式：0现金，1银行转账，2承兑
        public int pay_type { get; set; }

        //备注
        public string remark { get; set; }
        //状态（0：未对账，1：已对账）
        public int status { get; set; }
        //对账日期
        public DateTime confirm_time { get; set; }

    }
}
