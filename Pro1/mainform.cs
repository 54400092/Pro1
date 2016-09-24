using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//添加
using System.Data.SqlClient;



namespace Pro1
{
    public partial class mainform : Form
    {
        public mainform()
        {
            InitializeComponent();
        }

        Database data = new Database();

        public string str345;

        private void mainform_Load(object sender, EventArgs e)
        {
            this.MaximumSize.Height.CompareTo(this.Height);

            #region 权限判断,哪个选项可用
            if (Login._RCGL == true)
            {
                this.pictureBox8.Enabled = true;
            }
            else
            {
                this.pictureBox8.Enabled = false;
            }

            if (Login._FYGL == true)
            {
                this.pictureBox9.Enabled = true;
            }
            else
            {
                this.pictureBox9.Enabled = false;
            }

            if (Login._KHGL == true)
            {
                this.pictureBox10.Enabled = true;
            }
            else
            {
                this.pictureBox10.Enabled = false;
            }

            if (Login._NBTJ == true)
            {
                this.pictureBox11.Enabled = true;
            }
            else
            {
                this.pictureBox11.Enabled = false;
            }

            if (Login._XTSZ == true)
            {
                this.pictureBox12.Enabled = true;
            }
            else
            {
                this.pictureBox12.Enabled = false;
            }
            #endregion

            this.panel1.Visible = true;

            string sql = "select bianhao as 房源编号,date as 登记日期,zhuangtai as 当前状态,wuye as 物业名称,huxing as 户型结构,mianji as 建筑面积,area as 所在区域,z_floor as 总层数,n_floor as 位于层数,guwen as 置业顾问,yongtu as 物业用途,wuye_type as 物业类别,chengdu as 装修程度,fang_type as 房型,jiancheng as 建成年份,address as 具体地址 from fangyuan order by bianhao";
            DataTable da = data.Query(sql);
            this.dataGridView1.DataSource = da;

            //取出dataGridView1中选中那一行的第一个单元格的值
            str345 = this.dataGridView1.Rows[dataGridView1.SelectedRows[0].Index].Cells[0].Value.ToString();

            MessageBox.Show(str345, "提示信息", MessageBoxButtons.OK);

        }
    }
}
