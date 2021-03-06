﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class SaleReturn
    {
        //自增id
        public int id { get; set; }

        //公司名称
        public string company_name { get; set; }

        //客户
        public int customer_id { get; set; }
        //订单id
        public int order_id { get; set; }
        //订单号
        public string order_index { get; set; }
        //订单时间
        public DateTime order_time { get; set; }
        //客户订单号
        public string company_order_index { get; set; }
        //订单名称
        public string order_name { get; set; }
        //订单状态(用于出货开单时的判断，当为0时则可以开单，当为1时不允许开单)
        public int order_status { get; set; }

        //序号
        public int seq_id { get; set; }
        //下单数量
        public double order_num { get; set; }

        //需求出货时间
        public DateTime deliver_time { get; set; }
        //单价
        public double order_price { get; set; }
        //总价
        public double order_all_price { get; set; }
        //采购人员
        public string purchase_person { get; set; }
        //需求单位
        public string unit { get; set; }
        //开单数量
        public double open_num { get; set; }
        //图纸
        public string order_picture { get; set; }

        //出货单号
        public string deliver_index { get; set; }
        //出货公司抬头
        public string deliver_company_head { get; set; }
        //实际出货数量
        public double real_num { get; set; }
        //实际出货时间
        public DateTime real_time { get; set; }
        //出货单价
        public double deliver_price { get; set; }
        //出货总价
        public double deliver_all_price { get; set; }

        //退货单号
        public string return_index { get; set; }

        //实际退货数量
        public double return_num { get; set; }
        //实际退货时间
        public DateTime return_time { get; set; }
        //退货单价
        public double return_price { get; set; }
        //退货总价
        public double return_all_price { get; set; }

        //插入时间
        public DateTime insert_time { get; set; }

        //结款与否
        public int money_onoff { get; set; }
        //结款方式
        public int money_way { get; set; }

        //出货单状态
        public int deliver_status { get; set; }

        //退单的状态
        public int return_status { get; set; }
        //实际退货时间
        public DateTime confirm_time { get; set; }

        //备注
        public string remark { get; set; }
    }
}
