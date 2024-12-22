create database QL_MyPham2


CREATE TABLE KhoiLuong (
    MaKhoiLuong VARCHAR(10) PRIMARY KEY,
    TenKhoiLuong NVARCHAR(50)
);

CREATE TABLE Loai (
    MaLoai VARCHAR(10) PRIMARY KEY,
    TenLoai NVARCHAR(50)
);

CREATE TABLE HangSX (
    MaHangSX VARCHAR(10) PRIMARY KEY,
    TenHangSX NVARCHAR(100)
);

CREATE TABLE ChatLieu (
    MaChatLieu VARCHAR(10) PRIMARY KEY,
    TenChatLieu NVARCHAR(50)
);

CREATE TABLE NuocSX (
    MaNuocSX VARCHAR(10) PRIMARY KEY,
    TenNuocSX NVARCHAR(50)
);

CREATE TABLE MauSac (
    MaMau VARCHAR(10) PRIMARY KEY,
    TenMau NVARCHAR(50)
);

CREATE TABLE CongDung (
    MaCongDung VARCHAR(10) PRIMARY KEY,
    TenCongDung NVARCHAR(100)
);

CREATE TABLE Mua (
    MaMua VARCHAR(10) PRIMARY KEY,
    TenMua NVARCHAR(50)
);

CREATE TABLE HangHoa (
    MaHang VARCHAR(10) PRIMARY KEY,
    TenHang NVARCHAR(100),
    MaLoai VARCHAR(10) NULL,
    MaKhoiLuong VARCHAR(10) NULL,
    MaHangSX VARCHAR(10) NULL,
    MaChatLieu VARCHAR(10) NULL,
    MaCongDung VARCHAR(10) NULL,
    MaMau VARCHAR(10) NULL,
    MaNuocSX VARCHAR(10) NULL,
    MaMua VARCHAR(10) NULL,
    SoLuong INT,
    DonGiaNhap DECIMAL(18,0),
    DonGiaBan DECIMAL(18,0),
    ThoiGianBaoHanh INT,
    Anh NVARCHAR(255),
    GhiChu NVARCHAR(255),
    FOREIGN KEY (MaLoai) REFERENCES Loai(MaLoai) ON DELETE SET NULL ON UPDATE CASCADE,
    FOREIGN KEY (MaKhoiLuong) REFERENCES KhoiLuong(MaKhoiLuong) ON DELETE SET NULL ON UPDATE CASCADE,
    FOREIGN KEY (MaHangSX) REFERENCES HangSX(MaHangSX) ON DELETE SET NULL ON UPDATE CASCADE,
    FOREIGN KEY (MaChatLieu) REFERENCES ChatLieu(MaChatLieu) ON DELETE SET NULL ON UPDATE CASCADE,
    FOREIGN KEY (MaCongDung) REFERENCES CongDung(MaCongDung) ON DELETE SET NULL ON UPDATE CASCADE,
    FOREIGN KEY (MaMau) REFERENCES MauSac(MaMau) ON DELETE SET NULL ON UPDATE CASCADE,
    FOREIGN KEY (MaNuocSX) REFERENCES NuocSX(MaNuocSX) ON DELETE SET NULL ON UPDATE CASCADE,
    FOREIGN KEY (MaMua) REFERENCES Mua(MaMua) ON DELETE SET NULL ON UPDATE CASCADE
);

CREATE TABLE KhachHang (
    MaKhach VARCHAR(10) PRIMARY KEY,
    TenKhach NVARCHAR(100),
    DiaChi NVARCHAR(255),
    DienThoai VARCHAR(15)
);

CREATE TABLE NhaCungCap (
    MaNCC VARCHAR(10) PRIMARY KEY,
    TenNCC NVARCHAR(100),
    DiaChi NVARCHAR(255),
    DienThoai VARCHAR(15)
);

CREATE TABLE CongViec (
    MaCV VARCHAR(10) PRIMARY KEY,
    TenCV NVARCHAR(50),
    MucLuong DECIMAL(18,0)
);

CREATE TABLE NhanVien (
    MaNV VARCHAR(10) PRIMARY KEY,
    TenNV NVARCHAR(100),
    GioiTinh NVARCHAR(10),
    NgaySinh DATE,
    DienThoai VARCHAR(15),
    DiaChi NVARCHAR(255),
    MaCV VARCHAR(10) NULL,
    FOREIGN KEY (MaCV) REFERENCES CongViec(MaCV) ON DELETE SET NULL ON UPDATE CASCADE
);

