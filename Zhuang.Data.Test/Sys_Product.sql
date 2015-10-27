
/****** Object:  Table [dbo].[Sys_Product]    Script Date: 10/27/2015 23:57:29 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sys_Product]') AND type in (N'U'))
DROP TABLE [dbo].[Sys_Product]
GO

/****** Object:  Table [dbo].[Sys_Product]    Script Date: 10/27/2015 23:57:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Sys_Product](
	[ProductId] [int] IDENTITY(1,1) NOT NULL,
	[ProductCode] [varchar](100) NULL,
	[ProductName] [nvarchar](100) NOT NULL,
	[BarCode] [varchar](50) NULL,
	[SearchCode] [varchar](50) NULL,
	[Unit] [varchar](20) NULL,
	[ImageUrl] [nvarchar](500) NULL,
	[Description] [nvarchar](500) NULL,
	[RecordStatus] [varchar](20) NULL,
	[CreatedById] [varchar](50) NULL,
	[ModifiedById] [varchar](50) NULL,
	[CreationDate] [datetime] NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_dbo.Sys_Product] PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


