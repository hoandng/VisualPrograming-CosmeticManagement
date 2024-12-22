using Main.ChiTiet;
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
        string MaHang = "";
        private void Load_HangHoa()
        {
            string querry = "Select * from [HangHoa]";
            DataTable dtHangHoa = _data.ExecuteQuery(querry);
            dtg_HangHoa.DataSource = dtHangHoa;

            dtg_HangHoa.Columns["colGiaBan"].DefaultCellStyle.Format = "C0";
            dtg_HangHoa.Columns["colGiaNhap"].DefaultCellStyle.Format = "C0";
            dtg_HangHoa.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            

            dtg_HangHoa.VirtualMode = true;
            dtHangHoa.Dispose();
        }
        
        private void tp_HangHoa_Enter(object sender, EventArgs e)
        {
            // Đổ dữ liệu vào Combobox
            fill_ChatLieu();
            fill_CongDung();
            fill_KhoiLuong();
            fill_Loai();
            fill_Mau();
            fill_Mua();
            fill_NuocSX();
            fill_HangSX();

            Load_HangHoa();
            ResetValueTextBox_HangHoa();
            btn_HH_Them.Enabled = true;
            btn_HH_Xoa.Enabled = false;
            btn_ChiTiet.Enabled = false;
        }
        
        private void fill_ChatLieu()
        {
            string query = "SELECT MaChatLieu, TenChatLieu FROM [ChatLieu]";
            DataTable dataTable = _data.ExecuteQuery(query);

            cb_CL.DataSource = dataTable;
            cb_CL.DisplayMember = "TenChatLieu"; // Tên cột hiển thị
            cb_CL.ValueMember = "MaChatLieu";     // Tên cột giá trị
        }
        
        private void fill_CongDung()
        {
            string query = "SELECT MaCongDung, TenCongDung FROM [CongDung]";
            DataTable dataTable = _data.ExecuteQuery(query);

            cb_CD.DataSource = dataTable;
            cb_CD.DisplayMember = "TenCongDung"; // Tên cột hiển thị
            cb_CD.ValueMember = "MaCongDung";     // Tên cột giá trị
        }
        
        private void fill_KhoiLuong()
        {
            string query = "SELECT MaKhoiLuong, TenKhoiLuong FROM [KhoiLuong]";
            DataTable dataTable = _data.ExecuteQuery(query);

            cb_KL.DataSource = dataTable;
            cb_KL.DisplayMember = "TenKhoiLuong"; // Tên cột hiển thị
            cb_KL.ValueMember = "MaKhoiLuong";     // Tên cột giá trị
        }
        
        private void fill_Loai ()
        {
            string query = "SELECT MaLoai, TenLoai FROM [Loai]";
            DataTable dataTable = _data.ExecuteQuery(query);

            cb_Loai.DataSource = dataTable;
            cb_Loai.DisplayMember = "TenLoai"; // Tên cột hiển thị
            cb_Loai.ValueMember = "MaLoai";     // Tên cột giá trị
        }
        
        private void fill_Mau()
        {
            string query = "SELECT MaMau, TenMau FROM [MauSac]";
            DataTable dataTable = _data.ExecuteQuery(query);

            cb_Mau.DataSource = dataTable;
            cb_Mau.DisplayMember = "TenMau"; // Tên cột hiển thị
            cb_Mau.ValueMember = "MaMau";     // Tên cột giá trị
        }
        
        private void fill_Mua()
        {
            string query = "SELECT MaMua, TenMua FROM [Mua]";
            DataTable dataTable = _data.ExecuteQuery(query);

            cb_Mua.DataSource = dataTable;
            cb_Mua.DisplayMember = "TenMua"; // Tên cột hiển thị
            cb_Mua.ValueMember = "MaMua";     // Tên cột giá trị
        }
        
        private void fill_NuocSX()
        {
            string query = "SELECT MaNuocSX, TenNuocSX FROM [NuocSX]";
            DataTable dataTable = _data.ExecuteQuery(query);

            cb_NuocSX.DataSource = dataTable;
            cb_NuocSX.DisplayMember = "TenNuocSX"; // Tên cột hiển thị
            cb_NuocSX.ValueMember = "MaNuocSX";     // Tên cột giá trị
        }
        
        private void fill_HangSX()
        {
            string query = "SELECT MaHangSX, TenHangSX FROM [HangSX]";
            DataTable dataTable = _data.ExecuteQuery(query);

            cb_Hang.DataSource = dataTable;
            cb_Hang.DisplayMember = "TenHangSX"; // Tên cột hiển thị
            cb_Hang.ValueMember = "MaHangSX";     // Tên cột giá trị
        }
        
        private void ResetValueTextBox_HangHoa()
        {
            txt_TimKiem.Text = "";
            cb_Hang.Text = "";
            cb_Hang.SelectedIndex = -1;
            cb_Loai.Text = "";
            cb_Loai.SelectedIndex = -1;
            cb_Mau.Text = "";
            cb_Mau.SelectedIndex = -1;
            cb_KL.Text = "";
            cb_KL.SelectedIndex = -1;
            cb_CL.Text = "";
            cb_CL.SelectedIndex = -1;
            cb_CD.Text = "";
            cb_CD.SelectedIndex = -1;
            cb_NuocSX.Text = "";
            cb_NuocSX.SelectedIndex = -1;
            cb_Mua.Text = "";
            cb_Mua.SelectedIndex = -1;
        }
        
        private void dtg_HangHoa_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dtg_HangHoa.Rows.Count)
            {
                btn_HH_Them.Enabled = true;
                btn_HH_Xoa.Enabled = true;
                btn_ChiTiet.Enabled = true;
                DataGridViewRow row = dtg_HangHoa.Rows[e.RowIndex];
                
                MaHang = row.Cells["colMaHang"].Value.ToString();
            }
        }
        
        private void btn_HH_Them_Click(object sender, EventArgs e)
        {
            Them_HangHoa themmoi = new Them_HangHoa();
            themmoi.ShowDialog();
        }

        private void btn_HH_Xoa_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn xóa mã mặt hàng " + MaHang +
                           " không ? Nếu có ấn nút OK, không thì ấn nút Cancel", "Xóa sản phẩm", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (result == DialogResult.OK)
            {
                string query = "Delete From [HangHoa] Where MaHang = @mahang";
                var parameters = new Dictionary<string, object> { {"@mahang", MaHang } };
                _data.ExecuteQuery(query, parameters);
                MessageBox.Show("Xoá thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Load_HangHoa();
            }
        }
        
        private void btn_ChiTiet_Click(object sender, EventArgs e)
        {
            if(MaHang != "")
            {
                CT_HangHoa cthh = new CT_HangHoa(MaHang);
                cthh.ShowDialog();
            }
        }

        private void btn_TimKiem_Click(object sender, EventArgs e)
        {
            //Viet cau lenh SQL cho tim kiem
            String sql = "SELECT * FROM [HangHoa]";
            String dk = "";
            //Tim theo MaSP khac rong
            if (txt_TimKiem.Text.Trim() != "")
            {
                dk += " MaHang Like @mahang Or TenHang Like @tenhang";
            }
            //Ket hoi dk
            if (dk != "")
            {
                sql += " WHERE" + dk;
            }
            //Goi phương thức Load dữ liệu kết hợp điều kiện tìm kiếm
            var parameters = new Dictionary<string, object>
            {
                {"@mahang", "%" + txt_TimKiem.Text + "%"},
                {"@tenhang", "%" + txt_TimKiem.Text + "%"}
            };
            DataTable dtHangHoa = _data.ExecuteQuery(sql, parameters);
            if(dtHangHoa.Rows.Count > 0)
            {
                dtg_HangHoa.DataSource = dtHangHoa;
                dtg_HangHoa.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            else
            {
                MessageBox.Show("Không tìm thấy hàng hoá", "Thông báo", MessageBoxButtons.OK);
                Load_HangHoa();
            }
            dtHangHoa.Dispose();
        }

        private void FilterProducts()
        {
            btn_TimKiem.PerformClick();
            // Câu lệnh SQL cơ bản
            string query = "SELECT * FROM [HangHoa] WHERE 1=1";

            // Tạo Dictionary để lưu các tham số
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            // Kiểm tra và thêm điều kiện cho từng ComboBox nếu có giá trị
            if (cb_Loai.SelectedItem != null && cb_Loai.SelectedIndex != -1)
            {
                query += " AND MaLoai = @MaLoai";
                parameters.Add("@MaLoai", cb_Loai.SelectedValue.ToString());
            }

            if (cb_Hang.SelectedItem != null && cb_Hang.SelectedIndex != -1)
            {
                query += " AND MaHangSX = @MaHangSX";
                parameters.Add("@MaHangSX", cb_Hang.SelectedValue.ToString());
            }

            if (cb_NuocSX.SelectedItem != null && cb_NuocSX.SelectedIndex != -1)
            {
                query += " AND MaNuocSX = @MaNuocSX";
                parameters.Add("@MaNuocSX", cb_NuocSX.SelectedValue.ToString());
            }

            if (cb_CL.SelectedItem != null && cb_CL.SelectedIndex != -1)
            {
                query += " AND MaChatLieu = @MaChatLieu";
                parameters.Add("@MaChatLieu", cb_CL.SelectedValue.ToString());
            }

            if (cb_CD.SelectedItem != null && cb_CD.SelectedIndex != -1)
            {
                query += " AND MaCongDung = @MaCongDung";
                parameters.Add("@MaCongDung", cb_CD.SelectedValue.ToString());
            }

            if (cb_Mua.SelectedItem != null && cb_Mua.SelectedIndex != -1)
            {
                query += " AND MaMua = @MaMua";
                parameters.Add("@MaMua", cb_Mua.SelectedValue.ToString());
            }

            if (cb_Mau.SelectedItem != null && cb_Mau.SelectedIndex != -1)
            {
                query += " AND MaMau = @MaMau";
                parameters.Add("@MaMau", cb_Mau.SelectedValue.ToString());
            }

            if (cb_KL.SelectedItem != null && cb_KL.SelectedIndex != -1)
            {
                query += " AND MaKhoiLuong = @MaKhoiLuong";
                parameters.Add("@MaKhoiLuong", cb_KL.SelectedValue.ToString());
            }


            DataTable dt = _data.ExecuteQuery(query, parameters);
            dtg_HangHoa.DataSource = dt;

        }

        private void cb_Loai_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterProducts();
        }

        private void cb_Hang_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterProducts();
        }

        private void cb_NuocSX_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterProducts();
        }

        private void cb_CL_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterProducts();
        }

        private void cb_CD_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterProducts();
        }

        private void cb_Mua_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterProducts();
        }

        private void cb_Mau_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterProducts();
        }

        private void cb_KL_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterProducts();
        }

        private void btn_LamMoi_Click(object sender, EventArgs e)
        {
            ResetValueTextBox_HangHoa();
            Load_HangHoa();
        }
    }
}
