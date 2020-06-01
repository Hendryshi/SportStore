using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
		private Domain.Abstract.IProductRepository repository;
		public int PageSize = 4;

		public ProductController(Domain.Abstract.IProductRepository productRepository)
		{
			this.repository = productRepository;
		}

        // GET: Product
        public ViewResult List(string category, int page = 1)
		{
			ProductsListViewModel model = new ProductsListViewModel
			{
				Products = repository.Products.Where(p => category == p.Category || category == null).OrderBy(p => p.ProductID).Skip((page - 1) * PageSize).Take(PageSize),
				PagingInfo = new PagingInfo { CurrentPage = page, TotalItems = repository.Products.Count(), ItemPerPage = PageSize },
				CurrentCategory = category
			};
			return View(model);
		}
    }
}