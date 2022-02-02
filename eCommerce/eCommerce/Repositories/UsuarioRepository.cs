using eCommerce.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace eCommerce.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        //IDbConnection é uma interface do ADO.NET - se comunica com vários BD
        private IDbConnection _connection;
        public UsuarioRepository()
        {
            _connection = new SqlConnection(@"Data Source=DESKTOP-U72UVTF;Initial Catalog=CursoDapper;
                                                                    Integrated Security=True");
        }


        public List<Usuario> Get()
        {
            return _connection.Query<Usuario>("SELECT * FROM Usuarios;").ToList();
        }


        public Usuario Get(int id)
        {
            string sql = @"SELECT * 
                        FROM Usuarios as U 
                        INNER JOIN Contatos as C
                        ON U.id = C.Id WHERE U.Id = @Id;";

            return _connection.Query<Usuario, Contato, Usuario>(sql,

                //função anônima para mapeamento de usuario e contato
                (usuario, contato) =>
                {
                    usuario.Contato = contato;
                    return usuario;
                },
                new { Id = id }).SingleOrDefault();
        }

        /*GET BY ID OLD - RETORNA SOMENTE O Usuario
         * Retorna somento o usuário by Id
        public Usuario Get(int id)
        {
            return _connection.QuerySingleOrDefault<Usuario>("SELECT * FROM Usuarios WHERE Id = @Id", new { Id = id });
        }
        */


        /*
        //INSERT ORIGINAL - SOMENTE USUÁRIO
        public void Insert(Usuario usuario)
        {
            string sql = "INSERT INTO Usuarios(Nome, Email, Sexo, RG, CPF, NomeMae, SituacaoCadastro,DataCadastro)VALUES(@Nome, @Email, @Sexo, @RG, @CPF, @NomeMae, @SituacaoCadastro, @DataCadastro); SELECT CAST(SCOPE_IDENTITY()AS INT);";
            usuario.Id = _connection.Query<int>(sql, usuario).Single();
        
            //OBS: essa é uma query de seleção que retorna o id -  SELECT CAST(SCOPE_IDENTITY()AS INT);
        }
        */

        public void Update(Usuario usuario)
        {
            string sql = @"UPDATE Usuarios SET Nome = @Nome, Email = @Email, Sexo = @Sexo, RG = @RG, CPF = @CPF, NomeMae = @NomeMae, SituacaoCadastro = @SituacaoCadastro, DataCadastro = @DataCadastro WHERE Id = @Id";
            _connection.Execute(sql,usuario);
        }


        public void Delete(int id)
        {
            _connection.Execute("DELETE FROM Usuarios WHERE Id = @Id", new { Id = id });
        }






        


    }
}
