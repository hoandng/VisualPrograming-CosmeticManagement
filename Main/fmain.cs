using Main.HangHoa;
using Main.KhachHang;
using Main.NhanVien;
using Main.HoaDonBan;
using Main.HoaDonNhap;
using Main.ThongKe;
using Main.TaiKhoan;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Main.NhaCungCap;
using Login;
using System.Runtime.CompilerServices;
using Main.Models;
using System.IO;
namespace Main
{
    public partial class fmain : Form
    {
        ProcessDatabase _data;
        private Form activeform = null;
        private Button currentButton;
        private User curr_user;
        public fmain(User user)
        {
            _data = new ProcessDatabase();
            curr_user = new User();
            curr_user.Assign(user);
            InitializeComponent();
        }
        private void fmain_Load(object sender, EventArgs e)
        {
            btn_ThongKe.PerformClick();
            string query = "Select TenNV From [NhanVien] Where MaNV = @manv";
            var parameters = new Dictionary<string, object>
            {
                {"@manv", curr_user.EmployeeID }
            };
            DataTable dt = _data.ExecuteQuery(query, parameters);
            if (dt.Rows.Count > 0)
            {
                string name = dt.Rows[0]["TenNV"].ToString();
                if(name.Trim() != "")
                {
                    lb_TenTK.Text = name;
                }
            }
            else
            {
                lb_TenTK.Text = curr_user.Username;
            }
            lb_TenTK.Left = (panel2.Width - lb_TenTK.Width) / 2;
            //lb_TenTK.Top = (panel2.Height - lb_TenTK.Height) / 2;
            if (curr_user.Picture != "")
            {
                string imagePath = getImagePath(curr_user.Picture);
                if (File.Exists(imagePath))
                {
                    pb_TaiKhoan.ImageLocation = imagePath;
                }
            }
        }

        private string getImagePath(string imageName)
        {
            // Hiển thị ảnh
            string projectDirectory = Directory.GetParent(Application.StartupPath).Parent.FullName;
            string imagesFolder = Path.Combine(projectDirectory, "Images", "Account");
            string imagePath = Path.Combine(imagesFolder, imageName);

            return imagePath;
        }

        //Mở form khi click vào 1 button
        private void openChildForm(Form childForm)
        {
            if (activeform != null)
                activeform.Close();
            activeform = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            pannel_content.Controls.Add(childForm);
            pannel_content.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }
        
        private void ActivateButton(object btnSender)
        {
            if (btnSender != null)
            {
                if (currentButton != (Button)btnSender)
                {
                    DisableButton();
                    Color color = Color.FromArgb(13, 30, 76);
                    currentButton = (Button)btnSender;
                    currentButton.BackColor = color;
                    currentButton.ForeColor = Color.White;
                }
            }
        }
        
        private void DisableButton()
        {
            foreach (Control previousBtn in pannel_menu_btn.Controls)
            {
                if (previousBtn.GetType() == typeof(Button))
                {
                    previousBtn.BackColor = Color.FromArgb(196, 140, 179);
                    previousBtn.ForeColor = Color.FromArgb(13, 30, 76);
                }
            }
        }
        
        private void btn_HangHoa_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            openChildForm(new HangHoa.HangHoa());
        }
        
        private void btn_KhachHang_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            openChildForm(new fCustomer());
        }

        private void btn_NhanVien_Click(object sender, EventArgs e)
        {
            if(curr_user.IsAdmin == 0)
            {
                ActivateButton(sender);
                openChildForm(new NhanVien.NhanVien());
            }
            else
            {
                MessageBox.Show("Cần phải có quyền Admin!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btn_DonBan_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            openChildForm(new fHDB(curr_user));
        }

        private void btn_DonNhap_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            openChildForm(new fHDN());
        }

        private void brn_NhaCC_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            openChildForm(new fNhaCC());
        }

        private void btn_ThongKe_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            openChildForm(new ThongKe.ThongKe());
        }

        private void btn_DangXuat_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Bạn có muốn đăng xuất không", "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                this.Dispose();
                fLogin dangnhap = new fLogin();
                dangnhap.ShowDialog();
            }

        }

        private void fmain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void pb_TaiKhoan_Click(object sender, EventArgs e)
        {
            openChildForm(new TaiKhoan.TaiKhoan(curr_user));
        }

    }
}
