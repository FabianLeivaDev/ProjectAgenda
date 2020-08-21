CREATE TABLE [dbo].Agenda
(
	[ContactoID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Nombre] VARCHAR(50) NOT NULL, 
    [Telefono] VARCHAR(50) NOT NULL, 
    [Correo] VARCHAR(100) NOT NULL
)
