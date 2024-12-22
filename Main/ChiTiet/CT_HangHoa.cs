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

namespace Main.ChiTiet
{
    public partial class CT_HangHoa : Form
    {
        ProcessDatabase _data;
        bool isChangePicture = false;
        string id_Products = "";
        string oldImageName = "";

        public CT_HangHoa(string id)
        {
            id_Products = id;
            _data = new ProcessDatabase();
            InitializeComponent();
        }
        private void CT_HangHoa_Load(object sender, EventArgs e)
        {
            load_Comboboxs();
            Load_CTHH();
            enableControls(false);
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

        private void Load_CTHH()
        {
            string query = "Select * from [HangHoa] Where MaHang = @mahang";
            var parameters = new Dictionary<string, object>
            {
                { "@mahang", id_Products},
            };

            DataTable dataTable = _data.ExecuteQuery(query, parameters);

            if (dataTable.Rows.Count > 0)
            {
                // Đổ dữ liệu vào TextBox
                txt_MaHang.Text = dataTable.Rows[0]["MaHang"].ToString();
                txt_TenHang.Text = dataTable.Rows[0]["TenHang"].ToString();
                txt_SL.Text = dataTable.Rows[0]["SoLuong"].ToString() ?? "0";
                txt_GiaNhap.Text = dataTable.Rows[0]["DonGiaNhap"].ToString() ?? "0";
                txt_GiaBan.Text = dataTable.Rows[0]["DonGiaBan"].ToString() ?? "0";
                txt_BH.Text = dataTable.Rows[0]["ThoiGianBaoHanh"].ToString() ?? "12";
                txt_GhiChu.Text = dataTable.Rows[0]["GhiChu"].ToString();

                cb_Loai.SelectedValue = dataTable.Rows[0]["MaLoai"].ToString() ?? "Không";
                cb_Hang.SelectedValue = dataTable.Rows[0]["MaHangSX"].ToString() ?? "Không";
                cb_KL.SelectedValue = dataTable.Rows[0]["MaKhoiLuong"].ToString() ?? "Không";
                cb_CL.SelectedValue = dataTable.Rows[0]["MaChatLieu"].ToString() ?? "Không";
                cb_CD.SelectedValue = dataTable.Rows[0]["MaCongDung"].ToString() ?? "Không";
                cb_Mua.SelectedValue = dataTable.Rows[0]["MaMua"].ToString() ?? "Không";
                cb_Mau.SelectedValue = dataTable.Rows[0]["MaMau"].ToString() ?? "Không";
                cb_NuocSX.SelectedValue = dataTable.Rows[0]["MaNuocSX"].ToString() ?? "Không";

                // Lấy tên ảnh trong db
                oldImageName = dataTable.Rows[0]["Anh"].ToString();
                string imagePath = getImagePath(oldImageName);
                if (File.Exists(imagePath))
                {
                    pictureBox_AnhSP.ImageLocation = imagePath;
                }
                
            }
            else
            {
                MessageBox.Show("Không tìm thấy Hàng Hoá.");
            }
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
            txt_GiaBan.Text = "0";
            txt_GhiChu.Text = "";
            txt_BH.Text = "12";
            pictureBox_AnhSP.ImageLocation = "";
        }

        private void enableControls(bool tf)
        {
            txt_TenHang.Enabled = tf;
            cb_Loai.Enabled = tf;
            cb_Hang.Enabled = tf;
            cb_KL.Enabled = tf;
            cb_CL.Enabled = tf;
            cb_CD.Enabled = tf;
            cb_Mua.Enabled = tf;
            cb_Mau.Enabled = tf;
            cb_NuocSX.Enabled = tf;
            txt_GiaBan.Enabled = tf;
            txt_GhiChu.Enabled = tf;
            txt_BH.Enabled = tf;

            btn_TaiAnh.Enabled = tf;
            btn_Huy.Enabled = tf;
            btn_Luu.Enabled = tf;
        }

        private string getImagePath(string imageName)
        {
            // Hiển thị ảnh
            string projectDirectory = Directory.GetParent(Application.StartupPath).Parent.FullName;
            string imagesFolder = Path.Combine(projectDirectory, "Images", "Products");
            string imagePath = Path.Combine(imagesFolder, imageName);
            
            return imagePath;
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
            string imagesFolder = Path.Combine(projectDirectory, "Images","Products");
            
            /*Console.WriteLine(Application.StartupPath);
            Console.WriteLine(Directory.GetParent(Application.StartupPath));
            Console.WriteLine(Directory.GetParent(Application.StartupPath).Parent);
            Console.WriteLine(Directory.GetParent(Application.StartupPath).Parent.FullName);*/

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

        private void btn_Sua_Click(object sender, EventArgs e)
        {
            //resetValues();
            enableControls(true);
            btn_Sua.Enabled = false;
        }

        private void btn_Luu_Click(object sender, EventArgs e)
        {
            string sql = "";

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
            // Lấy đường dẫn của ảnh từ PictureBox
            if (isChangePicture)
            {
                string imagePath = pictureBox_AnhSP.ImageLocation;
                anh = SaveImageToFolder(imagePath, id_Products);
            }
            else
            {
                anh = oldImageName;
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

            sql = "Update [HangHoa] SET ";
            sql += "TenHang = @tenhang, " +
                   "MaLoai = @maloai," +
                   "MaKhoiLuong = @makl," +
                   "MaHangSX = @mahsx, " +
                   "MaChatLieu = @macl," +
                   "MaCongDung = @macd," +
                   "MaMau = @mamau," +
                   "MaNuocSX = @mansx," +
                   "MaMua = @mamua," +
                   "SoLuong = @sl," +
                   "DonGiaBan = @giaban," +
                   "DonGiaNhap = @gianhap," +
                   "ThoiGianBaoHanh = @bh," +
                   "GhiChu = @ghichu," +
                   "Anh = @anh";
            sql += " WHERE MaHang = @mahang";
            var parameters = new Dictionary<string, object>
                {
                    {"@mahang", id_Products},
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
                MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            enableControls(false);
            btn_Sua.Enabled = true;
        }

        private void btn_Huy_Click(object sender, EventArgs e)
        {
            enableControls(false);
            Load_CTHH();
            btn_Sua.Enabled = true;
        }

        private void btn_Thoat_Click(object sender, EventArgs e)
        {
            this.Close();
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
    }
}
