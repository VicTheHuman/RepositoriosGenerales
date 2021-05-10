using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Repository
{
    public interface IRepository<TParameters> : IDisposable where TParameters : class
    {
        #region Métodos CRUD clásicos

        /// <summary>
        /// Agrega un nuevo registro a la db.
        /// </summary>
        /// <typeparam name="TEntity">Tipo de objeto a crear por ORM.</typeparam>
        /// <param name="newEntity">Entidad a agregar en la db.</param>
        /// <returns>Entidad agregada.</returns>
        TEntity Create<TEntity>(TEntity newEntity) where TEntity : class;
        /// <summary>
        /// Actualiza un registro de la db.
        /// </summary>
        /// <typeparam name="TEntity">Tipo de objeto a modificar por ORM.</typeparam>
        /// <param name="modifiedEntity">Entidad a modificar.</param>
        /// <returns>True-Registro modificado exitosamente, false-Registro no modificado.</returns>
        bool? Update<TEntity>(TEntity modifiedEntity) where TEntity : class;
        /// <summary>
        /// Elimina un registro de la db.
        /// </summary>
        /// <typeparam name="TEntity">Tipo de objeto a eliminar por ORM.</typeparam>
        /// <param name="deletedEntity">Entidad a eliminar.</param>
        /// <returns>True-Registro eliminado exitosamente, false-Registro no eliminado.</returns>
        bool? Delete<TEntity>(TEntity deletedEntity) where TEntity : class;
        /// <summary>
        /// Obtiene un registro de la db.
        /// </summary>
        /// <typeparam name="TEntity">Tipo de objeto a obtener por ORM.</typeparam>
        /// <param name="criteria">Criterio para obtener la entidad</param>
        /// <returns>Entidad obtenida.</returns>
        TEntity FindEntity<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class;
        /// <summary>
        /// Obtiene un conjunto de registros de la db.
        /// </summary>
        /// <typeparam name="TEntity">Tipo de los objetos de la lista a obtener por ORM.</typeparam>
        /// <param name="criteria">Criterio para obtener los registros</param>
        /// <returns>Lista de entidades obtenidas</returns>
        IEnumerable<TEntity> FindEntitySet<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class;

        #endregion

        #region Métodos CRUD más completos

        /// <summary>
        /// Agrega un nuevo registro a la db.
        /// </summary>
        /// <param name="sql">Consulta sql.</param>
        /// <param name="parameters">Parámetros a usar en la consulta sql.</param>
        /// <returns>Número de filas afectadas.</returns>
        int? Create(string sql, object parameters);
        /// <summary>
        /// Actualiza un registro de la db.
        /// </summary>
        /// <param name="sql">Consulta sql.</param>
        /// <param name="parameters">Parámetros a usar en la consulta sql.</param>
        /// <returns>Número de filas afectadas.</returns>
        int? Update(string sql, object parameters);
        /// <summary>
        /// Elimina un registro de la db.
        /// </summary>
        /// <param name="sql">Consulta sql.</param>
        /// <param name="parameters">Parámetros a usar en la consulta sql.</param>
        /// <returns>Número de filas afectadas.</returns>
        int? Delete(string sql, object parameters);
        /// <summary>
        /// Obtiene un registro de la db.
        /// </summary>
        /// <typeparam name="TEntity">Tipo de objeto a retornar por ORM.</typeparam>
        /// <param name="sql">Consulta sql.</param>
        /// <param name="parameters">Parámetros a usar en la consulta sql.</param>
        /// <returns>Registro obtenido en forma de objeto.</returns>
        TEntity FindEntity<TEntity>(string sql, object parameters);
        /// <summary>
        /// Obtiene un conjunto de registros de la db.
        /// </summary>
        /// <typeparam name="TEntity">Tipo de objeto a retornar dentro de una lista por ORM.</typeparam>
        /// <param name="sql">Consulta sql.</param>
        /// <param name="parameters">Parámetros a usar en la consulta sql.</param>
        /// <returns>Conjunto de registros obtenidos en forma de lista de objeto.</returns>
        IEnumerable<TEntity> FindEntitySet<TEntity>(string sql, object parameters);

        #endregion

        #region Métodos con stored procedures

        /// <summary>
        /// Obtiene un registro de la db.
        /// </summary>
        /// <typeparam name="TEntity">Tipo de objeto a retornar por ORM.</typeparam>
        /// <param name="storedProcedure">Nombre del SP que se ejecutará.</param>
        /// <param name="parameters">Parámetros a usar por el SP.</param>
        /// <returns>Registro obtenido en forma de objeto.</returns>
        TEntity FindEntity<TEntity>(string storedProcedure, TParameters parameters) where TEntity : class;
        /// <summary>
        /// Obtiene un conjunto de registros de la db.
        /// </summary>
        /// <typeparam name="TEntity">Tipo de objeto a retornar dentro de una lista por ORM.</typeparam>
        /// <param name="storedProcedure">Nombre del SP que se ejecutará.</param>
        /// <param name="parameters">Parámetros a usar por el SP.</param>
        /// <returns>Conjunto de registros obtenidos en forma de lista de objeto.</returns>
        public IEnumerable<TEntity> FindEntitySet<TEntity>(string storedProcedure, TParameters parameters) where TEntity : class;
        /// <summary>
        /// Guarda un registro en la db y retorna una propiedad, puede ser el Id creado desde la db.
        /// </summary>
        /// <param name="storedProcedure">Nombre del SP que se ejecutará.</param>
        /// <param name="parameters">Parámetros a usar por el SP.</param>
        /// <param name="Return">Nombre de la propiedad que se retornará</param>
        /// <returns>Propiedad retornada, suele ser el Id.</returns>
        int? SaveEntityParametrizedWithReturn(string storedProcedure, TParameters parameters, string Return);
        /// <summary>
        /// Elimina un registro de la db.
        /// </summary>
        /// <param name="storedProcedure">Nombre del SP que se ejecutará.</param>
        /// <param name="parameters">Parámetros a usar por el SP.</param>
        /// <param name="Return">Nombre de la propiedad en donde se guardara el valor de retorno.</param>
        /// <returns>True-Registro eliminado exitosamente, false-Registro no eliminado.</returns>
        bool? DeleteEntity(string storedProcedure, TParameters parameters, string Return);
        /// <summary>
        /// Obtiene una o varias propiedades de un registro.
        /// </summary>
        /// <param name="storedProcedure">Nombre del SP que se ejecutará.</param>
        /// <param name="parameters">Parámetros a usar por el SP.</param>
        /// <param name="Return">Nombre de la propiedad en donde se guardara el valor de retorno (bool)</param>
        /// <returns>True-Registro eliminado exitosamente, false-Registro no eliminado.</returns>
        bool? FindProperty(string storedProcedure, TParameters parameters, string Return);

        #endregion
    }
}
