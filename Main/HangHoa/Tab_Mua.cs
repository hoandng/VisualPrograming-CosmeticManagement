using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace Main.HangHoa
{
    partial class HangHoa : Form
    {
        private void Load_Mua()
        {
            string querry = "Select * from [Mua]";
            DataTable dt = _data.ExecuteQuery(querry);
            dtg_Mua.DataSource = dt;
            dtg_Mua.Columns[0].HeaderText = "Mã Mùa";
            dtg_Mua.Columns[1].HeaderText = "Tên Mùa";

            dtg_Mua.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dt.Dispose();
        }
        private void Enable_Mua(Boolean hien)
        {
            txt_TMua.Enabled = hien;
            btn_Mua_Luu.Enabled = hien;
            btn_Mua_Huy.Enabled = hien;
        }
        
        private void ResetValueTextBox_Mua()
        {
            _mode = 0;
            txt_MMua.Text = "";
            txt_TMua.Text = "";
            lb_Mua_TrangThai.Text = "";
        }
        
        private void tp_Mua_Enter(object sender, EventArgs e)
        {
            ResetValueTextBox_Mua();
            Load_Mua();
            Enable_Mua(false);
            lb_Mua_TrangThai.Text = "";
            btn_Mua_Them.Enabled = true;
            btn_Mua_Sua.Enabled = false;
            btn_Mua_Xoa.Enabled = false;
        }

        private void dtg_Mua_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (btn_Mua_Them.Enabled == false)
            {
                return;
            }
            if (e.RowIndex >= 0 && e.RowIndex < dtg_Mua.Rows.Count)
            {
                btn_Mua_Them.Enabled = true;
                btn_Mua_Sua.Enabled = true;
                btn_Mua_Xoa.Enabled = true;
                Enable_Mua(false);
                DataGridViewRow row = dtg_Mua.Rows[e.RowIndex];
                txt_MMua.Text = row.Cells["MaMua"].Value.ToString();
                txt_TMua.Text = row.Cells["TenMua"].Value.ToString();
            }
        }

        private string autoGenerateId_Mua()
        {
            string query = "SELECT TOP 1 MaMua FROM [Mua] ORDER BY MaMua DESC";
            object result = _data.ExecuteScalar(query);

            if (result != null && !string.IsNullOrEmpty(result.ToString()))
            {
                string currentMaHang = result.ToString();
                int numericPart = int.Parse(currentMaHang.Substring(3));
                numericPart++;

                return "MUA" + numericPart.ToString("D2");
            }
            else
            {
                return "MUA01";
            }
        }
        private void btn_Mua_Them_Click(object sender, EventArgs e)
        {
            Enable_Mua(true);
            ResetValueTextBox_Mua();
            _mode = 1;
            lb_Mua_TrangThai.Text = "*Bạn đang ở chế độ Thêm";
            txt_MMua.Text = autoGenerateId_Mua();
            btn_Mua_Sua.Enabled = false;
            btn_Mua_Xoa.Enabled = false;
            btn_Mua_Them.Enabled = false;
        }

        private void btn_Mua_Sua_Click(object sender, EventArgs e)
        {
            Enable_Mua(true);
            _mode = 2;
            lb_Mua_TrangThai.Text = "*Bạn đang ở chế độ Sửa";
            btn_Mua_Sua.Enabled = false;
            btn_Mua_Xoa.Enabled = false;
            btn_Mua_Them.Enabled = false;
        }

        private void btn_Mua_Xoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn xóa mùa có mã là " + txt_MMua.Text +
               " không ? Nếu có ấn nút Lưu, không Thì ấn nút Hủy", "Xóa sản phẩm",
                MessageBoxButtons.OK) == DialogResult.OK)
            {
                Enable_Mua(false);
                _mode = 3;
                lb_Mua_TrangThai.Text = "*Bạn đang ở chế độ Xóa";
                btn_Mua_Sua.Enabled = false;
                btn_Mua_Xoa.Enabled = false;
                btn_Mua_Them.Enabled = false;
                btn_Mua_Luu.Enabled = true;
                btn_Mua_Huy.Enabled = true;
            }
        }

        private void btn_Mua_Luu_Click(object sender, EventArgs e)
        {
            string sql = "";

            string ma = txt_MMua.Text;
            string ten = txt_TMua.Text;

            //Kiểm tra dữ liêu
            if (ma.Trim() == "")
            {
                errHangHoa.SetError(txt_MMua, "Mã không được để trống");
                return;
            }
            else
            {
                errHangHoa.Clear();
            }

            if (ten.Trim() == "")
            {
                errHangHoa.SetError(txt_TMua, "Tên không được để trống");
                return;
            }
            else
            {
                errHangHoa.Clear();
            }

            if (_mode == 1)
            {
                sql = "Select Count(*) From [Mua] Where MaMua = @ma;";
                var parameters = new Dictionary<string, object>
                {
                    {"@ma", ma},
                };
                int count = Convert.ToInt32(_data.ExecuteScalar(sql, parameters));
                if (count > 0)
                {
                    MessageBox.Show($"Đã tồn tại mùa với mã {ma}", "Thông báo", MessageBoxButtons.OK);
                    return;
                }
                sql = "INSERT INTO [Mua] (MaMua, TenMua)";
                sql += $"VALUES(@ma, @ten);";
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
                sql = "Update [Mua] SET ";
                sql += "TenMua =  @ten ";
                sql += "WHERE MaMua = @ma";
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
                sql = "Delete From [Mua] Where MaMua = @ma";
                var parameters = new Dictionary<string, object>
                {
                    {"@ma", ma},
                    {"@ten", ten},
                };
                _data.ExecuteNonQuery(sql, parameters);
            }

            Load_Mua();

            ResetValueTextBox_Mua();
            Enable_Mua(false);
            lb_Mua_TrangThai.Text = "";
            btn_Mua_Them.Enabled = true;
            btn_Mua_Xoa.Enabled = false;
            btn_Mua_Sua.Enabled = false;
        }

        private void btn_Mua_Huy_Click(object sender, EventArgs e)
        {
            lb_Mua_TrangThai.Text = "";
            btn_Mua_Xoa.Enabled = false;
            btn_Mua_Sua.Enabled = false;
            btn_Mua_Them.Enabled = true;
            ResetValueTextBox_Mua();
            Enable_Mua(false);
            errHangHoa.Clear();
        }
    }
}
