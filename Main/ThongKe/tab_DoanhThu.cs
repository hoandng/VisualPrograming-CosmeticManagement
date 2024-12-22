using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Main.ThongKe
{
    public partial class ThongKe : Form
    {
        private void tabControl1_Enter(object sender, EventArgs e)
        {
            Load_DoanhThu();
            fill_Nam();
        }

        public void Load_DoanhThu(int? nam = null)
        {
            nam = nam == null ? DateTime.Today.Year : nam;
            string query = @"
            SELECT 
                MONTH(hd.NgayBan) AS Thang,
                YEAR(hd.NgayBan) AS Nam,
                COUNT(DISTINCT hd.SoHDB) AS SoHoaDon,
                SUM(cthd.SoLuong) AS SoLuongBan,
                SUM(cthd.ThanhTien) AS DoanhThu,
                SUM((cthd.ThanhTien - (cthd.SoLuong * hh.DonGiaNhap))) AS LoiNhuan
            FROM HoaDonBan hd
            JOIN ChiTietHDB cthd ON hd.SoHDB = cthd.SoHDB
            JOIN HangHoa hh ON cthd.MaHang = hh.MaHang
            WHERE YEAR(hd.NgayBan) = @nam
            GROUP BY MONTH(hd.NgayBan), YEAR(hd.NgayBan)
            ORDER BY Nam DESC, Thang DESC";

            var parameters = new Dictionary<string, object>
            {
                {"@nam", nam }
            };
            DataTable dt = _data.ExecuteQuery(query, parameters);
            dgv_DoanhThu.DataSource = dt;


            // Thiết lập tiêu đề cột cho dễ hiểu
            dgv_DoanhThu.Columns["Thang"].HeaderText = "Tháng";
            dgv_DoanhThu.Columns["Nam"].HeaderText = "Năm";
            dgv_DoanhThu.Columns["SoHoaDon"].HeaderText = "Số hoá đơn đã bán";
            dgv_DoanhThu.Columns["SoLuongBan"].HeaderText = "Số Lượng Bán";
            dgv_DoanhThu.Columns["DoanhThu"].HeaderText = "Doanh Thu";
            dgv_DoanhThu.Columns["LoiNhuan"].HeaderText = "Lợi Nhuận";

            // Định dạng cột Doanh Thu và Lợi Nhuận để hiển thị dưới dạng tiền tệ
            dgv_DoanhThu.Columns["DoanhThu"].DefaultCellStyle.Format = "C0";
            dgv_DoanhThu.Columns["LoiNhuan"].DefaultCellStyle.Format = "C0";

            // Tự động điều chỉnh kích thước cột
            dgv_DoanhThu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        private void fill_Nam()
        {
            string query = "SELECT DISTINCT YEAR(NgayBan) AS NAM FROM [HoaDonBan] ORDER BY YEAR(NgayBan) DESC";
            DataTable dataTable = _data.ExecuteQuery(query);

            cb_Nam.DataSource = dataTable;
            cb_Nam.DisplayMember = "NAM"; // Tên cột hiển thị
            cb_Nam.ValueMember = "NAM";
        }
        private void cb_Nam_SelectedIndexChanged(object sender, EventArgs e)
        {
            int? nam = cb_Nam.SelectedValue as int?;
            Load_DoanhThu(nam);
        }

    }
}
