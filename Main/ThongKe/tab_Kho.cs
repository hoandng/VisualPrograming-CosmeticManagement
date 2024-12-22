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
    partial class ThongKe : Form
    {
        private void dgv_HTK_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgv_HTK.Rows.Count)
            {
                DataGridViewRow row = dgv_HTK.Rows[e.RowIndex];
                txt_Kho_MaHang.Text = row.Cells["MaHang"].Value.ToString();
                txt_Kho_TenHang.Text = row.Cells["TenHang"].Value.ToString();
                txt_Kho_MaHangSX.Text = row.Cells["MaHangSX"].Value.ToString();
                txt_Kho_TenHangSX.Text = row.Cells["TenHangSX"].Value.ToString();
                txt_Kho_SL.Text = row.Cells["SoLuong"].Value.ToString();
                
                string imagePath = getImagePath(row.Cells["Anh"].Value.ToString());
                if (File.Exists(imagePath))
                {
                    pb_Kho_Anh.ImageLocation = imagePath;
                }
            }
        }

        private void cb_TrangThai_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedStatus = cb_TrangThai.SelectedItem.ToString();
            string query = "";
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (selectedStatus == "Tồn kho")
            {
                query = "SELECT hh.MaHang, hh.TenHang, hh.MaHangSX, hsx.TenHangSX, hh.SoLuong, hh.Anh " +
                        "From HangHoa hh " +
                        "JOIN HangSX hsx ON hsx.MaHangSX = hh.MaHangSX " +
                        "WHERE hh.SoLuong > 1 ORDER BY hh.SoLuong DESC";
            }
            else if (selectedStatus == "Sắp hết")
            {
                query = "SELECT hh.MaHang, hh.TenHang, hh.MaHangSX, hsx.TenHangSX, hh.SoLuong, hh.Anh " +
                        "From HangHoa hh " +
                        "JOIN HangSX hsx ON hsx.MaHangSX = hh.MaHangSX " +
                        "WHERE SoLuong > 0 AND SoLuong < 10";
            }
            else if (selectedStatus == "Đã hết")
            {
                query = "SELECT hh.MaHang, hh.TenHang, hh.MaHangSX, hsx.TenHangSX, hh.SoLuong, hh.Anh " +
                        "From HangHoa hh " +
                        "JOIN HangSX hsx ON hsx.MaHangSX = hh.MaHangSX " +
                        "WHERE hh.SoLuong = 0";
            }

            // Thực hiện truy vấn và lấy dữ liệu
            DataTable dataTable = _data.ExecuteQuery(query, parameters);

            // Gán dữ liệu vào DataGridView
            dgv_HTK.DataSource = dataTable;

            // Tự động điều chỉnh kích thước cột để vừa với `DataGridView`
            dgv_HTK.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }


        private void tp_Kho_Enter(object sender, EventArgs e)
        {
            cb_TrangThai.SelectedIndex = 0;
        }
    }
}
