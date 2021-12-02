using Backend.Model;


namespace Backend.DAL
{
    public class PriceUpdater<T> : IDataUpdater<T> where T : Price
    {
        public void UpdateDataObject(T objectToUpdate, T newObject)
        {
            objectToUpdate.ProductCode = newObject.ProductCode; ;
            objectToUpdate.Amount = newObject.Amount; ;
            objectToUpdate.Pack = newObject.Pack; ;

        }
    }
}
