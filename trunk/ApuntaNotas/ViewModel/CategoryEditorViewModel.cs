using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using ApuntaNotas.Business;
using ApuntaNotas.Messages;
using ApuntaNotas.Model;
using ApuntaNotas.Utilities;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Samples.CustomControls;

namespace ApuntaNotas.ViewModel
{
    /// <summary>
    /// This is the VM for the Category options Window.
    /// This class contains properties that the Category options View can data bind to.
    /// </summary>
    public class CategoryEditorViewModel : ViewModelBase
    {
        private ICategoryRepository _categoryRepository;
        private ObservableCollection<Category> _categories;
        private Category _selectedCategory;
        private Category _defaultCategory;

        private List<Guid> _categoryIds;
        private List<Guid> _notesToDelete;
        private List<Guid> _notesToTrash;

        private bool _onCategoryUpdate;
        private bool _deleteNotesToo = true;
        private bool _isValid = true;
        private bool _defaultCatChanged = false;

        public RelayCommand NewCategoryCommand { get; private set; }
        public RelayCommand<object> DeleteCategoryCommand { get; private set; }
        public RelayCommand<bool?> DeleteNotesCommand { get; private set; }
        public RelayCommand AcceptCategoryCommand { get; private set; }
        public RelayCommand CategoryBeenSelected { get; private set; }
        public RelayCommand SelectBgColorCommand { get; private set; }
        public RelayCommand SelectFontColorCommand { get; private set; }
        public RelayCommand<Category> DefaultCategoryChangedCommand { get; private set; }

