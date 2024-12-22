using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Main.HangHoa
{
    partial class HangHoa : Form
    {
        ProcessDatabase _data = new ProcessDatabase();

        public HangHoa()
        {
            InitializeComponent();
        }

        private void HangHoa_Load(object sender, EventArgs e)
        {
            btn_ChiTiet.Enabled = false;
            btn_HH_Xoa.Enabled = false;
            Load_HangHoa();
            ResetValueTextBox_HangHoa();
        }

    }
}
