using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SportsStore.UnitTests
{
	[TestClass]
	public class AdminTest
	{
		[TestMethod]
		public void Index_Contains_All_Products()
		{
			//Create mock repository
			Mock<IProductRepository> mock = new Mock<IProductRepository>();
			mock.Setup(m => m.Products).Returns(new Product[]
			{
				new Product {ProductID = 1, Name = "P1"},
				new Product {ProductID = 2, Name = "P2"},
				new Product {ProductID = 3, Name = "P3"},
			});

			//Arange
			AdminController target = new AdminController(mock.Object);

			//Action
			Product[] result = ((IEnumerable<Product>)target.Index().ViewData.Model).ToArray();

			//Assert
			Assert.AreEqual(result.Length, 3);
			Assert.AreEqual("P1", result[0].Name);
			Assert.AreEqual("P3", result[2].Name);
		} 

		[TestMethod]
		public void Can_Edit_Product()
		{
			//Arange
			Mock<IProductRepository> mock = new Mock<IProductRepository>();
			mock.Setup(m => m.Products).Returns(new Product[]
			{
				new Product {ProductID = 1, Name = "P1"},
				new Product {ProductID = 2, Name = "P2"},
				new Product {ProductID = 3, Name = "P3"},
			});

			AdminController target = new AdminController(mock.Object);

			//Act
			Product p1 = target.Edit(1).ViewData.Model as Product;
			Product p2 = target.Edit(2).ViewData.Model as Product;
			Product p3 = target.Edit(3).ViewData.Model as Product;

			//Assert
			Assert.AreEqual(1, p1.ProductID);
			Assert.AreEqual(2, p2.ProductID);
			Assert.AreEqual(3, p3.ProductID);
		}
	}
}
