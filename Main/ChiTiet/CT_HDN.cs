using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Main.ChiTiet
{
    public partial class CT_HDN : Form
    {
        ProcessDatabase _data;
        string CTHD_SoHDN;
        int _mode = 0;

        public CT_HDN(string SoHDN)
        {
            _data = new ProcessDatabase();
            CTHD_SoHDN = SoHDN;
            InitializeComponent();
        }
        private void CT_HDN_Load(object sender, EventArgs e)
        {
            //Load data thông tin chi tiết hoá đơn về nhân viên, khách hàng
            txt_SoHDN.Text = CTHD_SoHDN;
            string query = @"
                           SELECT nv.MaNV, nv.TenNV, ncc.MaNCC, ncc.TenNCC, ncc.DienThoai, hd.NgayNhap 
                           FROM [HoaDonNhap] hd
                           JOIN NhanVien nv ON hd.MaNV = nv.MaNV
                           JOIN NhaCungCap ncc ON hd.MaNCC = ncc.MaNCC
                           WHERE hd.SoHDN = @SoHDN";

            var parameters = new Dictionary<string, object>
            {
                { "@SoHDN", CTHD_SoHDN }
            };

            DataTable dataTable = _data.ExecuteQuery(query, parameters);

            if (dataTable.Rows.Count > 0)
            {
                // Đổ dữ liệu vào TextBox
                txt_MaNV.Text = dataTable.Rows[0]["MaNV"].ToString();
                txt_TenNV.Text = dataTable.Rows[0]["TenNV"].ToString();
                txt_MaNCC.Text = dataTable.Rows[0]["MaNCC"].ToString();
                txt_TenNCC.Text = dataTable.Rows[0]["TenNCC"].ToString();
                txt_SĐT.Text = dataTable.Rows[0]["DienThoai"].ToString();
                dtp_NgayNhap.Value = (DateTime)dataTable.Rows[0]["NgayNhap"];
            }
            else
            {
                MessageBox.Show("Không tìm thấy hóa đơn.");
            }

            Load_CTHDN();
            fill_HangHoa();
            resetValue();
            enableControls(false);
            btn_Sua.Enabled = false;
            btn_Xoa.Enabled = false;
        }

        private void resetValue()
        {
            _mode = 0;
            cb_MaHang.Text = "";
            cb_MaHang.SelectedIndex = -1;
            txt_TenHang.Text = "";
            txt_SL.Text = "1";
            txt_DonGia.Text = "";
            txt_GiamGia.Text = "0";
            txt_ThanhTien.Text = "";
            lb_TrangThai.Text = "";
        }

        private void enableControls(bool enable)
        {
            cb_MaHang.Enabled = enable;
            txt_SL.Enabled = enable;
            txt_GiamGia.Enabled = enable;
            txt_DonGia.Enabled = enable;

            btn_Luu.Enabled = enable;
            btn_Huy.Enabled = enable;
        }

        private void enableAED(bool enable)
        {
            btn_Them.Enabled = enable;
            btn_Sua.Enabled = enable;
            btn_Xoa.Enabled = enable;
        }

        private void Load_CTHDN()
        {
            string querry = "Select * From [ChiTietHDN ] Where SoHDN = @sohdn";
            var parameters = new Dictionary<string, object>{
                {"@sohdn", CTHD_SoHDN},
            };
            DataTable dataTable = _data.ExecuteQuery(querry, parameters);
            dgv_ChiTiet.DataSource = dataTable;
            dgv_ChiTiet.Columns["ThanhTien"].DefaultCellStyle.Format = "C0";

            dgv_ChiTiet.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataTable.Dispose();
        }

        private void fill_HangHoa()
        {
            string query = "SELECT MaHang, TenHang, DonGiaNhap FROM [HangHoa]";
            DataTable dataTable = _data.ExecuteQuery(query);

            cb_MaHang.DataSource = dataTable;
            cb_MaHang.DisplayMember = "MaHang"; // Tên cột hiển thị
            cb_MaHang.ValueMember = "MaHang";     // Tên cột giá trị
        }

        private void cb_MaHang_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Kiểm tra xem mục đã chọn có hợp lệ không
            if (cb_MaHang.SelectedValue != null)
            {
                // Lấy hàng tương ứng với mã hàng được chọn từ DataSource của ComboBox
                DataRowView selectedRow = cb_MaHang.SelectedItem as DataRowView;
                if (selectedRow != null)
                {
                    txt_TenHang.Text = selectedRow["TenHang"].ToString(); // Hiển thị tên hàng tương ứng
                    txt_DonGia.Text = selectedRow["DonGiaNhap"].ToString(); // Hiển thị tên hàng tương ứng
                }
            }
        }
        
        private void UpdateTongTien_HDN()
        {
            string query = @"
            UPDATE [HoaDonNhap]
            SET TongTien = (
                SELECT SUM(ThanhTien) 
                FROM [ChiTietHDN] cthd
                WHERE cthd.SoHDN = @sohdn
            )
            WHERE SoHDN = @sohdn";

            var parameters = new Dictionary<string, object>
            {
                { "@sohdn", CTHD_SoHDN },
            };
            _data.ExecuteQuery(query, parameters);
        }

        private void UpdateLatestPrice(string maHang)
        {
            string sql = @"
            SELECT TOP 1 cthn.DonGia
            FROM ChiTietHDN cthn
            JOIN HoaDonNhap hdn ON cthn.SoHDN = hdn.SoHDN
            WHERE cthn.MaHang = @MaHang
            ORDER BY hdn.NgayNhap DESC";

            var parameters = new Dictionary<string, object>
            {
                {"@MaHang", maHang}
            };

            ProcessDatabase db = new ProcessDatabase();
            object result = db.ExecuteScalar(sql, parameters);

            // Kiểm tra nếu lấy được đơn giá nhập mới nhất
            if (result != null)
            {
                decimal latestPrice = Convert.ToDecimal(result);

                // Cập nhật đơn giá nhập trong bảng HangHoa
                string updateSql = "UPDATE HangHoa SET DonGiaNhap = @DonGiaNhap WHERE MaHang = @MaHang";
                var updateParameters = new Dictionary<string, object>
                {
                    {"@DonGiaNhap", latestPrice},
                    {"@MaHang", maHang}
                };
                db.ExecuteNonQuery(updateSql, updateParameters);
            }
        }
        
        private void UpdateProductQuantity(string maHang, int quantityChange)
        {
            string query = @"
            UPDATE HangHoa
            SET SoLuong = SoLuong + @QuantityChange
            WHERE MaHang = @MaHang";

            var parameters = new Dictionary<string, object>
            {
                { "@QuantityChange", quantityChange },
                { "@MaHang", maHang }
            };

            _data.ExecuteNonQuery(query, parameters);
        }

        private void dgv_ChiTiet_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (_mode == 0 && e.RowIndex >= 0 && e.RowIndex < dgv_ChiTiet.Rows.Count)
            {
                enableControls(false);
                btn_Them.Enabled = true;
                btn_Sua.Enabled = true;
                btn_Xoa.Enabled = true;
                DataGridViewRow row = dgv_ChiTiet.Rows[e.RowIndex];
                cb_MaHang.Text = row.Cells["MaHang"].Value.ToString();
                txt_SL.Text = row.Cells["SoLuong"].Value.ToString();
                txt_GiamGia.Text = row.Cells["GiamGia"].Value.ToString();
                txt_ThanhTien.Text = row.Cells["ThanhTien"].Value.ToString();
                txt_DonGia.Text = row.Cells["DonGia"].Value.ToString();
            }
        }

        private void btn_Them_Click(object sender, EventArgs e)
        {
            enableControls(true);
            enableAED(false);
            resetValue();
            _mode = 1;
            lb_TrangThai.Text = "Bạn đang ở chế độ Thêm";
        }

        private void btn_Sua_Click(object sender, EventArgs e)
        {
            enableControls(true);
            enableAED(false);
            _mode = 2;
            cb_MaHang.Enabled = false;
            lb_TrangThai.Text = "Bạn đang ở chế độ Sửa";
        }

        private void btn_Xoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn xóa hãng sản xuất có mã là " + cb_MaHang.Text +
                " không ? Nếu có ấn nút Lưu, không thì ấn nút Hủy", "Xóa sản phẩm",
            MessageBoxButtons.OK) == DialogResult.OK)
            {
                enableControls(false);
                enableAED(false);
                _mode = 3;
                lb_TrangThai.Text = "Bạn đang ở chế độ Xoá";
                btn_Luu.Enabled = true;
                btn_Huy.Enabled = true;
            }
        }

        private void btn_Thoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_Luu_Click(object sender, EventArgs e)
        {
            string sql = "";

            string mahang = "";
            string sl = txt_SL.Text;
            string dongia = txt_DonGia.Text;
            string giamgia = txt_GiamGia.Text;
            string thanhtien = TinhTien().ToString();

            if (cb_MaHang.SelectedIndex == -1)
            {
                errChiTiet.SetError(cb_MaHang, "Phải chọn một hàng hoá");
                return;
            }
            else
            {
                mahang = cb_MaHang.SelectedValue.ToString();
                errChiTiet.Clear();
            }

            // Kiểm tra dữ liệu số lượng
            int quantity;
            if (!int.TryParse(sl, out quantity) || quantity <= 0)
            {
                errChiTiet.SetError(txt_SL, "Số lượng phải là một số nguyên dương");
                return;
            }
            else
            {
                errChiTiet.SetError(txt_SL, "");
            }

            // Kiểm tra đơn giá
            decimal price;
            if (!decimal.TryParse(dongia, out price) || price <= 0)
            {
                errChiTiet.SetError(txt_DonGia, "Đơn giá phải là một số dương");
                return;
            }
            else
            {
                errChiTiet.SetError(txt_DonGia, "");
            }

            // Kiểm tra giảm giá
            int discount;
            if (giamgia.Trim().Length > 0)
            {
                if (!int.TryParse(giamgia, out discount) || discount < 0 || discount > 100)
                {
                    errChiTiet.SetError(txt_GiamGia, "Giảm giá phải là một số từ 0 đến 100");
                    return;
                }
                else
                {
                    errChiTiet.SetError(txt_GiamGia, "");
                }
            }
            else
            {
                discount = 0;
            }

            if (_mode == 1)
            {
                // Kiểm tra xem mặt hàng đã tồn tại trong hóa đơn nhập chưa
                sql = "SELECT COUNT(*) FROM [ChiTietHDN] WHERE SoHDN = @sohdn AND MaHang = @mahang;";
                var parameters = new Dictionary<string, object>
                {
                    {"@sohdn", CTHD_SoHDN},
                    {"@mahang", mahang},
                };
                int count = Convert.ToInt32(_data.ExecuteScalar(sql, parameters));
                if (count > 0)
                {
                    MessageBox.Show($"Đã tồn tại hàng hóa với mã {mahang} trong hóa đơn nhập {CTHD_SoHDN}", "Thông báo", MessageBoxButtons.OK);
                    return;
                }

                // Thêm chi tiết hóa đơn nhập
                sql = "INSERT INTO [ChiTietHDN] (SoHDN, MaHang, SoLuong, DonGia, GiamGia, ThanhTien)";
                sql += "VALUES(@sohdn, @mahang, @soluong, @dongia, @giamgia, @thanhtien);";
                parameters = new Dictionary<string, object>
                {
                    {"@sohdn", CTHD_SoHDN},
                    {"@mahang", mahang},
                    {"@soluong", sl},
                    {"@dongia", dongia},
                    {"@giamgia", giamgia},
                    {"@thanhtien", thanhtien},
                };
                _data.ExecuteNonQuery(sql, parameters);

                // Tăng số lượng hàng hóa trong kho
                UpdateProductQuantity(mahang, quantity);

                // Cập nhật giá nhập mới nhất
                UpdateLatestPrice(mahang);
            }
            //mahang = cb_MaHang.SelectedValue.ToString();
            // Nếu nút Sửa enable thì thực hiện cập nhật dữ liệu
            if (_mode == 2)
            {
                // Lấy số lượng hiện tại từ chi tiết hóa đơn nhập
                sql = "SELECT SoLuong FROM [ChiTietHDN] WHERE SoHDN = @sohdn AND MaHang = @mahang";
                var selectParameters = new Dictionary<string, object>
                {
                    {"@sohdn", CTHD_SoHDN},
                    {"@mahang", mahang}
                };
                int oldQuantity = Convert.ToInt32(_data.ExecuteScalar(sql, selectParameters));

                // Tính sự chênh lệch giữa số lượng cũ và số lượng mới
                int quantityDifference = quantity - oldQuantity;

                // Cập nhật chi tiết hóa đơn nhập
                sql = "UPDATE [ChiTietHDN] SET ";
                sql += "SoLuong = @soluong, DonGia = @dongia, GiamGia = @giamgia, ThanhTien = @thanhtien ";
                sql += "WHERE SoHDN = @sohdn AND MaHang = @mahang";
                var updateParameters = new Dictionary<string, object>
                {
                    {"@sohdn", CTHD_SoHDN},
                    {"@mahang", mahang},
                    {"@soluong", sl},
                    {"@dongia", dongia},
                    {"@giamgia", giamgia},
                    {"@thanhtien", thanhtien}
                };
                _data.ExecuteNonQuery(sql, updateParameters);

                // Cập nhật số lượng hàng hóa trong kho
                UpdateProductQuantity(mahang, quantityDifference);

                // Cập nhật giá nhập mới nhất
                UpdateLatestPrice(mahang);
            }

            // Nếu nút Xóa enable thì thực hiện xóa dữ liệu
            if (_mode == 3)
            {
                // Lấy số lượng hiện tại từ chi tiết hóa đơn nhập
                sql = "SELECT SoLuong FROM [ChiTietHDN] WHERE SoHDN = @sohdn AND MaHang = @mahang";
                var deleteParameters = new Dictionary<string, object>
                {
                    {"@sohdn", CTHD_SoHDN},
                    {"@mahang", mahang}
                };
                int deleteQuantity = Convert.ToInt32(_data.ExecuteScalar(sql, deleteParameters));

                // Xóa chi tiết hóa đơn nhập
                sql = "DELETE FROM [ChiTietHDN] WHERE SoHDN = @sohdn AND MaHang = @mahang";
                _data.ExecuteNonQuery(sql, deleteParameters);

                // Giảm số lượng hàng hóa trong kho
                UpdateProductQuantity(mahang, -deleteQuantity);

                // Cập nhật giá nhập mới nhất
                UpdateLatestPrice(mahang);
            }

            // Tải lại chi tiết hóa đơn nhập và cập nhật tổng tiền
            Load_CTHDN();
            UpdateTongTien_HDN();

            // Đặt lại trạng thái của các điều khiển
            resetValue();
            enableControls(false);
            enableAED(false);
            btn_Them.Enabled = true;
        }

        private void btn_Huy_Click(object sender, EventArgs e)
        {
            resetValue();
            enableControls(false);
            enableAED(false);
            errChiTiet.Clear();
            lb_TrangThai.Text = "";
            btn_Them.Enabled = true;
        }

        private decimal TinhTien()
        {
            string sl = txt_SL.Text;
            string giamgia = txt_GiamGia.Text;
            string dongia = txt_DonGia.Text;

            // Kiểm tra dữ liệu số lượng
            int quantity;
            if (!int.TryParse(sl, out quantity) || quantity <= 0)
            {
                errChiTiet.SetError(txt_SL, "Số lượng phải là một số nguyên dương");
                return 0;
            }
            else
            {
                errChiTiet.SetError(txt_SL, "");
            }

            // Kiểm tra đơn giá
            decimal price;
            if (!decimal.TryParse(dongia, out price) || price <= 0)
            {
                errChiTiet.SetError(txt_DonGia, "Đơn giá phải là một số dương");
                return 0;
            }
            else
            {
                errChiTiet.SetError(txt_DonGia, "");
            }

            // Kiểm tra giảm giá
            int discount;
            if (giamgia.Trim().Length > 0)
            {
                if (!int.TryParse(giamgia, out discount) || discount < 0 || discount > 100)
                {
                    errChiTiet.SetError(txt_GiamGia, "Giảm giá phải là một số từ 0 đến 100");
                    return 0;
                }
                else
                {
                    errChiTiet.SetError(txt_GiamGia, "");
                }
            }
            else
            {
                discount = 0;
            }
            decimal total = price * quantity * (100 - discount) / 100;
            txt_ThanhTien.Text = total.ToString(); // Hiển thị số tiền với dấu phân cách hàng nghìn
            return total;
        }
        private void btn_TinhTien_Click_1(object sender, EventArgs e)
        {
            txt_ThanhTien.Text = TinhTien().ToString();
        }
    }
}
