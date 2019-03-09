/*

CreateDBAspNet.sql

20190217, Honda, Create for Asp.net identity usage. Based on https://github.com/github-honda/VS2015TemplateOriginal/blob/master/WebAuth1/doc/CreateIdentitySQLServer.sql
Test run on SQL Server 2012:
  Microsoft SQL Server Management Studio						11.0.6251.0
  Microsoft .NET Framework						4.0.30319.42000
  Operating System						6.3.17763

*/

use [master];
GO


CREATE DATABASE [DBAspNet1]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'DBAspNet1', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\DBAspNet1.mdf' , SIZE = 5120KB , FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'DBAspNet1_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\DBAspNet1_log.ldf' , SIZE = 2048KB , FILEGROWTH = 10%)
GO
ALTER DATABASE [DBAspNet1] SET COMPATIBILITY_LEVEL = 110
GO
ALTER DATABASE [DBAspNet1] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [DBAspNet1] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [DBAspNet1] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [DBAspNet1] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [DBAspNet1] SET ARITHABORT OFF 
GO
ALTER DATABASE [DBAspNet1] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [DBAspNet1] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [DBAspNet1] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [DBAspNet1] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [DBAspNet1] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [DBAspNet1] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [DBAspNet1] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [DBAspNet1] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [DBAspNet1] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [DBAspNet1] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [DBAspNet1] SET  DISABLE_BROKER 
GO
ALTER DATABASE [DBAspNet1] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [DBAspNet1] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [DBAspNet1] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [DBAspNet1] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [DBAspNet1] SET  READ_WRITE 
GO
ALTER DATABASE [DBAspNet1] SET RECOVERY FULL 
GO
ALTER DATABASE [DBAspNet1] SET  MULTI_USER 
GO
ALTER DATABASE [DBAspNet1] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [DBAspNet1] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO

use [DBAspNet1];
GO


/* __MigrationHistory 
CREATE TABLE [dbo].[__MigrationHistory] (
    [MigrationId]    NVARCHAR (150)  NOT NULL,
    [ContextKey]     NVARCHAR (300)  NOT NULL,
    [Model]          VARBINARY (MAX) NOT NULL,
    [ProductVersion] NVARCHAR (32)   NOT NULL,
    CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED ([MigrationId] ASC, [ContextKey] ASC)
);
GO
*/

/* AspNetUsers */
CREATE TABLE [dbo].[AspNetUsers] (
    [Id]                   NVARCHAR (128) NOT NULL,
    [Email]                NVARCHAR (256) NULL,
    [EmailConfirmed]       BIT            NOT NULL,
    [PasswordHash]         NVARCHAR (MAX) NULL,
    [SecurityStamp]        NVARCHAR (MAX) NULL,
    [PhoneNumber]          NVARCHAR (MAX) NULL,
    [PhoneNumberConfirmed] BIT            NOT NULL,
    [TwoFactorEnabled]     BIT            NOT NULL,
    [LockoutEndDateUtc]    DATETIME       NULL,
    [LockoutEnabled]       BIT            NOT NULL,
    [AccessFailedCount]    INT            NOT NULL,
    [UserName]             NVARCHAR (256) NOT NULL,
    CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex]
    ON [dbo].[AspNetUsers]([UserName] ASC);
GO


/* AspNetRoles */
CREATE TABLE [dbo].[AspNetRoles] (
    [Id]   NVARCHAR (128) NOT NULL,
    [Name] NVARCHAR (256) NOT NULL,
    CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex]
    ON [dbo].[AspNetRoles]([Name] ASC);
GO


/* AspNetUserClaims */
CREATE TABLE [dbo].[AspNetUserClaims] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [UserId]     NVARCHAR (128) NOT NULL,
    [ClaimType]  NVARCHAR (MAX) NULL,
    [ClaimValue] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[AspNetUserClaims]([UserId] ASC);


/* USE AspNetUserLogins */
CREATE TABLE [dbo].[AspNetUserLogins] (
    [LoginProvider] NVARCHAR (128) NOT NULL,
    [ProviderKey]   NVARCHAR (128) NOT NULL,
    [UserId]        NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED ([LoginProvider] ASC, [ProviderKey] ASC, [UserId] ASC),
    CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[AspNetUserLogins]([UserId] ASC);
GO

	
	
/* AspNetUserRoles */
CREATE TABLE [dbo].[AspNetUserRoles] (
    [UserId] NVARCHAR (128) NOT NULL,
    [RoleId] NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC),
    CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[AspNetUserRoles]([UserId] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_RoleId]
    ON [dbo].[AspNetUserRoles]([RoleId] ASC);
GO

	



/* Test data
INSERT INTO [dbo].[__MigrationHistory] ([MigrationId], [ContextKey], [Model], [ProductVersion]) VALUES (N'201902150317221_InitialCreate', N'WebAuth1.Models.ApplicationDbContext', <Binary Data>, N'6.1.3-40302')
GO
*/

INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'b709c15b-f805-44af-8237-1be89b4222a3', N'test1@some.com', 0, N'AJXCcIMNdksqvJMyLPleatRpak91jG4CW7sWYp46n7nUmAYIq053HblHTsuKWf+dgQ==', N'b190cd2c-54f9-49f1-9e05-5dcd6d3e5f0a', NULL, 0, 0, NULL, 1, 0, N'test1@some.com')
GO