CREATE TABLE HoaDonBan (
    SoHDB VARCHAR(10) PRIMARY KEY,
    MaNV VARCHAR(10) NULL,
    NgayBan DATE,
    MaKhach VARCHAR(10) NULL,
    TongTien DECIMAL(18,0),
    FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV) ON DELETE SET NULL ON UPDATE CASCADE,
    FOREIGN KEY (MaKhach) REFERENCES KhachHang(MaKhach) ON DELETE SET NULL ON UPDATE CASCADE
);

CREATE TABLE ChiTietHDB (
    SoHDB VARCHAR(10),
    MaHang VARCHAR(10),
    SoLuong INT,
    GiamGia INT,
    ThanhTien INT,
    PRIMARY KEY (SoHDB, MaHang),
    FOREIGN KEY (SoHDB) REFERENCES HoaDonBan(SoHDB) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (MaHang) REFERENCES HangHoa(MaHang) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE HoaDonNhap (
    SoHDN VARCHAR(10) PRIMARY KEY,
    MaNV VARCHAR(10) NULL,
    NgayNhap DATE,
    MaNCC VARCHAR(10) NULL,
    TongTien DECIMAL(18,0),
    FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV) ON DELETE SET NULL ON UPDATE CASCADE,
    FOREIGN KEY (MaNCC) REFERENCES NhaCungCap(MaNCC) ON DELETE SET NULL ON UPDATE CASCADE
);

CREATE TABLE ChiTietHDN (
    SoHDN VARCHAR(10),
    MaHang VARCHAR(10),
    SoLuong INT,
    DonGia DECIMAL(18,0),
    GiamGia INT,
    ThanhTien INT,
    PRIMARY KEY (SoHDN, MaHang),
    FOREIGN KEY (SoHDN) REFERENCES HoaDonNhap(SoHDN) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (MaHang) REFERENCES HangHoa(MaHang) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE TaiKhoan (
    Username VARCHAR(100) UNIQUE,
    Password NVARCHAR(255) NOT NULL,
    Anh NVARCHAR(255),
	isAdmin int NOT NULL,
    MaNV VARCHAR(10) NULL,
    FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV) ON DELETE SET NULL ON UPDATE CASCADE
);



INSERT INTO KhoiLuong (MaKhoiLuong, TenKhoiLuong) VALUES 
('KL01', '2g'),
('KL02', '250ml'),
('KL03', '500ml'),
('KL04', '1kg'),
('KL05', '5kg'),
('KL06', '100g'),
('KL07', '200ml'),
('KL08', '750ml'),
('KL09', '300g'),
('KL10', '1L');
INSERT INTO Loai (MaLoai, TenLoai) VALUES 
('L01', N'Nước hoa'),
('L02', N'Sơn móng tay'),
('L03', N'Nước rửa tay'),
('L04', N'Sữa tắm'),
('L05', N'Xà phòng'),
('L06', N'Dầu gội'),
('L07', N'Kem dưỡng da'),
('L08', N'Son môi'),
('L09', N'Mỹ phẩm'),
('L10', N'Kem chống nắng');

INSERT INTO HangSX (MaHangSX, TenHangSX) VALUES 
('HSX01', 'Chanel'),
('HSX02', 'Dior'),
('HSX03', 'Gucci'),
('HSX04', 'LOreal'),
('HSX05', 'Estee Lauder'),
('HSX06', 'Lancome'),
('HSX07', 'Shiseido'),
('HSX08', 'SK-II'),
('HSX09', 'Nivea'),
('HSX10', 'Maybelline');

INSERT INTO ChatLieu (MaChatLieu, TenChatLieu) VALUES 
('CL01', N'Tinh dầu tự nhiên'),
('CL02', N'Chất làm bóng'),
('CL03', N'Cồn'),
('CL04', N'Chất nhũ hóa'),
('CL05', N'Dầu thực vật'),
('CL06', N'Chất tẩy rửa'),
('CL07', N'Chất dưỡng ẩm'),
('CL08', N'Sáp ong'),
('CL09', N'Bột khoáng'),
('CL10', N'Chất dữ ẩm');

INSERT INTO NuocSX (MaNuocSX, TenNuocSX) VALUES 
('NSX01', N'Pháp'),
('NSX02', N'Ý'),
('NSX03', N'Nhật Bản'),
('NSX04', N'Hàn Quốc'),
('NSX05', N'Mỹ'),
('NSX06', N'Đức'),
('NSX07', N'Anh'),
('NSX08', N'Canada'),
('NSX09', N'Úc'),
('NSX10', N'Tây Ban Nha');

INSERT INTO MauSac (MaMau, TenMau) VALUES 
('M01', N'Đỏ'),
('M02', N'Xanh lá'),
('M03', N'Xanh dương'),
('M04', N'Vàng'),
('M05', N'Trắng'),
('M06', N'Đen'),
('M07', N'Tím'),
('M08', N'Hồng'),
('M09', N'Cam'),
('M10', N'Nâu');

