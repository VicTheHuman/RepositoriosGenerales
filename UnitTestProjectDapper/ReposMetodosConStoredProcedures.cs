using Microsoft.VisualStudio.TestTools.UnitTesting;
using DapperRepository;
using Dapper;
using System.Collections.Generic;
using System.Data;

namespace UnitTestProjectDapper
{
    [TestClass]
    public class ReposMetodosConStoredProcedures
    {
        [TestMethod]
        public void Given_InstanciaRepositorio_When_FindEntity_Then_ReturnEntity()
        {
            Persona result;
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add("@Id", 7);
            using (var repository = new RepositoryD("Server=.\\SQLExpress;Database=CSI;Trusted_Connection=True;"))
            {
                result = repository.FindEntity<Persona>("ObtenerPersona", parametros);
            }
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void Given_InstanciaRepositorio_When_FindEntitySet_Then_ReturnEntitySet()
        {
            List<Persona> result;
            DynamicParameters parametros = new DynamicParameters();
            using (var repositorio = new RepositoryD("Server=.\\SQLExpress;Database=CSI;Trusted_Connection=True;"))
            {
                result = repositorio.FindEntitySet<Persona>("ObtenerPersonas", parametros) as List<Persona>;
            }
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void Given_InstanciaRepositorio_When_Delete_Then_ReturnTrue()
        {
            
            bool? result;
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add("@Id", 8);
            parametros.Add("@result", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            using (var repositorio = new RepositoryD("Server=.\\SQLExpress;Database=CSI;Trusted_Connection=True;"))
            {
                result = repositorio.DeleteEntity("EliminarPersona", parametros, "@result");
            }
            if (result != null)
                Assert.IsTrue((bool)result);
            else
                Assert.IsNotNull(result);
        }
        [TestMethod]
        public void Given_InstanciaRepositorio_When_FindProperty_Then_ReturnTrue()
        {

            bool? result;
            string nombre = null;
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add("@Id", 7);
            parametros.Add("@Nombre", dbType: DbType.String, direction: ParameterDirection.Output, size: 10);
            parametros.Add("@result", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            using (var repositorio = new RepositoryD("Server=.\\SQLExpress;Database=CSI;Trusted_Connection=True;"))
            {
                result = repositorio.FindProperty("ObtenerPropiedadPersona", parametros, "@result");
                if (result != null)
                {
                    if (result == true)
                        nombre = parametros.Get<string>("@Nombre");
                }
            }
            Assert.IsNotNull(nombre);
            
        }
        [TestMethod]
        public void Given_InstanciaRepositorio_When_SaveEntityParametrizedWithReturn_Then_ReturnEntero()
        {

            int? result;
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add("@Nombre", "Samuel");
            parametros.Add("@Apellidos", "Zarate");
            parametros.Add("@Edad", 17);
            parametros.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
            using (var repositorio = new RepositoryD("Server=.\\SQLExpress;Database=CSI;Trusted_Connection=True;"))
            {
                result = repositorio.SaveEntityParametrizedWithReturn("CrearPersona", parametros, "@Id");
            }
            Assert.IsNotNull(result);

        }
    }
}
