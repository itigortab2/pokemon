
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 03/30/2017 18:01:03
-- Generated from EDMX file: C:\Users\AE\Documents\Visual Studio 2015\Projects\Pokemon\Pokemon\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [PokemonDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_PokType_Pokemon]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PokType] DROP CONSTRAINT [FK_PokType_Pokemon];
GO
IF OBJECT_ID(N'[dbo].[FK_PokType_Type]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PokType] DROP CONSTRAINT [FK_PokType_Type];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Pokemon]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Pokemon];
GO
IF OBJECT_ID(N'[dbo].[Type]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Type];
GO
IF OBJECT_ID(N'[dbo].[PokType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PokType];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Pokemon'
CREATE TABLE [dbo].[Pokemon] (
    [IdPokemon] int IDENTITY(1,1) NOT NULL,
    [title] nchar(255)  NOT NULL
);
GO

-- Creating table 'Type'
CREATE TABLE [dbo].[Type] (
    [idType] int IDENTITY(1,1) NOT NULL,
    [type1] nchar(255)  NOT NULL
);
GO

-- Creating table 'PokType'
CREATE TABLE [dbo].[PokType] (
    [Pokemon_IdPokemon] int  NOT NULL,
    [PType_idType] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [IdPokemon] in table 'Pokemon'
ALTER TABLE [dbo].[Pokemon]
ADD CONSTRAINT [PK_Pokemon]
    PRIMARY KEY CLUSTERED ([IdPokemon] ASC);
GO

-- Creating primary key on [idType] in table 'Type'
ALTER TABLE [dbo].[Type]
ADD CONSTRAINT [PK_Type]
    PRIMARY KEY CLUSTERED ([idType] ASC);
GO

-- Creating primary key on [Pokemon_IdPokemon], [PType_idType] in table 'PokType'
ALTER TABLE [dbo].[PokType]
ADD CONSTRAINT [PK_PokType]
    PRIMARY KEY CLUSTERED ([Pokemon_IdPokemon], [PType_idType] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Pokemon_IdPokemon] in table 'PokType'
ALTER TABLE [dbo].[PokType]
ADD CONSTRAINT [FK_PokType_Pokemon]
    FOREIGN KEY ([Pokemon_IdPokemon])
    REFERENCES [dbo].[Pokemon]
        ([IdPokemon])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [PType_idType] in table 'PokType'
ALTER TABLE [dbo].[PokType]
ADD CONSTRAINT [FK_PokType_Type]
    FOREIGN KEY ([PType_idType])
    REFERENCES [dbo].[Type]
        ([idType])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PokType_Type'
CREATE INDEX [IX_FK_PokType_Type]
ON [dbo].[PokType]
    ([PType_idType]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------