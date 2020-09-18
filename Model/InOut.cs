using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class InOut
    {
        /// <summary>
        /// id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 品名id
        /// </summary>
        public int material_id { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        public string material_name { get; set; }

        /// <summary>
        /// 仓位id
        /// </summary>
        public int store_id { get; set; }

        /// <summary>
        /// 仓位名称
        /// </summary>
        public string store_name { get; set; }
        /// <summary>
        /// 品名价格
        /// </summary>
        public double price { get; set; }
        /// <summary>
        /// 品名图纸
        /// </summary>
        public string picture { get; set; }
        /// <summary>
        /// 出入库数量
        /// </summary>
        public double inout_num { get; set; }
        /// <summary>
        /// 出入库单价
        /// </summary>
        public double inout_price { get; set; }
        /// <summary>
        /// 出入库总价
        /// </summary>
        public double inout_all_price { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime create_time { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remarks { get; set; }
        /// <summary>
        /// 入出入库标识，入库1，出库-1
        /// </summary>
        public int flag { get; set; }
    }
}
