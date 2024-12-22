using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Main.HangHoa
{
    partial class HangHoa : Form
    {
        private void Load_CongDung()
        {
            string querry = "Select * from [CongDung]";
            DataTable dt = _data.ExecuteQuery(querry);
            dtg_CongDung.DataSource = dt;
            dtg_CongDung.Columns[0].HeaderText = "Mã Công dụng";
            dtg_CongDung.Columns[1].HeaderText = "Tên Công dụng";

            dtg_CongDung.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dt.Dispose();
        }
       
        private void Enable_CongDung(Boolean hien)
        {
            txt_TCD.Enabled = hien;
            btn_CD_Luu.Enabled = hien;
            btn_CD_Huy.Enabled = hien;
        }
        
        private void ResetValueTextBox_CongDung()
        {
            _mode = 0;
            txt_MCD.Text = "";
            txt_TCD.Text = "";
            lb_CD_TrangThai.Text = "";
        }
        
        private void tp_CongDung_Enter(object sender, EventArgs e)
        {
            Load_CongDung();
            Enable_CongDung(false);
            ResetValueTextBox_CongDung();
            lb_CD_TrangThai.Text = "";
            btn_CD_Them.Enabled = true;
            btn_CD_Sua.Enabled = false;
            btn_CD_Xoa.Enabled = false;
        }

        private void dtg_CongDung_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (_mode == 0 && e.RowIndex >= 0 && e.RowIndex < dtg_CongDung.Rows.Count)
            {
                btn_CD_Them.Enabled = true;
                btn_CD_Sua.Enabled = true;
                btn_CD_Xoa.Enabled = true;
                Enable_CongDung(false);
                DataGridViewRow row = dtg_CongDung.Rows[e.RowIndex];
                txt_MCD.Text = row.Cells["MaCongDung"].Value.ToString();
                txt_TCD.Text = row.Cells["TenCongDung"].Value.ToString();
            }
            
        }

        private string autoGenerateId_CD()
        {
            // Lấy mã hàng lớn nhất hiện tại
            string query = "SELECT TOP 1 MaCongDung FROM [CongDung] ORDER BY MaCongDung DESC";
            object result = _data.ExecuteScalar(query);

            if (result != null && !string.IsNullOrEmpty(result.ToString()))
            {
                string currentMaHang = result.ToString();
                int numericPart = int.Parse(currentMaHang.Substring(2));
                numericPart++;

                return "CD" + numericPart.ToString("D2");
            }
            else
            {
                // Nếu chưa có mã hàng nào trong cơ sở dữ liệu, bắt đầu từ "H01"
                return "CD01";
            }
        }

        private void btn_CD_Them_Click(object sender, EventArgs e)
        {
            Enable_CongDung(true);
            ResetValueTextBox_CongDung();
            _mode = 1;
            lb_CD_TrangThai.Text = "*Bạn đang ở chế độ Thêm";
            txt_MCD.Text = autoGenerateId_CD();
            btn_CD_Sua.Enabled = false;
            btn_CD_Xoa.Enabled = false;
            btn_CD_Them.Enabled = false;
        }

        private void btn_CD_Sua_Click(object sender, EventArgs e)
        {
            Enable_CongDung(true);
            _mode = 2;
            lb_CD_TrangThai.Text = "*Bạn đang ở chế độ Sửa";
            btn_CD_Them.Enabled = false;
            btn_CD_Sua.Enabled = false;
            btn_CD_Xoa.Enabled = false;
        }

        private void btn_CD_Xoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn xóa công dụng có mã là " + txt_MCD.Text +
               " không ? Nếu có ấn nút Lưu, không Thì ấn nút Hủy", "Xóa sản phẩm",
                MessageBoxButtons.OK) == DialogResult.OK)
            {
                Enable_CongDung(false);
                _mode = 3;
                lb_CD_TrangThai.Text = "*Bạn đang ở chế độ Xóa";
                btn_CD_Them.Enabled = false;
                btn_CD_Sua.Enabled = false;
                btn_CD_Xoa.Enabled = false;
                btn_CD_Luu.Enabled = true;
                btn_CD_Huy.Enabled = true;
            }
        }

        private void Them_CD()
        {
            string sql = $"Select Count(*) From [CongDung] Where MaCongDung = @ma;";
            var parameters = new Dictionary<string, object>
            {
                {"@ma", txt_MCD.Text},
            };
            int count = Convert.ToInt32(_data.ExecuteScalar(sql, parameters));
            if (count > 0)
            {
                MessageBox.Show($"Đã tồn tại công dụng với mã {txt_MCD}", "Thông báo", MessageBoxButtons.OK);
                return;
            }
            sql = "INSERT INTO [CongDung] (MaCongDung, TenCongDUng)";
            sql += $"VALUES(@ma, @ten);";
            parameters = new Dictionary<string, object>
            {
                {"@ma", txt_MCD.Text},
                {"@ten", txt_TCD.Text},
            };
            _data.ExecuteNonQuery(sql, parameters);
        }

        private void Sua_CD()
        {
            string sql = "Update [CongDung] SET ";
            sql += $"TenCongDung = @ten ";
            sql += $"WHERE MaCongDung = @ma";
            var parameters = new Dictionary<string, object>
                {
                    {"@ma", txt_MCD.Text},
                    {"@ten", txt_TCD.Text},
                };
            _data.ExecuteNonQuery(sql, parameters);
        }

        private void Xoa_CD()
        {
            string sql = $"Delete From [CongDung] Where MaCongDung = @ma";
            var parameters = new Dictionary<string, object>
                {
                    {"@ma", txt_MCD.Text},
                };
            _data.ExecuteNonQuery(sql, parameters);
        }

        private void btn_CD_Luu_Click(object sender, EventArgs e)
        {
            string ma = txt_MCD.Text;
            string ten = txt_TCD.Text;

            //Kiểm tra dữ liêu
            if (ma.Trim() == "")
            {
                errHangHoa.SetError(txt_MCD, "Mã không được để trống");
                return;
            }
            else
            {
                errHangHoa.Clear();
            }

            if (ten.Trim() == "")
            {
                errHangHoa.SetError(txt_TCD, "Tên không được để trống");
                return;
            }
            else
            {
                errHangHoa.Clear();
            }

            if (_mode == 1)
            {
                Them_CD();
            }


            if (_mode == 2)
            {
                Sua_CD();
            }

            if (_mode == 3)
            {
                Xoa_CD();
            }

            Load_CongDung();

            ResetValueTextBox_CongDung();
            Enable_CongDung(false);
            lb_CD_TrangThai.Text = "";
            btn_CD_Them.Enabled = true;
            btn_CD_Xoa.Enabled = false;
            btn_CD_Sua.Enabled = false;
        }

        private void btn_CD_Huy_Click(object sender, EventArgs e)
        {
            lb_CD_TrangThai.Text = "";
            btn_CD_Xoa.Enabled = false;
            btn_CD_Sua.Enabled = false;
            btn_CD_Them.Enabled = true;
            ResetValueTextBox_CongDung();
            Enable_CongDung(false);
            errHangHoa.Clear();
        }
    }
}
