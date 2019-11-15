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

        //订单id
        public int order_id { get; set; }
        //订单号
        public string order_index { get; set; }
        //客户订单号
        public string company_order_index { get; set; }

        //发票类型
        public int invoice_type { get; set; }
        //发票号
        public string invoice_index { get; set; }
        //开票时间
        public DateTime invoice_time { get; set; }
        //金额
        public double invoice_price { get; set; }
        //税额
        public double invoice_ratio { get; set; }
        //价税合计
        public double invoice_all_price { get; set; }
        //付款方式
        public int pay_type { get; set; }

        //备注
        public string remark { get; set; }

    }
}