        /// <summary>
        /// Initializes a new instance of the CategoryEditorViewModel class.
        /// </summary>
        public CategoryEditorViewModel(ICategoryRepository categoryRepository)
        {
            CategoryRepository = categoryRepository;
            Categories = new ObservableCollection<Category>(_categoryRepository.FindAll());
            CategoryIds = new List<Guid>();
            NotesToDelete = new List<Guid>();
            NotesToTrash = new List<Guid>();
            
            NewCategoryCommand = new RelayCommand(NewCategory, () => !OnCategoryUpdate);
            DeleteCategoryCommand = new RelayCommand<object>(DeleteCategory, (noused) => OnCategoryUpdate && Categories.Count > 1);
            DeleteNotesCommand = new RelayCommand<bool?>((mark) => DeleteNotesToo = mark.Value, (noused) => OnCategoryUpdate);
            AcceptCategoryCommand = new RelayCommand(AcceptCategory, () => OnCategoryUpdate && IsValid);
            CategoryBeenSelected = new RelayCommand(() => OnCategoryUpdate = true);
            SelectBgColorCommand = new RelayCommand(SelectBgColor, () => OnCategoryUpdate);
            SelectFontColorCommand = new RelayCommand(SelectFontColor, () => OnCategoryUpdate);
            DefaultCategoryChangedCommand = new RelayCommand<Category>(DefaultCategoryChanged);

            // We register a default message containing a string (See SavingCatOptions) below.
            Messenger.Default.Register<string>(this, SavingCatOptions);

            // Default category
            _defaultCategory = Categories[0]; // The default one is always the first
            if (!_defaultCategory.IsDefault) // first time?
                _defaultCategory.IsDefault = true;
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
        /// Gets or sets the category repository.
        /// </summary>
        /// <value>The category repository.</value>
        public ICategoryRepository CategoryRepository
        {
            get { return _categoryRepository; }
            set
            {
                _categoryRepository = value;
                RaisePropertyChanged("CategoryRepository");
            }
        }

        /// <summary>
        /// Gets or sets the categories list.
        /// </summary>
        /// <value>The categories list.</value>
        public ObservableCollection<Category> Categories
        {
            get { return _categories; }
            set
            {
                _categories = value;
                RaisePropertyChanged("Categories");
            }
        }

        /// <summary>
        /// Gets or sets the category ids.
        /// </summary>
        /// <value>The category ids.</value>
        public List<Guid> CategoryIds
        {
            get { return _categoryIds; }
            set
            {
                _categoryIds = value;
                RaisePropertyChanged("CategoryIds");
            }
        }

        /// <summary>
        /// Gets or sets the notes to delete.
        /// </summary>
        /// <value>The notes to delete.</value>
        public List<Guid> NotesToDelete
        {
            get { return _notesToDelete; }
            set
            {
                _notesToDelete = value;
                RaisePropertyChanged("NotesToDelete");
            }
        }

        /// <summary>
        /// Gets or sets the notes to trash.
        /// </summary>
        /// <value>The notes to trash.</value>
        public List<Guid> NotesToTrash
        {
            get { return _notesToTrash; }
            set
            {
                _notesToTrash = value;
                RaisePropertyChanged("NotesToTrash");
            }
        }

        /// <summary>
        /// Gets or sets when we are updating a category or not
        /// </summary>
        /// <value><c>true</c> if [on category update]; otherwise, <c>false</c>.</value>
        public bool OnCategoryUpdate
        {
            get { return _onCategoryUpdate; }
            set
            {
                _onCategoryUpdate = value;
                RaisePropertyChanged("OnCategoryUpdate");
            }
        }

        /// <summary>
        /// Gets or sets if we want to Delete the notes too.
        /// </summary>
        /// <value><c>true</c> if [delete notes too]; otherwise, <c>false</c>.</value>
        public bool DeleteNotesToo
        {
            get { return _deleteNotesToo; }
            set
            {
                _deleteNotesToo = value;
                RaisePropertyChanged("DeleteNotesToo");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Category is valid
        /// </summary>
        /// <value><c>true</c> if this category is valid; otherwise, <c>false</c>.</value>
        public bool IsValid
        {
            get { return _isValid; }
            set
            {
                _isValid = value;
                RaisePropertyChanged("IsValid");
            }
        }

        /// <summary>
        /// Creates a new Category with a default name
        /// </summary>
        private void NewCategory()
        {
            var cat = new Category(Resources.Strings.Name, "#FFFFFF", "#000000");

            if (!_categories.Contains(cat))
                Categories.Add(cat);
        }

        /// <summary>
        /// Deletes the selected category.
        /// It adds the category's note to a list (depending on DeletesNotesToo value)
        /// </summary>
        /// <param name="category">The category.</param>
        private void DeleteCategory(object category)
        {
            var cat = category as Category;

            if (cat != null)
            {
                if (DeleteNotesToo)
                    NotesToDelete.Add(cat.Id);
                else
                    NotesToTrash.Add(cat.Id);

                SelectedCategory = null;
                if (_defaultCategory.Equals(cat))
                {
                    Categories.Remove(cat);
                    Categories[0].IsDefault = true;
                    _defaultCategory = Categories[0];
                }
                else
                {
                    Categories.Remove(cat);
                }
                
                _categoryRepository.SaveAll(Categories);
            }

        }

        /// <summary>
        /// Accepts the current category.
        /// </summary>
        private void AcceptCategory()
        {
            if (SelectedCategory != null)
                CategoryIds.Add(SelectedCategory.Id);

            SelectedCategory = null;
            _categoryRepository.SaveAll(Categories);
            OnCategoryUpdate = false;
        }

        /// <summary>
        /// Selects the color of the bg.
        /// </summary>
        private void SelectBgColor()
        {
            var cPicker = new ColorPickerDialog
                              {
                                  StartingColor = SelectedCategory.BackgroundColor.ToColor()
                              };

            bool? dialogResult = cPicker.ShowDialog();
            if (dialogResult != null && (bool)dialogResult)
            {
                SelectedCategory.BackgroundColor = cPicker.SelectedColor.ToString();
            }
        }

        /// <summary>
        /// Selects the color of the font.
        /// </summary>
        private void SelectFontColor()
        {
            var cPicker = new ColorPickerDialog
                              {
                                  StartingColor = SelectedCategory.FontColor.ToColor()
                              };

            bool? dialogResult = cPicker.ShowDialog();
            if (dialogResult != null && (bool)dialogResult)
            {
                SelectedCategory.FontColor = cPicker.SelectedColor.ToString();
            }
        }

        /// <summary>
        /// Here we change the category to be the default one.
        /// </summary>
        /// <param name="cat">The cat.</param>
        private void DefaultCategoryChanged(Category cat)
        {
            _defaultCategory.IsDefault = false;
            cat.IsDefault = true;
            _defaultCategory = cat;
            _defaultCatChanged = true;
        }

        /// <summary>
        /// Receives a message through a messenger and
        /// sends through the messenger all the list
        /// with the changes
        /// </summary>
        /// <param name="message">The messenger message.</param>
        private void SavingCatOptions(string message)
        {
            if (message.Equals("ClosingWindow"))
            {
                if (_defaultCatChanged)
                {
                    Categories.Remove(_defaultCategory);
                    Categories.Insert(0, _defaultCategory);
                }
                AcceptCategory();

                Messenger.Default.Send(new CategoryEditorChangesMessage
                                           {
                                               NotesToDelete = _notesToDelete,
                                               NotesToTrash = _notesToTrash,
                                               CategoriesIds = _categoryIds
                                           });
            }
        }
    }
}