using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.Domain.Entities;
using System.Linq;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.WebUI.Controllers;
using System.Web.Mvc;
using SportsStore.WebUI.Models;

namespace SportsStore.UnitTests
{
	[TestClass]
	public class CartTest
	{
		[TestMethod]
		public void Can_Add_New_Item()
		{
			Product p1 = new Product { ProductID = 1, Name = "P1" };
			Product p2 = new Product { ProductID = 2, Name = "P2" };
			Product p3 = new Product { ProductID = 1, Name = "P3" };
			Cart target = new Cart();
			target.AddItem(p1, 1);
			target.AddItem(p2, 1);
			target.AddItem(p3, 1);

			CartLine[] result = target.Lines().ToArray();

			Assert.AreEqual(2, result.Length);
			Assert.AreEqual(p1, result[0].Product);
			Assert.AreEqual(p2, result[1].Product);
			Assert.AreEqual(2, result[0].Quantity);
		}

		[TestMethod]
		public void Calculate_Cart_Total()
		{
			Product p1 = new Product { ProductID = 1, Name = "P1", Price = 100M };
			Product p2 = new Product { ProductID = 2, Name = "P2", Price = 50M};
			Cart target = new Cart();
			target.AddItem(p1, 1);
			target.AddItem(p2, 1);
			target.AddItem(p1, 3);

			decimal result = target.ComputeTotalValue();

			Assert.AreEqual(450M, result);
		}

		[TestMethod]
		public void Can_Clear_Cart()
		{
			Product p1 = new Product { ProductID = 1, Name = "P1", Price = 100M };
			Product p2 = new Product { ProductID = 2, Name = "P2", Price = 50M };
			Cart target = new Cart();
			target.AddItem(p1, 1);
			target.AddItem(p2, 1);
			target.AddItem(p1, 3);

			target.Clear();

			Assert.AreEqual(0, target.Lines().Count());
		}

		[TestMethod]
		public void Can_Add_To_Cart()
		{
			Mock<IProductRepository> mock = new Mock<IProductRepository>();
			mock.Setup(m => m.Products).Returns(new Product[]
			{
				new Product{ProductID = 1, Name = "P1", Category = "Apples"}
			}.AsQueryable());

			Cart cart = new Cart();

			CartController target = new CartController(mock.Object, null);

			target.AddToCart(cart, 1, null);

			Assert.AreEqual(1, cart.Lines().Count(), null);
			Assert.AreEqual(1, cart.Lines().ToArray()[0].Product.ProductID);
		}

		[TestMethod]
		public void Adding_Product_To_Cart_Goes_to_Cart_Screen()
		{
			Mock<IProductRepository> mock = new Mock<IProductRepository>();
			mock.Setup(m => m.Products).Returns(new Product[]
			{
				new Product{ProductID = 1, Name = "P1", Category = "Apples"}
			}.AsQueryable());

			Cart cart = new Cart();

			CartController target = new CartController(mock.Object, null);

			RedirectToRouteResult result = target.AddToCart(cart, 2, "myUrl");

			Assert.AreEqual(result.RouteValues["action"], "Index");
			Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
		}

		[TestMethod]
		public void Can_View_Cart_Contents()
		{
			Cart cart = new Cart();

			CartController target = new CartController(null, null);

			CartIndexViewModel result = (CartIndexViewModel)target.Index(cart, "myUrl").ViewData.Model;

			Assert.AreSame(cart, result.Cart);
			Assert.AreEqual("myUrl", result.ReturnUrl);
		}
	}
}
