using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

//新增
using System.Data.SqlClient;

namespace Pro1
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        bool t = false;//判断第一次登陆还是切换不通的用户登陆
        public static string _UserName;
        public static string _Pass;
        public static bool _RCGL;//日常管理权限
        public static bool _FYGL;//房源管理权限
        public static bool _KHGL;//客户管理权限
        public static bool _NBTJ;//内部统计权限
        public static bool _XTSZ;//系统设置权限
        Database data = new Database();
        Dl dl = new Dl();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bb"></param>
        public Login(bool bb)
        {
            t = bb;
        }

        private void Login_Load(object sender, EventArgs e)
        {
            try
            {
                string sql = "select * from login_user";
                DataTable dd = data.Query(sql);
                this.comboBox1.DataSource = dd;
                this.comboBox1.DisplayMember = "_name";


            }
            catch
            {
            }
            finally
            {
                data.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                SysHandleInfo syshandleInfo = new SysHandleInfo();
                bool b = false;
                int jibie_Id = 0;
                string sql = "select * from login_user";
                DataTable dt = data.Query(sql);
                foreach (DataRow dr in dt.Rows)
                {
                    if (this.comboBox1.Text == dr["_name"].ToString() && this.textBox1.Text == dr["_pass"].ToString())
                    {
                        b = true;
                        _UserName = dr["_name"].ToString();
                        _Pass = dr["_pass"].ToString();
                        jibie_Id = Convert.ToInt32(dr["_jibieid"]);
                        string sql_id = "select * from jibie where id = '" + jibie_Id + "'";
                        DataTable dt1 = data.Query(sql_id);
                        foreach (DataRow dr1 in dt1.Rows)
                        {
                            _RCGL = Convert.ToBoolean(dr1["_richang"]);
                            _FYGL = Convert.ToBoolean(dr1["_fangyuan"]);
                            _KHGL = Convert.ToBoolean(dr1["_kehu"]);
                            _NBTJ = Convert.ToBoolean(dr1["_neibu"]);
                            _XTSZ = Convert.ToBoolean(dr1["_xitong"]);

                        }

                    }

                }

                if (b == true)
                {
                    syshandleInfo.HandlePerson = this.comboBox1.Text;
                    syshandleInfo.HandleContent = "操作员登陆";
                    syshandleInfo.HandleRemark = this.comboBox1.Text + "登陆系统";
                    dl.InsertHandle(syshandleInfo);
                    if (t != true)//第一次登陆
                    {
                        mainform main = new mainform();
                        main.Show();
                        this.Visible = false;
                    }
                    else//登陆后切换用户
                    {
                        this.Visible = false;
                    }
                }
                else
                {
                    MessageBox.Show("用户名或密码错误!","提示信息",MessageBoxButtons.OK);
                }

            }
            catch(Exception ex)
            {

                MessageBox .Show(ex.ToString())  ;
            }
        }
	}

   
}
