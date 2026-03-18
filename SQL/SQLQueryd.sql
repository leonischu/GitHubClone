CREATE TABLE Branches (
    Id INT IDENTITY PRIMARY KEY,
    RepositoryId INT NOT NULL,
    Name NVARCHAR(100) NOT NULL,
    CreatedBy INT NOT NULL,
    CreatedAt DATETIME NOT NULL,

    FOREIGN KEY (RepositoryId) REFERENCES Repositories(Id),
    FOREIGN KEY (CreatedBy) REFERENCES Users(Id)
);

ALTER TABLE Commits
ADD BranchId INT;

ALTER TABLE Commits
ADD FOREIGN KEY (BranchId) REFERENCES Branches(Id);