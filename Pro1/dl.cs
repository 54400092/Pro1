using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//新增
using System.Data;
using System.Data.SqlClient;

namespace Pro1
{
    public class Dl
    {
        Database data = new Database();
        public int InsertHandle(SysHandleInfo syshandleInfo)
        {
            string date = DateTime.Now.ToString();
            SqlParameter[] para = {
                                      data.MakeInParam("@HandlePerson", SqlDbType.VarChar, syshandleInfo.HandlePerson), 
                                      data.MakeInParam("@HandleContent", SqlDbType.VarChar, syshandleInfo.HandleContent), 
                                      data.MakeInParam("@HandleRemark", SqlDbType.VarChar, syshandleInfo.HandleRemark), 
                                      data.MakeInParam("@date", SqlDbType.DateTime, date),
                                  };
            string strsql = "insert into rizhi(time,person,concent,remark) values(@date,@HandlePerson,@HandleContent,@HandleRemark)";
            return data.RunSql(strsql,para);



        }
    }
}
