using Microsoft.VisualStudio.TestTools.UnitTesting;
using DapperRepository;
using System.Collections.Generic;

namespace UnitTestProjectDapper
{
    [TestClass]
    public class ReposMetodosCRUDcompletos
    {
        private string connectionString = "Server=.\\SQLExpress;Database=CSI;Trusted_Connection=True;";
        [TestMethod]
        public void Given_InstanciaRepositorio_When_Create_Then_ReturnUnaFilaModificada()
        {
            string sql = "INSERT INTO Personal (Nombre, Apellidos, Edad) VALUES (@nombre, @apellidos, @edad)";
            object parameters = new { nombre = "Omar", apellidos = "Sanchez", edad = 23 };
            int? result;
            using (var repositorio = new RepositoryD(connectionString))
            {
                result = repositorio.Create(sql, parameters);
            }
            Assert.AreEqual(1, result);
        }
        [TestMethod]
        public void Given_InstanciaRepositorio_When_Update_Then_ReturnUnaFilaModificada()
        {
            string sql = "UPDATE Personal SET Nombre = @nombre, Apellidos = @apellidos, Edad = @edad WHERE Id = @id";
            object parameters = new { nombre = "Jessy", Apellidos = "Koch", edad = 33, id = 7};
            int? result;
            using (var repositorio = new RepositoryD(connectionString))
            {
                result = repositorio.Update(sql, parameters);
            }
            Assert.AreEqual(1, result);
        }
        [TestMethod]
        public void Given_InstanciaRepositorio_When_Delete_Then_ReturnUnaFilaModificada()
        {
            string sql = "DELETE FROM Personal WHERE Id = @id";
            object parameters = new { id = 17 };
            int? result;
            using (var repositorio = new RepositoryD(connectionString))
            {
                result = repositorio.Delete(sql, parameters);
            }
            Assert.AreEqual(1, result);
        }
        [TestMethod]
        public void Given_InstanciaRepositorio_When_FindEntity_Then_ReturnEntity()
        {
            string sql = "SELECT * FROM Personal WHERE Id = @id";
            object parameters = new { id = 7 };
            Persona result;
            using (var repositorio = new RepositoryD(connectionString))
            {
                result = repositorio.FindEntity<Persona>(sql, parameters);
            }
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void Given_InstanciaRepositorio_When_FindEntitySet_Then_ReturnEntitySet()
        {
            string sql = "SELECT * FROM Personal";
            object parameters = new { };
            List<Persona> result;
            using (var repositorio = new RepositoryD(connectionString))
            {
                result = repositorio.FindEntitySet<Persona>(sql, parameters) as List<Persona>;
            }
            Assert.IsNotNull(result);
        }
    }
}
