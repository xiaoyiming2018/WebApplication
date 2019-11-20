using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Purchase
    {
        //自增id
        public int id { get; set; }

        //供应商
        public int supplier_id { get; set; }
        //公司名称
        public string company_name { get; set; }

        //订单号
        public string purchase_index { get; set; }
        //采购时间
        public DateTime purchase_time { get; set; }
        //分类
        public string category { get; set; }
        //物料名称
        public string material_name { get; set; }
        //物料规格
        public string material_spec { get; set; }
        //物料数量
        public int material_num { get; set; }
        //物料单位
        public string material_unit { get; set; }
        //单价
        public double material_price { get; set; }
        //总价
        public double material_all_price { get; set; }
        //送货单号
        public string deliver_index { get; set; }
        //送货时间
        public DateTime deliver_time { get; set; }
        //结款与否
        public int money_onoff { get; set; }
        //结款方式
        public int money_way { get; set; }
        //备注——用于对账
        public int status { get; set; }

        //确认时间
        public DateTime confirm_time { get; set; }


    }
}
