using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.Models;
using SportsStore.WebUI.HtmlHelpers;

namespace SportsStore.UnitTests
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void Can_Paginate()
		{
			Mock<IProductRepository> mock = new Mock<IProductRepository>();
			mock.Setup(m => m.Products).Returns(new Product[]
			{
				new Product {ProductID = 1, Name = "P1"},
				new Product {ProductID = 2, Name = "P2"},
				new Product {ProductID = 3, Name = "P3"},
				new Product {ProductID = 4, Name = "P4"},
				new Product {ProductID = 5, Name = "P5"},
			});

			ProductController controller = new ProductController(mock.Object);
			controller.PageSize = 3;

			ProductsListViewModel result = (ProductsListViewModel)controller.List(null, 2).Model;

			Product[] prodArray = result.Products.ToArray();
			Assert.IsTrue(prodArray.Length == 2);
			Assert.AreEqual(prodArray[0].Name, "P4");
			Assert.AreEqual(prodArray[1].Name, "P5");
		}

		[TestMethod]
		public void Can_Generate_Page_Links()
		{
			HtmlHelper myHelper = null;

			PagingInfo pagingInfo = new PagingInfo { CurrentPage = 2, TotalItems = 28, ItemPerPage = 10 };
			Func<int, string> pageUrlDelegate = i => "Page" + i;

			MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

			Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>" +
				@"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>" +
				@"<a class=""btn btn-default"" href=""Page3"">3</a>", result.ToString()
				);
		}

		[TestMethod]
		public void Can_Send_Pagination_View_Model()
		{
			Mock<IProductRepository> mock = new Mock<IProductRepository>();
			mock.Setup(m => m.Products).Returns(new Product[]
			{
				new Product {ProductID = 1, Name = "P1"},
				new Product {ProductID = 2, Name = "P2"},
				new Product {ProductID = 3, Name = "P3"},
				new Product {ProductID = 4, Name = "P4"},
				new Product {ProductID = 5, Name = "P5"},
			});

			ProductController controller = new ProductController(mock.Object);
			controller.PageSize = 3;

			ProductsListViewModel result = (ProductsListViewModel)controller.List(null, 2).Model;

			PagingInfo pageInfo = result.PagingInfo;
			Assert.AreEqual(pageInfo.CurrentPage, 2);
			Assert.AreEqual(pageInfo.ItemPerPage, 3);
			Assert.AreEqual(pageInfo.TotalItems, 5);
			Assert.AreEqual(pageInfo.TotalPages, 2);
		}

		[TestMethod]
		public void Can_Filter_Products()
		{
			Mock<IProductRepository> mock = new Mock<IProductRepository>();
			mock.Setup(m => m.Products).Returns(new Product[]
			{
				new Product {ProductID = 1, Name = "P1", Category = "Cat1"},
				new Product {ProductID = 2, Name = "P2", Category = "Cat2"},
				new Product {ProductID = 3, Name = "P3", Category = "Cat1"},
				new Product {ProductID = 4, Name = "P4", Category = "Cat2"},
				new Product {ProductID = 5, Name = "P5", Category = "Cat3"},
			});

			ProductController controller = new ProductController(mock.Object);
			controller.PageSize = 3;

			Product[] result = ((ProductsListViewModel)controller.List("Cat2", 1).Model).Products.ToArray();

			Assert.AreEqual(2, result.Length);
		}

		[TestMethod]
		public void Can_Create_Categories()
		{
			Mock<IProductRepository> mock = new Mock<IProductRepository>();
			mock.Setup(m => m.Products).Returns(new Product[]
			{
				new Product {ProductID = 1, Name = "P1", Category = "Apple"},
				new Product {ProductID = 2, Name = "P2", Category = "Apple"},
				new Product {ProductID = 3, Name = "P3", Category = "Orange"},
				new Product {ProductID = 4, Name = "P4", Category = "Banana"},
				
			});

			NavController controller = new NavController(mock.Object);

			string[] results = ((IEnumerable<string>)controller.Menu().Model).ToArray();

			Assert.AreEqual(3, results.Length);
		}

		[TestMethod]
		public void Generate_Category_Product_Count()
		{
			Mock<IProductRepository> mock = new Mock<IProductRepository>();
			mock.Setup(m => m.Products).Returns(new Product[]
			{
				new Product {ProductID = 1, Name = "P1", Category = "Cat1"},
				new Product {ProductID = 2, Name = "P2", Category = "Cat2"},
				new Product {ProductID = 3, Name = "P3", Category = "Cat1"},
				new Product {ProductID = 4, Name = "P4", Category = "Cat2"},
				new Product {ProductID = 5, Name = "P5", Category = "Cat3"},
			});

			ProductController controller = new ProductController(mock.Object);
			controller.PageSize = 3;

			int res1 = ((ProductsListViewModel)controller.List("Cat1").Model).PagingInfo.TotalItems;
			int res2 = ((ProductsListViewModel)controller.List("Cat2").Model).PagingInfo.TotalItems;
			int res3 = ((ProductsListViewModel)controller.List("Cat3").Model).PagingInfo.TotalItems;
			int res4 = ((ProductsListViewModel)controller.List(null).Model).PagingInfo.TotalItems;

			Assert.AreEqual(2, res1);
			Assert.AreEqual(2, res2);
			Assert.AreEqual(1, res3);
			Assert.AreEqual(5, res4);
		}
	}
}
