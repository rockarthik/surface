using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using ApuntaNotas.Business;
using ApuntaNotas.Model;
using ApuntaNotas.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ApuntaNotas.Tests.ViewModelTests
{
    [TestClass]
    public class CategoryEditorViewModelTests
    {
        [TestMethod]
        public void ShouldCreateANewCategory()
        {
            var mockCategoryRepository = new Mock<ICategoryRepository>();
            mockCategoryRepository.Setup(rep => rep.FindAll()).Returns(new List<Category> { new Category("Dummy", "#123123123", "#123123123") });

            var viewModel = new CategoryEditorViewModel(mockCategoryRepository.Object);
            viewModel.NewCategoryCommand.Execute(null);

            Assert.AreEqual(viewModel.Categories.Count, 2);
            Assert.AreEqual(viewModel.Categories[1].Name, Resources.Strings.Name);
        }

        [TestMethod]
        public void CanDeleteACategory()
        {
            var mockCategoryRepository = new Mock<ICategoryRepository>();
            mockCategoryRepository.Setup(rep => rep.FindAll()).Returns(new List<Category> { new Category("Dummy", "#123123123", "#123123123") });

            var viewModel = new CategoryEditorViewModel(mockCategoryRepository.Object);

            var cat = new Category("Prueba", "#ffaaffaa", "#aaffaaff");
            viewModel.Categories.Add(cat);

            viewModel.DeleteCategoryCommand.Execute(cat);

            Assert.AreEqual(viewModel.Categories.Count, 1);
            Assert.IsFalse(viewModel.Categories.Contains(cat));
            mockCategoryRepository.Verify(x => x.SaveAll(viewModel.Categories));
        }

        [TestMethod]
        public void ShouldSaveCategoryChanges()
        {
            var mockCategoryRepository = new Mock<ICategoryRepository>();
            mockCategoryRepository.Setup(rep => rep.FindAll()).Returns(new List<Category> { new Category("Dummy", "#123123123", "#123123123") });

            var viewModel = new CategoryEditorViewModel(mockCategoryRepository.Object);

            // We create a new category
            viewModel.NewCategoryCommand.Execute(null);

            // we change it
            var cat = viewModel.Categories[1];
            cat.Name = "Car";

            // We put the cat as the selected one
            viewModel.SelectedCategory = cat;

            // And save it
            viewModel.AcceptCategoryCommand.Execute(null);

            Assert.AreEqual(viewModel.Categories[1], cat);
            mockCategoryRepository.Verify(x => x.SaveAll(viewModel.Categories));
        }

        [TestMethod]
        public void CanChangeDefaultCategory()
        {
            var mockCategoryRepository = new Mock<ICategoryRepository>();
            mockCategoryRepository.Setup(rep => rep.FindAll()).Returns(new List<Category> { new Category("Dummy", "#123123123", "#123123123") });

            var viewModel = new CategoryEditorViewModel(mockCategoryRepository.Object);
            viewModel.NewCategoryCommand.Execute(null);

            var cat = viewModel.Categories.Last();
            viewModel.DefaultCategoryChangedCommand.Execute(cat);

            Assert.IsTrue(cat.IsDefault == true);
        }
    }
}
