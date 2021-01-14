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
    public class ShoppingCartsController : Controller
    {
        private readonly ShopProjectContext _context;

        public ShoppingCartsController(ShopProjectContext context)
        {
            _context = context;
        }

        // GET: ShoppingCarts
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("userId") == null)
            {
                return View("../users/LogIn");
            }
            var shopProjectContext = _context.ShoppingCart
                .Where(shoppingCart1 => shoppingCart1.UserId == int.Parse(HttpContext.Session.GetString("userId")));
            try
            {
                shopProjectContext.Count();
                return View(await shopProjectContext.ToListAsync());
            }
            catch
            {
                return View("EmptyShoppingCarts");
            }
        }
        public IActionResult EmptyShoppingCarts()
        {
            if (HttpContext.Session.GetString("userId") == null)
            {
                return View("../users/LogIn");
            }
            return View();
        }

        // GET: ShoppingCarts/Details/5
        public IActionResult Load(int? id)
        {
            if (HttpContext.Session.GetString("userId") == null)
            {
                return View("../users/LogIn");
            }
            if (id == null)
            {
                return NotFound();
            }
            HttpContext.Session.SetString("MyShoppingCartId", id.ToString());
            return View("../Products/Index", _context.Product);
        }

        // GET: ShoppingCarts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (HttpContext.Session.GetString("userName") == null)
            {
                return View("../Products/Index", _context.Product);
            }
            if (id == null)
            {
                return NotFound();
            }

            var shoppingCart = await _context.ShoppingCart
                .Include(s => s.user)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shoppingCart == null)
            {
                return NotFound();
            }
            ViewData["finalPrice"] = _context.ProductInCart.Where(p => p.ShoppingCartId == shoppingCart.Id).Select(p1 => p1.FinalPrice).Sum();
            return View(shoppingCart);
        }


        // POST: ShoppingCarts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("userName") == null)
            {
                return View("../Products/Index", _context.Product);
            }
            var shoppingCart = await _context.ShoppingCart.FindAsync(id);
            _context.ShoppingCart.Remove(shoppingCart);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShoppingCartExists(int id)
        {
            return _context.ShoppingCart.Any(e => e.Id == id);
        }
    }
}
