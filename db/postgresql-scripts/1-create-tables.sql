CREATE TABLE IF NOT EXISTS FileInfo (
    FileInfoId SERIAL PRIMARY KEY,
    FileName VARCHAR(100),
    FilePath VARCHAR(200)
);
CREATE TABLE IF NOT EXISTS DownloadTokens (
    DownloadTokenId UUID PRIMARY KEY,
    FileInfoId INT,
    IsUsed BOOLEAN,
    CONSTRAINT FK_DownloadTokens_FileInfoId FOREIGN KEY (FileInfoId) REFERENCES FileInfo (FileInfoId)
);
CREATE INDEX IF NOT EXISTS IX_DownloadTokens_FileInfoId ON FileInfo(FileInfoId);