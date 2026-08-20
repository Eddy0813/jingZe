
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 10/24/2024 10:57:58
-- Generated from EDMX file: D:\Users\22154\Source\Repos\jingZe\JingZeServer\JZZS.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [JZZS];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[GZID]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GZID];
GO
IF OBJECT_ID(N'[dbo].[material]', 'U') IS NOT NULL
    DROP TABLE [dbo].[material];
GO
IF OBJECT_ID(N'[dbo].[MaterialLoss]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MaterialLoss];
GO
IF OBJECT_ID(N'[dbo].[ProWeigth]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProWeigth];
GO
IF OBJECT_ID(N'[dbo].[RERFID]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RERFID];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'RERFID'
CREATE TABLE [dbo].[RERFID] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [RFIDID] varchar(255)  NULL,
    [RFIDName] varchar(255)  NULL,
    [DateTime] datetime  NULL,
    [Reserver1] nchar(10)  NULL,
    [Reserver2] nchar(10)  NULL,
    [Reserver3] nchar(10)  NULL
);
GO

-- Creating table 'material'
CREATE TABLE [dbo].[material] (
    [id] int  NOT NULL,
    [number] varchar(255)  NULL,
    [name] varchar(255)  NULL,
    [specification] varchar(255)  NULL,
    [resinWeight] float  NULL,
    [fgWeight] float  NULL,
    [qbq] float  NULL,
    [qbh] float  NULL,
    [gcRange] float  NULL
);
GO

-- Creating table 'GZID'
CREATE TABLE [dbo].[GZID] (
    [ID] int  NOT NULL,
    [RFIDID] varchar(255)  NULL,
    [MateCode] varchar(255)  NULL,
    [DateTime] datetime  NULL
);
GO

-- Creating table 'MaterialLoss'
CREATE TABLE [dbo].[MaterialLoss] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [RFIDID] varchar(255)  NULL,
    [Line] varchar(255)  NULL,
    [Status] varchar(255)  NULL,
    [GFweight] float  NULL,
    [Resinweight] float  NULL,
    [DateTime] datetime  NULL,
    [OrderNo] nvarchar(1)  NULL,
    [Reserver1] varchar(255)  NULL,
    [Reserver2] varchar(255)  NULL,
    [Reserver3] varchar(255)  NULL,
    [CS] int  NULL
);
GO

-- Creating table 'ProWeigth'
CREATE TABLE [dbo].[ProWeigth] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [RFIDID] varchar(255)  NULL,
    [OrderNo] varchar(255)  NULL,
    [OrderNum] int  NULL,
    [FinishNum] int  NULL,
    [OrderProNo] varchar(255)  NULL,
    [ProNo] varchar(255)  NULL,
    [ProName] varchar(255)  NULL,
    [spec] varchar(255)  NULL,
    [Custoner] varchar(255)  NULL,
    [MPWeigth] float  NULL,
    [PGWeigth] float  NULL,
    [MGWeigth] float  NULL,
    [JGWeigth] float  NULL,
    [DateTime] datetime  NULL,
    [ProID] varchar(255)  NULL,
    [Reserver1] varchar(255)  NULL,
    [Reserver2] varchar(255)  NULL,
    [Reserver3] varchar(255)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID] in table 'RERFID'
ALTER TABLE [dbo].[RERFID]
ADD CONSTRAINT [PK_RERFID]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [id] in table 'material'
ALTER TABLE [dbo].[material]
ADD CONSTRAINT [PK_material]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [ID] in table 'GZID'
ALTER TABLE [dbo].[GZID]
ADD CONSTRAINT [PK_GZID]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'MaterialLoss'
ALTER TABLE [dbo].[MaterialLoss]
ADD CONSTRAINT [PK_MaterialLoss]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ProWeigth'
ALTER TABLE [dbo].[ProWeigth]
ADD CONSTRAINT [PK_ProWeigth]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------