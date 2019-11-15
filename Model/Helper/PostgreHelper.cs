using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text;
using Npgsql;

namespace Model.Helper
{
    /// <summary>
    /// 数据库连接属性
    /// </summary>
    public class DbConnectionString
    {
        public string DbType { set; get; }
        public string Database { set; get; }
        public string Port { set; get; }
        public string Host { set; get; }
        public string UserName { set; get; }
        public string Password { set; get; }
    }

    public abstract class PostgreBase
    {
        public static string connString = "";

        //private static readonly string connString = "Server=127.0.0.1;Port=5432;Database=postgres;User Id=postgres;Password=Postgre@sql1";


        #region 基础操作
        /// <summary>
        /// 执行增删改方法
        /// </summary>
        /// <param name="sql">执行的sql语句</param>
        /// <returns>0的执行失败</returns>
        public static int ExecuteNonQuery(string sql)
        {
            NpgsqlConnection SqlConn = new NpgsqlConnection(connString);

            NpgsqlCommand SqlCommand = new NpgsqlCommand(sql, SqlConn);
            try
            {
                SqlConn.Open();
                int count = SqlCommand.ExecuteNonQuery();  //执行查询并返回受影响的行数
                SqlConn.Close();
                return count;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ExecuteNonQuery error=" + sql + ex.Message);
            }
            finally
            {
                SqlConn.Close();
            }

            return 0;
        }

        /// <summary>
        /// 执行查询方法
        /// </summary>
        /// <param name="sql">执行的sql语句</param>
        /// <returns>返回DbDataReader</returns>
        public static DbDataReader GetReader(string sql)
        {
            NpgsqlConnection SqlConn = new NpgsqlConnection(connString);

            NpgsqlCommand SqlCommand = new NpgsqlCommand(sql, SqlConn);
            try
            {
                SqlConn.Open();
                DbDataReader reader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                return reader;
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetReader error=" + sql + ex.Message);
                SqlConn.Close();
                return null;
            }

        }
        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列
        /// </summary>
        /// <param name="sql">执行的sql</param>
        /// <returns>如果为空，则返回DBNull.Value</returns>
        public static object ExecuteScalar(string sql)
        {
            NpgsqlConnection SqlConn = new NpgsqlConnection(connString);

