USE [master]
GO
/****** Object:  Database [QuanLyKhachSan]    Script Date: 6/5/2019 8:55:00 AM ******/
CREATE DATABASE [QuanLyKhachSan]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'QuanLyKhachSan', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\QuanLyKhachSan.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'QuanLyKhachSan_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\QuanLyKhachSan_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [QuanLyKhachSan] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [QuanLyKhachSan].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [QuanLyKhachSan] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [QuanLyKhachSan] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [QuanLyKhachSan] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [QuanLyKhachSan] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [QuanLyKhachSan] SET ARITHABORT OFF 
GO
ALTER DATABASE [QuanLyKhachSan] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [QuanLyKhachSan] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [QuanLyKhachSan] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [QuanLyKhachSan] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [QuanLyKhachSan] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [QuanLyKhachSan] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [QuanLyKhachSan] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [QuanLyKhachSan] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [QuanLyKhachSan] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [QuanLyKhachSan] SET  ENABLE_BROKER 
GO
ALTER DATABASE [QuanLyKhachSan] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [QuanLyKhachSan] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [QuanLyKhachSan] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [QuanLyKhachSan] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [QuanLyKhachSan] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [QuanLyKhachSan] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [QuanLyKhachSan] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [QuanLyKhachSan] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [QuanLyKhachSan] SET  MULTI_USER 
GO
ALTER DATABASE [QuanLyKhachSan] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [QuanLyKhachSan] SET DB_CHAINING OFF 
GO
ALTER DATABASE [QuanLyKhachSan] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [QuanLyKhachSan] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [QuanLyKhachSan] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [QuanLyKhachSan] SET QUERY_STORE = OFF
GO
USE [QuanLyKhachSan]
GO
/****** Object:  Table [dbo].[tblChucVu]    Script Date: 6/5/2019 8:55:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblChucVu](
	[ma_chuc_vu] [int] IDENTITY(1,1) NOT NULL,
	[chuc_vu] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[ma_chuc_vu] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblDichVu]    Script Date: 6/5/2019 8:55:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblDichVu](
	[ma_dv] [int] IDENTITY(1,1) NOT NULL,
	[ten_dv] [nvarchar](100) NULL,
	[gia] [float] NULL,
	[don_vi] [nvarchar](30) NULL,
	[anh] [nvarchar](200) NULL,
	[ton_kho] [int] NULL,
	[trang_thai] [bit] NULL,
	[da_duoc_xoa] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ma_dv] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblDichVuDaDat]    Script Date: 6/5/2019 8:55:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblDichVuDaDat](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[ma_hd] [int] NULL,
	[ma_dv] [int] NULL,
	[so_luong] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblHoaDon]    Script Date: 6/5/2019 8:55:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblHoaDon](
	[ma_hd] [int] IDENTITY(1,1) NOT NULL,
	[ma_nv] [int] NULL,
	[ma_pdp] [int] NULL,
	[ngay_tra_phong] [datetime] NULL,
	[ma_tinh_trang] [int] NULL,
	[tien_phong] [float] NULL,
	[tien_dich_vu] [float] NULL,
	[phu_thu] [float] NULL,
	[tong_tien] [float] NULL,
PRIMARY KEY CLUSTERED 
(
	[ma_hd] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblKhachHang]    Script Date: 6/5/2019 8:55:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblKhachHang](
	[ma_kh] [int] IDENTITY(1,1) NOT NULL,
	[ten_dang_nhap] [varchar](30) NOT NULL,
	[mat_khau] [nvarchar](50) NULL,
	[ho_ten] [nvarchar](50) NULL,
	[cmt] [nvarchar](30) NULL,
	[sdt] [nvarchar](15) NULL,
	[mail] [nvarchar](100) NULL,
	[diem] [int] NULL,
	[dia_chi] [nvarchar](500) NULL,
	[so_cmnd] [char](12) NULL,
	[ngay_sinh] [date] NULL,
	[gioi_tinh] [nvarchar](20) NULL,
	[trang_thai] [bit] NULL,
 CONSTRAINT [PK_tblKhachHang] PRIMARY KEY CLUSTERED 
(
	[ma_kh] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblLoaiPhong]    Script Date: 6/5/2019 8:55:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblLoaiPhong](
	[loai_phong] [int] IDENTITY(1,1) NOT NULL,
	[mo_ta] [nvarchar](50) NULL,
	[gia] [float] NULL,
	[ti_le_phu_thu] [int] NULL,
	[anh] [nvarchar](300) NULL,
	[trang_thai] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[loai_phong] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblNhanVien]    Script Date: 6/5/2019 8:55:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblNhanVien](
	[ma_nv] [int] IDENTITY(1,1) NOT NULL,
	[ho_ten] [nvarchar](50) NULL,
	[ngay_sinh] [date] NULL,
	[dia_chi] [nvarchar](100) NULL,
	[sdt] [nvarchar](15) NULL,
	[tai_khoan] [nvarchar](50) NULL,
	[mat_khau] [nvarchar](50) NULL,
	[ma_chuc_vu] [int] NULL,
	[trang_thai_tai_khoan] [bit] NULL,
	[mail] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[ma_nv] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblPhieuDatPhong]    Script Date: 6/5/2019 8:55:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblPhieuDatPhong](
	[ma_pdp] [int] IDENTITY(1,1) NOT NULL,
	[ma_kh] [int] NULL,
	[ngay_dat] [datetime] NULL,
	[ngay_vao] [datetime] NULL,
	[ngay_ra] [datetime] NULL,
	[ma_phong] [int] NULL,
	[thong_tin_khach_thue] [nvarchar](400) NULL,
	[ma_tinh_trang] [int] NULL,
 CONSTRAINT [PK__tblPhieu__0A6A2A7F0226BB12] PRIMARY KEY CLUSTERED 
(
	[ma_pdp] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblPhong]    Script Date: 6/5/2019 8:55:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblPhong](
	[ma_phong] [int] IDENTITY(1,1) NOT NULL,
	[so_phong] [nvarchar](10) NULL,
	[loai_phong] [int] NULL,
	[ma_tang] [int] NULL,
	[ma_tinh_trang] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ma_phong] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblTang]    Script Date: 6/5/2019 8:55:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblTang](
	[ma_tang] [int] IDENTITY(1,1) NOT NULL,
	[ten_tang] [nvarchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[ma_tang] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblTinhTrangHoaDon]    Script Date: 6/5/2019 8:55:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblTinhTrangHoaDon](
	[ma_tinh_trang] [int] IDENTITY(1,1) NOT NULL,
	[mo_ta] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[ma_tinh_trang] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblTinhTrangPhieuDatPhong]    Script Date: 6/5/2019 8:55:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblTinhTrangPhieuDatPhong](
	[ma_tinh_trang] [int] IDENTITY(1,1) NOT NULL,
	[tinh_trang] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[ma_tinh_trang] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblTinhTrangPhong]    Script Date: 6/5/2019 8:55:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblTinhTrangPhong](
	[ma_tinh_trang] [int] IDENTITY(1,1) NOT NULL,
	[mo_ta] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[ma_tinh_trang] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblTinNhan]    Script Date: 6/5/2019 8:55:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblTinNhan](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[ngay_gui] [datetime] NULL,
	[ma_kh] [int] NULL,
	[ho_ten] [nvarchar](100) NULL,
	[mail] [nvarchar](100) NULL,
	[noi_dung] [nvarchar](500) NULL,
 CONSTRAINT [PK__tblTinNh__3213E83F083A0B20] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[tblChucVu] ON 

INSERT [dbo].[tblChucVu] ([ma_chuc_vu], [chuc_vu]) VALUES (1, N'Quản trị viên')
INSERT [dbo].[tblChucVu] ([ma_chuc_vu], [chuc_vu]) VALUES (2, N'Quản lý')
INSERT [dbo].[tblChucVu] ([ma_chuc_vu], [chuc_vu]) VALUES (3, N'Nhân viên')
SET IDENTITY_INSERT [dbo].[tblChucVu] OFF
SET IDENTITY_INSERT [dbo].[tblDichVu] ON 

INSERT [dbo].[tblDichVu] ([ma_dv], [ten_dv], [gia], [don_vi], [anh], [ton_kho], [trang_thai], [da_duoc_xoa]) VALUES (1, N'Nước ngọt', 8000, N'lon', N'/Content/Images/DichVu/1.png', 250, 0, 0)
INSERT [dbo].[tblDichVu] ([ma_dv], [ten_dv], [gia], [don_vi], [anh], [ton_kho], [trang_thai], [da_duoc_xoa]) VALUES (2, N'Bia 333', 16000, N'Lon', N'/Content/Images/DichVu/2.png', 175, 0, 0)
INSERT [dbo].[tblDichVu] ([ma_dv], [ten_dv], [gia], [don_vi], [anh], [ton_kho], [trang_thai], [da_duoc_xoa]) VALUES (3, N'Khăn lạnh', 50000, N'Bịch', N'/Content/Images/DichVu/3.jpg', 500, 0, 0)
INSERT [dbo].[tblDichVu] ([ma_dv], [ten_dv], [gia], [don_vi], [anh], [ton_kho], [trang_thai], [da_duoc_xoa]) VALUES (4, N'Bánh Snack ', 18000, N'Gói', N'/Content/Images/DichVu/snack.jpg', 186, 1, 0)
INSERT [dbo].[tblDichVu] ([ma_dv], [ten_dv], [gia], [don_vi], [anh], [ton_kho], [trang_thai], [da_duoc_xoa]) VALUES (5, N'Nước suối', 10000, N'chai', N'/Content/Images/DichVu/heineken.jpg', 493, 1, 0)
INSERT [dbo].[tblDichVu] ([ma_dv], [ten_dv], [gia], [don_vi], [anh], [ton_kho], [trang_thai], [da_duoc_xoa]) VALUES (6, N'Nước tăng lực', 16000, N'Lon', N'/Content/Images/DichVu/bo-huc.jpg', 85, 1, 0)
INSERT [dbo].[tblDichVu] ([ma_dv], [ten_dv], [gia], [don_vi], [anh], [ton_kho], [trang_thai], [da_duoc_xoa]) VALUES (7, N'Thuốc lá', 16000, N'Bao', N'/Content/Images/DichVu/7.png', 100, 0, 0)
INSERT [dbo].[tblDichVu] ([ma_dv], [ten_dv], [gia], [don_vi], [anh], [ton_kho], [trang_thai], [da_duoc_xoa]) VALUES (8, N'Cơm hộp', 30000, N'Hộp', N'/Content/Images/DichVu/8.jpg', 196, 1, 0)
INSERT [dbo].[tblDichVu] ([ma_dv], [ten_dv], [gia], [don_vi], [anh], [ton_kho], [trang_thai], [da_duoc_xoa]) VALUES (18, N'Bia Heineken', 18000, N'Chai', N'/Content/Images/DichVu/heineken.jpg', 1500, 1, 0)
INSERT [dbo].[tblDichVu] ([ma_dv], [ten_dv], [gia], [don_vi], [anh], [ton_kho], [trang_thai], [da_duoc_xoa]) VALUES (19, N'Sting dâu', 12000, N'Chai', N'/Content/Images/DichVu/sting-dau.jpg', 10, 0, 0)
SET IDENTITY_INSERT [dbo].[tblDichVu] OFF
SET IDENTITY_INSERT [dbo].[tblDichVuDaDat] ON 

INSERT [dbo].[tblDichVuDaDat] ([id], [ma_hd], [ma_dv], [so_luong]) VALUES (18, 18, 4, 2)
INSERT [dbo].[tblDichVuDaDat] ([id], [ma_hd], [ma_dv], [so_luong]) VALUES (20, 18, 5, 4)
INSERT [dbo].[tblDichVuDaDat] ([id], [ma_hd], [ma_dv], [so_luong]) VALUES (21, 20, 4, 2)
INSERT [dbo].[tblDichVuDaDat] ([id], [ma_hd], [ma_dv], [so_luong]) VALUES (22, 20, 5, 3)
INSERT [dbo].[tblDichVuDaDat] ([id], [ma_hd], [ma_dv], [so_luong]) VALUES (23, 22, 4, 10)
INSERT [dbo].[tblDichVuDaDat] ([id], [ma_hd], [ma_dv], [so_luong]) VALUES (24, 22, 6, 15)
INSERT [dbo].[tblDichVuDaDat] ([id], [ma_hd], [ma_dv], [so_luong]) VALUES (25, 22, 8, 4)
SET IDENTITY_INSERT [dbo].[tblDichVuDaDat] OFF
SET IDENTITY_INSERT [dbo].[tblHoaDon] ON 

INSERT [dbo].[tblHoaDon] ([ma_hd], [ma_nv], [ma_pdp], [ngay_tra_phong], [ma_tinh_trang], [tien_phong], [tien_dich_vu], [phu_thu], [tong_tien]) VALUES (18, 4, 1, CAST(N'2019-05-23T10:50:26.717' AS DateTime), 2, 0, 76000, 0, 76000)
INSERT [dbo].[tblHoaDon] ([ma_hd], [ma_nv], [ma_pdp], [ngay_tra_phong], [ma_tinh_trang], [tien_phong], [tien_dich_vu], [phu_thu], [tong_tien]) VALUES (19, 4, 3, CAST(N'2019-05-26T19:17:11.167' AS DateTime), 2, 400000, 0, 0, 400000)
INSERT [dbo].[tblHoaDon] ([ma_hd], [ma_nv], [ma_pdp], [ngay_tra_phong], [ma_tinh_trang], [tien_phong], [tien_dich_vu], [phu_thu], [tong_tien]) VALUES (20, 4, 4, CAST(N'2019-05-26T19:15:53.417' AS DateTime), 2, 400000, 66000, 0, 466000)
INSERT [dbo].[tblHoaDon] ([ma_hd], [ma_nv], [ma_pdp], [ngay_tra_phong], [ma_tinh_trang], [tien_phong], [tien_dich_vu], [phu_thu], [tong_tien]) VALUES (21, 1, 5, CAST(N'2019-05-27T22:00:47.250' AS DateTime), 2, 150000, 0, 0, 150000)
INSERT [dbo].[tblHoaDon] ([ma_hd], [ma_nv], [ma_pdp], [ngay_tra_phong], [ma_tinh_trang], [tien_phong], [tien_dich_vu], [phu_thu], [tong_tien]) VALUES (22, 4, 6, CAST(N'2019-06-02T22:16:52.860' AS DateTime), 2, 1050000, 540000, 0, 1590000)
INSERT [dbo].[tblHoaDon] ([ma_hd], [ma_nv], [ma_pdp], [ngay_tra_phong], [ma_tinh_trang], [tien_phong], [tien_dich_vu], [phu_thu], [tong_tien]) VALUES (23, NULL, 8, NULL, 1, NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[tblHoaDon] OFF
SET IDENTITY_INSERT [dbo].[tblKhachHang] ON 

INSERT [dbo].[tblKhachHang] ([ma_kh], [ten_dang_nhap], [mat_khau], [ho_ten], [cmt], [sdt], [mail], [diem], [dia_chi], [so_cmnd], [ngay_sinh], [gioi_tinh], [trang_thai]) VALUES (1, N'vanviet', N'123', N'Trần Quang Việt', N'221455685', N'0981019704', N'darkheaven@gmail.com', 0, N'Bình Phước', NULL, CAST(N'1992-06-09' AS Date), N'Nam', 1)
INSERT [dbo].[tblKhachHang] ([ma_kh], [ten_dang_nhap], [mat_khau], [ho_ten], [cmt], [sdt], [mail], [diem], [dia_chi], [so_cmnd], [ngay_sinh], [gioi_tinh], [trang_thai]) VALUES (2, N'ngoctuan123', N'123456', N'Đặng Ngọc Tuấn', NULL, N'0987456214', N'dangngoctuan@gmail.com', 0, N'Bắc Ninh', NULL, CAST(N'1997-05-04' AS Date), N'Nam', 0)
INSERT [dbo].[tblKhachHang] ([ma_kh], [ten_dang_nhap], [mat_khau], [ho_ten], [cmt], [sdt], [mail], [diem], [dia_chi], [so_cmnd], [ngay_sinh], [gioi_tinh], [trang_thai]) VALUES (3, N'poonnguyen', N'123', N'Nguyễn Duy Poon', N'22150121441', N'0168764214', N'duypoon@gmail.com', 0, N'Phú Yên', NULL, CAST(N'1996-12-03' AS Date), N'Nam', 1)
INSERT [dbo].[tblKhachHang] ([ma_kh], [ten_dang_nhap], [mat_khau], [ho_ten], [cmt], [sdt], [mail], [diem], [dia_chi], [so_cmnd], [ngay_sinh], [gioi_tinh], [trang_thai]) VALUES (5, N'huudanh', N'123', N'Lê Hữu Danh', N'32165416512', N'6516512315', N'lehuudanh@gmail.com', 0, N'Đà Lạt', NULL, CAST(N'1996-11-22' AS Date), N'Nam', 1)
INSERT [dbo].[tblKhachHang] ([ma_kh], [ten_dang_nhap], [mat_khau], [ho_ten], [cmt], [sdt], [mail], [diem], [dia_chi], [so_cmnd], [ngay_sinh], [gioi_tinh], [trang_thai]) VALUES (6, N'tranvietbao', N'12345', N'Trần Viết Bảo', N'2214585647', N'0168338754', N'tranvietbao@gmail.com', 0, NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[tblKhachHang] ([ma_kh], [ten_dang_nhap], [mat_khau], [ho_ten], [cmt], [sdt], [mail], [diem], [dia_chi], [so_cmnd], [ngay_sinh], [gioi_tinh], [trang_thai]) VALUES (7, N'thanhtrung', N'123456789', N'Nguyễn Thành Trung', N'22147897214', N'09879841465', N'thanhtrung@gmail.com', 0, NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[tblKhachHang] ([ma_kh], [ten_dang_nhap], [mat_khau], [ho_ten], [cmt], [sdt], [mail], [diem], [dia_chi], [so_cmnd], [ngay_sinh], [gioi_tinh], [trang_thai]) VALUES (8, N'nghia1', N'123', N'Nguyễn Trung  Nghĩa', N'226546546321', N'01687621321', N'darkheaven@gmail.com', NULL, NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[tblKhachHang] ([ma_kh], [ten_dang_nhap], [mat_khau], [ho_ten], [cmt], [sdt], [mail], [diem], [dia_chi], [so_cmnd], [ngay_sinh], [gioi_tinh], [trang_thai]) VALUES (9, N'khangnguyen123', N'1235', N'Khang', N'21321651321', N'01683368991', N'darkheaxzcxzven@gmail.com', NULL, NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[tblKhachHang] ([ma_kh], [ten_dang_nhap], [mat_khau], [ho_ten], [cmt], [sdt], [mail], [diem], [dia_chi], [so_cmnd], [ngay_sinh], [gioi_tinh], [trang_thai]) VALUES (10, N'huutuan', N'123456', N'Nguyễn Hữu Tuấn', NULL, N'01987321414', N'huutuan@gmail.com', 0, NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[tblKhachHang] ([ma_kh], [ten_dang_nhap], [mat_khau], [ho_ten], [cmt], [sdt], [mail], [diem], [dia_chi], [so_cmnd], [ngay_sinh], [gioi_tinh], [trang_thai]) VALUES (11, N'huanganxuyen', N'123456', N'Hứa Ngân Xuyên', NULL, N'0198765421', N'nganxuyen@gmail.com', 0, NULL, NULL, NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[tblKhachHang] OFF
SET IDENTITY_INSERT [dbo].[tblLoaiPhong] ON 

INSERT [dbo].[tblLoaiPhong] ([loai_phong], [mo_ta], [gia], [ti_le_phu_thu], [anh], [trang_thai]) VALUES (1, N'Phòng đơn', 100000, 20, N'/Content/Images/Phong/11.jpg', 1)
INSERT [dbo].[tblLoaiPhong] ([loai_phong], [mo_ta], [gia], [ti_le_phu_thu], [anh], [trang_thai]) VALUES (2, N'Phòng đôi', 150000, 25, N'/Content/Images/Phong/21.jpg', 1)
INSERT [dbo].[tblLoaiPhong] ([loai_phong], [mo_ta], [gia], [ti_le_phu_thu], [anh], [trang_thai]) VALUES (3, N'Phòng vip', 150000, 30, N'/Content/Images/Phong/31.jpg', 1)
INSERT [dbo].[tblLoaiPhong] ([loai_phong], [mo_ta], [gia], [ti_le_phu_thu], [anh], [trang_thai]) VALUES (5, N'Phòng cho sinh viên', 130000, 12, N'/Content/Images/Phong/phongsinhvien.jpg', 0)
INSERT [dbo].[tblLoaiPhong] ([loai_phong], [mo_ta], [gia], [ti_le_phu_thu], [anh], [trang_thai]) VALUES (7, N'Phòng cao cấp', 200000, 22, N'/Content/Images/Phong/phongcaocap.jpg', 0)
SET IDENTITY_INSERT [dbo].[tblLoaiPhong] OFF
SET IDENTITY_INSERT [dbo].[tblNhanVien] ON 

INSERT [dbo].[tblNhanVien] ([ma_nv], [ho_ten], [ngay_sinh], [dia_chi], [sdt], [tai_khoan], [mat_khau], [ma_chuc_vu], [trang_thai_tai_khoan], [mail]) VALUES (1, N'Trung Nghĩa', CAST(N'1998-06-11' AS Date), N'138 Quang Trung', N'01677915896', N'tieunghia', N'123', 1, 1, N'lightheaven98@gmail.com')
INSERT [dbo].[tblNhanVien] ([ma_nv], [ho_ten], [ngay_sinh], [dia_chi], [sdt], [tai_khoan], [mat_khau], [ma_chuc_vu], [trang_thai_tai_khoan], [mail]) VALUES (2, N'Khang Nguyễn', CAST(N'1998-01-01' AS Date), N'98 Phạm Văn Đồng', N'09987345645', N'khangnguyen', N'123456', 3, 1, N'khangnguyen@gmail.com')
INSERT [dbo].[tblNhanVien] ([ma_nv], [ho_ten], [ngay_sinh], [dia_chi], [sdt], [tai_khoan], [mat_khau], [ma_chuc_vu], [trang_thai_tai_khoan], [mail]) VALUES (3, N'Văn Hùng', CAST(N'1900-01-01' AS Date), N'194 Nguyễn Bình Chiểu', N'01564897987', N'hungoggy', N'123', 3, 1, N'vanhung@gmail.com')
INSERT [dbo].[tblNhanVien] ([ma_nv], [ho_ten], [ngay_sinh], [dia_chi], [sdt], [tai_khoan], [mat_khau], [ma_chuc_vu], [trang_thai_tai_khoan], [mail]) VALUES (4, N'Trung Nghĩa', NULL, N'Lý Thường Kiệt', N'0981019704', N'admin', N'123', 2, 1, N'nghia.trungnguyen1980@gmail.com')
INSERT [dbo].[tblNhanVien] ([ma_nv], [ho_ten], [ngay_sinh], [dia_chi], [sdt], [tai_khoan], [mat_khau], [ma_chuc_vu], [trang_thai_tai_khoan], [mail]) VALUES (5, N'Nguyễn Duy Poon', NULL, N'Hòa Mỹ', N'09865423171', N'duypoon', N'123456', 2, 0, N'duypoon@gmail.com')
INSERT [dbo].[tblNhanVien] ([ma_nv], [ho_ten], [ngay_sinh], [dia_chi], [sdt], [tai_khoan], [mat_khau], [ma_chuc_vu], [trang_thai_tai_khoan], [mail]) VALUES (6, N'Đặng Ngọc Tuấn', NULL, N'Bình Dương', N'0987321654', N'ngoctuan', N'123456', 1, 0, N'ngoctuan@gmail.com')
SET IDENTITY_INSERT [dbo].[tblNhanVien] OFF
SET IDENTITY_INSERT [dbo].[tblPhieuDatPhong] ON 

INSERT [dbo].[tblPhieuDatPhong] ([ma_pdp], [ma_kh], [ngay_dat], [ngay_vao], [ngay_ra], [ma_phong], [thong_tin_khach_thue], [ma_tinh_trang]) VALUES (1, 1, CAST(N'2019-05-21T19:30:00.000' AS DateTime), CAST(N'2019-05-23T15:30:00.000' AS DateTime), CAST(N'2019-05-24T00:00:00.000' AS DateTime), 1, NULL, 4)
INSERT [dbo].[tblPhieuDatPhong] ([ma_pdp], [ma_kh], [ngay_dat], [ngay_vao], [ngay_ra], [ma_phong], [thong_tin_khach_thue], [ma_tinh_trang]) VALUES (2, 2, CAST(N'2019-05-23T07:30:00.000' AS DateTime), CAST(N'2019-05-25T12:30:00.000' AS DateTime), CAST(N'2019-07-26T18:30:00.000' AS DateTime), 3, NULL, 3)
INSERT [dbo].[tblPhieuDatPhong] ([ma_pdp], [ma_kh], [ngay_dat], [ngay_vao], [ngay_ra], [ma_phong], [thong_tin_khach_thue], [ma_tinh_trang]) VALUES (3, 2, CAST(N'2019-05-23T20:30:32.857' AS DateTime), CAST(N'2019-05-23T20:30:32.663' AS DateTime), CAST(N'2019-05-24T20:30:00.000' AS DateTime), 7, N'[{"hoten":"","socmt":null,"tuoi":"","sodt":null},{"hoten":"Nguyễn Duy Poon","socmt":null,"tuoi":"21","sodt":null},{"hoten":"Trần Viết Bảo","socmt":null,"tuoi":"22","sodt":null}]', 4)
INSERT [dbo].[tblPhieuDatPhong] ([ma_pdp], [ma_kh], [ngay_dat], [ngay_vao], [ngay_ra], [ma_phong], [thong_tin_khach_thue], [ma_tinh_trang]) VALUES (4, NULL, CAST(N'2019-05-23T20:40:39.763' AS DateTime), CAST(N'2019-05-23T20:40:39.213' AS DateTime), CAST(N'2019-05-24T20:37:00.000' AS DateTime), 1, N'[{"hoten":"Nguyễn Thành Bảo","socmt":"22105478421","tuoi":"36","sodt":"0954841445"},{"hoten":"Lại Lý Huynh","socmt":null,"tuoi":"28","sodt":null},{"hoten":"Trềnh A Sáng","socmt":null,"tuoi":"45","sodt":null}]', 4)
INSERT [dbo].[tblPhieuDatPhong] ([ma_pdp], [ma_kh], [ngay_dat], [ngay_vao], [ngay_ra], [ma_phong], [thong_tin_khach_thue], [ma_tinh_trang]) VALUES (5, NULL, CAST(N'2019-05-27T20:26:56.343' AS DateTime), CAST(N'2019-05-27T20:26:53.703' AS DateTime), CAST(N'2019-05-28T00:00:00.000' AS DateTime), 16, N'[{"hoten":"Nguyễn Hữu Tuấn","socmt":"2210388724","tuoi":"27","sodt":"099872175"},{"hoten":"Nguyễn Hữu Tú","socmt":null,"tuoi":"21","sodt":null},{"hoten":"Trần Thị Kim Sang","socmt":null,"tuoi":"29","sodt":null},{"hoten":"Nguyễn Lê Bảo Nhi","socmt":null,"tuoi":"19","sodt":null}]', 4)
INSERT [dbo].[tblPhieuDatPhong] ([ma_pdp], [ma_kh], [ngay_dat], [ngay_vao], [ngay_ra], [ma_phong], [thong_tin_khach_thue], [ma_tinh_trang]) VALUES (6, 5, CAST(N'2019-05-27T22:14:22.463' AS DateTime), CAST(N'2019-05-27T22:14:22.463' AS DateTime), CAST(N'2019-05-28T00:00:00.000' AS DateTime), 17, N'[{"hoten":"","socmt":null,"tuoi":"","sodt":null}]', 4)
INSERT [dbo].[tblPhieuDatPhong] ([ma_pdp], [ma_kh], [ngay_dat], [ngay_vao], [ngay_ra], [ma_phong], [thong_tin_khach_thue], [ma_tinh_trang]) VALUES (7, 1, CAST(N'2019-06-03T10:43:09.513' AS DateTime), CAST(N'2019-06-03T12:00:00.000' AS DateTime), CAST(N'2019-06-06T12:00:00.000' AS DateTime), 1, NULL, 3)
INSERT [dbo].[tblPhieuDatPhong] ([ma_pdp], [ma_kh], [ngay_dat], [ngay_vao], [ngay_ra], [ma_phong], [thong_tin_khach_thue], [ma_tinh_trang]) VALUES (8, 1, CAST(N'2019-06-03T10:43:09.513' AS DateTime), CAST(N'2019-06-03T12:00:00.000' AS DateTime), CAST(N'2019-06-06T12:00:00.000' AS DateTime), 2, N'[{"hoten":"","socmt":null,"tuoi":"","sodt":null},{"hoten":"Nguyễn Hữu Tú","socmt":null,"tuoi":"21","sodt":null},{"hoten":"Trần Thị Kim Sang","socmt":null,"tuoi":"38","sodt":null},{"hoten":"Nguyễn Lê Bảo Nhi","socmt":null,"tuoi":"19","sodt":null},{"hoten":"Nguyễn Hữu Tuấn","socmt":null,"tuoi":"34","sodt":null}]', 2)
SET IDENTITY_INSERT [dbo].[tblPhieuDatPhong] OFF
SET IDENTITY_INSERT [dbo].[tblPhong] ON 

INSERT [dbo].[tblPhong] ([ma_phong], [so_phong], [loai_phong], [ma_tang], [ma_tinh_trang]) VALUES (1, N'101', 1, 1, 1)
INSERT [dbo].[tblPhong] ([ma_phong], [so_phong], [loai_phong], [ma_tang], [ma_tinh_trang]) VALUES (2, N'102', 1, 1, 2)
INSERT [dbo].[tblPhong] ([ma_phong], [so_phong], [loai_phong], [ma_tang], [ma_tinh_trang]) VALUES (3, N'103', 1, 1, 1)
INSERT [dbo].[tblPhong] ([ma_phong], [so_phong], [loai_phong], [ma_tang], [ma_tinh_trang]) VALUES (4, N'104', 2, 1, 1)
INSERT [dbo].[tblPhong] ([ma_phong], [so_phong], [loai_phong], [ma_tang], [ma_tinh_trang]) VALUES (5, N'105', 2, 1, 1)
INSERT [dbo].[tblPhong] ([ma_phong], [so_phong], [loai_phong], [ma_tang], [ma_tinh_trang]) VALUES (6, N'106', 3, 1, 1)
INSERT [dbo].[tblPhong] ([ma_phong], [so_phong], [loai_phong], [ma_tang], [ma_tinh_trang]) VALUES (7, N'201', 1, 2, 1)
INSERT [dbo].[tblPhong] ([ma_phong], [so_phong], [loai_phong], [ma_tang], [ma_tinh_trang]) VALUES (8, N'202', 1, 2, 1)
INSERT [dbo].[tblPhong] ([ma_phong], [so_phong], [loai_phong], [ma_tang], [ma_tinh_trang]) VALUES (9, N'203', 1, 2, 1)
INSERT [dbo].[tblPhong] ([ma_phong], [so_phong], [loai_phong], [ma_tang], [ma_tinh_trang]) VALUES (10, N'204', 2, 2, 1)
INSERT [dbo].[tblPhong] ([ma_phong], [so_phong], [loai_phong], [ma_tang], [ma_tinh_trang]) VALUES (11, N'205', 3, 2, 1)
INSERT [dbo].[tblPhong] ([ma_phong], [so_phong], [loai_phong], [ma_tang], [ma_tinh_trang]) VALUES (12, N'206', 3, 2, 1)
INSERT [dbo].[tblPhong] ([ma_phong], [so_phong], [loai_phong], [ma_tang], [ma_tinh_trang]) VALUES (13, N'301', 1, 3, 1)
INSERT [dbo].[tblPhong] ([ma_phong], [so_phong], [loai_phong], [ma_tang], [ma_tinh_trang]) VALUES (14, N'302', 1, 3, 1)
INSERT [dbo].[tblPhong] ([ma_phong], [so_phong], [loai_phong], [ma_tang], [ma_tinh_trang]) VALUES (15, N'303', 1, 3, 1)
INSERT [dbo].[tblPhong] ([ma_phong], [so_phong], [loai_phong], [ma_tang], [ma_tinh_trang]) VALUES (16, N'304', 2, 3, 1)
INSERT [dbo].[tblPhong] ([ma_phong], [so_phong], [loai_phong], [ma_tang], [ma_tinh_trang]) VALUES (17, N'305', 2, 3, 1)
INSERT [dbo].[tblPhong] ([ma_phong], [so_phong], [loai_phong], [ma_tang], [ma_tinh_trang]) VALUES (18, N'306', 3, 3, 1)
INSERT [dbo].[tblPhong] ([ma_phong], [so_phong], [loai_phong], [ma_tang], [ma_tinh_trang]) VALUES (19, N'107', 3, 1, 1)
INSERT [dbo].[tblPhong] ([ma_phong], [so_phong], [loai_phong], [ma_tang], [ma_tinh_trang]) VALUES (20, N'308', 3, 3, 1)
INSERT [dbo].[tblPhong] ([ma_phong], [so_phong], [loai_phong], [ma_tang], [ma_tinh_trang]) VALUES (21, N'401', 5, 4, 1)
INSERT [dbo].[tblPhong] ([ma_phong], [so_phong], [loai_phong], [ma_tang], [ma_tinh_trang]) VALUES (22, N'402', 5, 4, 5)
SET IDENTITY_INSERT [dbo].[tblPhong] OFF
SET IDENTITY_INSERT [dbo].[tblTang] ON 

INSERT [dbo].[tblTang] ([ma_tang], [ten_tang]) VALUES (1, N'Tầng 1')
INSERT [dbo].[tblTang] ([ma_tang], [ten_tang]) VALUES (2, N'Tầng 2')
INSERT [dbo].[tblTang] ([ma_tang], [ten_tang]) VALUES (3, N'Tầng 3')
INSERT [dbo].[tblTang] ([ma_tang], [ten_tang]) VALUES (4, N'Tầng 4')
SET IDENTITY_INSERT [dbo].[tblTang] OFF
SET IDENTITY_INSERT [dbo].[tblTinhTrangHoaDon] ON 

INSERT [dbo].[tblTinhTrangHoaDon] ([ma_tinh_trang], [mo_ta]) VALUES (1, N'Chưa thanh toán')
INSERT [dbo].[tblTinhTrangHoaDon] ([ma_tinh_trang], [mo_ta]) VALUES (2, N'Đã thanh toán')
SET IDENTITY_INSERT [dbo].[tblTinhTrangHoaDon] OFF
SET IDENTITY_INSERT [dbo].[tblTinhTrangPhieuDatPhong] ON 

INSERT [dbo].[tblTinhTrangPhieuDatPhong] ([ma_tinh_trang], [tinh_trang]) VALUES (1, N'Mới')
INSERT [dbo].[tblTinhTrangPhieuDatPhong] ([ma_tinh_trang], [tinh_trang]) VALUES (2, N'Đã nhận phòng')
INSERT [dbo].[tblTinhTrangPhieuDatPhong] ([ma_tinh_trang], [tinh_trang]) VALUES (3, N'Đã hủy')
INSERT [dbo].[tblTinhTrangPhieuDatPhong] ([ma_tinh_trang], [tinh_trang]) VALUES (4, N'Đã thanh toán')
SET IDENTITY_INSERT [dbo].[tblTinhTrangPhieuDatPhong] OFF
SET IDENTITY_INSERT [dbo].[tblTinhTrangPhong] ON 

INSERT [dbo].[tblTinhTrangPhong] ([ma_tinh_trang], [mo_ta]) VALUES (1, N'Trống')
INSERT [dbo].[tblTinhTrangPhong] ([ma_tinh_trang], [mo_ta]) VALUES (2, N'Đang được sử dụng')
INSERT [dbo].[tblTinhTrangPhong] ([ma_tinh_trang], [mo_ta]) VALUES (3, N'Đang dọn')
INSERT [dbo].[tblTinhTrangPhong] ([ma_tinh_trang], [mo_ta]) VALUES (4, N'Đang bảo trì')
INSERT [dbo].[tblTinhTrangPhong] ([ma_tinh_trang], [mo_ta]) VALUES (5, N'Dừng sử dụng')
SET IDENTITY_INSERT [dbo].[tblTinhTrangPhong] OFF
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_tblKhachHang]    Script Date: 6/5/2019 8:55:05 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_tblKhachHang] ON [dbo].[tblKhachHang]
(
	[ten_dang_nhap] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tblKhachHang] ADD  CONSTRAINT [DF_tblKhachHang_diem]  DEFAULT ((0)) FOR [diem]
GO
ALTER TABLE [dbo].[tblDichVuDaDat]  WITH CHECK ADD  CONSTRAINT [fk_ma_dv] FOREIGN KEY([ma_dv])
REFERENCES [dbo].[tblDichVu] ([ma_dv])
GO
ALTER TABLE [dbo].[tblDichVuDaDat] CHECK CONSTRAINT [fk_ma_dv]
GO
ALTER TABLE [dbo].[tblDichVuDaDat]  WITH CHECK ADD  CONSTRAINT [fk_ma_hd] FOREIGN KEY([ma_hd])
REFERENCES [dbo].[tblHoaDon] ([ma_hd])
GO
ALTER TABLE [dbo].[tblDichVuDaDat] CHECK CONSTRAINT [fk_ma_hd]
GO
ALTER TABLE [dbo].[tblHoaDon]  WITH CHECK ADD  CONSTRAINT [fk_ma_nv] FOREIGN KEY([ma_nv])
REFERENCES [dbo].[tblNhanVien] ([ma_nv])
GO
ALTER TABLE [dbo].[tblHoaDon] CHECK CONSTRAINT [fk_ma_nv]
GO
ALTER TABLE [dbo].[tblHoaDon]  WITH CHECK ADD  CONSTRAINT [fk_ma_pdp] FOREIGN KEY([ma_pdp])
REFERENCES [dbo].[tblPhieuDatPhong] ([ma_pdp])
GO
ALTER TABLE [dbo].[tblHoaDon] CHECK CONSTRAINT [fk_ma_pdp]
GO
ALTER TABLE [dbo].[tblHoaDon]  WITH CHECK ADD  CONSTRAINT [fk_ma_tthd] FOREIGN KEY([ma_tinh_trang])
REFERENCES [dbo].[tblTinhTrangHoaDon] ([ma_tinh_trang])
GO
ALTER TABLE [dbo].[tblHoaDon] CHECK CONSTRAINT [fk_ma_tthd]
GO
ALTER TABLE [dbo].[tblNhanVien]  WITH CHECK ADD  CONSTRAINT [fk_ma_cv] FOREIGN KEY([ma_chuc_vu])
REFERENCES [dbo].[tblChucVu] ([ma_chuc_vu])
GO
ALTER TABLE [dbo].[tblNhanVien] CHECK CONSTRAINT [fk_ma_cv]
GO
ALTER TABLE [dbo].[tblPhieuDatPhong]  WITH CHECK ADD  CONSTRAINT [FK_tblPhieuDatPhong_tblKhachHang] FOREIGN KEY([ma_kh])
REFERENCES [dbo].[tblKhachHang] ([ma_kh])
GO
ALTER TABLE [dbo].[tblPhieuDatPhong] CHECK CONSTRAINT [FK_tblPhieuDatPhong_tblKhachHang]
GO
ALTER TABLE [dbo].[tblPhieuDatPhong]  WITH CHECK ADD  CONSTRAINT [fk_tgd_ma_phong_2] FOREIGN KEY([ma_phong])
REFERENCES [dbo].[tblPhong] ([ma_phong])
GO
ALTER TABLE [dbo].[tblPhieuDatPhong] CHECK CONSTRAINT [fk_tgd_ma_phong_2]
GO
ALTER TABLE [dbo].[tblPhieuDatPhong]  WITH CHECK ADD  CONSTRAINT [fk_tgd_tt_2] FOREIGN KEY([ma_tinh_trang])
REFERENCES [dbo].[tblTinhTrangPhieuDatPhong] ([ma_tinh_trang])
GO
ALTER TABLE [dbo].[tblPhieuDatPhong] CHECK CONSTRAINT [fk_tgd_tt_2]
GO
ALTER TABLE [dbo].[tblPhong]  WITH CHECK ADD  CONSTRAINT [fk_ma_lp] FOREIGN KEY([loai_phong])
REFERENCES [dbo].[tblLoaiPhong] ([loai_phong])
GO
ALTER TABLE [dbo].[tblPhong] CHECK CONSTRAINT [fk_ma_lp]
GO
ALTER TABLE [dbo].[tblPhong]  WITH CHECK ADD  CONSTRAINT [fk_ma_tang] FOREIGN KEY([ma_tang])
REFERENCES [dbo].[tblTang] ([ma_tang])
GO
ALTER TABLE [dbo].[tblPhong] CHECK CONSTRAINT [fk_ma_tang]
GO
ALTER TABLE [dbo].[tblPhong]  WITH CHECK ADD  CONSTRAINT [fk_ma_tt_2] FOREIGN KEY([ma_tinh_trang])
REFERENCES [dbo].[tblTinhTrangPhong] ([ma_tinh_trang])
GO
ALTER TABLE [dbo].[tblPhong] CHECK CONSTRAINT [fk_ma_tt_2]
GO
ALTER TABLE [dbo].[tblTinNhan]  WITH CHECK ADD  CONSTRAINT [FK_tblTinNhan_tblKhachHang] FOREIGN KEY([ma_kh])
REFERENCES [dbo].[tblKhachHang] ([ma_kh])
GO
ALTER TABLE [dbo].[tblTinNhan] CHECK CONSTRAINT [FK_tblTinNhan_tblKhachHang]
GO
USE [master]
GO
ALTER DATABASE [QuanLyKhachSan] SET  READ_WRITE 
GO
