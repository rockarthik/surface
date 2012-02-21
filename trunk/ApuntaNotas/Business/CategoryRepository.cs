using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using ApuntaNotas.Model;

namespace ApuntaNotas.Business
{
    /// <summary>
    /// This class manages a category repository
    /// </summary>
    public class CategoryRepository : ICategoryRepository
    {
        private IList<Category> _categoryStore;
        private readonly string _dataFile;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryRepository"/> class.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        public CategoryRepository(string fileName)
        {
            fileName = string.Concat(fileName + ".an");
            _dataFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

            Deserialize();
        }

        /// <summary>
        /// Replace the old cotegories list with a new one.
        /// </summary>
        /// <param name="categories">The categories.</param>
        public void SaveAll(IList<Category> categories)
        {
            _categoryStore = categories;

            Serialize();
        }

        /// <summary>
        /// Returns the entire repository.
        /// </summary>
        /// <returns>A List with all categories</returns>
        public IList<Category> FindAll()
        {
            Deserialize();

            return _categoryStore;
        }

        /// <summary>
        /// Looking a category by ID
        /// </summary>
        /// <param name="id">The category id.</param>
        /// <returns>A category or null</returns>
        public Category GetById(Guid id)
        {
            return _categoryStore.SingleOrDefault(c => c.Id == id);
        }

        /// <summary>
        /// Resets the repository.
        /// </summary>
        public void RepositoryReset()
        {
            File.Delete(_dataFile);
            _categoryStore = new List<Category>();
        }

        /// <summary>
        /// Serializes all the categories to a file.
        /// </summary>
        private void Serialize()
        {
            using (var stream = File.Open(_dataFile, FileMode.OpenOrCreate))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, _categoryStore);
            }
        }

        /// <summary>
        /// Deserializes all categories or creates an empty List.
        /// </summary>
        private void Deserialize()
        {
            if (File.Exists(_dataFile))
                using (var stream = File.Open(_dataFile, FileMode.Open))
                {
                    var formatter = new BinaryFormatter();
                    _categoryStore = (IList<Category>) formatter.Deserialize(stream);
                }
            else
                _categoryStore = new List<Category>();
        }
    }
}
