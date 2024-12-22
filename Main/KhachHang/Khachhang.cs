using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Main.KhachHang
{
    public partial class fCustomer : Form
    {
        ProcessDatabase _database;
        int _mode;
        public fCustomer()
        {
            _database = new ProcessDatabase();
            InitializeComponent();
        }

        private void fCustomer_Load(object sender, EventArgs e)
        {
            Load_KhachHang();
            enableControl(false);
            resetTextBox();
        }
        private void Load_KhachHang()
        {
            string querry = "Select * from [KhachHang]";
            DataTable dt = _database.ExecuteQuery(querry);
            dgv_KhachHang.DataSource = dt;
            dgv_KhachHang.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        private void resetTextBox()
        {
            _mode = 0;
            txt_MaKH.Text = "";
            txt_TenKH.Text = "";
            txt_DC.Text = "";
            txt_SDT.Text = "";
        }
        private void enableControl(bool en)
        {
            txt_TenKH.Enabled = en;
            txt_DC.Enabled = en;
            txt_SDT.Enabled = en;

            btn_Luu.Enabled = en;
            btn_Huy.Enabled = en;
        }
        private void dgv_KhachHang_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (_mode == 0 && e.RowIndex >= 0 && e.RowIndex < dgv_KhachHang.Rows.Count)
            {
                enableControl(false);
                btn_Them.Enabled = true;
                btn_Sua.Enabled = true;
                btn_Xoa.Enabled = true;
                DataGridViewRow row = dgv_KhachHang.Rows[e.RowIndex];
                txt_MaKH.Text = row.Cells["colMaKH"].Value.ToString();
                txt_TenKH.Text = row.Cells["colTenKH"].Value.ToString();
                txt_DC.Text = row.Cells["colDiaChi"].Value.ToString();
                txt_SDT.Text = row.Cells["colSDT"].Value.ToString();
            }
        }

        private string autoGenerateId()
        {
            string query = "SELECT TOP 1 MaKhach FROM [KhachHang] ORDER BY MaKhach DESC";
            object result = _database.ExecuteScalar(query);

            if (result != null && !string.IsNullOrEmpty(result.ToString()))
            {
                string currentMaHang = result.ToString();
                int numericPart = int.Parse(currentMaHang.Substring(2));
                numericPart++;

                return "KH" + numericPart.ToString("D2");
            }
            else
            {
                return "KH01";
            }
        }

        private void btn_Them_Click(object sender, EventArgs e)
        {
            enableControl(true);
            resetTextBox();
            _mode = 1;
            txt_MaKH.Text = autoGenerateId();
            lb_TrangThai.Text = "*Bạn đang ở chế độ Thêm!";
            btn_Them.Enabled = false;
            btn_Sua.Enabled = false;
            btn_Xoa.Enabled = false;
        }

        private void btn_Sua_Click(object sender, EventArgs e)
        {
            enableControl(true);
            _mode = 2;
            lb_TrangThai.Text = "*Bạn đang ở chế độ Sửa!";
            btn_Them.Enabled = false;
            btn_Sua.Enabled = false;
            btn_Xoa.Enabled = false;
        }

        private void btn_Xoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn xóa Khách hàng có mã là " + txt_MaKH.Text +
              " không ? Nếu có ấn nút Lưu, không Thì ấn nút Hủy", "Thông báo!",
               MessageBoxButtons.OK) == DialogResult.OK)
            {
                enableControl(false);
                _mode = 3;
                lb_TrangThai.Text = "*Bạn đang ở chế độ Xóa";
                btn_Them.Enabled = false;
                btn_Sua.Enabled = false;
                btn_Xoa.Enabled = false;
                btn_Luu.Enabled = true;
                btn_Huy.Enabled = true;
            }
        }

        private void btn_Luu_Click(object sender, EventArgs e)
        {
            string sql = "";

            string ma = txt_MaKH.Text;
            string ten = txt_TenKH.Text;
            string diachi = txt_DC.Text;
            string sdt = txt_SDT.Text;
            //Kiểm tra dữ liêu
            if (ma.Trim() == "")
            {
                errKhachHang.SetError(txt_MaKH, "Mã không được để trống");
                return;
            }
            else
            {
                errKhachHang.Clear();
            }

            if (ten.Trim() == "")
            {
                errKhachHang.SetError(txt_TenKH, "Tên không được để trống");
                return;
            }
            else
            {
                errKhachHang.Clear();
            }

            if (sdt.Length != 12)
            {
                errKhachHang.SetError(txt_SDT, "Số điện thoại phải là 10 số");
                return;
            }
            else
            {
                errKhachHang.Clear();
            }

            if (_mode == 1)
            {

                sql = $"Select Count(*) From [KhachHang] Where MaKhach = @makh;";
                var parameters = new Dictionary<string, object>
                {
                    {"@makh", ma},

                };
                int count = Convert.ToInt32(_database.ExecuteScalar(sql, parameters));
                if (count > 0)
                {
                    MessageBox.Show($"Đã tồn tại Khách hàng với mã {ma}", "Thông báo", MessageBoxButtons.OK);
                    return;
                }
                sql = "INSERT INTO [KhachHang] (MaKhach, TenKhach, DiaChi, DienThoai)";
                sql += "VALUES(@makh, @tenkh, @diachi, @dienthoai);";
                parameters = new Dictionary<string, object>
                {
                    {"@makh", ma},
                    {"@tenkh", ten},
                    {"@diachi", diachi},
                    {"@dienthoai",sdt }
                };
                _database.ExecuteNonQuery(sql, parameters);
            }


            //Nếu nút Sửa enable TNSXì thực hiện cập nhật dữ liệu
            if (_mode == 2)
            {
                sql = "Update [KhachHang] SET ";
                sql += "TenKhach = @tenkh, DiaChi = @diachi, DienThoai = @sdt ";
                sql += "WHERE MaKhach = @makh";
                var parameters = new Dictionary<string, object>
                {
                    {"@makh", ma},
                    {"@tenkh", ten},
                    {"@diachi", diachi},
                    {"@sdt",sdt }
                };
                _database.ExecuteNonQuery(sql, parameters);
            }

            //Nếu nút Xóa enable thì thực hiện xóa dữ liệu
            if (_mode == 3)
            {
                sql = "Delete From [KhachHang] Where MaKhach = @makh";
                var parameters = new Dictionary<string, object>
                {
                    {"@makh", ma},
                };
                _database.ExecuteNonQuery(sql, parameters);
            }

            Load_KhachHang();

            resetTextBox();
            enableControl(false);
            lb_TrangThai.Text = "";
            btn_Them.Enabled = true;
            btn_Xoa.Enabled = false;
            btn_Sua.Enabled = false;
        }

        private void btn_Huy_Click(object sender, EventArgs e)
        {
            resetTextBox();
            enableControl(false);
            lb_TrangThai.Text = "";
            btn_Xoa.Enabled = false;
            btn_Sua.Enabled = false;
            btn_Them.Enabled = true;
            errKhachHang.Clear();
        }
    }
}
