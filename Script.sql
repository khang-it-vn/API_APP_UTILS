USE [AppUtils_V1]
GO
/****** Object:  Table [dbo].[REPAIRER]    Script Date: 1/4/2023 11:32:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[REPAIRER](
	[ID] [uniqueidentifier] NOT NULL,
	[HoTen] [nvarchar](200) NULL,
	[DiaChi] [nvarchar](200) NULL,
	[GioiTinh] [bit] NOT NULL,
	[DOB] [datetime2](7) NOT NULL,
	[NumberPhone] [nvarchar](13) NULL,
	[Email] [nvarchar](200) NULL,
	[Avatar] [nvarchar](max) NULL,
	[MatKhau] [nvarchar](100) NULL,
	[TrangThaiHoatDong] [bit] NOT NULL,
	[Latitude] [float] NOT NULL,
	[Longitude] [float] NOT NULL,
 CONSTRAINT [PK_REPAIRER] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[USER]    Script Date: 1/4/2023 11:32:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[USER](
	[UID] [uniqueidentifier] NOT NULL,
	[HoTen] [nvarchar](200) NULL,
	[DiaChi] [nvarchar](200) NOT NULL,
	[GioiTinh] [bit] NOT NULL,
	[DOB] [datetime2](7) NOT NULL,
	[NumberPhone] [nvarchar](13) NULL,
	[Email] [nvarchar](200) NULL,
	[Avatar] [nvarchar](max) NULL,
	[MatKhau] [nvarchar](100) NULL,
 CONSTRAINT [PK_USER] PRIMARY KEY CLUSTERED 
(
	[UID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[REPAIRER] ADD  DEFAULT (CONVERT([bit],(0))) FOR [TrangThaiHoatDong]
GO
ALTER TABLE [dbo].[REPAIRER] ADD  DEFAULT ((0.0000000000000000e+000)) FOR [Latitude]
GO
ALTER TABLE [dbo].[REPAIRER] ADD  DEFAULT ((0.0000000000000000e+000)) FOR [Longitude]
GO
ALTER TABLE [dbo].[USER] ADD  DEFAULT (N'') FOR [DiaChi]
GO
