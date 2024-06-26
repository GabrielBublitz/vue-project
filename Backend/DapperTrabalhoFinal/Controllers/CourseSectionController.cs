﻿using Dapper;
using DapperTrabalhoFinal.Config;
using DapperTrabalhoFinal.Models;
using Microsoft.AspNetCore.Mvc;

namespace DapperTrabalhoFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseSectionController
    {
        [HttpGet]
        public IEnumerable<CourseSection> ListCourseSections()
        {
            Connection c = new();
            using var connection = c.RealizarConexao();

            return connection.Query<CourseSection>("SELECT * FROM course_sections").ToList();
        }

        [HttpPost]
        public string RegisterCourseSections([FromBody] CourseSection cs)
        {
            Connection c = new();
            using var connection = c.RealizarConexao();

            connection.Execute(@"INSERT INTO course_sections (course_sect_name, id_course) VALUES (?Course_sect_name, ?Id_course)", cs);

            return "Cadastro efetuado com sucesso!";
        }

        [HttpPut]
        public string UpdateCourseSections([FromBody] CourseSection cs)
        {
            Connection c = new();
            using var conncetion = c.RealizarConexao();

            conncetion.Execute(@"UPDATE course_sections SET course_sect_name = ?Course_sect_name, id_course = ?Id_course WHERE id_course_sect = ?Id_course_sect", cs);

            return "Seção alterada com sucesso!";
        }

        [HttpDelete("{id_course_sect}")]
        public string DeleteCourseSections(int id_course_sect)
        {
            Connection c = new();
            using var connection = c.RealizarConexao();

            int count = Contabilizar(id_course_sect);

            if (count > 0)
            {
                connection.Execute($@"DELETE FROM course_sections WHERE id_course_sect = {id_course_sect}");
                return "Seção removida com sucesso!";
            }
            else
            {
                return $"Falha na remoção da seção {count}";
            }
        }

        private static int Contabilizar(int id)
        {
            Connection c = new();

            using var connection = c.RealizarConexao();

            return connection.ExecuteScalar<int>($@"SELECT COUNT(*) FROM course_sections WHERE id_course_sect = {id}");
        }
    }
}
