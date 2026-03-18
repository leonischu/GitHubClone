SELECT TOP (1000) [Id]
      ,[RepositoryId]
      ,[Name]
      ,[CreatedBy]
      ,[CreatedAt]

  FROM [GithubClone].[dbo].[Branches]


ALTER TABLE Branches
DROP COLUMN LatestCommitId;

ALTER TABLE Branches
DROP CONSTRAINT DF__Branches__IsActi__5EBF139D;