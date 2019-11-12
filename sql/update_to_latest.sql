IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20191111194809_InitialCreate')
BEGIN
    CREATE TABLE [People] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(500) NOT NULL,
        [BirthDate] datetime2 NOT NULL,
        CONSTRAINT [PK_People] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20191111194809_InitialCreate')
BEGIN
    CREATE TABLE [Contacts] (
        [Id] uniqueidentifier NOT NULL,
        [PersonId] uniqueidentifier NOT NULL,
        [Value] nvarchar(max) NULL,
        [Type] int NOT NULL,
        CONSTRAINT [PK_Contacts] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Contacts_People_PersonId] FOREIGN KEY ([PersonId]) REFERENCES [People] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20191111194809_InitialCreate')
BEGIN
    CREATE INDEX [IX_Contacts_PersonId] ON [Contacts] ([PersonId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20191111194809_InitialCreate')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20191111194809_InitialCreate', N'2.2.6-servicing-10079');
END;

GO

