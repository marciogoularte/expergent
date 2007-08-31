/* Generated on 8/31/2007 10:32:07 AM */

exec sp_dboption N'SampleCRM', N'autoclose', N'false'
GO

exec sp_dboption N'SampleCRM', N'bulkcopy', N'false'
GO

exec sp_dboption N'SampleCRM', N'trunc. log', N'true'
GO

exec sp_dboption N'SampleCRM', N'torn page detection', N'false'
GO

exec sp_dboption N'SampleCRM', N'read only', N'false'
GO

exec sp_dboption N'SampleCRM', N'dbo use', N'false'
GO

exec sp_dboption N'SampleCRM', N'single', N'false'
GO

exec sp_dboption N'SampleCRM', N'autoshrink', N'false'
GO

exec sp_dboption N'SampleCRM', N'ANSI null default', N'false'
GO

exec sp_dboption N'SampleCRM', N'recursive triggers', N'false'
GO

exec sp_dboption N'SampleCRM', N'ANSI nulls', N'false'
GO

exec sp_dboption N'SampleCRM', N'concat null yields null', N'false'
GO

exec sp_dboption N'SampleCRM', N'cursor close on commit', N'false'
GO

exec sp_dboption N'SampleCRM', N'default to local cursor', N'false'
GO

exec sp_dboption N'SampleCRM', N'quoted identifier', N'false'
GO

exec sp_dboption N'SampleCRM', N'ANSI warnings', N'false'
GO

exec sp_dboption N'SampleCRM', N'auto create statistics', N'true'
GO

exec sp_dboption N'SampleCRM', N'auto update statistics', N'true'
GO

if( (@@microsoftversion / power(2, 24) = 8) and (@@microsoftversion & 0xffff >= 724) )
	exec sp_dboption N'SampleCRM', N'db chaining', N'false'
GO

use [SampleCRM]
GO

