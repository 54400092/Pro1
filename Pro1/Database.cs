using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/****新添加****/

//数据库的操作
using System.Data;
//对sql server数据库的操作
using System.Data.SqlClient;



namespace Pro1
{
    public class Database
    {
        public SqlConnection getConnection()
        {
            //返回连接对象,
            return new SqlConnection("server=.;uid=sa;pwd=123456;database=build");
        }


        SqlConnection con = null;
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>返回一个具体的值</returns>
        public object QueryScalar(string sql)
        {
            //打开数据连接
            Open();

            //执行命令
            object result = null;
            try
            {
                //using此处使用:cmd对象用完后,自动释放该对象,回收内存
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    result = cmd.ExecuteScalar();
                    return result;
                }
            }
            catch
            {
                return null;
            }

        }

        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql">要执行的sql语句</param>
        /// <param name="prams">参数</param>
        /// <returns></returns>
        public object QueryScalar(string sql, SqlParameter[] prams)
        {
            object result = null;
            try
            {
                //using此处使用:cmd对象用完后,自动释放该对象,回收内存
                using (SqlCommand cmd = CreateCommandSql(sql, prams))
                {
                    result = cmd.ExecuteScalar();
                    return result;
                }
            }
            catch
            {
                return null;
            }

        }

        /// <summary>
        /// 创建一个Sqlcommand对象,用来构建SQL语句
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="prams">SQL语句所用到的参数</param>
        /// <returns></returns>
        public SqlCommand CreateCommandSql(string sql, SqlParameter[] prams)
        {
            Open();
            SqlCommand cmd = new SqlCommand(sql, con);
            if (prams != null)
            {
                foreach (SqlParameter parameter in prams)
                {
                    cmd.Parameters.Add(parameter);
                }
            }
            return cmd;
        }

        private void Open()
        {
            if (con == null)
            {
                con = new SqlConnection("server=.;uid=sa;pwd=123456;database=build");
                Console.WriteLine("debug:数据库已打开");  
            }

            if (con.State == ConnectionState.Closed)
            {
                con.Open();

            }
        }

        /// <summary>
        /// 执行SQL语句,该方法返回一个DataTable(内存中的一张表)
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>DataTable</returns>
        public DataTable Query(string sql)
        {
            Open();
            using (SqlDataAdapter sqlda = new SqlDataAdapter(sql, con))
            {
                using (DataTable dt = new DataTable())
                {
                    sqlda.Fill(dt);
                    return dt;

                }
            }
        }

        /// <summary>
        /// 执行SQL语句,返回DataTable
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="prams">SQL语句所用到的参数</param>
        /// <returns></returns>
        public DataTable Query(string sql, SqlParameter[] prams)
        {
            SqlCommand cmd = CreateCommandSql(sql, prams);
            using (SqlDataAdapter sqldata = new SqlDataAdapter(cmd))
            {
                using (DataTable dt = new DataTable())
                {
                    sqldata.Fill(dt);
                    return dt;
                }
            }
        }

