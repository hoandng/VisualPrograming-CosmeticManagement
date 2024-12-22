using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Main.HangHoa
{
    partial class HangHoa : Form
    {
        private void Load_KhoiLuong()
        {
            string query = "Select * from [KhoiLuong]";
            DataTable dt = _data.ExecuteQuery(query);
            dtg_KhoiLuong.DataSource = dt;
            dtg_KhoiLuong.Columns[0].HeaderText = "Mã Khối lượng";
            dtg_KhoiLuong.Columns[1].HeaderText = "Tên Khối lượng";

            dtg_KhoiLuong.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dt.Dispose();
        }
        
        private void Enable_KhoiLuong(Boolean hien)
        {
            txt_TKL.Enabled = hien;
            btn_KL_Luu.Enabled = hien;
            btn_KL_Huy.Enabled = hien;
        }
        
        private void ResetValueTextBox_KhoiLuong()
        {
            _mode = 0;
            txt_MKL.Text = "";
            txt_TKL.Text = "";
            lb_KL_TrangThai.Text = "";
        }

        private void tp_KhoiLuong_Enter(object sender, EventArgs e)
        {
            Load_KhoiLuong();
            Enable_KhoiLuong(false);
            ResetValueTextBox_KhoiLuong();
            lb_KL_TrangThai.Text = "";
            btn_KL_Them.Enabled = true;
            btn_KL_Sua.Enabled = false;
            btn_KL_Xoa.Enabled = false;
        }

        private void dtg_KhoiLuong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (_mode == 0 && e.RowIndex >= 0 && e.RowIndex < dtg_KhoiLuong.Rows.Count)
            {
                btn_KL_Them.Enabled = true;
                btn_KL_Sua.Enabled = true;
                btn_KL_Xoa.Enabled = true;
                Enable_ChatLieu(false);
                DataGridViewRow row = dtg_KhoiLuong.Rows[e.RowIndex];
                txt_MKL.Text = row.Cells["MaKhoiLuong"].Value.ToString();
                txt_TKL.Text = row.Cells["TenKhoiLuong"].Value.ToString();
            }
        }

        private string autoGenerateId_KL()
        {
            string query = "SELECT TOP 1 MaKhoiLuong FROM [KhoiLuong] ORDER BY MaKhoiLuong DESC";
            object result = _data.ExecuteScalar(query);

            if (result != null && !string.IsNullOrEmpty(result.ToString()))
            {
                string currentMaHang = result.ToString();
                int numericPart = int.Parse(currentMaHang.Substring(2));
                numericPart++;

                return "KL" + numericPart.ToString("D2");
            }
            else
            {
                return "KL01";
            }
        }

        private void btn_KL_Them_Click(object sender, EventArgs e)
        {
            Enable_KhoiLuong(true);
            ResetValueTextBox_KhoiLuong();
            _mode = 1;
            lb_KL_TrangThai.Text = "*Bạn đang ở chế độ Thêm";
            txt_MKL.Text = autoGenerateId_KL();
            btn_KL_Sua.Enabled = false;
            btn_KL_Xoa.Enabled = false;
            btn_KL_Them.Enabled = false;
        }

        private void btn_KL_Sua_Click(object sender, EventArgs e)
        {
            Enable_KhoiLuong(true);
            _mode = 2;
            lb_KL_TrangThai.Text = "*Bạn đang ở chế độ Sửa";
            btn_KL_Sua.Enabled = false;
            btn_KL_Xoa.Enabled = false;
            btn_KL_Them.Enabled = false;
        }

        private void btn_KL_Xoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn xóa khối lượng có mã là " + txt_MKL.Text +
               " không ? Nếu có ấn nút Lưu, không Thì ấn nút Hủy", "Xóa sản phẩm",
                MessageBoxButtons.OK) == DialogResult.OK)
            {
                Enable_ChatLieu(false);
                _mode = 3;
                lb_KL_TrangThai.Text = "*Bạn đang ở chế độ Xóa";
                btn_KL_Sua.Enabled = false;
                btn_KL_Xoa.Enabled = false;
                btn_KL_Them.Enabled = false;
                btn_KL_Luu.Enabled = true;
                btn_KL_Huy.Enabled = true;
            }
        }

        private void btn_KL_Luu_Click(object sender, EventArgs e)
        {
            string sql = "";

            string ma = txt_MKL.Text;
            string ten = txt_TKL.Text;

            //Kiểm tra dữ liêu
            if (ma.Trim() == "")
            {
                errHangHoa.SetError(txt_MKL, "Mã không được để trống");
                return;
            }
            else
            {
                errHangHoa.Clear();
            }

            if (ten.Trim() == "")
            {
                errHangHoa.SetError(txt_TKL, "Tên không được để trống");
                return;
            }
            else
            {
                errHangHoa.Clear();
            }

            if (_mode == 1)
            {

                sql = $"Select Count(*) From [KhoiLuong] Where MaKhoiLuong = @ma;";
                var parameters = new Dictionary<string, object>
                {
                    {"@ma", ma},
                };
                int count = Convert.ToInt32(_data.ExecuteScalar(sql, parameters));
                if (count > 0)
                {
                    MessageBox.Show($"Đã tồn tại khối lượng với mã {ma}", "Thông báo", MessageBoxButtons.OK);
                    return;
                }
                sql = "INSERT INTO [KhoiLuong] (MaKhoiLuong, TenKhoiLuong)";
                sql += "VALUES(@ma, @ten);";
                parameters = new Dictionary<string, object>
                {
                    {"@ma", ma},
                    {"@ten", ten},
                };
                _data.ExecuteNonQuery(sql, parameters);
            }


            //Nếu nút Sửa enable TNSXì thực hiện cập nhật dữ liệu
            if (_mode == 2)
            {
                sql = "Update [KhoiLuong] SET ";
                sql += "TenKhoiLuong = @ten ";
                sql += "WHERE MaKhoiLuong = @ma";
                var parameters = new Dictionary<string, object>
                {
                    {"@ma", ma},
                    {"@ten", ten},
                };
                _data.ExecuteNonQuery(sql, parameters);
            }

            //Nếu nút Xóa enable thì thực hiện xóa dữ liệu
            if (_mode == 3)
            {
                sql = "Delete From [KhoiLuong] Where MaKhoiLuong = @ma";
                var parameters = new Dictionary<string, object>
                {
                    {"@ma", ma},
                };
                _data.ExecuteNonQuery(sql, parameters);
            }


            Load_KhoiLuong();

            ResetValueTextBox_KhoiLuong();
            Enable_KhoiLuong(false);
            lb_KL_TrangThai.Text = "";
            btn_KL_Them.Enabled = true;
            btn_KL_Xoa.Enabled = false;
            btn_KL_Sua.Enabled = false;
        }

        private void btn_KL_Huy_Click(object sender, EventArgs e)
        {
            lb_KL_TrangThai.Text = "";
            btn_KL_Xoa.Enabled = false;
            btn_KL_Sua.Enabled = false;
            btn_KL_Them.Enabled = true;
            ResetValueTextBox_KhoiLuong();
            Enable_KhoiLuong(false);
            errHangHoa.Clear();
        }

    }
}
