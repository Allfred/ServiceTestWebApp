USE [{DBName}]
GO
/****** Object:  Table [dbo].[CartProducts]    Script Date: 04.08.2021 1:57:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CartProducts](
	[CartId] [int] NOT NULL,
	[ProductId] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Carts]    Script Date: 04.08.2021 1:57:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Carts](
	[Id] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[CreatedDateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_Carts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notifications]    Script Date: 04.08.2021 1:57:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notifications](
	[Message] [varchar](50) NOT NULL,
	[CreatedDateTime] [datetime] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 04.08.2021 1:57:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[Id] [int] NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[Cost] [decimal](18, 0) NOT NULL,
	[ForBonusPoints] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WebHooks]    Script Date: 04.08.2021 1:57:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WebHooks](
	[Id] [int] NULL,
	[WebHookType] [int] NULL,
	[ItemId] [int] NULL,
	[ExecutingUri] [varchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[CartProducts] ([CartId], [ProductId]) VALUES (1, 1)
INSERT [dbo].[CartProducts] ([CartId], [ProductId]) VALUES (1, 2)
INSERT [dbo].[CartProducts] ([CartId], [ProductId]) VALUES (2, 1)
INSERT [dbo].[CartProducts] ([CartId], [ProductId]) VALUES (11, 1)
INSERT [dbo].[CartProducts] ([CartId], [ProductId]) VALUES (3, 1)
INSERT [dbo].[CartProducts] ([CartId], [ProductId]) VALUES (4, 2)
GO
INSERT [dbo].[Carts] ([Id], [Name], [CreatedDateTime]) VALUES (1, N'Test1', CAST(N'2021-07-13T00:00:00.000' AS DateTime))
INSERT [dbo].[Carts] ([Id], [Name], [CreatedDateTime]) VALUES (2, N'Test2', CAST(N'2021-07-23T00:00:00.000' AS DateTime))
INSERT [dbo].[Carts] ([Id], [Name], [CreatedDateTime]) VALUES (3, N'Test2', CAST(N'2021-07-23T00:00:00.000' AS DateTime))
INSERT [dbo].[Carts] ([Id], [Name], [CreatedDateTime]) VALUES (4, N'Test212', CAST(N'2021-07-23T00:00:00.000' AS DateTime))
INSERT [dbo].[Carts] ([Id], [Name], [CreatedDateTime]) VALUES (5, N'Test1', CAST(N'2021-07-25T00:00:00.000' AS DateTime))
INSERT [dbo].[Carts] ([Id], [Name], [CreatedDateTime]) VALUES (10, N'Test11', CAST(N'2021-07-25T00:00:00.000' AS DateTime))
INSERT [dbo].[Carts] ([Id], [Name], [CreatedDateTime]) VALUES (11, N'Test11s', CAST(N'2021-07-25T00:00:00.000' AS DateTime))
GO
INSERT [dbo].[Notifications] ([Message], [CreatedDateTime]) VALUES (N'Cart:12 was deleted', CAST(N'2021-08-04T01:56:04.657' AS DateTime))
GO
INSERT [dbo].[Products] ([Id], [Name], [Cost], [ForBonusPoints]) VALUES (1, N'Apple', CAST(10 AS Decimal(18, 0)), 0)
INSERT [dbo].[Products] ([Id], [Name], [Cost], [ForBonusPoints]) VALUES (2, N'Mango', CAST(5 AS Decimal(18, 0)), 1)
INSERT [dbo].[Products] ([Id], [Name], [Cost], [ForBonusPoints]) VALUES (3, N'Orange', CAST(15 AS Decimal(18, 0)), 0)
INSERT [dbo].[Products] ([Id], [Name], [Cost], [ForBonusPoints]) VALUES (5, N'avocado', CAST(20 AS Decimal(18, 0)), 0)
GO
INSERT [dbo].[WebHooks] ([Id], [WebHookType], [ItemId], [ExecutingUri]) VALUES (1, 1, 12, N'http://localhost:5000/api/v1/NotificationListener/items')
GO
ALTER TABLE [dbo].[CartProducts]  WITH CHECK ADD  CONSTRAINT [FK_Cart] FOREIGN KEY([CartId])
REFERENCES [dbo].[Carts] ([Id])
GO
ALTER TABLE [dbo].[CartProducts] CHECK CONSTRAINT [FK_Cart]
GO
ALTER TABLE [dbo].[CartProducts]  WITH CHECK ADD  CONSTRAINT [FK_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([Id])
GO
ALTER TABLE [dbo].[CartProducts] CHECK CONSTRAINT [FK_Product]
GO
/****** Object:  StoredProcedure [dbo].[uspAddToCartProduct]    Script Date: 04.08.2021 1:57:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[uspAddToCartProduct]
@CartId int,
@ProductId int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO CartProducts(CartId, ProductId)
				Values(@CartId, @ProductId)
END
GO
/****** Object:  StoredProcedure [dbo].[uspCreateCart]    Script Date: 04.08.2021 1:57:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[uspCreateCart]
@Id int,
@Name varchar(50),
@CreatedDateTime datetime
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO Carts(Id, Name, CreatedDateTime)
				Values(@Id, @Name, @CreatedDateTime)
END
GO
/****** Object:  StoredProcedure [dbo].[uspCreateNotification]    Script Date: 04.08.2021 1:57:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Alfred>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[uspCreateNotification]
@Message varchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO dbo.Notifications(Message,CreatedDateTime)
				Values(@Message,GETDATE())
END
GO
/****** Object:  StoredProcedure [dbo].[uspCreateProduct]    Script Date: 04.08.2021 1:57:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [dbo].[uspCreateProduct]
@Id int,
@Name varchar(50),
@Cost decimal,
@ForBonusPoints bit
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO dbo.Products(Id, Name, Cost, ForBonusPoints)
				Values(@Id, @Name, @Cost, @ForBonusPoints)
END
GO
/****** Object:  StoredProcedure [dbo].[uspCreateWebHook]    Script Date: 04.08.2021 1:57:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[uspCreateWebHook]
@Id int,
@WebHookType int,
@ItemId int,
@ExecutingUri varchar(Max)
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO dbo.WebHooks(Id,WebHookType, ItemId, ExecutingUri)
				Values(@Id, @WebHookType, @ItemId, @ExecutingUri)
END
GO
/****** Object:  StoredProcedure [dbo].[uspDeleteCartById]    Script Date: 04.08.2021 1:57:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Alfred>
-- Description:	<Delete cart by id>
-- =============================================
CREATE PROCEDURE [dbo].[uspDeleteCartById] 
	@Id int
AS
BEGIN
	SET NOCOUNT ON;

	exec uspDeleteCartProductsByCartId @id

	delete from Carts
		where Id=@Id
END
GO
/****** Object:  StoredProcedure [dbo].[uspDeleteCartProductsByCartId]    Script Date: 04.08.2021 1:57:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Alfred>
-- Description:	<Delete cart by id>
-- =============================================
Create PROCEDURE [dbo].[uspDeleteCartProductsByCartId]
	@Id int
AS
BEGIN
	SET NOCOUNT ON;

	delete from CartProducts
		where CartId=@Id

END
GO
/****** Object:  StoredProcedure [dbo].[uspDeleteFromCartProduct]    Script Date: 04.08.2021 1:57:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create PROCEDURE [dbo].[uspDeleteFromCartProduct]
@CartId int,
@ProductId int
AS
BEGIN
	SET NOCOUNT ON;

	delete from CartProducts
		where CartId=@CartId AND ProductId=@ProductId
END
GO
/****** Object:  StoredProcedure [dbo].[uspDeleteProductById]    Script Date: 04.08.2021 1:57:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Alfred>
-- Description:	<Delete cart by id>
-- =============================================
Create PROCEDURE [dbo].[uspDeleteProductById] 
	@Id int
AS
BEGIN
	SET NOCOUNT ON;

	delete from Products
		where Id=@Id
END
GO
/****** Object:  StoredProcedure [dbo].[uspDeleteWebHookById]    Script Date: 04.08.2021 1:57:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Alfred>
-- Description:	<Delete webhook by id>
-- =============================================
Create PROCEDURE [dbo].[uspDeleteWebHookById] 
	@Id int
AS
BEGIN
	SET NOCOUNT ON;

	delete from WebHooks
		where Id=@Id
END
GO
/****** Object:  StoredProcedure [dbo].[uspGetCartById]    Script Date: 04.08.2021 1:57:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Alfred>
-- Description:	<Get cart by id>
-- =============================================
CREATE PROCEDURE [dbo].[uspGetCartById] 
	@Id int
AS
BEGIN
	SET NOCOUNT ON;

	Select Id, Name, CreatedDateTime 
		from Carts
		where Id = @id
END
GO
/****** Object:  StoredProcedure [dbo].[uspGetCarts]    Script Date: 04.08.2021 1:57:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Alfred>
-- Description:	<Get carts>
-- =============================================
Create PROCEDURE [dbo].[uspGetCarts] 
AS
BEGIN
	SET NOCOUNT ON;

	Select Id, Name, CreatedDateTime 
		from Carts
END
GO
/****** Object:  StoredProcedure [dbo].[uspGetProductById]    Script Date: 04.08.2021 1:57:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Alfred>
-- Description:	<Get product by id>
-- =============================================
CREATE PROCEDURE [dbo].[uspGetProductById] 
	@Id int
AS
BEGIN
	SET NOCOUNT ON;

	Select Id, Name, Cost, ForBonusPoints 
		from Products
		where Id = @id
END
GO
/****** Object:  StoredProcedure [dbo].[uspGetProducts]    Script Date: 04.08.2021 1:57:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Alfred>
-- Description:	<Get carts>
-- =============================================
Create PROCEDURE [dbo].[uspGetProducts] 
AS
BEGIN
	SET NOCOUNT ON;

	Select Id, Name, Cost, ForBonusPoints 
		from Products
END
GO
/****** Object:  StoredProcedure [dbo].[uspGetProductsByCartId]    Script Date: 04.08.2021 1:57:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Alfred>
-- Description:	<Get products by cartid>
-- =============================================
CREATE PROCEDURE [dbo].[uspGetProductsByCartId] 
	@Id int
AS
BEGIN
	SET NOCOUNT ON;

	select p.Id, p.Name, p.Cost, p.ForBonusPoints
		from products p
        inner join CartProducts cp on p.Id = cp.ProductId
		where cp.CartId=@Id;
END
GO
/****** Object:  StoredProcedure [dbo].[uspGetWebHookById]    Script Date: 04.08.2021 1:57:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Alfred>
-- Description:	<Get webhook by id>
-- =============================================
CREATE PROCEDURE [dbo].[uspGetWebHookById] 
	@Id int
AS
BEGIN
	SET NOCOUNT ON;

	Select Id, WebHookType, ItemId, ExecutingUri 
		from WebHooks
		where Id = @id
END
GO
/****** Object:  StoredProcedure [dbo].[uspGetWebHookByItemIdAndType]    Script Date: 04.08.2021 1:57:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Alfred>
-- Description:	<Get webhook by itemId and webhooktype>
-- =============================================
Create PROCEDURE [dbo].[uspGetWebHookByItemIdAndType] 
@WebHookType int,
@ItemId int
AS
BEGIN
	SET NOCOUNT ON;

	Select Id, WebHookType, ItemId, ExecutingUri
		from WebHooks
		where WebHookType=@WebHookType AND ItemId=@ItemId
END
GO
/****** Object:  StoredProcedure [dbo].[uspGetWebHooks]    Script Date: 04.08.2021 1:57:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Alfred>
-- Description:	<Get webhooks>
-- =============================================
Create PROCEDURE [dbo].[uspGetWebHooks] 
AS
BEGIN
	SET NOCOUNT ON;

	Select Id, WebHookType, ItemId, ExecutingUri 
		from WebHooks
END
GO
/****** Object:  StoredProcedure [dbo].[uspUpdateCart]    Script Date: 04.08.2021 1:57:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [dbo].[uspUpdateCart]
@id int,
@Name varchar(50),
@CreatedDateTime datetime
AS
BEGIN
	SET NOCOUNT ON;

	Update Carts 
		Set Name=@Name, CreatedDateTime = @CreatedDateTime  
		Where Id=@id
END
GO
/****** Object:  StoredProcedure [dbo].[uspUpdateProduct]    Script Date: 04.08.2021 1:57:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE [dbo].[uspUpdateProduct]
@Id int,
@Name varchar(50),
@Cost decimal,
@ForBonusPoints bit
AS
BEGIN
	SET NOCOUNT ON;

	Update Products 
		Set Name=@Name, Cost= @Cost, ForBonusPoints=@ForBonusPoints
		Where Id=@id
END
GO
