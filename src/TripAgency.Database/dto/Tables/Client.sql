﻿CREATE TABLE [dbo].[Client] (
    [Id] INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    [Surname] NVARCHAR(50) NULL,
    [Name] NVARCHAR(50) NULL,
    [Patronymic] NVARCHAR(50) NULL,
    [Passport] NVARCHAR(25) NULL,
    [Phone] NVARCHAR(25) NULL,
);
