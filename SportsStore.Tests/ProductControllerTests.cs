using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using SportsStore.Models;
using System.Linq;
using Xunit;
using SportsStore.Controllers;
using SportsStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace SportsStore.Tests
{
    public class ProductControllerTests
    {
        [Fact]
        public void Can_Send_Pagination_ModelView()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns( (new Product[] {
                new Product { Name = "P1", Price = 20 },
                new Product { Name = "P2", Price = 21 },
                new Product { Name = "P3", Price = 23 },
                new Product { Name = "P4", Price = 24 },
                new Product { Name = "P5", Price = 25 }               
            }).AsQueryable<Product>());

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            //Act 
            var results = controller.List(null, 2).ViewData.Model as ProductsListViewModel;
            //Product[] arrayProduct = results.ToArray();

            var products = results.Products;
            var pagingInfo = results.PagingInfo;

            //Assert - check if that return 2 item

            Assert.True(pagingInfo.TotalItems == 5);
            Assert.True(pagingInfo.CurrentPage == 2);
            Assert.Equal("2", pagingInfo.TotalPages.ToString());
            Assert.True(pagingInfo.ItemsPerPage == 3);

        }

        [Fact]
        public void Can_Paginate()
        {
            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[] {
            new Product {ProductID = 1, Name = "P1"},
            new Product {ProductID = 2, Name = "P2"},
            new Product {ProductID = 3, Name = "P3"},
            new Product {ProductID = 4, Name = "P4"},
            new Product {ProductID = 5, Name = "P5"}
            }).AsQueryable<Product>());
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;
            // Act
            ProductsListViewModel result =
            controller.List(null, 2).ViewData.Model as ProductsListViewModel;
            Product[] prodArray = result.Products.ToArray();

            // Assert
            Assert.True(prodArray.Length == 2);
            Assert.Equal("P4", prodArray[0].Name);
            Assert.Equal("P5", prodArray[1].Name);
        }

        [Fact]
        public void Can_Filter_Products_By_Category()
        {
            //Arrange
            Mock<IProductRepository> product = new Mock<IProductRepository>();
            product.Setup(m => m.Products).Returns((new Product[] {
                new Product { ProductID = 1, Name = "P1", Category ="Cat1" },
                new Product { ProductID = 2, Name = "P2", Category = "Cat2" },
                new Product { ProductID = 3, Name = "P3", Category = "Cat1" },
                new Product { ProductID = 4, Name = "P4", Category = "Cat2" },
                new Product { ProductID = 5, Name = "P5", Category = "Cat3" }
            }).AsQueryable<Product>());

            //Act
            var controller = new ProductController(product.Object);
            ViewResult view = controller.List("Cat1", 1);

            ProductsListViewModel result = view.ViewData.Model as ProductsListViewModel;
            var productArr = result.Products.ToArray();
            //Assert
            Assert.Equal("Cat1", productArr[1].Category);
            Assert.True(productArr.Length == 2);
        }

        [Fact]
        public void Generate_Category_Product_Count()
        {
            //Arrange
            Mock<IProductRepository> repo = new Mock<IProductRepository>();
            repo.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1", Category = "Cat1"},
                new Product {ProductID = 2, Name = "P2", Category = "Cat2"},
                new Product {ProductID = 3, Name = "P3", Category = "Cat1"},
                new Product {ProductID = 4, Name = "P4", Category = "Cat2"},
                new Product {ProductID = 5, Name = "P5", Category = "Cat3"}

            }.AsQueryable<Product>());

            ProductController target = new ProductController(repo.Object);
            //Act
            //ViewResult view = target.List("Cat1", 1);
            //var result = view.ViewData.Model as ProductsListViewModel;
            //PagingInfo page = result.PagingInfo;

            Func<ViewResult, ProductsListViewModel> GetModel = result =>
            result?.ViewData?.Model as ProductsListViewModel;

            int? res1 = GetModel(target.List("Cat1", 1))?.PagingInfo.TotalItems;
            int? res2 = GetModel(target.List("Cat2", 1))?.PagingInfo.TotalItems;
            int? res3 = GetModel(target.List("Cat3", 1))?.PagingInfo.TotalItems;
            int? resAll = GetModel(target.List(null))?.PagingInfo.TotalItems;

            //Assert

            Assert.True(res1 == 2);
            Assert.True(res2 == 2);
            Assert.True(res3 == 1);
            Assert.True(resAll == 5);
        }

        
    }
}
