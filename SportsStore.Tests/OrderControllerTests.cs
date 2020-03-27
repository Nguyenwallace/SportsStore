using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SportsStore.Tests
{
    public class OrderControllerTests
    {
        [Fact]
        public  void Can_Not_Checkout_Empty_Cart()
        {
            //Arrange
            Mock<IOrderRepository> repoMock = new Mock<IOrderRepository>();

            //Arrange empty Cart
            Cart cart = new Cart();

            //Arrange a OrderController
            OrderController orderCon = new OrderController(repoMock.Object, cart);

            //Arrange Order
            Order order = new Order();

            //Act
            ViewResult target = orderCon.Checkout(order) as ViewResult;

            //Assert - the Order hasnt been saved
            repoMock.Verify(m => m.SaveOrder(order), Times.Never);
            //Assert - Check ModelState IsValid
            Assert.False(target.ViewData.ModelState.IsValid);
            //Assert - Check return to the default View
            Assert.True(string.IsNullOrEmpty(target.ViewName));
               
        }

        [Fact]
        public void Can_Not_Checkout_Invalid_Shipping_Info()
        {
            //Arrange 
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);
            Order order = new Order();

            //Act
            OrderController target = new OrderController(mock.Object, cart);

            target.ModelState.AddModelError("error", "error");
            ViewResult result = target.Checkout(order) as ViewResult;
            
            // Arrange - add an error to the model
            

            //Assert - ModelState IsValid
            Assert.False(target.ViewData.ModelState.IsValid);

            //Assert - Return default View 
            Assert.True(string.IsNullOrEmpty(result.ViewName));

            //Assert - Not Save order
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);

        }

        [Fact]
        public void Can_Checkout_And_Submit_Order()
        {
            // Arrange - create a mock order repository
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
            // Arrange - create a cart with one item
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);
            // Arrange - create an instance of the controller
            OrderController target = new OrderController(mock.Object, cart);
            // Act - try to checkout
            RedirectToActionResult result =
            target.Checkout(new Order()) as RedirectToActionResult;
            // Assert - check that the order has been stored
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Once);
            // Assert - check that the method is redirecting to the Completed action
            Assert.Equal("Completed", result.ActionName);
        }
    }
}
