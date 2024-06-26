﻿using Dapper;
using DapperTrabalhoFinal.Config;
using DapperTrabalhoFinal.Models;
using Microsoft.AspNetCore.Mvc;

namespace DapperTrabalhoFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController
    {
        [HttpGet]
        public IEnumerable<Rating> ListRatings()
        {
            Connection c = new();
            using var connection = c.RealizarConexao();

            return connection.Query<Rating>("SELECT * FROM ratings").ToList();
        }

        [HttpPost]
        public string RegisterRatings([FromBody] Rating rating)
        {
            Connection c = new();
            using var connection = c.RealizarConexao();

            connection.Execute(@"INSERT INTO ratings (rating_text, id_user, id_course) VALUES (?Rating_text, ?Id_user, ?Id_course)", rating);

            return "Cadastro efetuado com sucesso!";
        }

        [HttpPut]
        public string UpdateRatings([FromBody] Rating rating)
        {
            Connection c = new();
            using var conncetion = c.RealizarConexao();

            conncetion.Execute(@"
UPDATE ratings 
   SET rating_text = ?Rating_text,
       id_user = ?Id_user,
       id_course = ?Id_course 
 WHERE id_rating = ?Id_rating", rating);

            return "Avaliação alterada com sucesso!";
        }

        [HttpDelete("{id_rating}")]
        public string DeleteRatings(int id_rating)
        {
            Connection c = new();
            using var connection = c.RealizarConexao();

            int count = Contabilizar(id_rating);

            if (count > 0)
            {
                connection.Execute($@"
DELETE FROM ratings 
      WHERE id_rating = {id_rating}");

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

            return connection.ExecuteScalar<int>($@"SELECT COUNT(*) FROM ratings WHERE id_rating = {id}");
        }

    }
}
