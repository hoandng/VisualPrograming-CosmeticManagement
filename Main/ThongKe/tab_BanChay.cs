using DocumentFormat.OpenXml.Bibliography;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Main.ThongKe
{
    public partial class ThongKe : Form
    {
        public void Load_MHBC(int? month  = null, int? year = null)
        {
            month = month == null ? DateTime.Today.Month : month;
            year = year == null ? DateTime.Today.Year : year;
            string query = @"
            SELECT TOP 5
                hh.MaHang,
                hh.TenHang,
                hsx.MaHangSX,
                hsx.TenHangSX,
                SUM(cthd.SoLuong) AS SoLuongDaBan,
                hh.Anh
            FROM ChiTietHDB cthd
            JOIN HoaDonBan hd ON cthd.SoHDB = hd.SoHDB
            JOIN HangHoa hh ON cthd.MaHang = hh.MaHang
            JOIN HangSX hsx ON hsx.MaHangSX = hh.MaHangSX
            WHERE MONTH(hd.NgayBan) = @Month AND YEAR(hd.NgayBan) = @Year
            GROUP BY hh.MaHang, hh.TenHang, hsx.MaHangSX, hsx.TenHangSX, hh.Anh
            ORDER BY SoLuongDaBan DESC";

            var parameters = new Dictionary<string, object>
            {
                { "@Month", month },
                { "@Year", year }
            };

            DataTable dataTable = _data.ExecuteQuery(query, parameters);

            if(dataTable.Rows.Count > 0)
            {
                dgv_MHBC.DataSource = dataTable;

                // Thiết lập tiêu đề cột cho dễ hiểu
                dgv_MHBC.Columns["MaHang"].HeaderText = "Mã Hàng";
                dgv_MHBC.Columns["TenHang"].HeaderText = "Tên Hàng";
                dgv_MHBC.Columns["MaHangSX"].HeaderText = "Mã Hãng SX";
                dgv_MHBC.Columns["TenHangSX"].HeaderText = "Tên Hãng SX";
                dgv_MHBC.Columns["SoLuongDaBan"].HeaderText = "Số Lượng Đã Bán";
                dgv_MHBC.Columns["Anh"].Visible = false;

                // Tự động điều chỉnh kích thước cột
                dgv_MHBC.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            else
            {
                MessageBox.Show("Không tìm thấy mặt hàng bán chạy", "Thông Báo", MessageBoxButtons.OK);
            }
            dataTable.Dispose();
        }

        public void resetValue()
        {
            txt_MaHang.Text = "";
            txt_TenHang.Text = "";
            txt_MaHangSX.Text = "";
            txt_TenHangSX.Text = "";
            txt_SL.Text = "";
        }
        public void fill_ComboBox()
        {

            // Thêm các tháng vào comboBoxMonth
            cb_MHBC_Thang.Items.Clear();
            for (int i = 1; i <= 12; i++)
            {
                cb_MHBC_Thang.Items.Add(i);
            }

            string query = "SELECT DISTINCT YEAR(NGAYBAN) AS NAM FROM [HoaDonBan] ORDER BY YEAR(NgayBan) DESC";
            DataTable dataTable = _data.ExecuteQuery(query);
            cb_MHBC_Nam.DataSource = dataTable;
            cb_MHBC_Nam.DisplayMember = "NAM"; // Tên cột hiển thị
            cb_MHBC_Nam.ValueMember = "NAM";

            cb_MHBC_Thang.SelectedItem = DateTime.Now.Month;
            cb_MHBC_Nam.SelectedItem = DateTime.Now.Year;
        }

        private void tp_MHBC_Enter(object sender, EventArgs e)
        {
            Load_MHBC();
            fill_ComboBox();
            resetValue();
        }

        private string getImagePath(string imageName)
        {
            // Hiển thị ảnh
            string projectDirectory = Directory.GetParent(Application.StartupPath).Parent.FullName;
            string imagesFolder = Path.Combine(projectDirectory, "Images", "Products");
            string imagePath = Path.Combine(imagesFolder, imageName);

            return imagePath;
        }


        private void dgv_MHBC_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgv_MHBC.Rows.Count)
            {
                DataGridViewRow row = dgv_MHBC.Rows[e.RowIndex];
                txt_MaHang.Text = row.Cells["MaHang"].Value.ToString();
                txt_TenHang.Text = row.Cells["TenHang"].Value.ToString();
                txt_MaHangSX.Text = row.Cells["MaHangSX"].Value.ToString();
                txt_TenHangSX.Text = row.Cells["TenHangSX"].Value.ToString();
                txt_SL.Text = row.Cells["SoLuongDaBan"].Value.ToString();

                string imagePath = getImagePath(row.Cells["Anh"].Value.ToString());
                if (File.Exists(imagePath))
                {
                    pb_AnhSP.ImageLocation = imagePath;
                }
            }
        }

        private void cb_MHBC_Thang_SelectedIndexChanged(object sender, EventArgs e)
        {
            int? thang = cb_MHBC_Thang.SelectedItem != null ? (int?)Convert.ToInt32(cb_MHBC_Thang.SelectedItem) : null;
            int? nam = cb_MHBC_Nam.SelectedValue != null ? cb_MHBC_Nam.SelectedValue as int? : null;

            if (thang.HasValue && nam.HasValue)
            {
                Load_MHBC(thang.Value, nam.Value);
            };
        }

        private void cb_MHBC_Nam_SelectedIndexChanged(object sender, EventArgs e)
        {
            int? thang = cb_MHBC_Thang.SelectedItem != null ? (int?)Convert.ToInt32(cb_MHBC_Thang.SelectedItem) : null;
            int? nam = cb_MHBC_Nam.SelectedValue != null ? cb_MHBC_Nam.SelectedValue as int? : null;

            if (thang.HasValue && nam.HasValue)
            {
                Load_MHBC(thang.Value, nam.Value);
            };
        }
    }
}
