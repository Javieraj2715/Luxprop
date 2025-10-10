CREATE TABLE [dbo].[Expediente] (
    [Expediente_ID]  INT           IDENTITY (1, 1) NOT NULL,
    [Tipo_Ocupacion] NVARCHAR (50) NULL,
    [Estado]         NVARCHAR (50) NULL,
    [Propiedad_ID]   INT           NULL,
    [Cliente_ID]     INT           NULL,
    [Fecha_Apertura] DATE          NULL,
    [Fecha_Cierre]   DATE          NULL,
    PRIMARY KEY CLUSTERED ([Expediente_ID] ASC),
    FOREIGN KEY ([Cliente_ID]) REFERENCES [dbo].[Cliente] ([Cliente_ID]),
    FOREIGN KEY ([Propiedad_ID]) REFERENCES [dbo].[Propiedad] ([Propiedad_ID])
);

