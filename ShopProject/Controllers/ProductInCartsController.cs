using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shop.Models;
using ShopProject.Data;

namespace ShopProject.Controllers
{
    public class ProductInCartsController : Controller
    {
        private readonly ShopProjectContext _context;

        public ProductInCartsController(ShopProjectContext context)
        {
            _context = context;
        }

        // GET: ProductInCarts
        public async Task<IActionResult> Index()
        {

            if (HttpContext.Session.GetString("userId") == null)
            {
                return View("../users/LogIn");
            }
            var shopProjectContext = _context.ProductInCart
                .Where(producInCart1 => producInCart1.ShoppingCartId == int.Parse(HttpContext.Session.GetString("MyShoppingCartId")));
            try
            {
                if (shopProjectContext.Count() == 0)
                {
                    return View("EmptyShoppingCart");
                }
                foreach (var producInCart2 in shopProjectContext)
                {
                    producInCart2.product = _context.Product.Where(product => product.Id == producInCart2.ProductId).FirstOrDefault();
                }
                return View(await shopProjectContext.ToListAsync());
            }
            catch
            {
                return View("EmptyShoppingCart");
            }
        }

        public IActionResult EmptyShoppingCart()
        {
            if (HttpContext.Session.GetString("userId") == null)
            {
                return View("../users/LogIn");
            }
            return View();
        }

        public IActionResult Create(int? id)
        {
            var productInCart = new ProductInCart();
            productInCart.product = _context.Product.Where(p => p.Id == id).FirstOrDefault();
            productInCart.ProductId = productInCart.product.Id;
            productInCart.Amount = 1;
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Desc");
            return View(productInCart);
        }

        // POST: ProductInCarts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductId,Amount,FinalPrice")] ProductInCart productInCart)
        {
            productInCart.product = _context.Product.Where(p => p.Id == productInCart.ProductId).FirstOrDefault();
            if (HttpContext.Session.GetString("userId") == null)
            {
                return View("../users/LogIn");
            }
            if (ModelState.IsValid)
            {
                var myShoppingCartId = HttpContext.Session.GetString("MyShoppingCartId");
                if (myShoppingCartId == null)
                {
                    var myNewShoppingCart = new ShoppingCart()
                    {
                        DateCreate = DateTime.Now,
                        products = new List<ProductInCart>(),
                        UserId = int.Parse(HttpContext.Session.GetString("userId")),
                        user = _context.User.Where(u => u.Id == int.Parse(HttpContext.Session.GetString("userId"))).FirstOrDefault()
                    };
                    _context.Add(myNewShoppingCart);
                    await _context.SaveChangesAsync();
                    myShoppingCartId = myNewShoppingCart.Id.ToString();
                    HttpContext.Session.SetString("MyShoppingCartId", myShoppingCartId);
                }

                var newProductInCart = new ProductInCart()
                {
                    Amount = productInCart.Amount,
                    product = _context.Product.Where(p => p.Id == productInCart.ProductId).FirstOrDefault(),
                    ProductId = productInCart.ProductId,
                    FinalPrice = (float)Math.Round(productInCart.Amount * productInCart.product.Price,2),
                    ShoppingCart = _context.ShoppingCart.Where(S => S.Id == int.Parse(myShoppingCartId)).FirstOrDefault(),
                    ShoppingCartId = int.Parse(myShoppingCartId)

                };
                var prodactExsits = _context.ProductInCart.Where(p => p.ProductId == productInCart.ProductId && p.ShoppingCartId == int.Parse(myShoppingCartId)).FirstOrDefault();
                if (prodactExsits != null)
                {
                    newProductInCart.Id = prodactExsits.Id;
                    return View("../Products/Index", _context.Product);
                }


                _context.Add(newProductInCart);
                await _context.SaveChangesAsync();
                return View("../Products/Index", _context.Product);
            }
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Desc", productInCart.ProductId);
            return View(productInCart);
        }



        // GET: ProductInCarts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetString("userId") == null)
            {
                return View("../users/LogIn");
            }
            if (id == null)
            {
                return NotFound();
            }

            var productInCart = await _context.ProductInCart.FindAsync(id);
            if (productInCart == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Desc", productInCart.ProductId);
            return View(productInCart);
        }

        // POST: ProductInCarts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductId,Amount,FinalPrice")] ProductInCart productInCart)
        {
            if (HttpContext.Session.GetString("userId") == null)
            {
                return View("../users/LogIn");
            }

            if (id != productInCart.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productInCart);
                    await _context.SaveChangesAsync();                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductInCartExists(productInCart.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return View("../Products/Index", _context.Product);
            }
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Desc", productInCart.ProductId);
            return View(productInCart);
        }


        // POST: ProductInCarts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("userId") == null)
            {
                return View("../users/LogIn");
            }
            var productInCart = await _context.ProductInCart.FindAsync(id);
            _context.ProductInCart.Remove(productInCart);
            await _context.SaveChangesAsync();

            var shopProjectContext = _context.ProductInCart
               .Where(producInCart1 => producInCart1.ShoppingCartId == int.Parse(HttpContext.Session.GetString("MyShoppingCartId")));
            try
            {
                if (shopProjectContext.Count() != 0)
                {
                    return RedirectToAction(nameof(Index));

                }
            }
            catch  {      }

            return View("EmptyShoppingCart");

        }

        private bool ProductInCartExists(int id)
        {
            return _context.ProductInCart.Any(e => e.Id == id);
        }
    }
}
