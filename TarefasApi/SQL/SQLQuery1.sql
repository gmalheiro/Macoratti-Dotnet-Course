USE TarefasDemoDB;

GO 
CREATE TABLE [dbo].Tarefas(
	[Id] [int] IDENTITY (1,1) NOT NULL,
	[ATIVIDADE] [nvarchar](255),
	[Status] [nvarchar](100)
);

GO
INSERT INTO [dbo].[Tarefas](Atividade,Status)
VALUES ('Tarefa 2', 'Em andamento');

GO
SELECT Atividade,Status FROM [dbo].[Tarefas];