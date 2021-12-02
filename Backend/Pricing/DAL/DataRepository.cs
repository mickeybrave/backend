
using Backend.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Backend.DAL
{
    public class DataRepository<T> : IDataRepository<T> where T : IDataObject
    {
        public const string ConstDefaultDataPath = "Pricing/DAL/Prices.json";
        private readonly string _filePath;
        private readonly IDataUpdater<T> _dataUpdater;
        private IConfiguration _config;
        private const string ConstDataPathConfig = "DataPath";
        public DataRepository(IConfiguration config, IDataUpdater<T> dataUpdater)
        {
            _config = config;
            var dataSourceJsonPath = _config[ConstDataPathConfig];
            if (string.IsNullOrEmpty(dataSourceJsonPath))
            {
                throw new ArgumentNullException(nameof(dataSourceJsonPath));
            }
            this._filePath = dataSourceJsonPath;
            this._dataUpdater = dataUpdater;
        }
        public DataRepository(IDataUpdater<T> dataUpdater)
        {
            this._filePath = ConstDefaultDataPath;
            this._dataUpdater = dataUpdater;
        }

        public Task<IEnumerable<T>> GetAllTask()
        {
            return Task.Run(() => GetAll());
        }

        public IEnumerable<T> GetAll()
        {
            using (StreamReader r = new StreamReader(_filePath))
            {
                string json = r.ReadToEnd();
                return  JsonConvert.DeserializeObject<IEnumerable<T>>(json);
            }
        }

        public T Get(string code)
        {
            using (StreamReader r = new StreamReader(_filePath))
            {
                string json = r.ReadToEnd();
                return JsonConvert.DeserializeObject<List<T>>(json).FirstOrDefault(f => f.ProductCode == code);
            }
        }

        public Task<T> GetTask(string id)
        {
            return Task.Run(() => this.Get(id));
        }


        public void Create(T newObject)
        {
            var json = File.ReadAllText(_filePath);
            var allData = JsonConvert.DeserializeObject<List<T>>(json);
            allData.Add(newObject);
            File.WriteAllText(_filePath, JsonConvert.SerializeObject(allData));
        }
        public Task CreateTask(T existingObjec)
        {
            return Task.Run(() => Create(existingObjec));
        }

        public void Delete(string code)
        {
            var json = File.ReadAllText(_filePath);
            var allData = JsonConvert.DeserializeObject<List<T>>(json);
            var indexToRemove = allData.FindIndex(i => i.ProductCode == code);
            allData.RemoveAt(indexToRemove);
            File.WriteAllText(_filePath, JsonConvert.SerializeObject(allData));
        }

        public Task DeleteTask(string code)
        {
            return Task.Run(() => Delete(code));
        }

        public void Update(T updatedData)
        {
            var json = File.ReadAllText(_filePath);
            var dataObjects = JsonConvert.DeserializeObject<List<T>>(json);

            foreach (var obj in dataObjects)
            {
                if (updatedData.ProductCode == obj.ProductCode)
                {
                    _dataUpdater.UpdateDataObject(obj, updatedData);
                }
            }

            File.WriteAllText(_filePath, JsonConvert.SerializeObject(dataObjects));
        }


        public Task UpdateTask(T updatedData)
        {
            return Task.Run(() => Update(updatedData));
        }
    }
}
