using DocumentFormat.OpenXml.Presentation;
using Main;
using Main.Models;
using Main.TaiKhoan;
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
using System.Xml.Linq;

namespace Login
{
    public partial class fLogin : Form
    {
        ProcessDatabase _data;
        User _user;
        public fLogin()
        {
            _user = new User();
            _data = new ProcessDatabase();
            InitializeComponent();
        }


        private void fLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                btn_submit.PerformClick();
            }
        }

        private void btn_submit_Click(object sender, EventArgs e)
        {
            string query = "SELECT * FROM [TaiKhoan] WHERE Username = @username AND Password = @password";


            var parameters = new Dictionary<string, object>
            {
               {"@username", txt_username.Text},
               {"@password", txt_password.Text}
            };

            DataTable dtTaiKhoan = _data.ExecuteQuery(query, parameters);

            if (dtTaiKhoan.Rows.Count > 0)
            {
                _user.Username = dtTaiKhoan.Rows[0]["Username"].ToString();
                _user.Password = dtTaiKhoan.Rows[0]["Password"].ToString();
                _user.IsAdmin = int.Parse(dtTaiKhoan.Rows[0]["isAdmin"].ToString());
                _user.Picture = dtTaiKhoan.Rows[0]["Anh"].ToString();
                _user.EmployeeID = dtTaiKhoan.Rows[0]["MaNV"].ToString();
                this.Hide();
                fmain main = new fmain(_user);
                main.Show();
            }
            else
            {
                MessageBox.Show("Tài khoản hoặc mật khẩu không đúng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn chắc chăn muốn thoát không?", "Thông báo!", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Application.Exit();
            }
        }
    }
}
