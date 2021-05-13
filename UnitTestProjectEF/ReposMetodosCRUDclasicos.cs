using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTestProjectEF.Models;
using EFRepository;
using System.Linq.Expressions;
using System;
using System.Collections.Generic;

namespace UnitTestProjectEF
{
    [TestClass]
    public class ReposMetodosCRUDclasicos
    {
        private string connectionString = "Server=.\\SQLExpress;Database=CSI;Trusted_Connection=True;";
        [TestMethod]
        public void Given_InstanciaRepositorio_When_Create_Then_ReturnLaEntidadCreada()
        {
            Personal persona = new Personal { Nombre = "Sinai", Apellidos = "Zarate Marin", Edad = 36 };
            Personal result;
            using (var repository = new RepositoryEF(new CSIContext(connectionString)))
            {
                result = repository.Create<Personal>(persona);
            }
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void Given_InstanciaRepositorio_When_Update_Then_ReturnTrue()
        {
            Personal persona = new Personal { Id = 19, Nombre = "Jazmin", Apellidos = "Zarate Marin", Edad = 36 };
            bool? result;
            using (var repository = new RepositoryEF(new CSIContext(connectionString)))
            {
                result = repository.Update<Personal>(persona);
            }
            if (result != null)
                Assert.IsTrue((bool)result);
            else
                Assert.IsNotNull(result);
        }
        [TestMethod]
        public void Given_InstanciaRepositorio_When_Delete_Then_ReturnTrue()
        {
            Personal persona = new Personal { Id = 19, Nombre = "Jazmin", Apellidos = "Zarate Marin", Edad = 36 };
            bool? result;
            using (var repository = new RepositoryEF(new CSIContext(connectionString)))
            {
                result = repository.Delete<Personal>(persona);
            }
            if (result != null)
                Assert.IsTrue((bool)result);
            else
                Assert.IsNotNull(result);
        }
        [TestMethod]
        public void Given_InstanciaRepositorio_When_FindEntity_Then_ReturnEntity()
        {
            Personal result;
            Expression<Func<Personal, bool>> criteria = c => c.Id == 7;
            using (var repository = new RepositoryEF(new CSIContext(connectionString)))
            {
                result = repository.FindEntity<Personal>(criteria);
            }
            Assert.IsNotNull(result);
         }
        [TestMethod]
        public void Given_InstanciaRepositorio_When_FindEntitySet_Then_ReturnEntitySet()
        {
            List<Personal> result;
            Expression<Func<Personal, bool>> criteria = c => true;
            using (var repository = new RepositoryEF(new CSIContext(connectionString)))
            {
                result = repository.FindEntitySet<Personal>(criteria) as List<Personal>;
            }
            Assert.IsNotNull(result);
        }
    }
}
