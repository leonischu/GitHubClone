SELECT TOP (1000) [Id]
      ,[Username]
      ,[Email]
      ,[PasswordHash]
      ,[CreatedAt]
  FROM [GithubClone].[dbo].[Users]


  ALTER TABLE Users
    ADD IsEmailVerified BIT NOT NULL DEFAULT 0,
    EmailVerificationToken NVARCHAR(255) NULL,
    EmailVerificationTokenExpiry DATETIME NULL;