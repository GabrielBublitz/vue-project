﻿using Dapper;
using DapperTrabalhoFinal.Config;
using DapperTrabalhoFinal.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace DapperTrabalhoFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController
    {
        [HttpGet]
        public IEnumerable<Course> ListCourses()
        {
            Connection c = new();

            using var connection = c.RealizarConexao();

            return connection.Query<Course>("SELECT * FROM courses").ToList();
        }

        [HttpGet("{id_course}")]
        public IEnumerable<Course> ListCoursesById(int id_course)
        {
            Connection c = new();
            using var connection = c.RealizarConexao();

            DynamicParameters Parametro = new();
            Parametro.Add("?id_course", id_course);

            SqlBuilder builder = new();
            builder.Where("?id_course = id_course", Parametro);

            var builderTemplate = builder.AddTemplate("SELECT * FROM courses /**where**/");

            var courses = connection.Query<Course>(builderTemplate.RawSql, builderTemplate.Parameters).ToList();

            return courses;
        }

        [HttpGet("validateCourse/{id_course}")]
        public Message ValidateCourse(int id_course)
        {
            Message message = new();

            Connection c = new();

            using var connection = c.RealizarConexao();

            DynamicParameters obj = new();
            obj.Add("?id_cours", id_course, direction: ParameterDirection.Input);
            obj.Add("?returns", "", direction: ParameterDirection.Output);

            connection.Query<Message>("validate_course", obj, commandType: CommandType.StoredProcedure).ToString();

            message.ReturnMessage = obj.Get<string>(":returns");

            return message;
        }

        [HttpPost]
        public string RegisterCourses([FromBody] Course cs)
        {
            Connection c = new();
            using var connection = c.RealizarConexao();

            connection.Execute(@"
INSERT INTO 
courses (course_name, course_subtitle, course_people_amt, course_rating, course_language, course_creation_date, course_description, course_requirements, course_time, course_link, course_audience, course_learnings, course_knowledge_level, course_message, id_author, id_category, id_price_course)  
VALUES (?Course_name, ?Course_subtitle, ?Course_people_amt, ?Course_rating, ?Course_language, ?Course_creation_date, ?Course_description, ?Course_requirements, ?Course_time, ?Course_link, ?Course_audience, ?Course_learnings, ?Course_knowledge_level, ?Course_message, ?Id_author, ?Id_category, ?Id_price_course)", cs);

            return "Cadastro efetuado com sucesso!";
        }
       
        [HttpPut]
        public string UpdateCourses([FromBody] Course cs)
        {
            Connection c = new();
            using var conncetion = c.RealizarConexao();

            conncetion.Execute(@"
UPDATE courses 
SET course_name = :Course_name,
course_subtitle = :Course_subtitle,
course_people_amt = :Course_people_amt, 
course_rating = :Course_rating,
course_language = :Course_language,
course_creation_date = :Course_creation_date,
course_description = :Course_description,
course_requirements = :Course_requirements,
course_time = :Course_time,
course_link = :Course_link,
course_audience = :Course_audience,
course_learnings = :Course_learnings,
course_knowledge_level = ?Course_knowledge_level,
course_message = ?Course_message,
id_category = ?Id_category
WHERE id_course = ?Id_course", cs);

            return "Curso alterado com sucesso!";
        }

        [HttpDelete("{id_course}")]
        public string DeleteCourses(int id_course)
        {
            Connection c = new();
            using var connection = c.RealizarConexao();

            int count = Contabilizar(id_course);

            if (count > 0)
            {
                connection.Execute($@"DELETE FROM courses WHERE id_course = {id_course}");
                return "Curso removido com sucesso!";
            }
            else
            {
                return $"Falha na remoção do curso {count}";
            }
        }

        private static int Contabilizar(int id)
        {
            Connection c = new();
            using var connection = c.RealizarConexao();

            return connection.ExecuteScalar<int>($@"SELECT COUNT(*) FROM courses WHERE id_course = {id}");
        }
    }
}
