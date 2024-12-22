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
        private void Load_Mau()
        {
            string querry = "Select * from [MauSac]";
            DataTable dt = _data.ExecuteQuery(querry);
            dtg_Mau.DataSource = dt;
            dtg_Mau.Columns[0].HeaderText = "Mã Màu";
            dtg_Mau.Columns[1].HeaderText = "Tên Màu";

            dtg_Mau.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dt.Dispose();
        }
        private void Enable_Mau(Boolean hien)
        {
            txt_TMau.Enabled = hien;
            btn_Mau_Luu.Enabled = hien;
            btn_Mau_Huy.Enabled = hien;
        }
        private void ResetValueTextBox_Mau()
        {
            _mode = 0;
            txt_MMau.Text = "";
            txt_TMau.Text = "";
            lb_Mau_TrangThai.Text = "";
        }
        private void tp_Mau_Enter(object sender, EventArgs e)
        {
            Load_Mau();
            Enable_Mau(false);
            ResetValueTextBox_Mau();
            lb_Mau_TrangThai.Text = "";
            btn_Mau_Them.Enabled = true;
            btn_Mau_Sua.Enabled = false;
            btn_Mau_Xoa.Enabled = false;
        }

        private void dtg_Mau_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (_mode == 0 && e.RowIndex >= 0 && e.RowIndex < dtg_Mau.Rows.Count)
            {
                btn_Mau_Them.Enabled = true;
                btn_Mau_Sua.Enabled = true;
                btn_Mau_Xoa.Enabled = true;
                Enable_Mau(false);
                DataGridViewRow row = dtg_Mau.Rows[e.RowIndex];
                txt_MMau.Text = row.Cells["MaMau"].Value.ToString();
                txt_TMau.Text = row.Cells["TenMau"].Value.ToString();
            }
        }

        private string autoGenerateId_Mau()
        {
            string query = "SELECT TOP 1 MaMau FROM [MauSac] ORDER BY MaMau DESC";
            object result = _data.ExecuteScalar(query);

            if (result != null && !string.IsNullOrEmpty(result.ToString()))
            {
                string currentMaHang = result.ToString();
                int numericPart = int.Parse(currentMaHang.Substring(1));
                numericPart++;

                return "M" + numericPart.ToString("D2");
            }
            else
            {
                return "M01";
            }
        }

        private void btn_Mau_Them_Click(object sender, EventArgs e)
        {
            Enable_Mau(true);
            ResetValueTextBox_Mau();
            _mode = 1;
            lb_Mau_TrangThai.Text = "*Bạn đang ở chế độ Thêm";
            txt_MMau.Text = autoGenerateId_Mau();
            btn_Mau_Sua.Enabled = false;
            btn_Mau_Xoa.Enabled = false;
            btn_Mau_Them.Enabled = false;
        }

        private void btn_Mau_Sua_Click(object sender, EventArgs e)
        {
            Enable_Mau(true);
            _mode = 2;
            lb_Mau_TrangThai.Text = "*Bạn đang ở chế độ Sửa";
            btn_Mau_Sua.Enabled = false;
            btn_Mau_Xoa.Enabled = false;
            btn_Mau_Them.Enabled = false;
        }

        private void btn_Mau_Xoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn xóa màu có mã là " + txt_MMau.Text +
               " không ? Nếu có ấn nút Lưu, không Thì ấn nút Hủy", "Xóa sản phẩm",
                MessageBoxButtons.OK) == DialogResult.OK)
            {
                Enable_Mau(false);
                _mode = 3;
                lb_Mau_TrangThai.Text = "*Bạn đang ở chế độ Xóa";
                btn_Mau_Sua.Enabled = false;
                btn_Mau_Xoa.Enabled = false;
                btn_Mau_Them.Enabled = false;
                btn_Mau_Luu.Enabled = true;
                btn_Mau_Huy.Enabled = true;
            }
        }

        private void btn_Mau_Luu_Click(object sender, EventArgs e)
        {
            string sql = "";

            string ma = txt_MMau.Text;
            string ten = txt_TMau.Text;

            //Kiểm tra dữ liêu
            if (ma.Trim() == "")
            {
                errHangHoa.SetError(txt_MMau, "Mã không được để trống");
                return;
            }
            else
            {
                errHangHoa.Clear();
            }

            if (ten.Trim() == "")
            {
                errHangHoa.SetError(txt_TMau, "Tên không được để trống");
                return;
            }
            else
            {
                errHangHoa.Clear();
            }

            if (_mode == 1)
            {
                sql = "INSERT INTO [MauSac] (MaMau, TenMau)";
                sql += $"VALUES(@ma, @ten);";
                var parameters = new Dictionary<string, object>
                {
                    {"@ma", ma},
                    {"@ten", ten},
                };
                _data.ExecuteNonQuery(sql, parameters);
            }

            //Nếu nút Sửa enable TNSXì thực hiện cập nhật dữ liệu
            if (_mode == 2)
            {
                sql = "Update [MauSac] SET ";
                sql += $"TenMau = @ten ";
                sql += $"WHERE MaMau = @ma";
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
                sql = $"Delete From [MauSac] Where MaMau = @ma";
                var parameters = new Dictionary<string, object>
                {
                    {"@ma", ma},
                };
                _data.ExecuteNonQuery(sql, parameters);
            }

            Load_Mau();

            ResetValueTextBox_Mau();
            Enable_Mau(false);
            lb_Mau_TrangThai.Text = "";
            btn_Mau_Them.Enabled = true;
            btn_Mau_Xoa.Enabled = false;
            btn_Mau_Sua.Enabled = false;
        }

        private void btn_Mau_Huy_Click(object sender, EventArgs e)
        {
            lb_Mau_TrangThai.Text = "";
            btn_Mau_Xoa.Enabled = false;
            btn_Mau_Sua.Enabled = false;
            btn_Mau_Them.Enabled = true;
            ResetValueTextBox_Mau();
            Enable_Mau(false);
            errHangHoa.Clear();
        }

    }
}
