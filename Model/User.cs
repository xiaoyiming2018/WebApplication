using System;

namespace Model
{
    public class User
    {
        //自增id
        public int id { get; set; }
        //账户
        public string user_name { get; set; }
        //密码
        public string pass_word { get; set; }
        /// <summary>
        /// 用户权限唯一表示
        /// </summary>
        public int level { get; set; }
        /// <summary>
        /// 用户权限
        /// </summary>
        public string level_name { get; set; }
        /// <summary>
        /// 用户权限
        /// </summary>
        public DateTime createtime { get; set; }
    }
}
