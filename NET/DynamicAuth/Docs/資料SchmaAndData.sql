USE [Auth]
GO
/****** Object:  Table [dbo].[Endpoint]    Script Date: 2024/5/5 下午 11:46:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Endpoint](
	[EndpointId] [varchar](50) NOT NULL,
	[EndpointName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Endpoint] PRIMARY KEY CLUSTERED 
(
	[EndpointId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 2024/5/5 下午 11:46:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[RoleId] [varchar](30) NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role_Endpoint]    Script Date: 2024/5/5 下午 11:46:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role_Endpoint](
	[SeqNo] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [varchar](30) NOT NULL,
	[EndpointId] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Role_Endpoint] PRIMARY KEY CLUSTERED 
(
	[SeqNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 2024/5/5 下午 11:46:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[UserId] [varchar](50) NOT NULL,
	[Password] [varchar](50) NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User_Role]    Script Date: 2024/5/5 下午 11:46:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User_Role](
	[SeqNo] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [varchar](50) NOT NULL,
	[RoleId] [varchar](30) NOT NULL,
 CONSTRAINT [PK_User_Role] PRIMARY KEY CLUSTERED 
(
	[SeqNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Endpoint] ([EndpointId], [EndpointName]) VALUES (N'AddFeature', N'AddFeature')
INSERT [dbo].[Endpoint] ([EndpointId], [EndpointName]) VALUES (N'DeleteFeature', N'DeleteFeature')
INSERT [dbo].[Endpoint] ([EndpointId], [EndpointName]) VALUES (N'EditFeature', N'EditFeature')
INSERT [dbo].[Endpoint] ([EndpointId], [EndpointName]) VALUES (N'QueryFeature', N'QueryFeature')
GO
INSERT [dbo].[Role] ([RoleId], [RoleName]) VALUES (N'Admin', N'系統管理員')
INSERT [dbo].[Role] ([RoleId], [RoleName]) VALUES (N'Guest', N'訪客')
GO
SET IDENTITY_INSERT [dbo].[Role_Endpoint] ON 

INSERT [dbo].[Role_Endpoint] ([SeqNo], [RoleId], [EndpointId]) VALUES (1, N'Admin', N'QueryFeature')
INSERT [dbo].[Role_Endpoint] ([SeqNo], [RoleId], [EndpointId]) VALUES (5, N'Guest', N'QueryFeature')
INSERT [dbo].[Role_Endpoint] ([SeqNo], [RoleId], [EndpointId]) VALUES (6, N'Admin', N'AddFeature')
INSERT [dbo].[Role_Endpoint] ([SeqNo], [RoleId], [EndpointId]) VALUES (8, N'Admin', N'EditFeature')
INSERT [dbo].[Role_Endpoint] ([SeqNo], [RoleId], [EndpointId]) VALUES (9, N'Admin', N'DeleteFeature')
SET IDENTITY_INSERT [dbo].[Role_Endpoint] OFF
GO
INSERT [dbo].[User] ([UserId], [Password]) VALUES (N'Admin', N'=-09poiu')
INSERT [dbo].[User] ([UserId], [Password]) VALUES (N'Jason', N'=-09poiu')
GO
SET IDENTITY_INSERT [dbo].[User_Role] ON 

INSERT [dbo].[User_Role] ([SeqNo], [UserId], [RoleId]) VALUES (1, N'Admin', N'Admin')
INSERT [dbo].[User_Role] ([SeqNo], [UserId], [RoleId]) VALUES (2, N'Jason', N'Guest')
SET IDENTITY_INSERT [dbo].[User_Role] OFF
GO
