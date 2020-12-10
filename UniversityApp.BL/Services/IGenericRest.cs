using System.Collections.Generic;
using System.Threading.Tasks;

namespace UniversityApp.BL.Services
{    
    public interface IGenericRest<TEntity>
    {
        //Tutorial 56 - Parte 10 - Validación de conexión a internet
        Task<bool> CheckConnection();
        Task<IEnumerable<TEntity>> GetAll(string url);

        Task<TEntity> GetById(string url, int id);

        Task<TEntity> Create(string url, TEntity entity);

        Task Update(string url, int id, TEntity entity);

        Task Delete(string url, int id);
    }
}