INSERT INTO CongDung (MaCongDung, TenCongDung) VALUES 
('CD01', N'Dưỡng ẩm'),
('CD02', N'Làm sạch'),
('CD03', N'Chống nắng'),
('CD04', N'Trị mụn'),
('CD05', N'Dưỡng trắng'),
('CD06', N'Giữ nếp tóc'),
('CD07', N'Bảo vệ da'),
('CD08', N'Chống lão hóa'),
('CD09', N'Dưỡng môi'),
('CD10', N'Khử mùi');

INSERT INTO Mua (MaMua, TenMua) VALUES 
('MUA01', N'Xuân'),
('MUA02', N'Hạ'),
('MUA03', N'Thu'),
('MUA04', N'Đông'),
('MUA05', N'Xuân-Hè'),
('MUA06', N'Thu-Đông'),
('MUA07', N'Hè-Thu'),
('MUA08', N'Đông-Xuân'),
('MUA09', N'Mùa mưa'),
('MUA10', N'Mùa khô');

INSERT INTO HangHoa (MaHang, TenHang, MaLoai, MaKhoiLuong, MaHangSX, MaChatLieu, MaCongDung, MaMau, MaNuocSX, MaMua, SoLuong, DonGiaNhap, DonGiaBan, ThoiGianBaoHanh, Anh, GhiChu) VALUES 
('H01', N'Nước hoa Chanel No.5', 'L01', 'KL01', 'HSX01', 'CL01', 'CD01', 'M01', 'NSX01', 'MUA01', 50, 1000000, 1500000, 12, N'chanel_no5.jpg', N'Nước hoa cao cấp'),
('H02', N'Son Dior Addict', 'L02', 'KL02', 'HSX02', 'CL02', 'CD02', 'M02', 'NSX02', 'MUA02', 100, 500000, 700000, 6, N'dior_addict.jpg', N''),
('H03', N'Gucci Bloom', 'L01', 'KL03', 'HSX03', 'CL03', 'CD03', 'M03', 'NSX03', 'MUA03', 30, 1200000, 1800000, 12, N'gucci_bloom.jpg', N'Hương hoa tự nhiên'),
('H04', N'Nước rửa tay Dettol', 'L03', 'KL04', 'HSX04', 'CL04', 'CD04', 'M04', 'NSX04', 'MUA04', 75, 30000, 45000, 0, N'dettol.jpg', N''),
('H05', N'Sữa tắm Dove', 'L04', 'KL05', 'HSX05', 'CL05', 'CD05', 'M05', 'NSX05', 'MUA05', 90, 50000, 75000, 0, N'dove_shower.jpg', N''),
('H06', N'Xà phòng Lifebuoy', 'L05', 'KL06', 'HSX06', 'CL06', 'CD06', 'M06', 'NSX06', 'MUA06', 200, 20000, 35000, 0, N'lifebuoy.jpg', N''),
('H07', N'Dầu gội Clear', 'L06', 'KL07', 'HSX07', 'CL07', 'CD07', 'M07', 'NSX07', 'MUA07', 50, 80000, 120000, 6, N'clear_shampoo.jpg', N''),
('H08', N'Kem dưỡng da SK-II', 'L07', 'KL08', 'HSX08', 'CL08', 'CD08', 'M08', 'NSX08', 'MUA08', 30, 3000000, 4500000, 24, N'skii_cream.jpg', N'Dưỡng trắng da'),
('H09', N'Son môi Maybelline', 'L08', 'KL09', 'HSX10', 'CL09', 'CD09', 'M09', 'NSX10', 'MUA09', 120, 200000, 300000, 6, N'maybelline_lipstick.jpg', N''),
('H10', N'Kem chống nắng Nivea', 'L10', 'KL10', 'HSX09', 'CL10', 'CD10', 'M10', 'NSX09', 'MUA10', 40, 120000, 200000, 12, N'nivea_sunscreen.jpg', N'');

INSERT INTO KhachHang (MaKhach, TenKhach, DiaChi, DienThoai) VALUES 
('KH01', N'Nguyễn Văn Tuấn', N'Hà Nội', '0123456789'),
('KH02', N'Trần Thị Ánh', N'TP. HCM', '0987654321'),
('KH03', N'Lê Thanh Trà', N'Đà Nẵng', '0912345678'),
('KH04', N'Phạm Tuấn Anh', N'Hải Phòng', '0908765432'),
('KH05', N'Thành Đặng Hoàn', N'Nghệ An ', '0923456789'),
('KH06', N'Hồ Thị Ngọc Tú', N'Nghệ An', '0934567890'),
('KH07', N'Nguyễn Hữu Tài', N'Quảng Ninh', '0945678901'),
('KH08', N'Hoàng Thu Hường', N'Bắc Ninh', '0956789012'),
('KH09', N'Nguyễn An Thạch', N'Nghệ An', '0967890123'),
('KH10', N'Ngô Thanh Đồng', N'Hà Nam', '0978901234');

