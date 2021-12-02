

using Backend.Model;

namespace Backend.DAL
{
    public interface IDataUpdater<T> where T : IDataObject
    {
        void UpdateDataObject(T objectToUpdate, T newObject);
    }
}