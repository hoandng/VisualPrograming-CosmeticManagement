using DocumentFormat.OpenXml.Office.Word;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Main.HangHoa
{
    public partial class Them_HangHoa : Form
    {
        ProcessDatabase _data;
        bool isChangePicture = false;

        public Them_HangHoa()
        {
            _data = new ProcessDatabase();
            InitializeComponent();
        }

        private void Them_HangHoa_Load(object sender, EventArgs e)
        {
            load_Comboboxs();
            txt_MaHang.Text = autoGenerateId().ToString();
        }

        private void load_Comboboxs()
        {
            // Load Chất liệu
            string query = "SELECT MaChatLieu, TenChatLieu FROM [ChatLieu]";
            DataTable dataTable = _data.ExecuteQuery(query);
            cb_CL.DataSource = dataTable;
            cb_CL.DisplayMember = "TenChatLieu"; // Tên cột hiển thị
            cb_CL.ValueMember = "MaChatLieu";     // Tên cột giá trị

            // Load Cong dung
            query = "SELECT MaCongDung, TenCongDung FROM [CongDung]";
            dataTable = _data.ExecuteQuery(query);
            cb_CD.DataSource = dataTable;
            cb_CD.DisplayMember = "TenCongDung";
            cb_CD.ValueMember = "MaCongDung";

            // Load Khối lượng
            query = "SELECT MaKhoiLuong, TenKhoiLuong FROM [KhoiLuong]";
            dataTable = _data.ExecuteQuery(query);
            cb_KL.DataSource = dataTable;
            cb_KL.DisplayMember = "TenKhoiLuong";
            cb_KL.ValueMember = "MaKhoiLuong";

            // Load loại
            query = "SELECT MaLoai, TenLoai FROM [Loai]";
            dataTable = _data.ExecuteQuery(query);
            cb_Loai.DataSource = dataTable;
            cb_Loai.DisplayMember = "TenLoai";
            cb_Loai.ValueMember = "MaLoai";


            //load Màu
            query = "SELECT MaMau, TenMau FROM [MauSac]";
            dataTable = _data.ExecuteQuery(query);
            cb_Mau.DataSource = dataTable;
            cb_Mau.DisplayMember = "TenMau";
            cb_Mau.ValueMember = "MaMau";

            // load mùa
            query = "SELECT MaMua, TenMua FROM [Mua]";
            dataTable = _data.ExecuteQuery(query);
            cb_Mua.DataSource = dataTable;
            cb_Mua.DisplayMember = "TenMua";
            cb_Mua.ValueMember = "MaMua";

            //load nước sản xuất
            query = "SELECT MaNuocSX, TenNuocSX FROM [NuocSX]";
            dataTable = _data.ExecuteQuery(query);
            cb_NuocSX.DataSource = dataTable;
            cb_NuocSX.DisplayMember = "TenNuocSX";
            cb_NuocSX.ValueMember = "MaNuocSX";

            // load hãng sản xuất
            query = "SELECT MaHangSX, TenHangSX FROM [HangSX]";
            dataTable = _data.ExecuteQuery(query);
            cb_Hang.DataSource = dataTable;
            cb_Hang.DisplayMember = "TenHangSX";
            cb_Hang.ValueMember = "MaHangSX";

        }

        private void resetValues()
        {
            txt_TenHang.Text = "";
            cb_Loai.Text = "";
            cb_Hang.Text = "";
            cb_KL.Text = "";
            cb_CL.Text = "";
            cb_CD.Text = "";
            cb_Mua.Text = "";
            cb_Mau.Text = "";
            cb_NuocSX.Text = "";
            txt_SL.Text = "0";
            txt_GiaNhap.Text = "0";
            txt_GiaBan.Text = "0";
            txt_GhiChu.Text = "";
            txt_BH.Text = "12";
            pictureBox_AnhSP.ImageLocation = "";
        }

        private string autoGenerateId()
        {
            // Lấy mã hàng lớn nhất hiện tại
            string query = "SELECT TOP 1 MaHang FROM [HangHoa] ORDER BY MaHang DESC";
            object result = _data.ExecuteScalar(query);

            if (result != null && !string.IsNullOrEmpty(result.ToString()))
            {
                // Giả sử mã hàng có dạng "H01", "H02", ..., bạn sẽ cần tách phần số và tăng giá trị lên
                string currentMaHang = result.ToString();
                int numericPart = int.Parse(currentMaHang.Substring(1)); // Bỏ ký tự đầu ("H") và lấy phần số
                numericPart++; // Tăng giá trị phần số lên 1

                // Tạo mã hàng mới với định dạng "H" + số mới có 2 chữ số (ví dụ: H01, H02, ...)
                return "H" + numericPart.ToString("D2");
            }
            else
            {
                // Nếu chưa có mã hàng nào trong cơ sở dữ liệu, bắt đầu từ "H01"
                return "H01";
            }
        }
       

        private string SaveImageToFolder(string imagePath, string maHang)
        {
            // Kiểm tra nếu ảnh chưa được chọn hoặc mã sản phẩm rỗng
            if (string.IsNullOrEmpty(imagePath) || string.IsNullOrEmpty(maHang))
            {
                MessageBox.Show("Vui lòng chọn ảnh và nhập mã sản phẩm.");
                return null;
            }

            // Đường dẫn thư mục "Images" nằm ở thư mục gốc của dự án
            string projectDirectory = Directory.GetParent(Application.StartupPath).Parent.FullName;
            string imagesFolder = Path.Combine(projectDirectory, "Images", "Products");

            // Tạo thư mục "Images" nếu chưa tồn tại
            if (!Directory.Exists(imagesFolder))
            {
                Directory.CreateDirectory(imagesFolder);
            }

            // Lấy phần mở rộng của tệp ảnh gốc
            string fileExtension = Path.GetExtension(imagePath);
            // Tạo tên tệp ảnh mới dựa trên mã sản phẩm
            string newFileName = $"{maHang}{fileExtension}";
            string destinationFilePath = Path.Combine(imagesFolder, newFileName);

            try
            {
                // Sao chép ảnh vào thư mục "Images" và ghi đè nếu đã tồn tại
                File.Copy(imagePath, destinationFilePath, true);
                return newFileName; // Trả về tên file để lưu vào cơ sở dữ liệu
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi lưu ảnh: " + ex.Message);
                return null;
            }
        }


        private void btn_TaiAnh_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            openFileDialog.Title = "Chọn ảnh";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Chỉ hiển thị ảnh trong PictureBox, không lưu vào thư mục
                pictureBox_AnhSP.ImageLocation = openFileDialog.FileName;
                isChangePicture = true;
            }
        }

        private void btn_Luu_Click(object sender, EventArgs e)
        {
            string sql = "";

            string mahang = txt_MaHang.Text;
            string tenhang = txt_TenHang.Text;
            string mahangsx = cb_Hang.SelectedValue.ToString();
            string maloai = cb_Loai.SelectedValue.ToString();
            string mamau = cb_Mau.SelectedValue.ToString();
            string makl = cb_KL.SelectedValue.ToString();
            string macl = cb_CL.SelectedValue.ToString();
            string macd = cb_CD.SelectedValue.ToString();
            string mansx = cb_NuocSX.SelectedValue.ToString();
            string mamua = cb_Mua.SelectedValue.ToString();
            string sl = txt_SL.Text;
            string bh = txt_BH.Text;
            string giaban = txt_GiaBan.Text;
            string gianhap = txt_GiaNhap.Text;
            string ghichu = txt_GhiChu.Text;
            string anh = "";
            if (isChangePicture)
            {
                string imagePath = pictureBox_AnhSP.ImageLocation;
                anh = SaveImageToFolder(imagePath, mahang);
            }

            // Kiểm tra dữ liệu
            if (txt_TenHang.Text.Trim() == "")
            {
                errHangHoa.SetError(txt_TenHang, "Bạn không để trống tên sản phẩm!");
                return;
            }
            else
            {
                errHangHoa.Clear();
            }
            int soluong;
            decimal hdb;
            if (!int.TryParse(sl, out soluong) || soluong < 0)
            {
                errHangHoa.SetError(txt_SL, "Số lượng phải là một số!");
                return;
            }
            else
            {
                errHangHoa.Clear();
            }
            if (!decimal.TryParse(giaban, out hdb))
            {
                errHangHoa.SetError(txt_GiaBan, "Giá bán phải là một số!");
                return;
            }
            else
            {
                errHangHoa.Clear();
            }

            sql = "INSERT INTO [HangHoa] (MaHang, TenHang, MaLoai, MaKhoiLuong, MaHangSX, MaChatLieu, MaCongDung," +
                      "MaMau, MaNuocSX, MaMua, SoLuong, DonGiaNhap, DonGiaBan, ThoiGianBaoHanh, GhiChu, Anh)";
            sql += "VALUES(@mahang,@tenhang, @maloai, @makl, @mahsx, @macl, @macd," +
                   "@mamau, @mansx, @mamua, @sl, @gianhap, @giaban, @bh, @ghichu, @anh);";
            var parameters = new Dictionary<string, object>
                {
                    {"@mahang", mahang},
                    {"@tenhang", tenhang},
                    {"@maloai", maloai},
                    {"@makl", makl},
                    {"@mahsx", mahangsx},
                    {"@macl", macl},
                    {"@macd", macd},
                    {"@mamau", mamau},
                    {"@mansx", mansx},
                    {"@mamua", mamua},
                    {"@sl", sl},
                    {"@gianhap", gianhap},
                    {"@giaban", giaban},
                    {"@bh", bh},
                    {"@ghichu", ghichu},
                    {"@anh", anh},
                };
            int count = _data.ExecuteNonQuery(sql, parameters);
            if (count > 0)
            {
                MessageBox.Show("Thêm mới một Hàng Hoá thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        private void btn_Huy_Click(object sender, EventArgs e)
        {
            resetValues();
        }

        private void btn_Thoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
