USE [master]
GO
/****** Object:  Database [wmCali]    Script Date: 12/29/2015 9:01:44 AM ******/
CREATE DATABASE [wmCali]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'wmCali', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\wmCali.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'wmCali_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\wmCali_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [wmCali] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [wmCali].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [wmCali] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [wmCali] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [wmCali] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [wmCali] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [wmCali] SET ARITHABORT OFF 
GO
ALTER DATABASE [wmCali] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [wmCali] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [wmCali] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [wmCali] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [wmCali] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [wmCali] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [wmCali] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [wmCali] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [wmCali] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [wmCali] SET  DISABLE_BROKER 
GO
ALTER DATABASE [wmCali] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [wmCali] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [wmCali] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [wmCali] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [wmCali] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [wmCali] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [wmCali] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [wmCali] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [wmCali] SET  MULTI_USER 
GO
ALTER DATABASE [wmCali] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [wmCali] SET DB_CHAINING OFF 
GO
ALTER DATABASE [wmCali] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [wmCali] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [wmCali] SET DELAYED_DURABILITY = DISABLED 
GO
USE [wmCali]
GO
/****** Object:  Table [dbo].[Agent]    Script Date: 12/29/2015 9:01:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Agent](
	[ID] [int] NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Details] [nvarchar](max) NULL,
	[TypeID] [int] NULL,
 CONSTRAINT [PK_Agency] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AgentType]    Script Date: 12/29/2015 9:01:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AgentType](
	[ID] [int] NOT NULL,
	[Name] [varchar](max) NULL,
 CONSTRAINT [PK_AgencyType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Employee]    Script Date: 12/29/2015 9:01:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employee](
	[ID] [int] NOT NULL,
	[Name] [nvarchar](max) NULL,
	[AgentID] [int] NULL,
 CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Inventory]    Script Date: 12/29/2015 9:01:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Inventory](
	[ID] [int] NOT NULL,
	[DateCreated] [datetime] NULL,
	[MerchandiseID] [int] NULL,
	[AgentID] [int] NULL,
	[EmployeeID] [int] NOT NULL,
	[Quantity] [float] NULL,
	[RealQuantity] [float] NULL,
	[AgentFromID] [int] NULL,
 CONSTRAINT [PK_Inventory] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[InventoryTransaction]    Script Date: 12/29/2015 9:01:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InventoryTransaction](
	[ID] [int] NOT NULL,
	[AgentFromID] [int] NULL,
	[AgentToID] [int] NULL,
 CONSTRAINT [PK_InventoryTransaction] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[InventoryTransactionDetails]    Script Date: 12/29/2015 9:01:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InventoryTransactionDetails](
	[ID] [int] NOT NULL,
	[TransactionID] [int] NULL,
	[MerchandiseID] [int] NULL,
	[Quantity] [float] NULL,
 CONSTRAINT [PK_InventoryTransactionDetails] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Merchandise]    Script Date: 12/29/2015 9:01:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Merchandise](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NameUnicode] [nvarchar](max) NULL,
	[NameAnsi] [nvarchar](max) NULL,
	[Unit] [nvarchar](50) NULL,
	[Details] [nvarchar](max) NULL,
	[CategoryID] [int] NULL,
	[TypeID] [int] NULL,
 CONSTRAINT [PK_Merchandise] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MerchandiseCategory]    Script Date: 12/29/2015 9:01:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MerchandiseCategory](
	[ID] [int] NOT NULL,
	[Name] [nvarchar](max) NULL,
	[IdParentNode] [int] NULL,
 CONSTRAINT [PK_GoodType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MerchandiseCategory_AgencyType_Junction]    Script Date: 12/29/2015 9:01:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MerchandiseCategory_AgencyType_Junction](
	[AgencyTypeID] [int] NULL,
	[MerchandiseCategoryID] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MerchandiseHistory]    Script Date: 12/29/2015 9:01:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MerchandiseHistory](
	[ID] [int] NOT NULL,
	[MerchandiseID] [int] NULL,
	[NameAccounting] [nvarchar](max) NULL,
	[TimeCreated] [datetime] NULL,
	[EmployeeCreatedID] [int] NULL,
 CONSTRAINT [PK_MerchandiseHistory] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MerchandiseQuantification]    Script Date: 12/29/2015 9:01:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MerchandiseQuantification](
	[ProductID] [int] NULL,
	[RawFoodID] [int] NULL,
	[Quantity] [float] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MerchandiseType]    Script Date: 12/29/2015 9:01:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MerchandiseType](
	[ID] [int] NOT NULL,
	[Name] [nvarchar](max) NULL,
 CONSTRAINT [PK_MerchandiseType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Order]    Script Date: 12/29/2015 9:01:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[ID] [int] NOT NULL,
	[DateCreated] [datetime] NULL,
	[AgentID] [int] NULL,
	[Name] [nvarchar](max) NULL,
 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Order_Merchandise_Junction]    Script Date: 12/29/2015 9:01:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order_Merchandise_Junction](
	[OrderValidationID] [int] NULL,
	[MerchandiseID] [int] NULL,
	[InventoryNumber] [int] NULL,
	[OrderNumber] [int] NULL,
	[Note] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[OrderValidation]    Script Date: 12/29/2015 9:01:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderValidation](
	[ID] [int] NOT NULL,
	[OrderID] [int] NULL,
	[EmployeeID] [int] NULL,
	[TimeValidated] [datetime] NULL,
	[Note] [nvarchar](max) NULL,
 CONSTRAINT [PK_OrderValidation] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[Agent]  WITH CHECK ADD  CONSTRAINT [FK_Agent_AgentType] FOREIGN KEY([TypeID])
REFERENCES [dbo].[AgentType] ([ID])
GO
ALTER TABLE [dbo].[Agent] CHECK CONSTRAINT [FK_Agent_AgentType]
GO
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD  CONSTRAINT [FK_Employee_Agent] FOREIGN KEY([AgentID])
REFERENCES [dbo].[Agent] ([ID])
GO
ALTER TABLE [dbo].[Employee] CHECK CONSTRAINT [FK_Employee_Agent]
GO
ALTER TABLE [dbo].[Inventory]  WITH CHECK ADD  CONSTRAINT [FK_Inventory_Agent] FOREIGN KEY([AgentID])
REFERENCES [dbo].[Agent] ([ID])
GO
ALTER TABLE [dbo].[Inventory] CHECK CONSTRAINT [FK_Inventory_Agent]
GO
ALTER TABLE [dbo].[Inventory]  WITH CHECK ADD  CONSTRAINT [FK_Inventory_Merchandise] FOREIGN KEY([MerchandiseID])
REFERENCES [dbo].[Merchandise] ([ID])
GO
ALTER TABLE [dbo].[Inventory] CHECK CONSTRAINT [FK_Inventory_Merchandise]
GO
ALTER TABLE [dbo].[InventoryTransactionDetails]  WITH CHECK ADD  CONSTRAINT [FK_InventoryTransactionDetails_InventoryTransaction] FOREIGN KEY([TransactionID])
REFERENCES [dbo].[InventoryTransaction] ([ID])
GO
ALTER TABLE [dbo].[InventoryTransactionDetails] CHECK CONSTRAINT [FK_InventoryTransactionDetails_InventoryTransaction]
GO
ALTER TABLE [dbo].[InventoryTransactionDetails]  WITH CHECK ADD  CONSTRAINT [FK_InventoryTransactionDetails_Merchandise] FOREIGN KEY([MerchandiseID])
REFERENCES [dbo].[Merchandise] ([ID])
GO
ALTER TABLE [dbo].[InventoryTransactionDetails] CHECK CONSTRAINT [FK_InventoryTransactionDetails_Merchandise]
GO
ALTER TABLE [dbo].[Merchandise]  WITH CHECK ADD  CONSTRAINT [FK_Merchandise_MerchandiseCategory] FOREIGN KEY([CategoryID])
REFERENCES [dbo].[MerchandiseCategory] ([ID])
GO
ALTER TABLE [dbo].[Merchandise] CHECK CONSTRAINT [FK_Merchandise_MerchandiseCategory]
GO
ALTER TABLE [dbo].[Merchandise]  WITH CHECK ADD  CONSTRAINT [FK_Merchandise_MerchandiseType] FOREIGN KEY([TypeID])
REFERENCES [dbo].[MerchandiseType] ([ID])
GO
ALTER TABLE [dbo].[Merchandise] CHECK CONSTRAINT [FK_Merchandise_MerchandiseType]
GO
ALTER TABLE [dbo].[MerchandiseCategory_AgencyType_Junction]  WITH CHECK ADD  CONSTRAINT [FK_MerchandiseCategory_AgencyType_Junction_AgentType] FOREIGN KEY([AgencyTypeID])
REFERENCES [dbo].[AgentType] ([ID])
GO
ALTER TABLE [dbo].[MerchandiseCategory_AgencyType_Junction] CHECK CONSTRAINT [FK_MerchandiseCategory_AgencyType_Junction_AgentType]
GO
ALTER TABLE [dbo].[MerchandiseCategory_AgencyType_Junction]  WITH CHECK ADD  CONSTRAINT [FK_MerchandiseCategory_AgencyType_Junction_MerchandiseCategory] FOREIGN KEY([MerchandiseCategoryID])
REFERENCES [dbo].[MerchandiseCategory] ([ID])
GO
ALTER TABLE [dbo].[MerchandiseCategory_AgencyType_Junction] CHECK CONSTRAINT [FK_MerchandiseCategory_AgencyType_Junction_MerchandiseCategory]
GO
ALTER TABLE [dbo].[MerchandiseHistory]  WITH CHECK ADD  CONSTRAINT [FK_MerchandiseHistory_Employee] FOREIGN KEY([EmployeeCreatedID])
REFERENCES [dbo].[Employee] ([ID])
GO
ALTER TABLE [dbo].[MerchandiseHistory] CHECK CONSTRAINT [FK_MerchandiseHistory_Employee]
GO
ALTER TABLE [dbo].[MerchandiseHistory]  WITH CHECK ADD  CONSTRAINT [FK_MerchandiseHistory_Merchandise] FOREIGN KEY([MerchandiseID])
REFERENCES [dbo].[Merchandise] ([ID])
GO
ALTER TABLE [dbo].[MerchandiseHistory] CHECK CONSTRAINT [FK_MerchandiseHistory_Merchandise]
GO
ALTER TABLE [dbo].[MerchandiseQuantification]  WITH CHECK ADD  CONSTRAINT [FK_MerchandiseQuantification_Merchandise] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Merchandise] ([ID])
GO
ALTER TABLE [dbo].[MerchandiseQuantification] CHECK CONSTRAINT [FK_MerchandiseQuantification_Merchandise]
GO
ALTER TABLE [dbo].[MerchandiseQuantification]  WITH CHECK ADD  CONSTRAINT [FK_MerchandiseQuantification_Merchandise1] FOREIGN KEY([RawFoodID])
REFERENCES [dbo].[Merchandise] ([ID])
GO
ALTER TABLE [dbo].[MerchandiseQuantification] CHECK CONSTRAINT [FK_MerchandiseQuantification_Merchandise1]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_Agent] FOREIGN KEY([AgentID])
REFERENCES [dbo].[Agent] ([ID])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_Agent]
GO
ALTER TABLE [dbo].[Order_Merchandise_Junction]  WITH CHECK ADD  CONSTRAINT [FK_Order_Merchandise_Junction_Merchandise] FOREIGN KEY([MerchandiseID])
REFERENCES [dbo].[Merchandise] ([ID])
GO
ALTER TABLE [dbo].[Order_Merchandise_Junction] CHECK CONSTRAINT [FK_Order_Merchandise_Junction_Merchandise]
GO
ALTER TABLE [dbo].[Order_Merchandise_Junction]  WITH CHECK ADD  CONSTRAINT [FK_Order_Merchandise_Junction_OrderValidation] FOREIGN KEY([OrderValidationID])
REFERENCES [dbo].[OrderValidation] ([ID])
GO
ALTER TABLE [dbo].[Order_Merchandise_Junction] CHECK CONSTRAINT [FK_Order_Merchandise_Junction_OrderValidation]
GO
ALTER TABLE [dbo].[OrderValidation]  WITH CHECK ADD  CONSTRAINT [FK_OrderValidation_Order] FOREIGN KEY([OrderID])
REFERENCES [dbo].[Order] ([ID])
GO
ALTER TABLE [dbo].[OrderValidation] CHECK CONSTRAINT [FK_OrderValidation_Order]
GO
ALTER TABLE [dbo].[OrderValidation]  WITH CHECK ADD  CONSTRAINT [FK_OrderValidation_Order1] FOREIGN KEY([OrderID])
REFERENCES [dbo].[Order] ([ID])
GO
ALTER TABLE [dbo].[OrderValidation] CHECK CONSTRAINT [FK_OrderValidation_Order1]
GO
USE [master]
GO
ALTER DATABASE [wmCali] SET  READ_WRITE 
GO
