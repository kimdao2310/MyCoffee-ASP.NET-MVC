
CREATE DATABASE QLMYCF_65130390;
GO
USE QLMYCF_65130390;
GO


CREATE TABLE NHAN_VIEN (
    MaNV INT IDENTITY(1,1) PRIMARY KEY,
    HoTenNV NVARCHAR(100) NOT NULL,
    Email VARCHAR(100) UNIQUE NOT NULL,
    SoDienThoai VARCHAR(15) UNIQUE,
    MatKhau VARCHAR(255) NOT NULL,
    Quyen NVARCHAR(50) NOT NULL DEFAULT N'Nhân viên',
    NgayVaoLam DATE,
    DiaChi NVARCHAR(255)
);
GO

INSERT INTO NHAN_VIEN (HoTenNV, Email, SoDienThoai, MatKhau, Quyen, NgayVaoLam, DiaChi) VALUES
(N'Nguyễn Văn A', 'admin@cafe.com', '0901000001', 'mk', N'Admin', '2020-01-01', N'123 Đường Lê Lợi'),
(N'Trần Thị B', 'quanly@cafe.com', '0901000002', 'mk', N'Quản lý', '2021-03-15', N'456 Đường Trần Phú'),
(N'Lê Văn C', 'nv01@cafe.com', '0901000003', 'mk', N'Nhân viên', '2022-05-20', N'789 Đường Nguyễn Huệ'),
(N'Phạm Văn D', 'nv02@cafe.com', '0901000004', 'mk', N'Nhân viên', '2023-02-10', N'12 Đường Lê Lợi'),
(N'Nguyễn Thị E', 'nv03@cafe.com', '0901000005', 'mk', N'Nhân viên', '2023-06-18', N'25 Đường Trần Phú'),
(N'Võ Văn F', 'giaohang01@cafe.com', '0901000006', 'mk', N'Giao hàng', '2024-01-05', N'88 Đường Nguyễn Thiện Thuật'),
(N'Trần Quốc G', 'giaohang02@cafe.com', '0901000007', 'mk', N'Giao hàng', '2024-04-12', N'102 Đường 2/4'),
(N'Lê Minh H', 'giaohang03@cafe.com', '0901000008', 'mk', N'Giao hàng', '2024-07-20', N'55 Đường Yersin');
GO

CREATE TABLE LOAI_SAN_PHAM (
    MaLSP VARCHAR(10) PRIMARY KEY,
    TenLSP NVARCHAR(50) NOT NULL
);
GO

INSERT INTO LOAI_SAN_PHAM (MaLSP, TenLSP) VALUES
('CF', N'Cà Phê'),
('TR', N'Trà'),
('ST', N'Sinh Tố'),
('DA', N'Đồ Ăn'),
('KM', N'Kem'),
('BK', N'Bánh Kem');
GO


CREATE TABLE SAN_PHAM (
    MaSP VARCHAR(10) PRIMARY KEY,
    TenSP NVARCHAR(100) NOT NULL,
    MoTaSP NVARCHAR(255),
    DonGia DECIMAL(10,0) NOT NULL,
    DonViTinh NVARCHAR(20) NOT NULL,
    AnhSanPham VARCHAR(255),
    MaLSP VARCHAR(10) NOT NULL,
    FOREIGN KEY (MaLSP) REFERENCES LOAI_SAN_PHAM(MaLSP)
);
GO

INSERT INTO SAN_PHAM (MaSP, TenSP, MoTaSP, DonGia, DonViTinh, AnhSanPham, MaLSP) VALUES
('CF01', N'Cà Phê Đen Đá', N'Cà phê rang xay truyền thống', 25000, N'Ly', NULL, 'CF'),
('CF02', N'Cà Phê Sữa Đá', N'Cà phê sữa đậm đà', 30000, N'Ly', NULL, 'CF'),
('CF03', N'Bạc Xỉu', N'Cà phê sữa nhiều sữa ít cà phê', 35000, N'Ly', NULL, 'CF'),
('CF04', N'Cappuccino', N'Cà phê Ý với lớp bọt sữa mịn', 55000, N'Ly', NULL, 'CF'),
('CF05', N'Latte', N'Cà phê sữa phong cách Ý', 55000, N'Ly', NULL, 'CF'),

('TR01', N'Trà Đào Cam Sả', N'Trà đào kết hợp cam và sả', 45000, N'Ly', NULL, 'TR'),
('TR02', N'Trà Vải', N'Trà vải thơm mát', 40000, N'Ly', NULL, 'TR'),
('TR03', N'Trà Chanh', N'Trà chanh giải khát', 25000, N'Ly', NULL, 'TR'),
('TR04', N'Trà Tắc', N'Trà tắc thanh mát', 25000, N'Ly', NULL, 'TR'),


('ST01', N'Sinh Tố Bơ', N'Sinh tố bơ béo ngậy', 50000, N'Ly', NULL, 'ST'),
('ST02', N'Sinh Tố Dâu', N'Sinh tố dâu tươi', 50000, N'Ly', NULL, 'ST'),
('ST03', N'Sinh Tố Xoài', N'Sinh tố xoài chín', 45000, N'Ly', NULL, 'ST'),
('ST04', N'Sinh Tố Mãng Cầu', N'Sinh tố mãng cầu thơm ngon', 55000, N'Ly', NULL, 'ST'),