        /// <summary>
        /// 执行SQL语句,返回影响的记录行数
        /// </summary>
        /// <param name="sql">要执行的SQL语句</param>
        /// <returns></returns>
        public int RunSql(string sql)
        {
            int result = -1;
            try
            {
                Open();
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    result = cmd.ExecuteNonQuery();
                    con.Close();
                    return result;
                }

            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 执行SQL语句,返回影响的记录行数
        /// </summary>
        /// <param name="sql">要执行的SQL语句</param>
        /// <param name="prams">SQL语句所用到的参数</param>
        /// <returns></returns>
        public int RunSql(string sql, SqlParameter[] prams)
        {
            int result = -1;
            try
            {
                SqlCommand cmd = CreateCommandSql(sql, prams);
                result = cmd.ExecuteNonQuery();
                this.Close();
                return result;
            }
            catch
            {
                return 0;

            }

        }

        public void Close()
        {
            if (con != null)
            {
                con.Close();
                Console.WriteLine("debug:数据库已关闭"); 
            }
        }

        /// <summary>
        /// 执行SQL语句,返回一个SqlDataReader
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dataReader"></param>
        public void RunSql(string sql, out SqlDataReader dataReader)
        {
            SqlCommand cmd = CreateCommandSql(sql, null);
            dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public void RunSql(string sql, SqlParameter[] parms, out SqlDataReader dataReader)
        {
            SqlCommand cmd = CreateCommandSql(sql, parms);
            //当关闭SqlDataReader时,同时关闭con
            dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

        //存储语句都能封装为存储过程,存储过程安全性比较高,效率快
        //以下都是存储过程函数,但本项目并没有用

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procName">存储过程的名称</param>
        /// <returns></returns>
        public int RunProc(string procName)
        {
            SqlCommand cmd = CreateCommand(procName, null);
            cmd.ExecuteNonQuery();
            this.Close();
            return (int)cmd.Parameters["ReturnValue"].Value;

        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procName">存储过程的名称</param>
        /// <param name="prams">构造存储过程所需要的参数</param>
        /// <returns></returns>
        public int RunProc(string procName, SqlParameter[] prams)
        {
            SqlCommand cmd = CreateCommand(procName, prams);
            cmd.ExecuteNonQuery();
            this.Close();
            return (int)cmd.Parameters["ReturnValue"].Value;
        }

        /// <summary>
        /// 执行存储过程,返回SqlDataReader
        /// </summary>
        /// <param name="procName">存储过程的名称</param>
        /// <param name="dataReader">要返回的SqlDataReader</param>
        public void RunProc(string procName, out SqlDataReader dataReader)
        {
            SqlCommand cmd = CreateCommand(procName, null);
            dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

        /// <summary>
        /// 执行存储过程,返回SqlDataReader
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="dataReader"></param>
        public void RunProc(string procName, SqlParameter[] prams, out SqlDataReader dataReader)
        {
            SqlCommand cmd = CreateCommand(procName, prams);
            dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }


        /// <summary>
        /// 创建一个SqlCommand对象,用来执行存储过程
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="prams">构建存储过程多需要的参数</param>
        /// <returns></returns>
        public SqlCommand CreateCommand(string procName, SqlParameter[] prams)
        {
            Open();
            SqlCommand cmd = new SqlCommand(procName, con);
            //存储过程
            cmd.CommandType = CommandType.StoredProcedure;
            if (prams != null)
                foreach (SqlParameter parameter in prams)
                {
                    cmd.Parameters.Add(parameter);
                }
            cmd.Parameters.Add(new SqlParameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.ReturnValue, false, 0, 0, string.Empty, DataRowVersion.Default, null));
            return cmd;
        }

        /// <summary>
        /// 对DateTime型数据做限制
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public SqlParameter MakeInParamDate(string paramName, SqlDbType dbType, int size, DateTime value)
        {
            if (value.ToShortDateString() == "0001-1-1")
            {
                return MakeParam(paramName, dbType, size, ParameterDirection.Input, System.DBNull.Value);
            }
            else
            {
                return MakeParam(paramName, dbType, size, ParameterDirection.Input, value);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        /// <param name="direction"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public SqlParameter MakeParam(string paramName, SqlDbType dbType, int size, ParameterDirection direction, object value)
        {

            SqlParameter param;
            if (size > 0)
            {
                param = new SqlParameter(paramName, dbType, size);
            }
            else
            {
                param = new SqlParameter(paramName, dbType);
            }
            param.Direction = direction;

            if (!(direction == ParameterDirection.Output && value == null))
            {
                param.Value = value;
            }
            return param;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public SqlParameter MakeInParamStr(string paramName, SqlDbType dbType, int size, string value)
        {
            if (value == null)
            {
                return MakeParam(paramName, dbType, size, ParameterDirection.Input, System.DBNull.Value);
            }
            else
            {
                return MakeParam(paramName, dbType, size, ParameterDirection.Input, value);
            }
        }

        /// <summary>
        /// 对int,float数据的限制
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public SqlParameter MakeInParamIntF(string paramName, SqlDbType dbType, int size, object value)
        {
            if (float.Parse(value.ToString()) == 0)
            {
                return MakeParam(paramName, dbType, size, ParameterDirection.Input, System.DBNull.Value);
            }
            else
            {
                return MakeParam(paramName, dbType, size, ParameterDirection.Input, value);
            }
        }

        public SqlParameter MakeInParam(string paramName, SqlDbType dbType, object value)
        {
            return MakeParam(paramName, dbType, 0, ParameterDirection.Input, value);
        }

        public SqlParameter MakeInParam(string paramName, SqlDbType dbType, int size, object value)
        {
            return MakeParam(paramName, dbType, size, ParameterDirection.Input, value);
        }

        public SqlParameter MakeOutParam(string paramName, SqlDbType dbType, int size)
        {
            return MakeParam(paramName, dbType, size, ParameterDirection.Output, null);
        }
        public SqlParameter MakeReturnParam(string paramName, SqlDbType dbType, int size)
        {
            return MakeParam(paramName, dbType, size, ParameterDirection.ReturnValue, null);
        }


    }
}
