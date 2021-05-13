using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTestProjectEF.Models;
using EFRepository;
using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace UnitTestProjectEF
{
    [TestClass]
    public class ReposMetodosConStoredProcedures
    {
        private string connectionString = "Server=.\\SQLExpress;Database=CSI;Trusted_Connection=True;";
        [TestMethod]
        public void Given_InstanciaRepositorio_When_FindEntity_Then_ReturnEntity()
        {
            Personal result;
            var parameters = new[]
            {
                new SqlParameter("@Id", System.Data.SqlDbType.Int)
                {
                    Direction = System.Data.ParameterDirection.Input,
                    Value = 7
                }
            };
            using (var repository = new RepositoryEF(new CSIContext(connectionString)))
            {
                result = repository.FindEntity<Personal>("ObtenerPersona", parameters);
            }
            Assert.IsNotNull(result);

        }
        [TestMethod]
        public void Given_InstanciaRepositorio_When_FindEntitySet_Then_ReturnEntitySet()
        {
            List<Personal> result;
            var parameters = new SqlParameter[] { };
            using (var repository = new RepositoryEF(new CSIContext(connectionString)))
            {
                result = repository.FindEntitySet<Personal>("ObtenerPersonas", parameters) as List<Personal>;
            }
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void Given_InstanciaRepositorio_When_SaveEntityParametrizedWithReturn_Then_ReturnEntero()
        {
            int? result;
            var parameters = new[]
            {
                new SqlParameter("@Nombre", System.Data.SqlDbType.NVarChar)
                {
                    Direction = System.Data.ParameterDirection.Input,
                    Value = "Ana",
                    Size = 10
                },
                new SqlParameter("@Apellidos", System.Data.SqlDbType.NVarChar)
                {
                    Direction = System.Data.ParameterDirection.Input,
                    IsNullable = true,
                    Value = DBNull.Value,
                    Size = 10
                },
                new SqlParameter("@Edad", System.Data.SqlDbType.Int)
                {
                    Direction = System.Data.ParameterDirection.Input,
                    Value = 32

                },
                new SqlParameter("@Id", System.Data.SqlDbType.Int)
                {
                    Direction = System.Data.ParameterDirection.Output,
                    
                },
            };
            using (var repository = new RepositoryEF(new CSIContext(connectionString)))
            {
                result = repository.SaveEntityParametrizedWithReturn("CrearPersona", parameters, "@Id");
            }
            Assert.IsNotNull(result);

        }
        [TestMethod]
        public void Given_InstanciaRepositorio_When_DeleteEntity_Then_ReturnTrue()
        {
            bool? result;
            var parameters = new[]
            {
                new SqlParameter("@Id", System.Data.SqlDbType.Int)
                {
                    Direction = System.Data.ParameterDirection.Input,
                    Value = 20
                },
                new SqlParameter("@result", System.Data.SqlDbType.Bit)
                {
                    Direction = System.Data.ParameterDirection.Output,

                }
            };
            using (var repository = new RepositoryEF(new CSIContext(connectionString)))
            {
                result = repository.DeleteEntity("EliminarPersona", parameters, "@result");
            }
            Assert.IsNotNull(result);

        }
        [TestMethod]
        public void Given_InstanciaRepositorio_When_FindProperty_Then_ReturnTrue()
        {
            string nombre = null;
            bool? result;
            var parameters = new[]
            {
                new SqlParameter("@Id", System.Data.SqlDbType.Int)
                {
                    Direction = System.Data.ParameterDirection.Input,
                    Value = 7
                },
                new SqlParameter("@Nombre", System.Data.SqlDbType.NVarChar)
                {
                    Direction = System.Data.ParameterDirection.Output,
                    Size = 10
                },
                new SqlParameter("@result", System.Data.SqlDbType.Bit)
                {
                    Direction = System.Data.ParameterDirection.Output,
                    
                }
            };
            using (var repository = new RepositoryEF(new CSIContext(connectionString)))
            {
                result = repository.FindProperty("ObtenerPropiedadPersona", parameters, "@result");
                if (result == true)
                    nombre = parameters[1].Value.ToString();
            }
            Assert.IsNotNull(nombre);

        }
        

    }
}
