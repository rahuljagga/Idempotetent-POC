/****** Object:  Table [dbo].[Transactions]    Script Date: 3/11/2021 6:55:17 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Transactions](
	[TransactionID] [bigint] IDENTITY(1,1) NOT NULL,
	[Amount] [money] NOT NULL,
	[State] [nvarchar](50) NOT NULL,
	[Created_Date] [datetime] NOT NULL,
	[CustomerID] [bigint] NOT NULL,
	[TransactionRef] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Transactions] PRIMARY KEY CLUSTERED 
(
	[TransactionID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Transactions]  WITH CHECK ADD  CONSTRAINT [FK_Transactions_Customer_Detail] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customer_Detail] ([CustomerID])
GO

ALTER TABLE [dbo].[Transactions] CHECK CONSTRAINT [FK_Transactions_Customer_Detail]
GO


