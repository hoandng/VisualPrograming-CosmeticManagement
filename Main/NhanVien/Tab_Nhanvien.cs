using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Main.NhanVien
{
    partial class NhanVien : Form
    {
        ProcessDatabase _database = new ProcessDatabase();
        int _mode = 0;
        private void Load_NhanVien()
        {
            string querry = "Select nv.MaNV, nv.TenNV, nv.MaCV, nv.GioiTinh, nv.NgaySinh, nv.DienThoai, nv.DiaChi, tk.Username, tk.Password, tk.isAdmin from [NhanVien] nv " +
                "Left Join  [TaiKhoan] tk On nv.MaNV = tk.MaNV";
            DataTable dtNhanVien = _database.ExecuteQuery(querry);
            dtg_NhanVien.DataSource = dtNhanVien;
            dtg_NhanVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dtNhanVien.Dispose();
        }

        private void fill_CongViec()
        {
            string query = "SELECT MaCV, TenCV FROM [CongViec]";
            DataTable dataTable = _database.ExecuteQuery(query);

            cb_CongViec.DataSource = dataTable;
            cb_CongViec.DisplayMember = "TenCV"; // Tên cột hiển thị
            cb_CongViec.ValueMember = "MaCV";     // Tên cột giá trị
        }
        private void resetTextBox()
        {
            _mode = 0;
            txt_MaNV.Text = "";
            txt_TenNV.Text = "";
            cb_CongViec.Text = "";
            cb_CongViec.SelectedIndex = -1;
            txt_SDT.Text = "";
            cb_GioiTinh.Text = "";
            cb_GioiTinh.SelectedIndex = -1;
            dtp_NgaySinh.Value = DateTime.Now;
            txt_DiaChi.Text = "";
            chb_admin.Checked = false;
        }
        private void enableControl(bool enable)
        {
            txt_TenNV.Enabled = enable;
            cb_CongViec.Enabled = enable;
            txt_SDT.Enabled = enable;
            dtp_NgaySinh.Enabled = enable;
            txt_DiaChi.Enabled = enable;
            cb_GioiTinh.Enabled = enable;
            chb_admin.Enabled = enable;

            btn_NV_Luu.Enabled = enable;
            btn_NV_Huy.Enabled = enable;
        }
        private void tp_NhanVien_Enter(object sender, EventArgs e)
        {
            resetTextBox();
            Load_NhanVien();
            enableControl(false);
            fill_CongViec();
            lb_NV_TrangThai.Text = "";
            btn_NV_Them.Enabled = true;
            btn_NV_Sua.Enabled = false;
            btn_NV_Xoa.Enabled = false;
        }

        private void dtg_NhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (_mode == 0 && e.RowIndex >= 0 && e.RowIndex < dtg_NhanVien.Rows.Count)
            {
                enableControl(false);
                btn_NV_Them.Enabled = true;
                btn_NV_Sua.Enabled = true;
                btn_NV_Xoa.Enabled = true;
                DataGridViewRow row = dtg_NhanVien.Rows[e.RowIndex];
                txt_MaNV.Text = row.Cells["MaNV"].Value.ToString();
                txt_TenNV.Text = row.Cells["TenNV"].Value.ToString();
                cb_GioiTinh.Text = row.Cells["GioiTinh"].Value.ToString();
                dtp_NgaySinh.Text = row.Cells["NgaySinh"].Value.ToString();
                txt_DiaChi.Text = row.Cells["DiaChi"].Value.ToString();
                txt_SDT.Text = row.Cells["DienThoai"].Value.ToString();
                cb_CongViec.SelectedValue = row.Cells["MaCV"].Value.ToString();
                chb_admin.Checked = int.Parse(row.Cells["isAdmin"].Value.ToString()) ==  0 ? true : false;
            }
        }

        private string autoGenerateId()
        {
            string query = "SELECT TOP 1 MaNV FROM [NhanVien] ORDER BY MaNV DESC";
            object result = _database.ExecuteScalar(query);

            if (result != null && !string.IsNullOrEmpty(result.ToString()))
            {
                string currentMaHang = result.ToString();
                int numericPart = int.Parse(currentMaHang.Substring(2));
                numericPart++;

                return "NV" + numericPart.ToString("D2");
            }
            else
            {
                return "NV01";
            }
        }

        private void autoGenerateAccount(string manv, string tennv, int isAdmin = 2)
        {
            string new_username = "nhanvien"+manv.Substring(2);
            string new_password = "abc123";

            string query = "Insert Into [TaiKhoan](Username, Password, isAdmin, MaNV )" +
                "Values(@username, @password, @isadmin, @manv)";
            var parameter = new Dictionary<string, object>
            {
                {"@username", new_username},
                {"@password", new_password},
                {"@isadmin", isAdmin},
                {"@manv", manv},
            };
            _database.ExecuteNonQuery(query, parameter);
        }
        
        private void btn_NV_Them_Click(object sender, EventArgs e)
        {
            resetTextBox();
            enableControl(true);
            _mode = 1;
            txt_MaNV.Text = autoGenerateId();
            btn_NV_Sua.Enabled = false;
            btn_NV_Xoa.Enabled = false;
            btn_NV_Them.Enabled = false;
            lb_NV_TrangThai.Text = "*Bạn đang ở chế độ Thêm!";
        }

        private void btn_NV_Sua_Click(object sender, EventArgs e)
        {
            enableControl(true);
            _mode = 2;
            btn_NV_Them.Enabled = false;
            btn_NV_Xoa.Enabled=false;
            lb_NV_TrangThai.Text = "*Bạn đang ở chế độ Sửa!";
        }

        private void btn_NV_Xoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn xóa Công việc có mã là " + txt_MaNV.Text +
               " không ? Nếu có ấn nút Lưu, không Thì ấn nút Hủy", "Thông báo!",
                MessageBoxButtons.OK) == DialogResult.OK)
            {
                _mode = 3;
                enableControl(false);
                btn_NV_Luu.Enabled = true;
                btn_NV_Huy.Enabled = true;
                btn_NV_Them.Enabled = false;
                btn_NV_Sua.Enabled = false;
                btn_NV_Xoa.Enabled = false;
                lb_NV_TrangThai.Text = "*Bạn đang ở chế độ Xoá!";
            }
           
        }

        private void btn_NV_Luu_Click(object sender, EventArgs e)
        {
            string sql = "";

            string ma = txt_MaNV.Text;
            string ten = txt_TenNV.Text;
            string diachi = txt_DiaChi.Text;
            string sdt = txt_SDT.Text;
            string macv = "";
            string ngaysinh = dtp_NgaySinh.Value.ToString("yyyy-MM-dd");
            string gioitinh = "";

            int admin = 1;
            if(chb_admin.Checked == true)
            {
                admin = 0;
            }
            if (ten.Trim() == "")
            {
                errNhanVien.SetError(txt_TenNV, "Tên không được để trống");
                return;
            }
            else
            {
                errNhanVien.Clear();
            }

            if (cb_GioiTinh.SelectedIndex == -1)
            {
                errNhanVien.SetError(cb_GioiTinh, "Phải chọn Nam hoặc Nữ");
                return;
            }
            else
            {
                gioitinh = cb_GioiTinh.Text;
                errNhanVien.Clear();
            }

            // Kiểm tra giới tính có đúng không
            if (!string.Equals(gioitinh, "Nam", StringComparison.OrdinalIgnoreCase) 
                && !string.Equals(gioitinh, "Nữ", StringComparison.OrdinalIgnoreCase))
            {
                errNhanVien.SetError(cb_GioiTinh, "Chọn đúng giới tính Nam hoặc Nữ");
                return;
            }
            else
            {
                errNhanVien.Clear();
            }

            if (cb_CongViec.SelectedIndex == -1)
            {
                errNhanVien.SetError(cb_CongViec, "Phải chọn một Công Việc");
                return;
            }
            else
            {
                macv = cb_CongViec.SelectedValue.ToString();
                errNhanVien.Clear();
            }

            // Kiểm tra định dạng ngày sinh
            DateTime dob;
            if (!DateTime.TryParse(ngaysinh, out dob))
            {
                errNhanVien.SetError(dtp_NgaySinh, "Nhập đúng định dạng ngày tháng");
                return;
            }
            else
            {
                errNhanVien.Clear();
            }

            if (sdt.Length != 12)
            {
                Console.WriteLine(sdt.Length);
                errNhanVien.SetError(txt_SDT, "Số điện thoại phải là 10 số");
                return;
            }
            else
            {
                errNhanVien.Clear();
            }

            if (_mode == 1)
            {

                sql = $"Select Count(*) From [NhanVien] Where MaNV = @ma;";
                var parameters = new Dictionary<string, object>
                {
                    {"@ma", ma},
                };
                int count = Convert.ToInt32(_database.ExecuteScalar(sql, parameters));
                if (count > 0)
                {
                    MessageBox.Show($"Đã tồn tại Nhân viên với mã {ma}", "Thông báo", MessageBoxButtons.OK);
                    return;
                }
                sql = "INSERT INTO [NhanVien] (MaNV, TenNV, GioiTinh, NgaySinh, DienThoai, DiaChi, MaCV)";
                sql += "VALUES(@ma, @ten, @gioitinh, @ngaysinh, @dienthoai, @diachi, @macv);";
                parameters = new Dictionary<string, object>
                {
                    {"@ma", ma},
                    {"@ten", ten},
                    {"@diachi", diachi},
                    {"@dienthoai",sdt },
                    {"@ngaysinh", ngaysinh },
                    {"@gioitinh", gioitinh},
                    {"@macv", macv }
                };
                _database.ExecuteNonQuery(sql, parameters);

                autoGenerateAccount(ma, ten, admin);
            }

            //Nếu nút Sửa enable thì thực hiện cập nhật dữ liệu
            if (_mode == 2)
            {
                sql = "Update [NhanVien] SET ";
                sql += "TenNV = @ten, GioiTinh = @gioitinh, NgaySinh = @ngaysinh, DienThoai = @dienthoai, DiaChi = @diachi, MaCV = @macv ";
                sql += "WHERE MaNV = @ma";
                var parameters = new Dictionary<string, object>
                {
                    {"@ma", ma},
                    {"@ten", ten},
                    {"@diachi", diachi},
                    {"@dienthoai",sdt },
                    {"@ngaysinh", ngaysinh },
                    {"@gioitinh", gioitinh},
                    {"@macv", macv }
                };
                _database.ExecuteNonQuery(sql, parameters);

                sql = "Update [TaiKhoan] SET ";
                sql += "isAdmin = @admin ";
                sql += "WHERE MaNV = @ma";
                parameters = new Dictionary<string, object>
                {
                    {"@ma", ma},
                    {"@admin", admin},
                };
                _database.ExecuteNonQuery(sql, parameters);
            }

            //Nếu nút Xóa enable thì thực hiện xóa dữ liệu
            if (_mode == 3)
            {
                sql = $"Delete From [TaiKhoan] Where MaNV = @ma";
                var parameters = new Dictionary<string, object>
                {
                    {"@ma", ma},
                };
                _database.ExecuteNonQuery(sql, parameters);


                sql = $"Delete From [NhanVien] Where MaNV = @ma";
                parameters = new Dictionary<string, object>
                {
                    {"@ma", ma},
                };
                _database.ExecuteNonQuery(sql, parameters);
            }

            Load_NhanVien();
            
            resetTextBox();
            enableControl(false);
            lb_NV_TrangThai.Text = "";
            btn_NV_Them.Enabled = true;
            btn_NV_Xoa.Enabled = false;
            btn_NV_Sua.Enabled = false;
        }

        private void btn_NV_Huy_Click(object sender, EventArgs e)
        {
            resetTextBox();
            enableControl(false);
            lb_NV_TrangThai.Text = "";
            btn_NV_Xoa.Enabled = false;
            btn_NV_Sua.Enabled = false;
            errNhanVien.Clear();
            btn_NV_Them.Enabled = true;
        }

    }
}
