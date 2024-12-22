using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Main.ThongKe
{
    public partial class ThongKe : Form
    {
        ProcessDatabase _data;
        public ThongKe()
        {
            _data = new ProcessDatabase();
            InitializeComponent();
        }
        private void ThongKe_Load(object sender, EventArgs e)
        {
            Load_DoanhThu();
            fill_Nam();
        }

    }
}
