using OfferApp.Core.Entities;
using OfferApp.Core.Repositories;
using System.Reflection;
using System.Runtime.Serialization;

namespace OfferApp.Infrastructure.Repositories
{
    internal sealed class XmlRepository<T> : IRepository<T>
        where T : BaseEntity
    {
        private readonly string _filePathEntities;
        // XmlSerializer nie jest w stanie serializować i deserializować prywatne pola
        // Zobacz
        // https://stackoverflow.com/questions/54573370/how-to-deserialize-an-xml-string-into-an-object-that-has-properties-with-private
        private readonly DataContractSerializer _serializer;

        public XmlRepository()
        {
            var type = typeof(T);
            // ścieżka do głównego folder ConsoleApp
            _filePathEntities = Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.FullName + Path.DirectorySeparatorChar + type.Name + ".xml";
            _serializer = new DataContractSerializer(typeof(List<T>));

            if (!File.Exists(_filePathEntities))
            {
                SaveFile(new List<T>());
            }
        }

        public Task<int> Add(T entity)
        {
            var list = OpenFile();
            var id = list.LastOrDefault()?.Id + 1 ?? 1;
            SetId(entity, id);
            list.Add(entity);
            SaveFile(list);
            return Task.FromResult(id);
        }

        public Task Delete(T entity)
        {
            var list = OpenFile();
            var entityToDelete = list.FirstOrDefault(e => e.Id == entity.Id);
            if (entityToDelete is null)
            {
                return Task.CompletedTask;
            }

            list.Remove(entityToDelete);
            SaveFile(list);
            return Task.CompletedTask;
        }

        public Task<T?> Get(int id)
        {
            var list = OpenFile();
            var entityToReturn = list.FirstOrDefault(e => e.Id == id);
            return Task.FromResult<T?>(entityToReturn);
        }

        public async Task<IReadOnlyList<T>> GetAll()
        {
            await Task.CompletedTask;
            return OpenFile();
        }

        public async Task<bool> Update(T entity)
        {
            await Task.CompletedTask;
            var list = OpenFile();
            var entityToUpdateIndex = list.FindIndex(e => e.Id == entity.Id);
            if (entityToUpdateIndex == -1)
            {
                return false;
            }

            list[entityToUpdateIndex] = entity;
            SaveFile(list);
            return true;
        }

        private static void SetId(T entity, int value)
        {
            var type = typeof(T);
            var field = type?.BaseType?.GetField($"<{nameof(BaseEntity.Id)}>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance);
            field?.SetValue(entity, value);
        }

        private List<T> OpenFile()
        {
            using FileStream fileStreamOpen = new(path: _filePathEntities, FileMode.Open);
            var list = (List<T>)(_serializer.ReadObject(fileStreamOpen) ?? new List<T>());
            return list;
        }

        private void SaveFile(List<T> entities)
        {
            using FileStream fileStreamSave = new(path: _filePathEntities, FileMode.Create);
            _serializer.WriteObject(fileStreamSave, entities);
        }
    }
}