/****** Object:  Table [dbo].[Addresses]    Script Date: 8/31/2007 10:32:07 AM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[Addresses]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [Addresses]
GO


/****** Object:  Table [dbo].[Customers]    Script Date: 8/31/2007 10:32:07 AM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[Customers]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [Customers]
GO


/****** Object:  Table [dbo].[Inventory]    Script Date: 8/31/2007 10:32:07 AM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[Inventory]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [Inventory]
GO


/****** Object:  Table [dbo].[OrderItems]    Script Date: 8/31/2007 10:32:07 AM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[OrderItems]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [OrderItems]
GO


/****** Object:  Table [dbo].[Orders]    Script Date: 8/31/2007 10:32:07 AM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[Orders]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [Orders]
GO


/****** Object:  Table [dbo].[Products]    Script Date: 8/31/2007 10:32:07 AM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[Products]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [Products]
GO


/****** Object:  Table [dbo].[Status]    Script Date: 8/31/2007 10:32:07 AM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[Status]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [Status]
GO


/****** Object:  Table [dbo].[Addresses]    Script Date: 8/31/2007 10:32:07 AM ******/
CREATE TABLE [Addresses] (
	[AddressID] [uniqueidentifier] NOT NULL ,
	[Address1] [varchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Address2] [varchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[City] [varchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[State] [char] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Zip] [char] (5) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Zip4] [char] (4) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO


/****** Object:  Table [dbo].[Customers]    Script Date: 8/31/2007 10:32:08 AM ******/
CREATE TABLE [Customers] (
	[CustomerID] [int] IDENTITY (1, 1) NOT NULL ,
	[Name] [varchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[MailingAddressID] [uniqueidentifier] NOT NULL ,
	[DeliveryAddressID] [uniqueidentifier] NOT NULL ,
	[StatusID] [int] NOT NULL ,
	[Remarks] [varchar] (512) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO


/****** Object:  Table [dbo].[Inventory]    Script Date: 8/31/2007 10:32:08 AM ******/
CREATE TABLE [Inventory] (
	[InventoryID] [uniqueidentifier] NOT NULL ,
	[ProductID] [uniqueidentifier] NOT NULL ,
	[StockOnHand] [int] NOT NULL 
) ON [PRIMARY]
GO


/****** Object:  Table [dbo].[OrderItems]    Script Date: 8/31/2007 10:32:08 AM ******/
CREATE TABLE [OrderItems] (
	[OrderItemID] [int] IDENTITY (1, 1) NOT NULL ,
	[OrderID] [int] NOT NULL ,
	[ProductID] [uniqueidentifier] NOT NULL ,
	[Quantity] [int] NOT NULL ,
	[UnitPrice] [decimal](18, 0) NOT NULL 
) ON [PRIMARY]
GO


/****** Object:  Table [dbo].[Orders]    Script Date: 8/31/2007 10:32:08 AM ******/
CREATE TABLE [Orders] (
	[OrderID] [int] IDENTITY (1, 1) NOT NULL ,
	[CustomerID] [int] NOT NULL ,
	[OrderDate] [datetime] NOT NULL ,
	[OrderAmount] [decimal](18, 0) NOT NULL 
) ON [PRIMARY]
GO


/****** Object:  Table [dbo].[Products]    Script Date: 8/31/2007 10:32:08 AM ******/
CREATE TABLE [Products] (
	[ProductID] [uniqueidentifier] NOT NULL ,
	[Description] [varchar] (512) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Supplier] [varchar] (512) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO


/****** Object:  Table [dbo].[Status]    Script Date: 8/31/2007 10:32:08 AM ******/
CREATE TABLE [Status] (
	[StatusID] [int] IDENTITY (1, 1) NOT NULL ,
	[Name] [varchar] (56) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO


/* Data for table Addresses */
INSERT INTO "Addresses" ("AddressID", "Address1", "Address2", "City", "State", "Zip", "Zip4") VALUES ('aeaf2c53-5a7d-4a88-855f-0c459c6f6da1', '11 West Tryon', NULL, 'Charlotte', 'NC', '28270', '2545')
INSERT INTO "Addresses" ("AddressID", "Address1", "Address2", "City", "State", "Zip", "Zip4") VALUES ('4bc42e6e-281d-4902-a30a-16795ebef4da', '1234 Apple Avenue', NULL, 'Payson', 'AZ', '85541', '5555')
INSERT INTO "Addresses" ("AddressID", "Address1", "Address2", "City", "State", "Zip", "Zip4") VALUES ('66a11966-bb5c-4933-9f38-250257d9195d', '4343 Thornbush Court', 'Suite 3', 'CHarlotte', 'NC', '28277', '9856')
INSERT INTO "Addresses" ("AddressID", "Address1", "Address2", "City", "State", "Zip", "Zip4") VALUES ('aa9845fe-f518-4bf8-b13d-c2568dcd1ed1', '555 Main Street', NULL, 'Phoenix', 'AZ', '85541', '1234')
/* Data for table Customers */
SET identity_insert [Customers] on

INSERT INTO "Customers" ("CustomerID", "Name", "MailingAddressID", "DeliveryAddressID", "StatusID", "Remarks") VALUES (1, 'Joe Blow', '4bc42e6e-281d-4902-a30a-16795ebef4da', 'aa9845fe-f518-4bf8-b13d-c2568dcd1ed1', 1, 'Test')
INSERT INTO "Customers" ("CustomerID", "Name", "MailingAddressID", "DeliveryAddressID", "StatusID", "Remarks") VALUES (2, 'Geek Store', 'aeaf2c53-5a7d-4a88-855f-0c459c6f6da1', '66a11966-bb5c-4933-9f38-250257d9195d', 1, 'Not in AZ')
SET identity_insert [Customers] off
GO
/* Data for table Inventory */
INSERT INTO "Inventory" ("InventoryID", "ProductID", "StockOnHand") VALUES ('450bee23-23b7-4b21-a941-77f8ba4a0a7e', 'b47ca5b0-9b65-4ec2-9f9e-a2c8e45b24d5', 50)
/* Data for table OrderItems */
SET identity_insert [OrderItems] on

INSERT INTO "OrderItems" ("OrderItemID", "OrderID", "ProductID", "Quantity", "UnitPrice") VALUES (1, 1, 'b47ca5b0-9b65-4ec2-9f9e-a2c8e45b24d5', 2, 30)
SET identity_insert [OrderItems] off
GO
/* Data for table Orders */
SET identity_insert [Orders] on

INSERT INTO "Orders" ("OrderID", "CustomerID", "OrderDate", "OrderAmount") VALUES (1, 1, '12/12/2006 12:00:00 AM', 25)
SET identity_insert [Orders] off
GO
/* Data for table Products */
INSERT INTO "Products" ("ProductID", "Description", "Supplier") VALUES ('b47ca5b0-9b65-4ec2-9f9e-a2c8e45b24d5', 'Troll Food', 'Arania')
/* Data for table Status */
SET identity_insert [Status] on

INSERT INTO "Status" ("StatusID", "Name") VALUES (1, 'Active')
SET identity_insert [Status] off
GO
/****** Object:  Table [dbo].[Addresses]    Script Date: 8/31/2007 10:32:10 AM ******/
ALTER TABLE [Addresses] WITH NOCHECK ADD 
	CONSTRAINT [DF_Addresses_AddressID] DEFAULT (newid()) FOR [AddressID],
	CONSTRAINT [pk_Addresses] PRIMARY KEY  CLUSTERED 
	(
		[AddressID]
	)  ON [PRIMARY] 
GO


/****** Object:  Table [dbo].[Customers]    Script Date: 8/31/2007 10:32:11 AM ******/
ALTER TABLE [Customers] WITH NOCHECK ADD 
	CONSTRAINT [DF_Customers_StatusID] DEFAULT ((1)) FOR [StatusID],
	CONSTRAINT [pk_Customers] PRIMARY KEY  CLUSTERED 
	(
		[CustomerID]
	)  ON [PRIMARY] 
GO


/****** Object:  Table [dbo].[Inventory]    Script Date: 8/31/2007 10:32:11 AM ******/
ALTER TABLE [Inventory] WITH NOCHECK ADD 
	CONSTRAINT [DF_Inventory_InventoryID] DEFAULT (newid()) FOR [InventoryID],
	CONSTRAINT [pk_Inventory] PRIMARY KEY  CLUSTERED 
	(
		[InventoryID]
	)  ON [PRIMARY] 
GO


/****** Object:  Table [dbo].[OrderItems]    Script Date: 8/31/2007 10:32:11 AM ******/
ALTER TABLE [OrderItems] WITH NOCHECK ADD 
	CONSTRAINT [pk_OrderItems] PRIMARY KEY  CLUSTERED 
	(
		[OrderItemID]
	)  ON [PRIMARY] 
GO


/****** Object:  Table [dbo].[Orders]    Script Date: 8/31/2007 10:32:11 AM ******/
ALTER TABLE [Orders] WITH NOCHECK ADD 
	CONSTRAINT [pk_Orders] PRIMARY KEY  CLUSTERED 
	(
		[OrderID]
	)  ON [PRIMARY] 
GO


/****** Object:  Table [dbo].[Products]    Script Date: 8/31/2007 10:32:11 AM ******/
ALTER TABLE [Products] WITH NOCHECK ADD 
	CONSTRAINT [DF_Products_ProductID] DEFAULT (newid()) FOR [ProductID],
	CONSTRAINT [pk_Products] PRIMARY KEY  CLUSTERED 
	(
		[ProductID]
	)  ON [PRIMARY] 
GO


/****** Object:  Table [dbo].[Status]    Script Date: 8/31/2007 10:32:11 AM ******/
ALTER TABLE [Status] WITH NOCHECK ADD 
	CONSTRAINT [pk_Status] PRIMARY KEY  CLUSTERED 
	(
		[StatusID]
	)  ON [PRIMARY] 
GO



/****** Object:  Table [dbo].[Customers]    Script Date: 8/31/2007 10:32:11 AM ******/
ALTER TABLE [Customers] ADD 
	CONSTRAINT [fk_Customers_DeliveryAddress] FOREIGN KEY 
	(
		[DeliveryAddressID]
	) REFERENCES [Addresses] (
		[AddressID]
	),
	CONSTRAINT [fk_Customers_MailingAddress] FOREIGN KEY 
	(
		[MailingAddressID]
	) REFERENCES [Addresses] (
		[AddressID]
	),
	CONSTRAINT [fk_Customers_Status] FOREIGN KEY 
	(
		[StatusID]
	) REFERENCES [Status] (
		[StatusID]
	)
GO


/****** Object:  Table [dbo].[Inventory]    Script Date: 8/31/2007 10:32:11 AM ******/
ALTER TABLE [Inventory] ADD 
	CONSTRAINT [fk_Inventory_Products] FOREIGN KEY 
	(
		[ProductID]
	) REFERENCES [Products] (
		[ProductID]
	)
GO


/****** Object:  Table [dbo].[OrderItems]    Script Date: 8/31/2007 10:32:11 AM ******/
ALTER TABLE [OrderItems] ADD 
	CONSTRAINT [fk_OrderItems_Order] FOREIGN KEY 
	(
		[OrderID]
	) REFERENCES [Orders] (
		[OrderID]
	),
	CONSTRAINT [fk_OrderItems_Product] FOREIGN KEY 
	(
		[ProductID]
	) REFERENCES [Products] (
		[ProductID]
	)
GO


/****** Object:  Table [dbo].[Orders]    Script Date: 8/31/2007 10:32:11 AM ******/
ALTER TABLE [Orders] ADD 
	CONSTRAINT [fk_Orders_Customer] FOREIGN KEY 
	(
		[CustomerID]
	) REFERENCES [Customers] (
		[CustomerID]
	)
GO


















/*-----END SCRIPT------*/