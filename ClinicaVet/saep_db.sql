IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261112182857_CriacaoInicial'
)
BEGIN
    CREATE TABLE [Tutores] (
        [Id] int NOT NULL IDENTITY,
        [Nome] nvarchar(100) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [CpfCriptografado] nvarchar(500) NOT NULL,
        [Telefone] nvarchar(20) NULL,
        CONSTRAINT [PK_Tutores] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261112182857_CriacaoInicial'
)
BEGIN
    CREATE TABLE [Usuarios] (
        [Id] int NOT NULL IDENTITY,
        [Nome] nvarchar(80) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [SenhaHash] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Usuarios] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261112182857_CriacaoInicial'
)
BEGIN
    CREATE TABLE [Pets] (
        [Id] int NOT NULL IDENTITY,
        [Nome] nvarchar(80) NOT NULL,
        [Especie] nvarchar(50) NULL,
        [DataNascimento] date NULL,
        [TutorId] int NOT NULL,
        CONSTRAINT [PK_Pets] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Pets_Tutores_TutorId] FOREIGN KEY ([TutorId]) REFERENCES [Tutores] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261112182857_CriacaoInicial'
)
BEGIN
    CREATE TABLE [Agendamentos] (
        [Id] int NOT NULL IDENTITY,
        [DataHora] datetime2 NOT NULL,
        [Servico] nvarchar(300) NOT NULL,
        [PetId] int NOT NULL,
        CONSTRAINT [PK_Agendamentos] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Agendamentos_Pets_PetId] FOREIGN KEY ([PetId]) REFERENCES [Pets] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261112182857_CriacaoInicial'
)
BEGIN
    CREATE INDEX [IX_Agendamentos_PetId] ON [Agendamentos] ([PetId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261112182857_CriacaoInicial'
)
BEGIN
    CREATE INDEX [IX_Pets_TutorId] ON [Pets] ([TutorId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261112182857_CriacaoInicial'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261112182857_CriacaoInicial', N'10.0.15');
END;

COMMIT;
GO