INSERT INTO NhaCungCap (MaNCC, TenNCC, DiaChi, DienThoai) VALUES
('NCC01', N'Chanel', N'Paris, Pháp', '0123456789'),
('NCC02', N'Dior', N'Paris, Pháp', '0987654321'),
('NCC03', N'Gucci', N'Milan, Ý', '0912345678'),
('NCC04', N'Dettol', N'London, Anh', '0908765432'),
('NCC05', N'Dove', N'New York, Mỹ', '0923456789'),
('NCC06', N'Lifebuoy', N'Amsterdam, Hà Lan', '0934567890'),
('NCC07', N'Clear', N'Tokyo, Nhật Bản', '0945678901'),
('NCC08', N'SK-II', N'Tokyo, Nhật Bản', '0956789012'),
('NCC09', N'Nivea', N'Hamburg, Đức', '0967890123'),
('NCC10', N'Maybelline', N'New York, Mỹ', '0978901234');

INSERT INTO CongViec (MaCV, TenCV, MucLuong) VALUES
('CV01', N'Nhân viên bán hàng', 5000000),
('CV02', N'Quản lý cửa hàng', 10000000),
('CV03', N'Kế toán', 8000000),
('CV04', N'Quản lý kho', 7000000),
('CV05', N'Quản lý bán hàng', 9500000);

INSERT INTO NhanVien (MaNV, TenNV, GioiTinh, NgaySinh, DienThoai, DiaChi, MaCV) VALUES
('NV01', N'Nguyễn Văn An', N'Nam', '1990-01-15', '0912345678', N'123 Đường ABC, Hà Nội', 'CV01'),
('NV02', N'Trần Thị Bích', N'Nữ', '1985-03-20', '0987654321', N'456 Đường DEF, TP.HCM', 'CV02'),
('NV03', N'Lê Văn Cường', N'Nam', '1992-05-10', '0912345679', N'789 Đường GHI, Đà Nẵng', 'CV03'),
('NV04', N'Phạm Thị Đào', N'Nữ', '1993-08-30', '0987654322', N'321 Đường JKL, Cần Thơ', 'CV01'),
('NV05', N'Hoàng Văn Bảo', N'Nam', '1988-12-25', '0912345680', N'654 Đường MNO, Hải Phòng', 'CV02');

INSERT INTO HoaDonNhap (SoHDN, MaNV, NgayNhap, MaNCC, TongTien) VALUES
('HDN01', 'NV01', '2024-09-01', 'NCC01', 0), -- Chanel
('HDN02', 'NV02', '2024-09-02', 'NCC02', 0), -- Dior
('HDN03', 'NV03', '2024-09-03', 'NCC04', 0), -- Dettol
('HDN04', 'NV01', '2024-09-04', 'NCC05', 0), -- Dove
('HDN05', 'NV02', '2024-09-05', 'NCC06', 0), -- Lifebuoy
('HDN06', 'NV03', '2024-09-06', 'NCC07', 0), -- Clear
('HDN07', 'NV01', '2024-09-07', 'NCC08', 0), -- SK-II
('HDN08', 'NV02', '2024-09-08', 'NCC09', 0),  -- Nivea
('HDN09', 'NV03', '2024-09-09', 'NCC10', 0);  -- Maybelline


INSERT INTO HoaDonBan (SoHDB, MaNV, NgayBan, MaKhach, TongTien) VALUES
('HDB01', 'NV01', '2024-06-13', 'KH01', 0),
('HDB02', 'NV02', '2024-06-22', 'KH02', 0),
('HDB03', 'NV03', '2024-07-03', 'KH03', 0),
('HDB04', 'NV01', '2024-08-24', 'KH04', 0),
('HDB05', 'NV02', '2024-08-25', 'KH05', 0),
('HDB06', 'NV03', '2024-09-06', 'KH06', 0),
('HDB07', 'NV01', '2024-09-10', 'KH07', 0),
('HDB08', 'NV02', '2024-10-08', 'KH08', 0),
('HDB09', 'NV03', '2024-10-19', 'KH09', 0),
('HDB10', 'NV01', '2024-11-03', 'KH10', 0);

insert into TaiKhoan (Username,Password, isAdmin) VALUES
('admin','12345','1');

