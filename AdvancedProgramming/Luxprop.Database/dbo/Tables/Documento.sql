CREATE TABLE [dbo].[Documento] (
    [Documento_ID]   INT IDENTITY (1, 1) NOT NULL,
    [Nombre]         NVARCHAR (150) NOT NULL,
    [Tipo_Documento] NVARCHAR (100) NULL,
    [Estado]         NVARCHAR (50)  NULL,
    [Fecha_Carga]    DATETIME DEFAULT GETDATE(),
    [UrlArchivo]     NVARCHAR(500)  NULL,
    [Expediente_ID]  INT NULL,
    CONSTRAINT [PK_Documento] PRIMARY KEY CLUSTERED ([Documento_ID] ASC),
    CONSTRAINT [FK_Documento_Expediente]
        FOREIGN KEY ([Expediente_ID]) REFERENCES [dbo].[Expediente] ([Expediente_ID])
);


