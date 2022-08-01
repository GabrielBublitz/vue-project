﻿using Dapper;
using DapperTrabalhoFinal.Config;
using DapperTrabalhoFinal.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace DapperTrabalhoFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CategoriesController
    {

        [HttpGet]

        public IEnumerable<Categories> ListCategories()
        {
            Conexao c = new Conexao();

            using var connection = c.RealizarConexao();

            return connection.Query<Categories>("SELECT * FROM categories").ToList();
        }

        [HttpGet("{id_categorie}")]

        public IEnumerable<Courses> ListCourses(int id_categorie)
        {
            Conexao c = new();
            using var connection = c.RealizarConexao();

            DynamicParameters Parametro = new DynamicParameters();
            Parametro.Add(":id_categorie", id_categorie);

            var builder = new SqlBuilder();
            builder.Where(":id_categorie = id_categorie", Parametro);

            var builderTemplate = builder.AddTemplate("SELECT * FROM courses /**where**/");

            var courses = connection.Query<Courses>(builderTemplate.RawSql, builderTemplate.Parameters).ToList();

            return courses;
        }

        [HttpPost]

        public string RegisterCategories([FromBody] Categories cg)
        {
            Conexao c = new();

            using var connection = c.RealizarConexao();

            connection.Execute(@"INSERT INTO categories (categorie_name) VALUES (:Categorie_name)", cg);

            return "Cadastro efetuado com sucesso!";
        }

        [HttpPut]

        public string UpdateCategories([FromBody] Categories cg)
        {
            Conexao c = new();

            using var conncetion = c.RealizarConexao();

            conncetion.Execute(@"UPDATE categories SET categorie_name = :Categorie_name WHERE id_categorie = :Id_categorie", cg);

            return "Categoria alterada com sucesso!";
        }

        [HttpDelete("{id_categorie}")]

        public string DeleteCategories(int id_categorie)
        {
            Conexao c = new();

            using var connection = c.RealizarConexao();

            int count = contabilizar(id_categorie);

            if (count > 0)
            {
                connection.Execute(@"DELETE FROM categories WHERE id_categorie = " + id_categorie);
                return "Categoria removida com sucesso!";
            }
            else
            {
                return $"Falha na remoção da categoria {count}";
            }
        }

        private int contabilizar(int id)
        {
            Conexao c = new Conexao();


            using var connection = c.RealizarConexao();

            return connection.ExecuteScalar<int>(@"SELECT COUNT(*) FROM categories WHERE id_categorie = " + id);
        }


        [HttpGet("{codigo}")]
        public Mensagem Teste(int codigo)
        {

            // Instanciar objeto da classe Mensagem
            Mensagem m = new Mensagem();

            // Instanciar objeto da classe Conexão
            Conexao c = new();

            // Realizar conexão com o banco de dados Oracle - DAPPER
            using var connection = c.RealizarConexao();

            // Objeto dinâmico para executar a procedure
            var obj = new DynamicParameters();
            obj.Add(":id_categ", codigo, direction: ParameterDirection.Input);
            obj.Add(":returns", "", direction: ParameterDirection.Output);

            // Executar a inserção
            connection.Query<Mensagem>("validate_removal", obj, commandType: CommandType.StoredProcedure).ToString();

            // Retornar a mensagem e armazenar em um objeto do tipo Mensagem
            m.MensagemRetorno = obj.Get<string>(":returns");

            // Retorno da API
            return m;
        }
    }
}
