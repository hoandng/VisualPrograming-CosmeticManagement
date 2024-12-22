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

namespace Main.NhaCungCap
{
    public partial class fNhaCC : Form
    {
        ProcessDatabase _data;
        int _mode = 0;
        public fNhaCC()
        {
            _data = new ProcessDatabase();
            InitializeComponent();
        }
        private void fNhaCC_Load(object sender, EventArgs e)
        {
            Load_NCC();
            resetTextBox();
            enableControls(false);
            btn_Sua.Enabled = false;
            btn_Xoa.Enabled = false;
        }

        private void Load_NCC()
        {
            string querry = "Select * from [NhaCungCap]";
            DataTable dataTable = _data.ExecuteQuery(querry);
            dgv_NhaCC.DataSource = dataTable;
            
            dgv_NhaCC.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void resetTextBox()
        {
            _mode = 0;
            txt_MaNCC.Text = "";
            txt_TenNCC.Text = "";
            txt_SDT.Text = "";
            txt_DiaChi.Text = "";
        }

        private void enableControls(bool enable)
        {
            txt_SDT.Enabled = enable;
            txt_TenNCC.Enabled = enable;
            txt_DiaChi.Enabled = enable;
            
            btn_Luu.Enabled = enable;
            btn_Huy.Enabled = enable;
        }

        private void dgv_NhaCC_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (_mode == 0 && e.RowIndex >= 0 && e.RowIndex < dgv_NhaCC.Rows.Count)
            {
                enableControls(false);
                btn_Them.Enabled = true;
                btn_Sua.Enabled = true;
                btn_Xoa.Enabled = true;
                DataGridViewRow row = dgv_NhaCC.Rows[e.RowIndex];
                txt_MaNCC.Text = row.Cells["MaNCC"].Value.ToString();
                txt_TenNCC.Text = row.Cells["TenNCC"].Value.ToString();
                txt_DiaChi.Text = row.Cells["DiaChi"].Value.ToString();
                txt_SDT.Text = row.Cells["DienThoai"].Value.ToString();
            }
        }

        private string autoGenerateId()
        {
            string query = "SELECT TOP 1 MaNCC FROM [NhaCungCap] ORDER BY MaNCC DESC";
            object result = _data.ExecuteScalar(query);

            if (result != null && !string.IsNullOrEmpty(result.ToString()))
            {
                string currentMaHang = result.ToString();
                int numericPart = int.Parse(currentMaHang.Substring(3));
                numericPart++;

                return "NCC" + numericPart.ToString("D2");
            }
            else
            {
                return "NCC01";
            }
        }

        private void btn_Them_Click(object sender, EventArgs e)
        {
            enableControls(true);
            resetTextBox();
            txt_MaNCC.Text = autoGenerateId();
            _mode = 1;
            lb_TrangThai.Text = "*Bạn đang ở chế độ Thêm!";
            btn_Them.Enabled = false;
            btn_Sua.Enabled = false;
            btn_Xoa.Enabled = false;
        }

        private void btn_Sua_Click(object sender, EventArgs e)
        {
            enableControls(true);
            _mode = 2;
            lb_TrangThai.Text = "*Bạn đang ở chế độ Sửa!";
            btn_Them.Enabled = false;
            btn_Sua.Enabled = false;
            btn_Xoa.Enabled = false;
        }

        private void btn_Xoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn xóa Nhà cung cấp có mã là " + txt_MaNCC.Text +
             " không ? Nếu có ấn nút Lưu, không Thì ấn nút Hủy", "Thông báo!",
              MessageBoxButtons.OK) == DialogResult.OK)
            {
                enableControls(false);
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

            string ma = txt_MaNCC.Text;
            string ten = txt_TenNCC.Text;
            string diachi = txt_DiaChi.Text;
            string sdt = txt_SDT.Text;
            //Kiểm tra dữ liêu
            if (ma.Trim() == "")
            {
                errNCC.SetError(txt_MaNCC, "Mã không được để trống");
                return;
            }
            else
            {
                errNCC.Clear();
            }

            if (ten.Trim() == "")
            {
                errNCC.SetError(txt_TenNCC, "Tên không được để trống");
                return;
            }
            else
            {
                errNCC.Clear();
            }

            if (_mode == 1)
            {
                sql = "INSERT INTO [NhaCungCap] (MaNCC, TenNCC, DiaChi, DienThoai)";
                sql += $"VALUES(@ma, @ten, @diachi, @dienthoai);";
                var parameters = new Dictionary<string, object>
                {
                    {"@ma", ma},
                    {"@ten", ten},
                    {"@diachi", diachi},
                    {"@dienthoai",sdt }
                };
                _data.ExecuteNonQuery(sql, parameters);
            }


            //Nếu nút Sửa enable TNSXì thực hiện cập nhật dữ liệu
            if (_mode == 2)
            {
                sql = "Update [NhaCungCap] SET ";
                sql += $"TenNCC = @ten, DiaChi = @diachi, DienThoai = @dienthoai ";
                sql += $"WHERE MaNCC = @ma";
                var parameters = new Dictionary<string, object>
                {
                    {"@ma", ma},
                    {"@ten", ten},
                    {"@diachi", diachi},
                    {"@dienthoai",sdt }
                };
                _data.ExecuteNonQuery(sql, parameters);
            }

            //Nếu nút Xóa enable thì thực hiện xóa dữ liệu
            if (_mode == 3)
            {
                sql = $"Delete From [NhaCungCap] Where MaNCC = @ma";
                var parameters = new Dictionary<string, object>
                {
                    {"@ma", ma},
                };
                _data.ExecuteNonQuery(sql, parameters);
            }

            Load_NCC();

            resetTextBox();
            enableControls(false);
            lb_TrangThai.Text = "";
            btn_Them.Enabled = true;
            btn_Xoa.Enabled = false;
            btn_Sua.Enabled = false;
        }

        private void btn_Huy_Click(object sender, EventArgs e)
        {
            resetTextBox();
            enableControls(false);
            errNCC.Clear();
            lb_TrangThai.Text = "";
            btn_Them.Enabled = true;
            btn_Xoa.Enabled = false;
            btn_Sua.Enabled = false;
        }
    }
}
