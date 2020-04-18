CREATE DATABASE IssueTrackerWeb
GO

USE [IssueTrackerWeb]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE dbo.Severity(
	SeverityId int IDENTITY (1, 1) PRIMARY KEY,
	SeverityName nvarchar(255) NOT NULL
);

CREATE TABLE dbo.Status(
	StatusId int IDENTITY (1, 1) PRIMARY KEY,
	StatusName nvarchar(255) NOT NULL
);

CREATE TABLE dbo.Project(
	ProjectId int IDENTITY (1, 1) PRIMARY KEY,
	Title nvarchar(255) NOT NULL,
	Description nvarchar(1000) NOT NULL,
	CreationDate datetime NOT NULL
);

CREATE TABLE dbo.Person(
	PersonId int IDENTITY (1, 1) PRIMARY KEY,
	FirstName nvarchar(255) NOT NULL,
	LastName nvarchar(255) NOT NULL,
	Email nvarchar(255) NOT NULL,
);

CREATE TABLE dbo.Issue(
	IssueId int IDENTITY (1, 1) PRIMARY KEY,
	Title nvarchar(255) NOT NULL,
	Description nvarchar(1000) NOT NULL,
	CreationDate datetime NOT NULL,
);

CREATE TABLE dbo.Assignee(
	IssueId int NOT NULL,
	PersonId int NOT NULL,
FOREIGN KEY (IssueId) REFERENCES Issue(IssueId),
FOREIGN KEY (PersonId) REFERENCES Person(PersonId)
);

CREATE TABLE dbo.ProjectMember(
	ProjectId int NOT NULL,
	PersonId int NOT NULL,
FOREIGN KEY (ProjectId) REFERENCES Project(ProjectId),
FOREIGN KEY (PersonId) REFERENCES Person(PersonId)
);

CREATE TABLE dbo.Comment(
	CommentId int IDENTITY (1, 1) PRIMARY KEY,
	Content nvarchar(255),
	CreationDate datetime,
	IssueId int NOT NULL,
	PersonId int NOT NULL,
FOREIGN KEY (IssueId) REFERENCES Issue(IssueId),
FOREIGN KEY (PersonId) REFERENCES Person(PersonId)
);

