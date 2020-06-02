using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Setting
    {
        //自增id
        public int id { get; set; }

        //列表（采购物料单位，分类，出货公司抬头）
        public string config_list { get; set; }
        //开头（订单开头，采购单开头，出货单头）
        //辨识配置信息：1：采购物料单位，2：物品类别，3：订单开头，4：采购单开头,5:出货单头，6：出货公司抬头，7：退货单开头，8：对账单号开头
        //9：开票公司,10:物料名称
        public int all_type { get; set; }
        //正在使用的
        public int in_use { get; set; }

    }
}
