using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Store
    {
        /// <summary>
        /// id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 仓位名称
        /// </summary>
        public string store_name { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime create_time { get; set; }
    }
}
