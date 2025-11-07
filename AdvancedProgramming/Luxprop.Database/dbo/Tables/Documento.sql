CREATE TABLE [dbo].[Documento] (
    [Documento_ID]   INT            IDENTITY (1, 1) NOT NULL,
    [Nombre]         NVARCHAR (150) NULL,
    [Tipo_Documento] NVARCHAR (100) NULL,
    [Estado]         NVARCHAR (50)  NULL,
    [Fecha_Carga]    DATE           NULL,
    [Expediente_ID]  INT            NULL,
    [UrlArchivo]     NVARCHAR (500) NULL,
    [Etiquetas]      NVARCHAR (255) NULL,
    PRIMARY KEY CLUSTERED ([Documento_ID] ASC),
    FOREIGN KEY ([Expediente_ID]) REFERENCES [dbo].[Expediente] ([Expediente_ID])
);

