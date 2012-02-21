using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using ApuntaNotas.Business;
using ApuntaNotas.Messages;
using ApuntaNotas.Model;
using ApuntaNotas.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ApuntaNotas.Tests.ViewModelTests
{
    [TestClass]
    public class MainViewModelTests
    {
        [TestMethod]
        public void ShouldAddANewNote()
        {
            var noteMock = new Mock<INoteRepository>();
            var categoryMock = new Mock<ICategoryRepository>();

            categoryMock.Setup(rep => rep.FindAll()).Returns(new List<Category> { new Category("Dummy", "#123123123", "#123123123") });
            noteMock.Setup(rep => rep.FindAll()).Returns(new List<Note>());

            var viewModel = new MainViewModel(noteMock.Object, categoryMock.Object);

            var note = new Note("A note", viewModel.Categories.First());

            viewModel.ActualNote = note;

            viewModel.AddNoteCommand.Execute(null);

            Assert.AreEqual(note, viewModel.Notes.Last());
        }

        [TestMethod]
        public void YouCantAddTheSameNoteTwice()
        {
            var noteMock = new Mock<INoteRepository>();
            var categoryMock = new Mock<ICategoryRepository>();

            categoryMock.Setup(rep => rep.FindAll()).Returns(new List<Category> { new Category("Dummy", "#123123123", "#123123123") });
            noteMock.Setup(rep => rep.FindAll()).Returns(new List<Note>());

            var viewModel = new MainViewModel(noteMock.Object, categoryMock.Object);

            var note = new Note("A note", viewModel.Categories.First());

            viewModel.ActualNote = note;

            viewModel.AddNoteCommand.Execute(null);
            var count = viewModel.Notes.Count;

            viewModel.ActualNote = note;
            viewModel.AddNoteCommand.Execute(null);

            Assert.AreEqual(count, viewModel.Notes.Count);
        }

        [TestMethod]
        public void DeletingACategoryWithoutNotes()
        {
            // Arrange: We create the repo's mocks
            var noteMock = new Mock<INoteRepository>();
            var categoryMock = new Mock<ICategoryRepository>();

            var category = new Category("Dummy", "#123123123", "#123123123");

            categoryMock.Setup(rep => rep.FindAll()).Returns(new List<Category> { category });
            noteMock.Setup(rep => rep.FindAll()).Returns(new List<Note> {new Note("Note", category)});

            // Act: we create a viewModel's instance, a list with the category to delete and we send a message
            var viewModel = new MainViewModel(noteMock.Object, categoryMock.Object);
            var notesToTrash = new List<Guid> {category.Id};
            var categoryEditorChanges = new CategoryEditorChangesMessage
                                            {
                                                NotesToTrash = notesToTrash
                                            };
            Messenger.Default.Send(categoryEditorChanges);

            // Assert: and the note's category should be trash
            viewModel.Notes.FirstOrDefault(n => n.Content == "Note").Category.Name.ShouldEqual("Trash");
        }

        [TestMethod]
        public void DeletingACategoryWithItsNotes()
        {
            // Arrange: We create the repo's mocks
            var noteMock = new Mock<INoteRepository>();
            var categoryMock = new Mock<ICategoryRepository>();

            var category = new Category("Dummy", "#123123123", "#123123123");

            categoryMock.Setup(rep => rep.FindAll()).Returns(new List<Category> { category });
            noteMock.Setup(rep => rep.FindAll()).Returns(new List<Note> { new Note("Note", category) });

            // Act: we create a viewModel's instance, a list with the category to delete and we send a message
            var viewModel = new MainViewModel(noteMock.Object, categoryMock.Object);
            var notesToDelete = new List<Guid> { category.Id };
            var categoryEditorChanges = new CategoryEditorChangesMessage
            {
                NotesToDelete = notesToDelete
            };
            Messenger.Default.Send(categoryEditorChanges);

            // Assert: and the note is gone
            viewModel.Notes.Count.ShouldEqual(0);
        }

        [TestMethod]
        public void ChangingCategoryNameChangeNoteCategory()
        {
            // Arrange: We create the repo's mocks
            var noteMock = new Mock<INoteRepository>();
            var categoryMock = new Mock<ICategoryRepository>();

            var category = new Category("Dummy", "#123123123", "#123123123");

            categoryMock.Setup(rep => rep.FindAll()).Returns(new List<Category> { category });
            categoryMock.Setup(rep => rep.GetById(category.Id)).Returns(category);
            noteMock.Setup(rep => rep.FindAll()).Returns(new List<Note> { new Note("Note", category) });

            // Act: we create a viewModel's instance, a list with the new category changes and we send a mesage
            var viewModel = new MainViewModel(noteMock.Object, categoryMock.Object);

            category.Name = "Party";
            var categoryId = new List<Guid> { category.Id };
            var categoryEditorChanges = new CategoryEditorChangesMessage
            {
                CategoriesIds = categoryId
            };
            Messenger.Default.Send(categoryEditorChanges);

            // Assert: and the category and the note have the new name
            viewModel.Categories.First(c => c.Id == category.Id).Name.ShouldEqual("Party");
            viewModel.Notes.First(n => n.Content == "Note").Category.Name.ShouldEqual("Party");
        }
    }
}
