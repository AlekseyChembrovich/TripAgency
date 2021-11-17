CREATE TABLE [dbo].[Trip] (
    [Id] INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    [Name] NVARCHAR(100) NULL,
    [Price] MONEY NULL,
    [DepartureDate] DATE NULL,
    [CountNights] INT NULL,
    [CountKids] INT NULL,
    [Country] NVARCHAR(50) NULL,
    [TypeTrip] NVARCHAR(50) NULL,
    [Nutrition] NVARCHAR(50) NULL
);
