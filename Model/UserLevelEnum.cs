using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public enum UserLevelEnum : int
    {
        /// <summary>
        /// 普通员工
        /// </summary>
        Operator = 0,
        /// <summary>
        /// 生管
        /// </summary>
        ProductManager = 1,
        /// <summary>
        /// 老板
        /// </summary>
        Boss = 2,
        /// <summary>
        /// 系统后台人员
        /// </summary>
        Developer = 3
    }
}
