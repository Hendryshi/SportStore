using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.Domain.Entities;
using System.Linq;

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
	}
}
