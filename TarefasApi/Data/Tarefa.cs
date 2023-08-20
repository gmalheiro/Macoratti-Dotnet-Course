using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TarefasApi.Data;

  [Table("Tarefas")]
  public record Tarefa (int Id,string Atividade,string Status);
