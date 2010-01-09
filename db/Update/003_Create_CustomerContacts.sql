USE [FluentLinqToSql]
GO

/****** Object:  Table [dbo].[CustomerContacts]    Script Date: 01/09/2010 19:26:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CustomerContacts](
	[Id] [int] NOT NULL,
	[PhoneNumber] [nvarchar](50) NULL,
	[Email] [nvarchar](250) NULL,
 CONSTRAINT [PK_CustomerContacts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[CustomerContacts]  WITH CHECK ADD  CONSTRAINT [FK_CustomerContacts_Customer] FOREIGN KEY([Id])
REFERENCES [dbo].[Customers] ([Id])
GO

ALTER TABLE [dbo].[CustomerContacts] CHECK CONSTRAINT [FK_CustomerContacts_Customer]
GO


