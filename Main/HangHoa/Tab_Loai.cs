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
        private void Load_Loai()
        {
            string querry = "Select * from [Loai]";
            DataTable dt = _data.ExecuteQuery(querry);
            dtg_Loai.DataSource = dt;
            dtg_Loai.Columns[0].HeaderText = "Mã Loại";
            dtg_Loai.Columns[1].HeaderText = "Tên Loại";

            dtg_Loai.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dt.Dispose();
        }
        private void Enable_Loai(Boolean hien)
        {
            txt_TL.Enabled = hien;
            btn_Loai_Luu.Enabled = hien;
            btn_Loai_Huy.Enabled = hien;
        }
        private void ResetValueTextBox_Loai()
        {
            _mode = 0;
            txt_TL.Text = "";
            txt_ML.Text = "";
            lb_Loai_TrangThai.Text = "";
        }
        private void tp_Loai_Enter(object sender, EventArgs e)
        {
            Enable_Loai(false);
            ResetValueTextBox_Loai();
            Load_Loai();
            btn_Loai_Them.Enabled = true;
            btn_Loai_Sua.Enabled = false;
            btn_Loai_Xoa.Enabled = false;

            lb_Loai_TrangThai.Text = "";
        }

        private void dtg_Loai_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (_mode == 0 && e.RowIndex >= 0 && e.RowIndex < dtg_Loai.Rows.Count)
            {
                //Hien thi nut sua va xoa
                btn_Loai_Them.Enabled = true;
                btn_Loai_Sua.Enabled = true;
                btn_Loai_Xoa.Enabled = true;
                Enable_Loai(false);
                DataGridViewRow row = dtg_Loai.Rows[e.RowIndex];
                txt_ML.Text = row.Cells["MaLoai"].Value.ToString();
                txt_TL.Text = row.Cells["TenLoai"].Value.ToString();
            }
        }

        private string autoGenerateId_Loai()
        {
            string query = "SELECT TOP 1 MaLoai FROM [Loai] ORDER BY MaLoai DESC";
            object result = _data.ExecuteScalar(query);

            if (result != null && !string.IsNullOrEmpty(result.ToString()))
            {
                string currentMaHang = result.ToString();
                int numericPart = int.Parse(currentMaHang.Substring(1));
                numericPart++;

                return "L" + numericPart.ToString("D2");
            }
            else
            {
                return "L01";
            }
        }

        private void btn_Loai_Them_Click(object sender, EventArgs e)
        {
            Enable_Loai(true);
            ResetValueTextBox_Loai();
            _mode = 1;
            lb_Loai_TrangThai.Text = "*Bạn đang ở chế độ Thêm";
            txt_ML.Text = autoGenerateId_Loai();
            btn_Loai_Sua.Enabled = false;
            btn_Loai_Xoa.Enabled = false;
            btn_Loai_Them.Enabled = false;
        }

        private void btn_Loai_Sua_Click(object sender, EventArgs e)
        {
            Enable_Loai(true);
            _mode = 2;
            lb_Loai_TrangThai.Text = "*Bạn đang ở chế độ Sửa";
            btn_Loai_Sua.Enabled = false;
            btn_Loai_Xoa.Enabled = false;
            btn_Loai_Them.Enabled = false;
        }

        private void btn_Loai_Xoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn xóa loại có mã là " + txt_ML.Text +
               " không ? Nếu có ấn nút Lưu, không TNSXì ấn nút Hủy", "Xóa sản phẩm",
                MessageBoxButtons.OK) == DialogResult.OK)
            {
                Enable_Loai(false);
                _mode = 3;
                lb_Loai_TrangThai.Text = "*Bạn đang ở chế độ Xóa";
                btn_Loai_Sua.Enabled = false;
                btn_Loai_Xoa.Enabled = false;
                btn_Loai_Them.Enabled = false;
                btn_Loai_Luu.Enabled = true;
                btn_Loai_Huy.Enabled = true;
            }
        }

        private void btn_Loai_Luu_Click(object sender, EventArgs e)
        {
            string sql = "";

            string ma = txt_ML.Text;
            string ten = txt_TL.Text;

            //Kiểm tra dữ liêu
            if (ma.Trim() == "")
            {
                errHangHoa.SetError(txt_ML, "Mã không được để trống");
                return;
            }
            else
            {
                errHangHoa.Clear();
            }

            if (ten.Trim() == "")
            {
                errHangHoa.SetError(txt_TL, "Tên không được để trống");
                return;
            }
            else
            {
                errHangHoa.Clear();
            }

            if (_mode == 1)
            {
                sql = "INSERT INTO [Loai] (MaLoai, TenLoai)";
                sql += "VALUES(@ma, @ten);";
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
                sql = "Update [Loai] SET ";
                sql += "TenLoai = @ten ";
                sql += "WHERE MaLoai = @ma";
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
                sql = "Delete From [Loai] Where MaLoai = @ma";
                var parameters = new Dictionary<string, object>
                {
                    {"@ma", ma},
                };
                _data.ExecuteNonQuery(sql, parameters);
            }

            Load_Loai();

            ResetValueTextBox_Loai();
            Enable_Loai(false);
            lb_Loai_TrangThai.Text = "";
            btn_Loai_Them.Enabled = true;
            btn_Loai_Xoa.Enabled = false;
            btn_Loai_Sua.Enabled = false;
        }

        private void btn_Loai_Huy_Click(object sender, EventArgs e)
        {
            lb_Loai_TrangThai.Text = "";
            btn_Loai_Xoa.Enabled = false;
            btn_Loai_Sua.Enabled = false;
            btn_Loai_Them.Enabled = true;
            ResetValueTextBox_Loai();
            Enable_Loai(false);
            errHangHoa.Clear();
        }

    }
}
