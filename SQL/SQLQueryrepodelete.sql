SELECT TOP (1000) [Id]
      ,[RepositoryId]
      ,[Name]
      ,[CreatedBy]
      ,[CreatedAt]
  FROM [GithubClone].[dbo].[Branches]

  ALTER TABLE Branches
DROP CONSTRAINT FK__Branches__Reposi__5BE2A6F2;

ALTER TABLE Branches
ADD CONSTRAINT FK_Branches_Repositories
FOREIGN KEY (RepositoryId)
REFERENCES Repositories(Id)
ON DELETE CASCADE;