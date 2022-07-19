﻿using Dapper;
using DapperTrabalhoFinal.Config;
using DapperTrabalhoFinal.Models;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;

namespace DapperTrabalhoFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CadastroController
    {

        [HttpGet("Conexao")]

        public string TestarConexao()
        {
            Conexao c = new Conexao();

            OracleConnection obj = c.RealizarConexao();

            obj.Open();

            string Mensagem;

            if (obj.State.ToString() == "Open")
            {
                Mensagem = "Conexão efetuada";
            }
            else
            {
                Mensagem = "Falha ao cadastrar";
            }
            return Mensagem;
        }

        [HttpGet]

        public IEnumerable<Cadastro> ListUsers()
        {
            Conexao c = new Conexao();

            using var connection = c.RealizarConexao();

            return connection.Query<Cadastro>("SELECT * FROM usuarios");
        }

        [HttpPost]

        public string RegisterUsersTeste([FromBody] Cadastro u)
        {
            Conexao c = new();

            using var connection = c.RealizarConexao();

            connection.Execute(@"INSERT INTO usuarios (user_name, user_email, user_password, user_role) VALUES (:User_name, :User_email, :User_password, :User_role)", u);

            return "Cadastro efetuado com sucesso!";
        }
    }
}
