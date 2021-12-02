
using Backend.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.DAL
{
    public interface IDataRepository<T> where T : IDataObject
    {
        //Task<IEnumerable<T>> GetPrices();
        void Update(T updatedData);
        Task UpdateTask(T updatedData);


        Task<IEnumerable<T>> GetAllTask();
        IEnumerable<T> GetAll();
        T Get(string code);
        Task<T> GetTask(string code);


        void Create(T newObject);

        Task CreateTask(T existingObjec);

        void Delete(string code);

        Task DeleteTask(string code);

    }
}
