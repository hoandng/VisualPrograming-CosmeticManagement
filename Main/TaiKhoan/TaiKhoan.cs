using DocumentFormat.OpenXml.Drawing.Charts;
using Main.Models;
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

namespace Main.TaiKhoan
{
    public partial class TaiKhoan : Form
    {
        ProcessDatabase _data;
        private User _user;
        bool isChangePicture = false;
        string oldPicturePath = "";

        public TaiKhoan(User curr_user)
        {
            _user = new User();
            _user.Assign(curr_user);
            _data = new ProcessDatabase();
            InitializeComponent();
        }

        private void TaiKhoan_Load(object sender, EventArgs e)
        {
            Load_ThongTin();
            enableControl(false);
        }

        private void Load_ThongTin()
        {
            txt_TenTK.Text = _user.Username;
            /*txt_MK.Text = _user.Password;
            oldPicturePath = _user.Picture;*/
            string query = "Select * From TaiKhoan Where Username =  @username";
            var parameter = new Dictionary<string, object>
            {
                {"@username" , _user.Username}
            };
            System.Data.DataTable dt = _data.ExecuteQuery(query, parameter);
            txt_MK.Text = dt.Rows[0]["Password"].ToString();
            oldPicturePath = dt.Rows[0]["Anh"].ToString();
            if (oldPicturePath != "")
            {
                string imagePath = getImagePath(oldPicturePath);
                if (File.Exists(imagePath))
                {
                    pb_Anh.ImageLocation = imagePath;
                }
            }
        }


        private void enableControl(bool en)
        {
            txt_MK.Enabled = en;
            btn_Luu.Enabled = en;
            btn_Huy.Enabled = en;
            btn_TaiAnh.Enabled = en;
        }

        private string getImagePath(string imageName)
        {
            // Hiển thị ảnh
            string projectDirectory = Directory.GetParent(Application.StartupPath).Parent.FullName;
            string imagesFolder = Path.Combine(projectDirectory, "Images", "Account");
            string imagePath = Path.Combine(imagesFolder, imageName);

            return imagePath;
        }

        private string SaveImageToFolder(string imagePath, string manv)
        {
            // Kiểm tra nếu ảnh chưa được chọn hoặc mã sản phẩm rỗng
            if (string.IsNullOrEmpty(imagePath) || string.IsNullOrEmpty(manv))
            {
                MessageBox.Show("Vui lòng chọn ảnh và nhập mã sản phẩm.");
                return null;
            }

            // Đường dẫn thư mục "Images" nằm ở thư mục gốc của dự án
            string projectDirectory = Directory.GetParent(Application.StartupPath).Parent.FullName;
            string imagesFolder = Path.Combine(projectDirectory, "Images", "Account");

            // Tạo thư mục "Images" nếu chưa tồn tại
            if (!Directory.Exists(imagesFolder))
            {
                Directory.CreateDirectory(imagesFolder);
            }

            // Lấy phần mở rộng của tệp ảnh gốc
            string fileExtension = Path.GetExtension(imagePath);
            // Tạo tên tệp ảnh mới dựa trên mã sản phẩm
            string newFileName = $"{manv}{fileExtension}";
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

        private void btn_CapNhat_Click(object sender, EventArgs e)
        {
            enableControl(true);
        }

        private void btn_Huy_Click(object sender, EventArgs e)
        {
            enableControl(false);
            Load_ThongTin();
        }

        private void btn_TaiAnh_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";  // Chỉ cho phép chọn các file ảnh
            openFileDialog.Title = "Chọn ảnh";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Hiển thị ảnh trong PictureBox
                pb_Anh.ImageLocation = openFileDialog.FileName;
                isChangePicture = true;
            }
        }

        private void btn_Luu_Click(object sender, EventArgs e)
        {
            string tk = txt_TenTK.Text;
            string mk = txt_MK.Text;
            string anh = "";
            if (isChangePicture)
            {
                string newPath = pb_Anh.ImageLocation;
                anh = SaveImageToFolder(newPath, tk);
            }
            else
            {
                anh = oldPicturePath;
            }
            string querry = "Update [TaiKhoan] SET " +
                             "Password = @mk, Anh = @anh " +
                             "Where Username = @tk";
            var parameters = new Dictionary<string, object>
            {
                {"@tk", tk},
                {"@mk", mk},
                {"@anh", anh},
            };
            _data.ExecuteNonQuery(querry, parameters);
            _user.Password = mk;
            Load_ThongTin();
            enableControl(false);
        }
    }
}
