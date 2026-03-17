CREATE TABLE Commits (
    Id INT IDENTITY PRIMARY KEY,
    RepositoryId INT NOT NULL,
    Message NVARCHAR(500),
    CreatedBy INT NOT NULL,
    CreatedAt DATETIME NOT NULL,

    FOREIGN KEY (RepositoryId) REFERENCES Repositories(Id),
    FOREIGN KEY (CreatedBy) REFERENCES Users(Id)
);

CREATE TABLE Files (
    Id INT IDENTITY PRIMARY KEY,
    FileName NVARCHAR(200),
    Content NVARCHAR(MAX)
);


CREATE TABLE RepositoryFiles (
    Id INT IDENTITY PRIMARY KEY,
    RepositoryId INT NOT NULL,
    FileId INT NOT NULL,
    CommitId INT NOT NULL,

    FOREIGN KEY (RepositoryId) REFERENCES Repositories(Id),
    FOREIGN KEY (FileId) REFERENCES Files(Id),
    FOREIGN KEY (CommitId) REFERENCES Commits(Id)
);
