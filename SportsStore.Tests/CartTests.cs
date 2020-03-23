using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SportsStore.Tests
{
    public class CartTests
    {
        [Fact]
        public void CartLine_Can_Set_Quantity()
        {
            //Arrange
            CartLine line = new CartLine { Quantity = 5 };
            //Act
            line.Quantity = 4;
            //Assert
            Assert.True(line.Quantity == 4);
        }

        [Fact]
        public void Cart_Can_Add_New_Line()
        {
            //Arrange
            Product p1 = new Product { Name = "P1", Price = 5, ProductID = 1 };
            Product p2 = new Product { Name = "P2", Price = 4, ProductID = 2 };

            Cart cart = new Cart();
            cart.AddItem(p1, 1);
            cart.AddItem(p2, 3);          

            //Act
            CartLine[] lines = cart.Lines.ToArray();

            //Assert
            Assert.True(lines.Length == 2);
            Assert.True(lines[0].Product == p1);
            Assert.True(lines[1].Product == p2);
        }

        [Fact]
        public void Can_Add_Quantity_For_Exising_Line()
        {
            //Arrange
            Product p1 = new Product { Name = "P1", Price = 5, ProductID = 1 };
            Product p2 = new Product { Name = "P2", Price = 4, ProductID = 2 };

            Cart cart = new Cart();
            cart.AddItem(p1, 1);
            cart.AddItem(p2, 3);
            cart.AddItem(p2, 3);
            

            //Act
             var target = cart.Lines.Select(l => new { l.Quantity, l.CartLineID }).OrderBy(l => l.CartLineID).ToArray();

            //Assert
            Assert.True(target.Length == 2);
            Assert.True(target[0].Quantity == 1);
            Assert.True(target[1].Quantity == 6);
            
        }

        [Fact]
        public void Can_Remove_Line_Item()
        {
            //Arrange
            Product p1 = new Product { Name = "P1", Price = 5, ProductID = 1 };
            Product p2 = new Product { Name = "P2", Price = 4, ProductID = 2 };

            Cart cart = new Cart();
            cart.AddItem(p1, 1);
            cart.AddItem(p2, 3);
            cart.AddItem(p2, 3);

            //Act
            cart.RemoveLine(p1);
           var res = cart.Lines;

            //Assert
            Assert.True(res.Count() == 1);
        }

        [Fact]
        public void Can_Calculate_Total()
        {
            //Arrange
            Product p1 = new Product { Name = "P1", Price = 5, ProductID = 1 };
            Product p2 = new Product { Name = "P2", Price = 4, ProductID = 2 };

            Cart cart = new Cart();
            cart.AddItem(p1, 1);
            cart.AddItem(p2, 3);

            //Act
            decimal result = cart.ComputeTotalValue();

            //Assert
            Assert.Equal(17, result);

        }

        [Fact]
        public void Can_Clear_Line_Items()
        {
            //Arrange
            Product p1 = new Product { Name = "P1", Price = 5, ProductID = 1 };
            Product p2 = new Product { Name = "P2", Price = 4, ProductID = 2 };

            Cart cart = new Cart();
            cart.AddItem(p1, 1);
            cart.AddItem(p2, 3);

            //Act
            cart.Clear();

            //Assert

            Assert.True(cart.Lines.Count() == 0);

        }
    }
}
