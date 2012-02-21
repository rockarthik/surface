using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using ApuntaNotas.Business;
using ApuntaNotas.Model;

namespace ApuntaNotas.Business
{
    /// <summary>
    /// This class manages a note repository.
    /// The notes will be saved using serialization.
    /// </summary>
    public class NoteRepository : INoteRepository
    {
        private IList<Note> _noteStore;
        private readonly string _dataFile;

        /// <summary>
        /// Initializes a new instance of the <see cref="NoteRepository"/> class.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        public NoteRepository(string fileName)
        {
            fileName = string.Concat(fileName + ".an");
            _dataFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

            Deserialize();
        }

        /// <summary>
        /// Saves the specified note.
        /// </summary>
        /// <param name="note">The note.</param>
        public void Save(Note note)
        {
            if (!_noteStore.Contains(note))
                _noteStore.Add(note);

            Serialize();
        }

        /// <summary>
        /// Deletes the specified note.
        /// </summary>
        /// <param name="note">The note.</param>
        public void Delete(Note note)
        {
            _noteStore.Remove(note);

            Serialize();
        }

        /// <summary>
        /// Resets the repository.
        /// </summary>
        public void RepositoryReset()
        {
            File.Delete(_dataFile);
            _noteStore = new List<Note>();
        }

        /// <summary>
        /// Returns the entire repository.
        /// </summary>
        /// <returns>A List with all Notes</returns>
        public IList<Note> FindAll()
        {
            return _noteStore;
        }

        /// <summary>
        /// Serializes all the notes to a file.
        /// </summary>
        private void Serialize()
        {
            using (var stream = File.Open(_dataFile, FileMode.OpenOrCreate))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, _noteStore);
            }
        }

        /// <summary>
        /// Deserializes all notes or creates an empty List.
        /// </summary>
        private void Deserialize()
        {
            if (File.Exists(_dataFile))
                using (var stream = File.Open(_dataFile, FileMode.Open))
                {
                    var formatter = new BinaryFormatter();
                    _noteStore = (IList<Note>) formatter.Deserialize(stream);
                }
            else
                _noteStore = new List<Note>();
        }
    }
}