            NpgsqlCommand SqlCommand = new NpgsqlCommand(sql, SqlConn);
            try
            {
                SqlConn.Open();
                object s = SqlCommand.ExecuteScalar();
                SqlConn.Close();
                return s;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ExecuteScalar error=" + sql + ex.Message);
            }
            finally
            {
                SqlConn.Close();
            }
            return null;
        }
        #endregion
    }
    /// <summary>
    /// 数据库操作基类(for PostgreSQL)
    /// </summary>
    public class PostgreHelper : PostgreBase
    {
        private static object insertLockObj = new object();
        private static object updateLockObj = new object();
        private static object getSingleLockObj = new object();
        private static object getListLockObj = new object();
        /// <summary>
        /// 将对象实体插入数据库
        /// </summary>
        /// <param name="cmdText">命令语句</param>
        /// <param name="Obj">对象</param>
        /// <param name="RetId">=true则返回插入的id</param>
        /// <returns>影响的行数，或者id</returns> 
        public static int InsertSingleEntity<T>(string TableName, T Obj, bool RetId = false) where T : new()
        {
            //lock (insertLockObj)
            //{
            string CommandString;
            string Keys = string.Empty;
            string Values = string.Empty;
            object fieldValue = null;
            Type type = Obj.GetType();
            PropertyInfo[] properties = type.GetProperties();//BindingFlags.Instance | BindingFlags.Public

            foreach (PropertyInfo field in properties)//反射类型与值并拼接命令字
            {
                if (field.Name.Equals("id")) continue;//id为自动生成，忽略
                fieldValue = field.GetValue(Obj, null);
                if (fieldValue == null) continue;

                try
                {
                    if (Values.Length > 0)
                    {
                        Values += ",";
                        Keys += ",";
                    }

                    Values += "'" + fieldValue.ToString().Trim() + "'";
                    Keys += field.Name;
                }
                catch (Exception ex) { string s = ex.Message; }
            }
            CommandString = string.Format("Insert into {0}({1}) Values({2})", TableName, Keys, Values);
            if (RetId)
            {
                CommandString += " returning id";//返回id
                DbDataReader reader = GetReader(CommandString);
                if (reader != null)
                {
                    reader.Read();
                    int id = Convert.ToInt32(reader["id"]);//返回id
                    reader.Close();
                    return id;
                }
            }
            else
            {
                int count = ExecuteNonQuery(CommandString);//返回数量
                return count;
            }
            return -1;//返回error
            //}
        }
        /// <summary>
        /// 更新实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="TableName">表名称</param>
        /// <param name="id">主键id值</param>
        /// <param name="Obj">命令对象</param>
        /// <returns></returns>
        public static int UpdateSingleEntity<T>(string TableName, int id, T Obj) where T : new()
        {
            lock (updateLockObj)
            {
                string CommandString;
                string KeysAndValues = string.Empty;

                Type type = Obj.GetType();
                PropertyInfo[] properties = type.GetProperties();//BindingFlags.Instance | BindingFlags.Public
                foreach (PropertyInfo field in properties)//反射类型与值并拼接命令字
                {
                    if (field.GetValue(Obj, null) == null) continue;
                    if (field.Name.Trim().ToUpper().Equals("ID")) continue;//id字段过滤
                    if (KeysAndValues.Length > 0) KeysAndValues = KeysAndValues + ",";
                    KeysAndValues += string.Format("{0}='{1}'", field.Name, field.GetValue(Obj, null).ToString());
                }

                CommandString = string.Format("Update {0} Set {1} where id={2}", TableName, KeysAndValues, id);//update tabe1 set username='youname' where userid=5

                return ExecuteNonQuery(CommandString);
            }
        }
        /// <summary>
        /// 更新实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="TableName">表名称</param>
        /// <param name="id">主键id值</param>
        /// <param name="UpdateDic">跟新的键值对字典</param>
        /// <returns></returns>
        public static int UpdateSingleEntity<T>(string TableName, int id, Dictionary<string, object> UpdateDic) where T : new()
        {
            //lock (updateLockObj)
            //{
            string CommandString;
            string KeysAndValues = string.Empty;

            foreach (var item in UpdateDic)//反射类型与值并拼接命令字
            {
                if (KeysAndValues.Length > 0) KeysAndValues = KeysAndValues + ",";
                KeysAndValues += string.Format("{0}='{1}'", item.Key, item.Value);
            }

            CommandString = string.Format("Update {0} Set {1} where id={2}", TableName, KeysAndValues, id);//update tabe1 set username='youname' where userid=5

            return ExecuteNonQuery(CommandString);
            //}
        }
        /// <summary>
        /// 获取一个对象后返回
        /// </summary>
        /// <param name="cmdText">命令语句</param>
        /// <returns>T对象</returns> 
        public static T GetSingleEntity<T>(string cmdText) where T : new()
        {
            lock (getSingleLockObj)
            {
                DbDataReader mySqlDataReader = PostgreHelper.GetReader(cmdText);
                List<T> list = ReadEntityListByReader<T>(mySqlDataReader);
                if (list.Count > 0)
                {
                    return list[0];
                }
                return default(T);
            }
        }

        /// <summary>
        /// 读取队列后返回
        /// </summary>
        /// <param name="cmdText">命令语句</param>
        /// <returns>list对象</returns> 
        public static List<T> GetEntityList<T>(string cmdText) where T : new()
        {
            //lock (getListLockObj)
            //{
            DbDataReader mySqlDataReader = PostgreHelper.GetReader(cmdText);
            return ReadEntityListByReader<T>(mySqlDataReader);
            //}
        }
        /// <summary>
        /// Read entity list by reader
        /// </summary>
        /// <typeparam name="T">entity</typeparam>
        /// <param name="reader">data reader</param>
        /// <returns>entity</returns>
        private static List<T> ReadEntityListByReader<T>(DbDataReader reader) where T : new()
        {
            List<T> listT = new List<T>();
            List<string> errProperties = new List<string>();
            PropertyInfo[] propertyInfos = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            if (reader != null)
            {
                using (reader)
                {
                    while (reader.Read())
                    {
                        T inst = new T();
                        foreach (var pi in propertyInfos)
                        {
                            var objValue = new object();
                            try
                            {
                                if (errProperties.Count > 0 && errProperties.Contains(pi.Name))
                                {
                                    continue;//报错的属性忽略，直接跳过
                                }
                                objValue = reader[pi.Name];
                            }
                            catch (Exception ex)
                            {
                                errProperties.Add(pi.Name);//记录报错的属性
                                continue;
                            }

                            if (objValue == DBNull.Value || objValue == null)
                                continue;
                            //var si = pi.GetSetMethod();
                            //if (si == null)
                            //    continue;
                            pi.SetValue(inst, objValue, null);
                        }
                        listT.Add(inst);
                    }
                    reader.Close();
                }
            }
            return listT;
        }
    }
}