('DA01', N'Bánh Mì Thịt Nướng', N'Bánh mì thịt nướng đặc biệt', 30000, N'Phần', NULL, 'DA'),
('DA02', N'Khoai Tây Chiên', N'Khoai tây chiên giòn', 35000, N'Phần', NULL, 'DA'),
('DA03', N'Gà Rán', N'Gà rán giòn cay', 55000, N'Phần', NULL, 'DA'),


('KM01', N'Kem Vani', N'Kem vị vani', 25000, N'Ly', NULL, 'KM'),
('KM02', N'Kem Socola', N'Kem vị socola', 30000, N'Ly', NULL, 'KM'),
('KM03', N'Kem Dâu', N'Kem vị dâu', 30000, N'Ly', NULL, 'KM'),


('BK01', N'Bánh Kem Socola', N'Bánh kem vị socola', 250000, N'Cái', NULL, 'BK'),
('BK02', N'Bánh Kem Dâu', N'Bánh kem vị dâu', 270000, N'Cái', NULL, 'BK'),
('BK03', N'Bánh Kem Trái Cây', N'Bánh kem trái cây tươi', 320000, N'Cái', NULL, 'BK');
GO

CREATE TABLE KHACH_HANG (
    MaKH INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    SoDienThoai VARCHAR(15) UNIQUE,
    Email VARCHAR(100) UNIQUE,
    DiaChi NVARCHAR(255),
    MatKhau VARCHAR(255) NOT NULL
);
GO


CREATE TABLE BAN (
    MaBan VARCHAR(10) PRIMARY KEY,
    TenBan NVARCHAR(50) NOT NULL,
    TrangThaiBan INT NOT NULL DEFAULT 0,
    ViTri NVARCHAR(100)
);
GO


CREATE TABLE TRANG_THAI_DON (
    MaTrangThai INT PRIMARY KEY,
    TenTrangThai NVARCHAR(50)
);
GO

INSERT INTO TRANG_THAI_DON VALUES
(0, N'Chờ xác nhận'),
(1, N'Đã duyệt'),
(2, N'Đang giao hàng'),
(3, N'Hoàn thành'),
(4, N'Huy nhận');
GO


CREATE TABLE GIO_HANG (
    MaHD INT IDENTITY(1,1) PRIMARY KEY,
    NgayDatHang DATETIME NOT NULL DEFAULT GETDATE(),
    NgayThanhToan DATETIME NULL,
    TongTien DECIMAL(15,0) NOT NULL DEFAULT 0,
    TrangThai INT NOT NULL DEFAULT 0,
    MaBan VARCHAR(10),
    MaKH INT,
    MaNV_DatHang INT NULL,
    MaNV_Duyet INT,
    MaNV_GiaoHang INT,
    FOREIGN KEY (MaBan) REFERENCES BAN(MaBan),
    FOREIGN KEY (MaKH) REFERENCES KHACH_HANG(MaKH),
    FOREIGN KEY (MaNV_DatHang) REFERENCES NHAN_VIEN(MaNV),
    FOREIGN KEY (MaNV_Duyet) REFERENCES NHAN_VIEN(MaNV),
    FOREIGN KEY (MaNV_GiaoHang) REFERENCES NHAN_VIEN(MaNV),
    FOREIGN KEY (TrangThai) REFERENCES TRANG_THAI_DON(MaTrangThai)
);
GO


CREATE TABLE CHI_TIET_GH (
    MaHD INT NOT NULL,
    MaSP VARCHAR(10) NOT NULL,
    SoLuong INT NOT NULL,
    DonGia DECIMAL(10,0) NOT NULL,
    GhiChu NVARCHAR(255),
    PRIMARY KEY (MaHD, MaSP),
    FOREIGN KEY (MaHD) REFERENCES GIO_HANG(MaHD),
    FOREIGN KEY (MaSP) REFERENCES SAN_PHAM(MaSP)
);
GO


CREATE TABLE THANH_TOAN (
    MaTT INT IDENTITY(1,1) PRIMARY KEY,
    MaHD INT NOT NULL,
    PhuongThuc NVARCHAR(50),
    NgayThanhToan DATETIME NOT NULL DEFAULT GETDATE(),
    SoTien DECIMAL(15,0),
    FOREIGN KEY (MaHD) REFERENCES GIO_HANG(MaHD)
);
GO
CREATE TABLE TIN_TUC (
    MaTT INT IDENTITY(1,1) PRIMARY KEY,
    TieuDe NVARCHAR(200) NOT NULL,
    NoiDung NVARCHAR(MAX) NOT NULL,
    AnhDaiDien VARCHAR(255),
    NgayDang DATETIME NOT NULL DEFAULT GETDATE(),
    MaNV INT,
    TrangThai INT NOT NULL DEFAULT 1, 

    FOREIGN KEY (MaNV) REFERENCES NHAN_VIEN(MaNV)
);
GO
CREATE TABLE QUANG_CAO (
    MaQC INT IDENTITY(1,1) PRIMARY KEY,
    TieuDe NVARCHAR(200),
    HinhAnh VARCHAR(255) NOT NULL,
    Link VARCHAR(255),                
    NoiDung NVARCHAR(MAX),
    NgayBatDau DATE,
    NgayKetThuc DATE,
    TrangThai INT NOT NULL DEFAULT 1,
    MaNV INT,

    FOREIGN KEY (MaNV) REFERENCES NHAN_VIEN(MaNV)
);
GO
