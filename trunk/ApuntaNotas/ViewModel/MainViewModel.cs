using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ApuntaNotas.Business;
using ApuntaNotas.Messages;
using ApuntaNotas.Model;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace ApuntaNotas.ViewModel
{
    /// <summary>
    /// This is the VM for the Main Window.
    /// This class contains properties that the main View can data bind to.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly INoteRepository _noteRepository;
        private readonly ICategoryRepository _categoryRepository;
        private ObservableCollection<Note> _notes;
        private ObservableCollection<Category> _categories;

        private Category _selectedCategory;
        private Note _actualNote;
        private Category Trash { get; set; }

        public RelayCommand AddNoteCommand { get; private set; }
        public RelayCommand<Note> EditNoteCommand { get; private set; }
        public RelayCommand<Note> DeleteNoteCommand { get; private set; }
        public RelayCommand DeleteAllNotesCommand { get; private set; }
        public RelayCommand CategoryOptionsCommand { get; private set; }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(INoteRepository noteRepository, ICategoryRepository categoryRepository)
        {
            _noteRepository = noteRepository;
            _categoryRepository = categoryRepository;

            Notes = new ObservableCollection<Note>(_noteRepository.FindAll());
            Categories = new ObservableCollection<Category>(_categoryRepository.FindAll());

            // Is there categories list empty?
            if (Categories.Count == 0)
            {
                // In this case, I will create a default category with a welcome note
                var cat = new Category(Resources.Strings.GeneralCat, "#33CC00", "#FFFFFF");
                Categories.Add(cat);
                _categoryRepository.SaveAll(Categories);

                var note = new Note(Resources.Strings.WelcomeMessage, cat);
                Notes.Add(note);
                _noteRepository.Save(note);
            }

            ActualNote = new Note();
            SelectedCategory = _categories[0]; // We need to this for Category's ComboBox sake.
            Trash = new Category("Trash", "#f8f8f8", "#777777");
            
            AddNoteCommand = new RelayCommand(AddNote, CanAddNote);
            EditNoteCommand = new RelayCommand<Note>(EditNote);
            DeleteNoteCommand = new RelayCommand<Note>(DeleteNote);
            DeleteAllNotesCommand = new RelayCommand(DeleteAllNotes);
            CategoryOptionsCommand = new RelayCommand(OpenCategoryOptions);
            
            // We expect a message with some lists with changes.
            Messenger.Default.Register<CategoryEditorChangesMessage>(this, MakingNewCatChanges);
        }

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        /// <value>The notes.</value>
        public ObservableCollection<Note> Notes
        {
            get
            {
                return _notes;
            }
            set
            {
                _notes = value;
                RaisePropertyChanged("Notes");
            }
        }

        /// <summary>
        /// Gets or sets the categories.
        /// </summary>
        /// <value>The categories.</value>
        public ObservableCollection<Category> Categories
        {
            get
            {
                return _categories;
            }
            set
            {
                _categories = value;
                RaisePropertyChanged("Categories");
            }
        }

        /// <summary>
        /// Gets or sets the selected category.
        /// </summary>
        /// <value>The selected category.</value>
        public Category SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                _selectedCategory = value;
                RaisePropertyChanged("SelectedCategory");
            }
        }

        /// <summary>
        /// Gets or sets the actual note.
        /// </summary>
        /// <value>The actual note.</value>
        public Note ActualNote
        {
            get { return _actualNote; }
            set
            {
                _actualNote = value;
                RaisePropertyChanged("ActualNote");
            }
        }

        /// <summary>
        /// Adds a note to the list and repo.
        /// </summary>
        private void AddNote()
        {
            if (ActualNote.Content == string.Empty) return; // We don't want an empty note :)

            ActualNote.Category = SelectedCategory;

            if (!Notes.Contains(ActualNote))
                Notes.Add(ActualNote);
            _noteRepository.Save(ActualNote);

            ActualNote = new Note();

            if (SelectedCategory.Name == "Trash") // We don't want the Trash selectionable
                SelectedCategory = Categories[0];
        }

        /// <summary>
        /// Determines whether you can create a note.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if you can; otherwise, <c>false</c>.
        /// </returns>
        private bool CanAddNote()
        {
            return SelectedCategory != null;
        }

        /// <summary>
        /// Edits the selected note.
        /// </summary>
        /// <param name="other">The selected note.</param>
        private void EditNote(Note other)
        {
            ActualNote = other;
            SelectedCategory = other.Category;
        }

        /// <summary>
        /// Deletes the selected note.
        /// </summary>
        /// <param name="other">The selected note.</param>
        private void DeleteNote(Note other)
        {
            if (other == null) return;

            Notes.Remove(other);
            _noteRepository.Delete(other);
        }

        /// <summary>
        /// Deletes all notes.
        /// </summary>
        private void DeleteAllNotes()
        {
            var msgBox = new Views.DeleteAllMsgBox();
            msgBox.ShowDialog();
            if (msgBox.Response)
            {
                _noteRepository.RepositoryReset();
                Notes = new ObservableCollection<Note>();
            }
        }

        /// <summary>
        /// Opens the category options window.
        /// </summary>
        private void OpenCategoryOptions()
        {
            var catOpts = new Views.CategoryEditorView();
            catOpts.ShowDialog();
        }

        /// <summary>
        /// Takes the 3 lists with changes and makes the correct changes.
        /// </summary>
        /// <param name="lists">The list of lists :P.</param>
        private void MakingNewCatChanges(CategoryEditorChangesMessage changes)
        {
            UpdatingCatAndNotes(changes.CategoriesIds);
            if (changes.NotesToDelete.Count > 0)
                DeleteNotesWithoutCategory(changes.NotesToDelete);
            if (changes.NotesToTrash.Count > 0)
                MoveNotesToTrash(changes.NotesToTrash);

        }

        /// <summary>
        /// Updatings the note's categories when a category change.
        /// </summary>
        /// <param name="list">The list.</param>
        private void UpdatingCatAndNotes(List<Guid> list)
        {
            // Reloading the Category list
            Categories = new ObservableCollection<Category>(_categoryRepository.FindAll());
            // We need a category selected for the combobox
            SelectedCategory = Categories[0];

            // Now we update the notes (changing it category)
            if (list.Count == 0) return;
            foreach (var id in list)
            {
                foreach (var note in Notes.Where(note => note.Category.Id == id))
                {
                    note.Category = _categoryRepository.GetById(id);
                    _noteRepository.Save(note);
                }
            }
        }

        /// <summary>
        /// Deletes the notes without category.
        /// </summary>
        /// <param name="list">The list.</param>
        private void DeleteNotesWithoutCategory(List<Guid> list)
        {
            // We need a temporary collection to save the notes we need to delete

            var temp = list.SelectMany(id => Notes.Where(note => note.Category.Id == id)).ToList();

            foreach (var note in temp)
            {
                Notes.Remove(note);
                _noteRepository.Delete(note);
            }
        }

        /// <summary>
        /// Moves the notes to trash.
        /// </summary>
        /// <param name="list">The list.</param>
        private void MoveNotesToTrash(List<Guid> list)
        {
            // looping through every note that we need to move to the Trash
            foreach (var note in list.SelectMany(guid => Notes.Where(note => note.Category.Id == guid)))
            {
                note.Category = Trash;
                _noteRepository.Save(note);
            }
        }

        /// <summary>
        /// Changes the note category.
        /// </summary>
        /// <param name="theNote">The note.</param>
        /// <param name="newCategory">The new category.</param>
        public void ChangeNoteCategory(Note theNote, Category newCategory)
        {
            // Selecting the right note
            var note = Notes.Single(n => n == theNote);
            note.Category = newCategory;
            _noteRepository.Save(note);
        }
    }
}