using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class DuiZhang
    {
        //自增id
        public int id { get; set; }

        //出货单号
        public string deliver_index { get; set; }
        //出货公司抬头
        public string company_name { get; set; }
        //出货公司抬头
        public string deliver_company_head { get; set; }
        //出货公司抬头
        public string company_order_index { get; set; }
        //送货日期
        public DateTime deliver_time { get; set; }

        //送货单id
        public int sale_id { get; set; }
        //订单号
        public string order_index { get; set; }
        //订单名称
        public string order_name { get; set; }
        //单位
        public string unit { get; set; }

        // 对账单号
        public string dz_index { get; set; }
        //对账数量
        public double dui_num { get; set; }
        //单价
        public double dui_price { get; set; }
        //总价
        public double dui_all_price { get; set; }
        //对账时间
        public DateTime dui_time { get; set; }


        // 对账发票单号
        public string invoice_index { get; set; }
        //关联发票时间
        public DateTime invoice_time { get; set; }





    }
}
