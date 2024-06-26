﻿using Dapper;
using DapperTrabalhoFinal.Config;
using DapperTrabalhoFinal.Models;
using Microsoft.AspNetCore.Mvc;

namespace DapperTrabalhoFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishController
    {
        [HttpGet]
        public IEnumerable<Wish> ListWishes()
        {
            Connection c = new();
            using var connection = c.RealizarConexao();

            return connection.Query<Wish>("SELECT * FROM wishes");
        }

        [HttpPost]
        public string RegisterWishes([FromBody] Wish wish)
        {
            Connection c = new();
            using var connection = c.RealizarConexao();

            connection.Execute(@"
INSERT INTO wishes 
            (id_user, id_course) 
     VALUES (?Id_user, ?Id_course)", wish);

            return "Cadastro efetuado com sucesso!";
        }

        [HttpPut]
        public string UpdateWishes([FromBody] Wish wish)
        {
            Connection c = new();
            using var conncetion = c.RealizarConexao();

            conncetion.Execute(@"
UPDATE wishes 
   SET id_user = ?Id_user, 
       id_course = ?Id_course 
 WHERE id_wish = ?Id_wish", wish);

            return "Desejo alterada com sucesso!";
        }

        [HttpDelete("{id_wish}")]
        public string DeleteWishes(int id_wish)
        {
            Connection c = new();
            using var connection = c.RealizarConexao();

            int count = Contabilizar(id_wish);

            if (count > 0)
            {
                connection.Execute($@"DELETE FROM wishes WHERE id_wish = {id_wish}");
                return "Removido com sucesso!";
            }
            else
            {
                return $"Falha na remoção {count}";
            }
        }

        private static int Contabilizar(int id)
        {
            Connection c = new();
            using var connection = c.RealizarConexao();

            return connection.ExecuteScalar<int>($@"SELECT COUNT(*) FROM wishes WHERE id_wish = {id}");
        }
    }
}
