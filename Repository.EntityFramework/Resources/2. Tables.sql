USE [Bookstore]
GO
/****** Object:  Table [dbo].[Log]    Script Date: 01/02/2021 22:51:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Log](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Message] [nvarchar](max) NULL,
	[MessageTemplate] [nvarchar](max) NULL,
	[Level] [nvarchar](128) NULL,
	[TimeStamp] [datetimeoffset](7) NOT NULL,
	[Exception] [nvarchar](max) NULL,
	[Properties] [xml] NULL,
	[LogEvent] [nvarchar](max) NULL,
 CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductData]    Script Date: 01/02/2021 22:51:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductData](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[imagePath] [varchar](100) NULL,
	[title] [varchar](50) NULL,
	[description] [varchar](500) NULL,
	[price] [varchar](30) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SubscriptionData]    Script Date: 01/02/2021 22:51:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SubscriptionData](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[prodid] [int] NULL,
	[usrid] [int] NULL,
	[price] [varchar](20) NULL,
	[state] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserData]    Script Date: 01/02/2021 22:51:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserData](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NULL,
	[lastname] [varchar](50) NULL,
	[email] [varchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[SP_NewSubscription]    Script Date: 01/02/2021 22:51:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[SP_NewSubscription]
	@prodid int,
	@email varchar(50),
	@name varchar(50),
	@lastname varchar(50),
	@newid int output
as

DECLARE @id int, @userid int, @price varchar(20)

SELECT @userid = [id] FROM [dbo].[UserData] WHERE [email]=@email
SELECT @price = [id] FROM [dbo].[ProductData] WHERE [id]=@prodid

IF @userid IS NOT NULL AND @price IS NOT NULL
	BEGIN
		UPDATE [dbo].[SubscriptionData] SET [state]=1
		WHERE [usrid] = @userid AND [prodid]=@prodid
	END
ELSE
	BEGIN

		IF @userid IS NULL
			BEGIN
				INSERT INTO [dbo].[UserData] ([name], [lastname], [email])
				VALUES (@name, @lastname, @email)
				SET @newid = IDENT_CURRENT('SubscriptionData') 
			END
		ELSE
			BEGIN
				SET @newid = @userid
			END

		IF @price IS NOT NULL
			BEGIN
				SELECT @id = [id] FROM [dbo].[SubscriptionData]
				WHERE [prodid]=@prodid and [usrid]=@newid
		
				IF @id IS NULL
					BEGIN -- insert
						INSERT INTO [dbo].[SubscriptionData] ([prodid],[usrid],[price],[state])
						VALUES (
							@prodid, 
							(SELECT [id] FROM [dbo].[UserData] WHERE [email]=@email),
							(SELECT [price] FROM [dbo].[ProductData] WHERE id=@prodid),
							1)
							SET @newid = IDENT_CURRENT('SubscriptionData') 
					END
				END

	END
GO
