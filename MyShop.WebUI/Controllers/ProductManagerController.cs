using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using MyShop.DataAccess.InMemory;

namespace MyShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
       
        ProductRepository context;
        ProductCategoryRepository productCategories;

        public ProductManagerController()
        {
            context = new ProductRepository();
            productCategories = new ProductCategoryRepository();
        }


        // GET: ProductManager
        public ActionResult Index()
        {
            List<Product> products = context.Collection().ToList();
            return View(products);
        }

        public ActionResult Create()
        {
            ProductManagerViewModels viewModels = new ProductManagerViewModels();
            viewModels.Product = new Product();
            viewModels.ProductCategories = productCategories.Collection();
            //Product product = new Product();
            return View(viewModels);

        }
        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            else {
                
                context.Insert(product);
                context.Commit();

                return RedirectToAction("Index");
            }
        }

        public ActionResult Edit(string Id)
        {
            Product product = context.Find(Id);
         

            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                ProductManagerViewModels viewModels = new ProductManagerViewModels();
                viewModels.Product = new Product();
                viewModels.ProductCategories = productCategories.Collection();
                return View(viewModels);
            }

        }

        [HttpPost]
        public ActionResult Edit(Product product, string Id)
        {
            Product productToEdit = context.Find(Id);

            if (productToEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(product);
                }
                productToEdit.Category = product.Category;
                productToEdit.Description = product.Description;
                productToEdit.Image = product.Image;
                productToEdit.Name = product.Name;
                productToEdit.Price = product.Price;

                context.Commit();

                return RedirectToAction("Index");

             }

          }

             public ActionResult Delete(string Id)
             {

                        Product productTodelete = context.Find(Id);

                        if (productTodelete == null)
                        {
                            return HttpNotFound();
                        }


                        else 
                        {
                            return View(productTodelete); 
                        }


           }

                    [HttpPost]
                    [ActionName("Delete")]
                    public ActionResult ConfirmDelete(string Id)
                    {
                        Product productTodelete = context.Find(Id);

                        if (productTodelete == null)
                        {
                            return HttpNotFound();
                        }


                        else 
                        {
                              context.Delete(Id);
                             context.Commit();
                                 return RedirectToAction("Index");
                          }

                    }

        }
    }