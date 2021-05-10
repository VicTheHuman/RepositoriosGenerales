using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Configuration;
using Dapper;
using Repository;

namespace DapperRepository
{
    public class RepositoryD : IRepository<DynamicParameters>
    {
        /// <summary>
        /// Si una excepción se produce, en esta variable se guarda el mensaje
        /// </summary>
        public string MensajeError;
        /// <summary>
        /// Objeto que maneja la conexión con la db.
        /// </summary>
        protected IDbConnection connection;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString">Cadena de conexión</param>
        public RepositoryD(string connectionString) => connection = new SqlConnection(connectionString);
        /// <summary>
        /// Termina la conexión, necesario para implementar IDisposable
        /// </summary>
        public void Dispose() => connection.Dispose();

        #region Métodos CRUD clásicos

        public TEntity Create<TEntity>(TEntity newEntity) where TEntity : class
        {
            throw new NotImplementedException();
        }
        
        public bool? Update<TEntity>(TEntity modifiedEntity) where TEntity : class
        {
            throw new NotImplementedException();
        }
        
        public bool? Delete<TEntity>(TEntity deletedEntity) where TEntity : class
        {
            throw new NotImplementedException();
        }
        
        public TEntity FindEntity<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class
        {
            throw new NotImplementedException();
        }
        
        public IEnumerable<TEntity> FindEntitySet<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Métodos CRUD más completos

        /// <summary>
        /// Agrega un nuevo registro a la db.
        /// </summary>
        /// <param name="sql">Consulta sql.</param>
        /// <param name="parameters">Parámetros a usar en la consulta sql.</param>
        /// <returns>Número de filas afectadas.</returns>
        public int? Create(string sql, object parameters)
        {
            int? result;
            try
            {
                result = connection.Execute(sql, parameters);
            }
            catch (Exception e)
            {
                result = null;
                MensajeError = e.Message;
            }
            return result;
        }
        /// <summary>
        /// Actualiza un registro de la db.
        /// </summary>
        /// <param name="sql">Consulta sql.</param>
        /// <param name="parameters">Parámetros a usar en la consulta sql.</param>
        /// <returns>Número de filas afectadas.</returns>
        public int? Update(string sql, object parameters)
        {
            int? result;
            try
            {
                result = connection.Execute(sql, parameters);
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
        /// <param name="sql">Consulta sql.</param>
        /// <param name="parameters">Parámetros a usar en la consulta sql.</param>
        /// <returns>Número de filas afectadas.</returns>
        public int? Delete(string sql, object parameters)
        {
            int? result;
            try
            {
                result = connection.Execute(sql, parameters);
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
        /// <typeparam name="TEntity">Tipo de objeto a retornar por ORM.</typeparam>
        /// <param name="sql">Consulta sql.</param>
        /// <param name="parameters">Parámetros a usar en la consulta sql.</param>
        /// <returns>Registro obtenido en forma de objeto.</returns>
        public TEntity FindEntity<TEntity>(string sql, object parameters)
        {
            TEntity result;
            try
            {
                result = connection.QueryFirst<TEntity>(sql, parameters);
            }
            catch (Exception e)
            {
                result = default(TEntity);
                MensajeError = e.Message;
            }
            return result;
        }
        /// <summary>
        /// Obtiene un conjunto de registros de la db.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql">Consulta sql.</param>
        /// <param name="parameters">Parámetros a usar en la consulta sql.</param>
        /// <returns>Conjunto de registros obtenidos en forma de lista de objeto.</returns>
        public IEnumerable<TEntity> FindEntitySet<TEntity>(string sql, object parameters)
        {
            IEnumerable<TEntity> result;
            try
            {
                result = connection.Query<TEntity>(sql, parameters).ToList();
            }
            catch (Exception e)
            {
                result = default(IEnumerable<TEntity>);
                MensajeError = e.Message;
            }
            return result;
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
        public TEntity FindEntity<TEntity>(string storedProcedure, DynamicParameters parameters) where TEntity : class
        {
            TEntity result;
            try
            {
                result = connection.QueryFirst<TEntity>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception e)
            {
                result = default(TEntity);
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
        public IEnumerable<TEntity> FindEntitySet<TEntity>(string storedProcedure, DynamicParameters parameters) where TEntity : class
        {
            List<TEntity> result;
            try
            {
                result = connection.Query<TEntity>(storedProcedure, parameters, commandType: CommandType.StoredProcedure).ToList();
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
        public int? SaveEntityParametrizedWithReturn(string storedProcedure, DynamicParameters parameters, string Return)
        {
            int? result;
            try
            {
                connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                result = parameters.Get<int?>(Return);
            }
            catch (Exception e)
            {
                MensajeError = e.Message;
                result = null;
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
        public bool? DeleteEntity(string storedProcedure, DynamicParameters parameters, string Return)
        {
            bool? result;
            try
            {
                connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                result = parameters.Get<bool>(Return);
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
        public bool? FindProperty(string storedProcedure, DynamicParameters parameters, string Return)
        {
            bool? result;
            try
            {
                connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                result = parameters.Get<bool>(Return);
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
