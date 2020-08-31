using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SportsStore.WebUI.Infrastructure.Abstract;
using SportsStore.WebUI.Models;

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

		[TestMethod]
		public void Can_Save_Valid_Changes()
		{
			Mock<IProductRepository> mock = new Mock<IProductRepository>();
			AdminController target = new AdminController(mock.Object);
			Product product = new Product { Name = "Test" };

			ActionResult result = target.Edit(product);

			mock.Verify(m => m.SaveProduct(product));

			Assert.IsNotInstanceOfType(result, typeof(ViewResult));
		}

		[TestMethod]
		public void Can_Login_With_Valid_Credentials()
		{
			Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
			mock.Setup(m => m.Authenticate("admin", "secret")).Returns(true);

			LoginViewModel model = new LoginViewModel { UserName = "admin", Password = "secret" };

			AccountController target = new AccountController(mock.Object);

			ActionResult result = target.Login(model, "/MyUrl");

			Assert.IsInstanceOfType(result, typeof(RedirectResult));
			Assert.AreEqual("/MyUrl", ((RedirectResult)result).Url);
		}
	}
}
