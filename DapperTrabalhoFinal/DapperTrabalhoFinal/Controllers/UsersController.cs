﻿using Dapper;
using DapperTrabalhoFinal.Config;
using DapperTrabalhoFinal.Models;
using Microsoft.AspNetCore.Mvc;

namespace DapperTrabalhoFinal.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    public class UsersController
    {

        [HttpGet]

        public IEnumerable<Users> ListUsers()
        {
            Conexao c = new Conexao();

            using var connection = c.RealizarConexao();

            return connection.Query<Users>("SELECT * FROM usuarios");
        }

        [HttpGet("{id_user}")]

        public IEnumerable<Users> ListIdUsers(int id_user)
        {
            Conexao c = new Conexao();

            using var connection = c.RealizarConexao();

            return connection.Query<Users>("SELECT * FROM usuarios WHERE id_user = " + id_user);
        }

        [HttpPost]

        public string RegisterUsersTeste([FromBody] Users u)
        {
            Conexao c = new();

            using var connection = c.RealizarConexao();

            bool existeEmail = emailExiste(u.User_email);

            if (existeEmail == true)
            {
                connection.Execute(@"INSERT INTO usuarios (user_name, user_email, user_password, user_role) VALUES (:User_name, :User_email, :User_password, :User_role)", u);

                return "Cadastro efetuado com sucesso!";
            }
            else
            {
                return "Falha no cadastro";
            }

        }
        private bool emailExiste(string email)
        {
            Cadastro cd = new Cadastro();

            if (email == cd.User_email)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        [HttpPut]

        public string UpdateUsers([FromBody] Users u)
        {
            Conexao c = new();

            using var conncetion = c.RealizarConexao();

            conncetion.Execute(@"UPDATE usuarios SET user_name = :User_name, user_email = :User_email, user_password = :User_password, user_description = :User_description, user_link = :User_link, user_socialmedia = :User_socialmedia, user_profession = :User_profession, user_hours_week = :User_hours_week, user_experience = :User_experience, user_role = :User_Role WHERE id_user = :Id_user", u);

            return "Pessoa alterada com sucesso!";
        }

        [HttpPut("perfil")]

        public string UpdateUsersPerfil([FromBody] Users u)
        {
            Conexao c = new();

            using var conncetion = c.RealizarConexao();

            conncetion.Execute(@"UPDATE usuarios SET user_name = :User_name, user_email = :User_email, user_password = :User_password, user_description = :User_description WHERE id_user = :Id_user", u);

            return "Pessoa alterada com sucesso!";
        }

        [HttpDelete("{id_user}")]

        public string DeleteUsers(int id_user)
        {
            Conexao c = new();

            using var connection = c.RealizarConexao();

            int count = contabilizar(id_user);

            if (count > 0)
            {
                connection.Execute(@"DELETE FROM usuarios WHERE id_user = " + id_user);
                return "Removido com sucesso!";
            }
            else
            {
                return $"Falha na remoção {count}";
            }
        }

        private int contabilizar(int id)
        {
            Conexao c = new Conexao();

            using var connection = c.RealizarConexao();

            return connection.ExecuteScalar<int>(@"SELECT COUNT(*) FROM usuarios WHERE id_user = " + id);
        }

    }
}