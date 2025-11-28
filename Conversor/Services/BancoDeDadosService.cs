using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Conversor.Models;

namespace Conversor.Services
{
    public class BancoDeDadosService
    {
        private readonly string _dbPath;

        public BancoDeDadosService(string dbPath)
        {
            _dbPath = dbPath;
            CriarBancoSeNaoExistir();
        }

        private void CriarBancoSeNaoExistir()
        {
            using var conexao = new SqliteConnection($"Data Source = {_dbPath}");
            conexao.Open();
            using var comando = conexao.CreateCommand();
            comando.CommandText =
            @"
                CREATE TABLE IF NOT EXISTS Conversoes (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    MoedaOrigem TEXT NOT NULL,
                    MoedaDestino TEXT NOT NULL,
                    Valor REAL NOT NULL,
                    Resultado REAL NOT NULL,
                    CriadoEm TEXT NOT NULL
                );
            ";
            comando.ExecuteNonQuery();
        }

        public List<Conversao> ObterTodos()
        {
            var lista = new List<Conversao>();
            using var conexao = new SqliteConnection($"Data Source = {_dbPath}");
            conexao.Open();
            using var comando = conexao.CreateCommand();
            comando.CommandText = "SELECT Id, MoedaOrigem, MoedaDestino, Valor, Resultado, CriadoEm FROM Conversoes;";
            using var leitor = comando.ExecuteReader();
            while (leitor.Read())
            {
                var conversao = new Conversao
                {
                    Id = leitor.GetInt32(0),
                    MoedaOrigem = leitor.GetString(1),
                    MoedaDestino = leitor.GetString(2),
                    Valor = leitor.GetDouble(3),
                    Resultado = leitor.GetDouble(4),
                    Criadoem = DateTime.Parse(leitor.GetString(5))
                };
            }
            return lista;
        }

        public int inserir(Conversao c)
        {
            using var conexao = new SqliteConnection($"Data Source = {_dbPath}");
            conexao.Open();
            using var comando = conexao.CreateCommand();
            comando.CommandText =
            @"
                INSERT INTO Conversoes (MoedaOrigem, MoedaDestino, Valor, Resultado, CriadoEm)
                VALUES ($origem, $destino, $valor, $resultado, $criadoEm);
                SELECT last_insert_rowid();
            ";
            comando.Parameters.AddWithValue("$moedaOrigem", c.MoedaOrigem);
            comando.Parameters.AddWithValue("$moedaDestino", c.MoedaDestino);
            comando.Parameters.AddWithValue("$valor", c.Valor);
            comando.Parameters.AddWithValue("$resultado", c.Resultado);
            comando.Parameters.AddWithValue("$criadoEm", c.Criadoem.ToString("o"));
            return (int)(long)comando.ExecuteScalar();
        }

        public void Atulizar (Conversao c)
        {
            using var conexao = new SqliteConnection($"Data Source = {_dbPath}");
            conexao.Open();
            using var comando = conexao.CreateCommand();
            comando.CommandText =
            @"
                UPDATE Conversoes SET
                    MoedaOrigem = $origem,
                    MoedaDestino = $destino,
                    Valor = $valor,
                    Resultado = $resultado
                WHERE Id = $id;
            ";
            comando.Parameters.AddWithValue("$origem", c.MoedaOrigem);
            comando.Parameters.AddWithValue("$destino", c.MoedaDestino);
            comando.Parameters.AddWithValue("$valor", c.Valor);
            comando.Parameters.AddWithValue("$resultado", c.Resultado);
            comando.Parameters.AddWithValue("$id", c.Id);
            comando.ExecuteNonQuery();
        }

        public void Excluir(int id)
        {
            using var conexao = new SqliteConnection($"Data Source = {_dbPath}");
            conexao.Open();
            using var comando = conexao.CreateCommand();
            comando.CommandText = "DELETE FROM Conversoes WHERE Id = $id";
            comando.Parameters.AddWithValue("$id", id);
            comando.ExecuteNonQuery();
        }

    }
}