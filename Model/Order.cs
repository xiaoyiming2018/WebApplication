using System;

namespace Model
{
    public class Order
    {
        //自增id
        public int id { get; set; }

        //客户
        public int customer_id { get; set; }
        //公司名称
        public string company_name { get; set; }        
        //订单号
        public string order_index { get; set; }
        //客户订单号
        public string company_order_index { get; set; }
        //
        public int order_id { get; set; }
        //序号
        public int seq_id { get; set; }
        //订单名称
        public string order_name { get; set; }
        //下单数量
        public double order_num { get; set; }
        //下单时间(登记时间)
        public DateTime order_time { get; set; }
        //需求出货时间
        public DateTime deliver_time { get; set; }
        //单价
        public double order_price { get; set; }
        //总价
        public double order_all_price { get; set; }
        //出货总价
        public double deliver_all_price { get; set; }
        //采购人员
        public string purchase_person { get; set; }
        //单位
        public string unit { get; set; }
        //开单数量
        public double open_num { get; set; }
        //退单数量
        public double tui_num { get; set; }
        //剩余数量
        public double remain_num { get; set; }
        //图纸
        public string order_picture { get; set; }
        //插入时间
        public DateTime insert_time { get; set; }

        //real_num(用于判断是否加入到历史订单中)
        public double real_num { get; set; }
        //订单状态
        public int order_status { get; set; }
        //订单是否已开票
        public int invoice_status { get; set; }

    }
}