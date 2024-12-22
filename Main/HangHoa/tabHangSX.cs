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

namespace Main.HangHoa
{
    public partial class HangHoa : Form
    {
        int _mode = 0;
        private void tp_HangSX_Enter(object sender, EventArgs e)
        {
            Load_HangSX();
            Enable_HangSX(false);
            ResetValueTextBox_HangSX();
            btn_HSX_Them.Enabled = true;
            btn_HSX_Sua.Enabled = false;
            btn_HSX_Xoa.Enabled = false;
            lb_HSX_TrangThai.Text = "";
        }
        
        private void Load_HangSX()
        {
            DataTable dtHangHoa = _data.ExecuteQuery("Select * from [HangSX]");
            dtg_HangSX.DataSource = dtHangHoa;
            dtg_HangSX.Columns[0].HeaderText = "Mã Hãng";
            dtg_HangSX.Columns[1].HeaderText = "Tên Hãng";

            dtg_HangSX.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dtHangHoa.Dispose();
        }
       
        private void Enable_HangSX(Boolean hien)
        {
            txt_TH.Enabled = hien;
            //Ẩn hiện 2 nút Lưu và Hủy
            btn_HSX_Luu.Enabled = hien;
            btn_HSX_Huy.Enabled = hien;
        }
        private void ResetValueTextBox_HangSX()
        {
            _mode = 0;
            txt_MH.Text = "";
            txt_TH.Text = "";
            lb_HSX_TrangThai.Text = "";
        }
       
        private void dtg_HangSX_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (_mode == 0 && e.RowIndex >= 0 && e.RowIndex < dtg_HangSX.Rows.Count)
            {
                //Hien thi nut sua va xoa
                btn_HSX_Them.Enabled = true;
                btn_HSX_Sua.Enabled = true;
                btn_HSX_Xoa.Enabled = true;
                Enable_HangSX(false);
                DataGridViewRow row = dtg_HangSX.Rows[e.RowIndex];
                txt_MH.Text = row.Cells["MaHangSX"].Value.ToString();
                txt_TH.Text = row.Cells["TenHangSX"].Value.ToString();
            }
        }

        private string autoGenerateId_HangSX()
        {
            string query = "SELECT TOP 1 MaHangSX FROM [HangSX] ORDER BY MaHangSX DESC";
            object result = _data.ExecuteScalar(query);

            if (result != null && !string.IsNullOrEmpty(result.ToString()))
            {
                string currentMaHang = result.ToString();
                int numericPart = int.Parse(currentMaHang.Substring(3));
                numericPart++;

                return "HSX" + numericPart.ToString("D2");
            }
            else
            {
                return "HSX01";
            }
        }

        private void btn_HSX_Them_Click(object sender, EventArgs e)
        {
            Enable_HangSX(true);
            ResetValueTextBox_HangSX();
            _mode = 1;
            lb_HSX_TrangThai.Text = "*Bạn đang ở chế độ Thêm";
            txt_MH.Text = autoGenerateId_HangSX();
            btn_HSX_Sua.Enabled = false;
            btn_HSX_Xoa.Enabled = false;
            btn_HSX_Them.Enabled = false;
        }
        private void btn_HSX_Sua_Click(object sender, EventArgs e)
        {
            Enable_HangSX(true);
            _mode = 2;
            lb_HSX_TrangThai.Text = "*Bạn đang ở chế độ Sửa";
            btn_HSX_Sua.Enabled = false;
            btn_HSX_Xoa.Enabled = false;
            btn_HSX_Them.Enabled = false;
        }

        private void btn_HSX_Xoa_Click(object sender, EventArgs e)
        {
            //Bật Message Box cảnh báo người sử dụng
            if (MessageBox.Show("Bạn có chắc chắn xóa hãng sản xuất có mã là " + txt_MH.Text +
                " không ? Nếu có ấn nút Lưu, không thì ấn nút Hủy", "Xóa sản phẩm",
           MessageBoxButtons.OK) == DialogResult.OK)
            {
                Enable_HangSX(false);
                _mode = 3;
                lb_HSX_TrangThai.Text = "*Bạn đang ở chế độ Xóa";
                btn_HSX_Sua.Enabled = false;
                btn_HSX_Xoa.Enabled = false;
                btn_HSX_Them.Enabled = false;
                btn_HSX_Luu.Enabled = true;
                btn_HSX_Huy.Enabled = true;
                //Hiện gropbox chi tiết
            }
        }

        private void btn_HSX_Luu_Click(object sender, EventArgs e)
        {
            string sql = "";

            string mahsx = txt_MH.Text;
            string tenhsx = txt_TH.Text;

            //Kiểm tra dữ liêu
            if (mahsx.Trim() == "")
            {
                errHangHoa.SetError(txt_MH, "Mã không được để trống");
                return;
            }
            else
            {
                errHangHoa.Clear();
            }

            if (tenhsx.Trim() == "")
            {
                errHangHoa.SetError(txt_TH, "Tên không được để trống");
                return;
            }
            else
            {
                errHangHoa.Clear();
            }

            if (_mode == 1)
            {
                sql = "Select Count(*) From [HangSX] Where MaHangSX = @mahsx;";
                var parameters = new Dictionary<string, object>
                {
                    {"@mahsx", mahsx},
                };
                int count = Convert.ToInt32(_data.ExecuteScalar(sql, parameters));
                if (count > 0)
                {
                    MessageBox.Show($"Đã tồn tại hãng sản xuất với mã {mahsx}", "Thông báo", MessageBoxButtons.OK);
                    return;
                }
                sql = "INSERT INTO [HangSX] (MaHangSX, TenHangSX)";
                sql += "VALUES(@mahsx, @tenhsx);";
                parameters = new Dictionary<string, object>
                {
                    {"@mahsx", mahsx},
                    {"@tenhsx", tenhsx},
                };
                _data.ExecuteNonQuery(sql, parameters);
            }


            //Nếu nút Sửa enable thì thực hiện cập nhật dữ liệu
            if (_mode == 2)
            {
                sql = "Update [HangSX] SET ";
                sql += $"TenHangSX = @tenhsx ";
                sql += $"WHERE MaHangSX = @mahsx";
                var parameters = new Dictionary<string, object>
                {
                    {"@tenhsx", tenhsx},
                    {"@mahsx", mahsx},
                };
                _data.ExecuteNonQuery(sql, parameters);
            }

            //Nếu nút Xóa enable thì thực hiện xóa dữ liệu
            if (_mode == 3)
            {
                sql = $"Delete From [HangSX] Where MaHangSX = @mahsx";
                var parameters = new Dictionary<string, object>
                {
                    {"@mahsx", mahsx},
                };
                _data.ExecuteNonQuery(sql, parameters);
            }

            Load_HangSX();

            ResetValueTextBox_HangSX();
            Enable_HangSX(false);
            lb_HSX_TrangThai.Text = "";
            btn_HSX_Them.Enabled = true;
            btn_HSX_Xoa.Enabled = false;
            btn_HSX_Sua.Enabled = false;
        }

        private void btn_HSX_Huy_Click(object sender, EventArgs e)
        {
            lb_HSX_TrangThai.Text = "";
            //Thiết lập lại các nút như ban đầu
            btn_HSX_Xoa.Enabled = false;
            btn_HSX_Sua.Enabled = false;
            btn_HSX_Them.Enabled = true;
            //xoa trang
            ResetValueTextBox_HangSX();
            //Cam nhap
            Enable_HangSX(false);
            errHangHoa.Clear();
        }
    }
}
