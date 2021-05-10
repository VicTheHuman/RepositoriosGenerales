using System;
using System.Collections.Generic;
using System.Text;
using Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Linq.Expressions;
using System.Data;

namespace EFRepository
{
    public class RepositoryEF : IRepository<SqlParameter[]>
    {
        /// <summary>
        /// Si una excepción se produce, en esta variable se guarda el mensaje
        /// </summary>
        public string MensajeError;
        /// <summary>
        /// Contexto que maneja la sesión con la db.
        /// </summary>
        protected DbContext context;
        /// <summary>
        /// Constructor base.
        /// </summary>
        /// <param name="context">Contexto que maneja la sesión con la db.</param>
        public RepositoryEF(DbContext context) => this.context = context;
        /// <summary>
        /// Aplica cambios a la db después de una consulta.
        /// </summary>
        /// <returns>Número de entidades escritas en la db.</returns>
        protected virtual int TrySaveChanges() => context.SaveChanges();
        /// <summary>
        /// Libera los recursos del contexto.
        /// </summary>
        public void Dispose()
        {
            if (context != null)
                context.Dispose();
        }

        #region Métodos CRUD clásicos

        /// <summary>
        /// Agrega un nuevo registro a la db.
        /// </summary>
        /// <typeparam name="TEntity">Tipo de objeto a crear por ORM.</typeparam>
        /// <param name="newEntity">Entidad a agregar en la db.</param>
        /// <returns>Entidad agregada.</returns>
        public TEntity Create<TEntity>(TEntity newEntity) where TEntity : class
        {
            TEntity result;
            try
            {
                result = context.Set<TEntity>().Add(newEntity).Entity;
                TrySaveChanges();
            }
            catch (Exception e)
            {
                result = default(TEntity);
                MensajeError = e.Message;
            }
            return result;
        }
        /// <summary>
        /// Actualiza un registro de la db.
        /// </summary>
        /// <typeparam name="TEntity">Tipo de objeto a modificar por ORM.</typeparam>
        /// <param name="modifiedEntity">Entidad a modificar.</param>
        /// <returns>True-Registro modificado exitosamente, false-Registro no modificado.</returns>
        public bool? Update<TEntity>(TEntity modifiedEntity) where TEntity : class
        {
            bool? result;
            try
            {
                context.Update<TEntity>(modifiedEntity);
                result = TrySaveChanges() > 0;
            }
            catch (Exception e)
            {
                result = null;
                MensajeError = e.Message;
            }
            return result;
        }
        /// <summary>
        /// Elimina un registro de la db.
        /// </summary>
        /// <typeparam name="TEntity">Tipo de objeto a eliminar por ORM.</typeparam>
        /// <param name="deletedEntity">Entidad a eliminar.</param>
        /// <returns>True-Registro eliminado exitosamente, false-Registro no eliminado.</returns>
        public bool? Delete<TEntity>(TEntity deletedEntity) where TEntity : class
        {
            bool? result;
            try
            {
                context.Remove<TEntity>(deletedEntity);
                result = TrySaveChanges() > 0;
            }
            catch (Exception e)
            {
                result = null;
                MensajeError = e.Message;
            }
            return result;
        }
        /// <summary>
        /// Obtiene un registro de la db.
        /// </summary>
        /// <typeparam name="TEntity">Tipo de objeto a obtener por ORM.</typeparam>
        /// <param name="criteria">Criterio para obtener la entidad</param>
        /// <returns>Entidad obtenida.</returns>
        public TEntity FindEntity<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class
        {
            TEntity result = null;
            try
            {
                result = context.Set<TEntity>().FirstOrDefault(criteria);
                if (result == null)
                    MensajeError = "No se encontraron registros con ese criterio de buqueda";
            }
            catch (Exception e)
            {
                result = null;
                MensajeError = e.Message;
            }
            return result;
        }
        /// <summary>
        /// Obtiene un conjunto de registros de la db.
        /// </summary>
        /// <typeparam name="TEntity">Tipo de los objetos de la lista a obtener por ORM.</typeparam>
        /// <param name="criteria">Criterio para obtener los registros</param>
        /// <returns>Lista de entidades obtenidas</returns>
        public IEnumerable<TEntity> FindEntitySet<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class
        {
            List<TEntity> result = null;
            try
            {
                result = context.Set<TEntity>().Where(criteria).ToList();
                if (result == null)
                    MensajeError = "No se encontraron registros con ese criterio de buqueda";
            }
            catch (Exception e)
            {
                result = null;
                MensajeError = e.Message;
            }
            return result;
        }

