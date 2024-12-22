using ClosedXML.Excel;
using Main.ChiTiet;
using Main.HoaDonBan;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Main.HoaDonNhap
{
    public partial class fHDN : Form
    {
        ProcessDatabase _data;
        int _mode = 0;
        public fHDN()
        {
            _data = new ProcessDatabase();
            InitializeComponent();
        }
        private void fHDN_Load(object sender, EventArgs e)
        {
            fill_NhanVien();
            fill_NhaCC();

            Load_HDN();
            resetTextBox();
            enableControls(false);
            btn_Sua.Enabled = false;
            btn_Xoa.Enabled = false;
            btn_ChiTiet.Enabled = false;
        }

        private void Load_HDN()
        {
            string query = @"
            SELECT hdn.SoHDN, hdn.MaNV, hdn.NgayNhap, hdn.MaNCC, 
                   COALESCE(SUM(cthd.ThanhTien), 0) AS TongTien
            FROM HoaDonNhap hdn
            LEFT JOIN ChiTietHDN cthd ON hdn.SoHDN = cthd.SoHDN
            GROUP BY hdn.SoHDN, hdn.MaNV, hdn.NgayNhap, hdn.MaNCC
            ORDER BY hdn.NgayNhap DESC";
            DataTable dataTable = _data.ExecuteQuery(query);
            dgv_HDN.DataSource = dataTable;

            dgv_HDN.Columns["TongTien"].DefaultCellStyle.Format = "C0";
            dgv_HDN.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataTable.Dispose();
        }

        private void fill_NhanVien()
        {
            string query = "SELECT MaNV, TenNV FROM [NhanVien]";
            DataTable dataTable = _data.ExecuteQuery(query);

            cb_MaNV.DataSource = dataTable;
            cb_MaNV.DisplayMember = "TenNV"; // Tên cột hiển thị
            cb_MaNV.ValueMember = "MaNV";     // Tên cột giá trị
        }

        private void fill_NhaCC()
        {
            string query = "SELECT MaNCC, TenNCC FROM [NhaCungCap]";
            DataTable dataTable = _data.ExecuteQuery(query);

            cb_MaNCC.DataSource = dataTable;
            cb_MaNCC.DisplayMember = "TenNCC"; // Tên cột hiển thị
            cb_MaNCC.ValueMember = "MaNCC";     // Tên cột giá trị
        }

        private void resetTextBox()
        {
            _mode = 0;
            txt_SoHDN.Text = "";
            cb_MaNV.Text = "";
            dtp_NgayNhap.Value = DateTime.Now;
            cb_MaNCC.Text = "";
            txt_TT.Text = "0";
            lb_TrangThai.Text = "";
        }

        private void enableControls(bool enable)
        {
            cb_MaNV.Enabled = enable;
            dtp_NgayNhap.Enabled = enable;
            cb_MaNCC.Enabled = enable;
            txt_TT.Enabled = enable;

            btn_Luu.Enabled = enable;
            btn_Huy.Enabled = enable;
        }

        private void enableAED(bool enable)
        {
            btn_Them.Enabled = enable;
            btn_Sua.Enabled = enable;
            btn_Xoa.Enabled = enable;

            btn_XuatFile.Enabled = enable;
            btn_ChiTiet.Enabled = enable;
        }


        private void dgv_HDN_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (_mode == 0 && e.RowIndex >= 0 && e.RowIndex < dgv_HDN.Rows.Count)
            {
                enableControls(false);
                enableAED(true);
                DataGridViewRow row = dgv_HDN.Rows[e.RowIndex];
                txt_SoHDN.Text = row.Cells["SoHDN"].Value.ToString();
                cb_MaNV.SelectedValue = row.Cells["MaNV"].Value.ToString();
                dtp_NgayNhap.Text = row.Cells["NgayNhap"].Value.ToString();
                cb_MaNCC.SelectedValue = row.Cells["MaNCC"].Value.ToString();
                txt_TT.Text = row.Cells["TongTien"].Value.ToString();
            }
        }

        private string autoGenerateId()
        {
            string query = "SELECT TOP 1 SoHDN FROM [HoaDonNhap] ORDER BY SoHDN DESC";
            object result = _data.ExecuteScalar(query);

            if (result != null && !string.IsNullOrEmpty(result.ToString()))
            {
                string currentMaHang = result.ToString();
                int numericPart = int.Parse(currentMaHang.Substring(3));
                numericPart++;

                return "HDN" + numericPart.ToString("D2");
            }
            else
            {
                return "HDN01";
            }
        }

        private void btn_Them_Click(object sender, EventArgs e)
        {
            enableControls(true);
            resetTextBox();
            enableAED(false);
            _mode = 1;
            lb_TrangThai.Text = "*Bạn đang ở chế độ Thêm!";
            txt_SoHDN.Text = autoGenerateId();
        }

        private void btn_Sua_Click(object sender, EventArgs e)
        {
            _mode = 2;
            enableControls(true);
            enableAED(false);
            lb_TrangThai.Text = "*Bạn đang ở chế độ Sửa!";
        }

        private void btn_Xoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn xóa Hoá đơn nhập có mã là " + txt_SoHDN.Text +
              " không ? Nếu có ấn nút Lưu, không Thì ấn nút Hủy", "Thông báo!",
               MessageBoxButtons.OK) == DialogResult.OK)
            {
                enableControls(false);
                enableAED(false);
                _mode = 03;
                lb_TrangThai.Text = "*Bạn đang ở chế độ Xóa";
                btn_Luu.Enabled = true;
                btn_Huy.Enabled = true;
            }
        }

        private void btn_Luu_Click(object sender, EventArgs e)
        {
            string sql = "";

            string sohdn = txt_SoHDN.Text;
            string manv = cb_MaNV.SelectedValue.ToString();
            string mancc = cb_MaNCC.SelectedValue.ToString();
            string ngaynhap = dtp_NgayNhap.Value.ToString("MM/dd/yyyy");
            string tongtien = txt_TT.Text;
            //Kiểm tra dữ liêu
            if (sohdn.Trim() == "")
            {
                errHDB.SetError(txt_SoHDN, "Mã không được để trống");
                return;
            }
            else
            {
                errHDB.Clear();
            }

            if (_mode == 1)
            {
                sql = "INSERT INTO [HoaDonNhap] (SoHDN, MaNV, MaNCC, NgayNhap, TongTien)";
                sql += "VALUES(@sohdn, @manv, @mancc, @ngaynhap, @tongtien);";
                var parameters = new Dictionary<string, object>
                {
                    {"@sohdn", sohdn},
                    {"@manv", manv},
                    {"@ngaynhap", ngaynhap},
                    {"@mancc", mancc},
                    {"@tongtien", tongtien},

                };
                _data.ExecuteNonQuery(sql, parameters);
            }


            //Nếu nút Sửa enable thì thực hiện cập nhật dữ liệu
            if (_mode == 2)
            {
                sql = "Update [HoaDonNhap] SET ";
                sql += $"MaNV = @manv, MaNCC = @mancc, NgayNhap = @ngaynhap, TongTien = @tongtien ";
                sql += $"WHERE SoHDN = @sohdn";
                var parameters = new Dictionary<string, object>
                {
                    {"@sohdn", sohdn},
                    {"@manv", manv},
                    {"@ngaynhap", ngaynhap},
                    {"@mancc", mancc},
                    {"@tongtien", tongtien},

                };
                _data.ExecuteNonQuery(sql, parameters);
            }

            //Nếu nút Xóa enable thì thực hiện xóa dữ liệu
            if (_mode == 3)
            {
                sql = $"Delete From [HoaDonNhap] Where SoHDN = @sohdn";
                var parameters = new Dictionary<string, object>
                {
                    {"@sohdn", sohdn},
                };
                _data.ExecuteNonQuery(sql, parameters);
            }

            Load_HDN();

            resetTextBox();
            enableControls(false);
            enableAED(false);
            btn_Them.Enabled = true;
            btn_XuatFile.Enabled = true;
        }

        private void btn_Huy_Click(object sender, EventArgs e)
        {
            resetTextBox();
            enableControls(false);
            enableAED(false);
            errHDB.Clear();
            btn_Them.Enabled = true;
            btn_XuatFile.Enabled = true;
        }

        private void btn_ChiTiet_Click(object sender, EventArgs e)
        {
            if (txt_SoHDN.Text.Trim() != null)
            {
                CT_HDN ct = new CT_HDN(txt_SoHDN.Text);
                ct.ShowDialog();
            }
        }

        private void btn_XuatFile_Click(object sender, EventArgs e)
        {
            // Sử dụng SaveFileDialog để cho phép người dùng chọn vị trí và tên tệp
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Excel Files|*.xlsx";
                saveFileDialog.Title = "Chọn vị trí lưu file Excel";
                saveFileDialog.FileName = "DanhSachHoaDonNhap.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;

                    // Truy vấn tất cả hóa đơn, lấy thêm tháng và năm để nhóm
                    string invoiceQuery = @"
                    SELECT SoHDN, NgayNhap, MaNV, MaNCC, TongTien, MONTH(NgayNhap) AS Thang, YEAR(NgayNhap) AS Nam 
                    FROM HoaDonNhap
                    ORDER BY Nam, Thang";

                    DataTable dtAllInvoices = _data.ExecuteQuery(invoiceQuery);

                    // Nhóm dữ liệu theo tháng và năm
                    var groupedInvoices = dtAllInvoices.AsEnumerable()
                        .GroupBy(row => new { Thang = row.Field<int>("Thang"), Nam = row.Field<int>("Nam") });

                    // Tạo tệp Excel với ClosedXML
                    using (var workbook = new XLWorkbook())
                    {
                        foreach (var group in groupedInvoices)
                        {
                            int thang = group.Key.Thang;
                            int nam = group.Key.Nam;

                            // Tạo worksheet cho mỗi tháng
                            var worksheet = workbook.Worksheets.Add($"Thang_{thang}_{nam}");

                            // Tạo tiêu đề cột cho bảng hóa đơn
                            worksheet.Cell(1, 1).Value = "Danh sách hóa đơn";
                            for (int i = 0; i < dtAllInvoices.Columns.Count - 2; i++) // Bỏ cột Thang và Nam
                            {
                                worksheet.Cell(2, i + 1).Value = dtAllInvoices.Columns[i].ColumnName;
                            }

                            // Ghi dữ liệu hóa đơn vào worksheet
                            int rowIndex = 3;
                            foreach (var row in group)
                            {
                                for (int colIndex = 0; colIndex < dtAllInvoices.Columns.Count - 2; colIndex++)
                                {
                                    worksheet.Cell(rowIndex, colIndex + 1).Value = row[colIndex].ToString();
                                }

                                // Lấy mã hóa đơn để truy vấn chi tiết hóa đơn
                                string soHDN = row["SoHDN"].ToString();

                                // Truy vấn chi tiết hóa đơn cho mã hóa đơn hiện tại
                                string detailQuery = @"
                                SELECT MaHang, SoLuong, GiamGia, ThanhTien 
                                FROM ChiTietHDN
                                WHERE SoHDN = @SoHDN";
                                var detailParams = new Dictionary<string, object> { { "@SoHDN", soHDN } };
                                DataTable dtInvoiceDetails = _data.ExecuteQuery(detailQuery, detailParams);

                                // Ghi tiêu đề cột cho chi tiết hóa đơn bên dưới hóa đơn tương ứng
                                int detailStartRow = rowIndex + 1;
                                worksheet.Cell(detailStartRow, 1).Value = "Chi tiết hóa đơn: " + soHDN;
                                for (int j = 0; j < dtInvoiceDetails.Columns.Count; j++)
                                {
                                    worksheet.Cell(detailStartRow + 1, j + 1).Value = dtInvoiceDetails.Columns[j].ColumnName;
                                }

                                // Ghi dữ liệu chi tiết hóa đơn
                                int detailRowIndex = detailStartRow + 2;
                                foreach (DataRow detailRow in dtInvoiceDetails.Rows)
                                {
                                    for (int j = 0; j < dtInvoiceDetails.Columns.Count; j++)
                                    {
                                        worksheet.Cell(detailRowIndex, j + 1).Value = detailRow[j].ToString();
                                    }
                                    detailRowIndex++;
                                }

                                // Tăng hàng tiếp theo cho hóa đơn
                                rowIndex = detailRowIndex + 1;
                            }

                            // Tự động điều chỉnh kích thước cột
                            worksheet.Columns().AdjustToContents();
                        }

                        // Lưu workbook vào vị trí đã chọn
                        workbook.SaveAs(filePath);
                        MessageBox.Show("Xuất dữ liệu thành công! Tệp đã được lưu tại: " + filePath);
                    }
                }
            }
        }
    }
}