        #endregion

        #region Métodos CRUD más completos

        public int? Create(string sql, object parameters)
        {
            throw new NotImplementedException();
        }

        public int? Update(string sql, object parameters)
        {
            throw new NotImplementedException();
        }

        public int? Delete(string sql, object parameters)
        {
            throw new NotImplementedException();
        }

        public TEntity FindEntity<TEntity>(string sql, object parameters)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> FindEntitySet<TEntity>(string sql, object parameters)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Métodos con stored procedures

        /// <summary>
        /// Obtiene un registro de la db.
        /// </summary>
        /// <typeparam name="TEntity">Tipo de objeto a retornar por ORM.</typeparam>
        /// <param name="storedProcedure">Nombre del SP que se ejecutará.</param>
        /// <param name="parameters">Parámetros a usar por el SP.</param>
        /// <returns>Registro obtenido en forma de objeto.</returns>
        public TEntity FindEntity<TEntity>(string storedProcedure, SqlParameter[] parameters) where TEntity : class
        {
            TEntity result;
            string storeProcedureWhitParameters = storedProcedure + " ";
            foreach (var item in parameters)
            {
                if (item.Direction.Equals(ParameterDirection.Input))
                    storeProcedureWhitParameters += item.ParameterName + ",";
                else if (item.Direction.Equals(ParameterDirection.Output))
                    storeProcedureWhitParameters += item.ParameterName + " output,";
            }
            storeProcedureWhitParameters = storeProcedureWhitParameters.Remove(storeProcedureWhitParameters.Count() - 1);
            try
            {
                result = context.Set<TEntity>().FromSqlRaw(storeProcedureWhitParameters, parameters).ToList().First();
            }
            catch (Exception e)
            {
                result = null;
                MensajeError = e.Message;
            }
            return result;
        }
        /// <summary>
        /// Obtiene un conjunto de registros de la db.
        /// </summary>
        /// <typeparam name="TEntity">Tipo de objeto a retornar dentro de una lista por ORM.</typeparam>
        /// <param name="storedProcedure">Nombre del SP que se ejecutará.</param>
        /// <param name="parameters">Parámetros a usar por el SP.</param>
        /// <returns>Conjunto de registros obtenidos en forma de lista de objeto.</returns>
        public IEnumerable<TEntity> FindEntitySet<TEntity>(string storedProcedure, SqlParameter[] parameters) where TEntity : class
        {
            List<TEntity> result;
            string storeProcedureWhitParameters = storedProcedure + " ";
            foreach (var item in parameters)
            {
                if (item.Direction.Equals(ParameterDirection.Input))
                    storeProcedureWhitParameters += item.ParameterName + ",";
                else if (item.Direction.Equals(ParameterDirection.Output))
                    storeProcedureWhitParameters += item.ParameterName + " output,";
            }
            storeProcedureWhitParameters = storeProcedureWhitParameters.Remove(storeProcedureWhitParameters.Count() - 1);
            try
            {
                result = context.Set<TEntity>().FromSqlRaw(storeProcedureWhitParameters, parameters).ToList();
            }
            catch (Exception e)
            {
                result = null;
                MensajeError = e.Message;
            }
            return result;
        }
        /// <summary>
        /// Guarda un registro en la db y retorna una propiedad, puede ser el Id creado desde la db.
        /// </summary>
        /// <param name="storedProcedure">Nombre del SP que se ejecutará.</param>
        /// <param name="parameters">Parámetros a usar por el SP.</param>
        /// <param name="Return">Nombre de la propiedad que se retornará</param>
        /// <returns>Propiedad retornada, suele ser el Id.</returns>
        public int? SaveEntityParametrizedWithReturn(string storedProcedure, SqlParameter[] parameters, string Return)
        {
            int? result;
            string storeProcedureWhitParameters = storedProcedure + " ";
            foreach (var item in parameters)
            {
                if (item.Direction.Equals(ParameterDirection.Input))
                    storeProcedureWhitParameters += item.ParameterName + ",";
                else if (item.Direction.Equals(ParameterDirection.Output))
                    storeProcedureWhitParameters += item.ParameterName + " output,";
            }
            storeProcedureWhitParameters = storeProcedureWhitParameters.Remove(storeProcedureWhitParameters.Count() - 1);
            try
            {
                context.Database.ExecuteSqlRaw(storeProcedureWhitParameters, parameters);
                //context.Database.ExecuteSqlInterpolated($"ObtenerCliente {param[0]}, {param[1]} output");
                Func<SqlParameter, bool> predicate = c => c.ParameterName == Return;
                result = (int?)parameters.First(predicate).Value;
            }
            catch (Exception e)
            {
                result = null;
                MensajeError = e.Message;
            }
            return result;
        }
        /// <summary>
        /// Elimina un registro de la db.
        /// </summary>
        /// <param name="storedProcedure">Nombre del SP que se ejecutará.</param>
        /// <param name="parameters">Parámetros a usar por el SP.</param>
        /// <param name="Return">Nombre de la propiedad en donde se guardara el valor de retorno.</param>
        /// <returns>True-Registro eliminado exitosamente, false-Registro no eliminado.</returns>
        public bool? DeleteEntity(string storedProcedure, SqlParameter[] parameters, string Return)
        {
            bool? result;
            string storeProcedureWhitParameters = storedProcedure + " ";
            foreach (var item in parameters)
            {
                if (item.Direction.Equals(ParameterDirection.Input))
                    storeProcedureWhitParameters += item.ParameterName + ",";
                else if (item.Direction.Equals(ParameterDirection.Output))
                    storeProcedureWhitParameters += item.ParameterName + " output,";
            }
            storeProcedureWhitParameters = storeProcedureWhitParameters.Remove(storeProcedureWhitParameters.Count() - 1);
            try
            {
                context.Database.ExecuteSqlRaw(storeProcedureWhitParameters, parameters);
                //context.Database.ExecuteSqlInterpolated($"ObtenerCliente {param[0]}, {param[1]} output");
                Func<SqlParameter, bool> predicate = c => c.ParameterName == Return;
                result = (bool?)parameters.First(predicate).Value;
            }
            catch (Exception e)
            {
                result = null;
                MensajeError = e.Message;
            }
            return result;
        }
        /// <summary>
        /// Obtiene una o varias propiedades de un registro.
        /// </summary>
        /// <param name="storedProcedure">Nombre del SP que se ejecutará.</param>
        /// <param name="parameters">Parámetros a usar por el SP.</param>
        /// <param name="Return">Nombre de la propiedad en donde se guardara el valor de retorno (bool)</param>
        /// <returns>True-Registro eliminado exitosamente, false-Registro no eliminado.</returns>
        public bool? FindProperty(string storedProcedure, SqlParameter[] parameters, string Return)
        {
            bool? result;
            string storeProcedureWhitParameters = storedProcedure + " ";
            foreach (var item in parameters)
            {
                if (item.Direction.Equals(ParameterDirection.Input))
                    storeProcedureWhitParameters += item.ParameterName + ",";
                else if (item.Direction.Equals(ParameterDirection.Output))
                    storeProcedureWhitParameters += item.ParameterName + " output,";
            }
            storeProcedureWhitParameters = storeProcedureWhitParameters.Remove(storeProcedureWhitParameters.Count() - 1);
            try
            {
                context.Database.ExecuteSqlRaw(storeProcedureWhitParameters, parameters);
                //context.Database.ExecuteSqlInterpolated($"ObtenerCliente {param[0]}, {param[1]} output");
                Func<SqlParameter, bool> predicate = c => c.ParameterName == Return;
                result = (bool?)parameters.First(predicate).Value;
            }
            catch (Exception e)
            {
                result = null;
                MensajeError = e.Message;
            }
            return result;
        }

        #endregion

    }
}
