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
    public class UsersController : Controller
    {
        private readonly ShopProjectContext _context;

        public UsersController(ShopProjectContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("userName") == null || !HttpContext.Session.GetString("userName").Equals("admin"))
            {
                return View("../Products/Index", _context.Product);
            }
            return View(await _context.User.ToListAsync());
        }

        public IActionResult About()
        {
            return View();
        }


        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,EmailAddress,Passowrd,City,Street")] User user)
        {
            if (ModelState.IsValid)
            {
                if(HttpContext.Session.GetString("userName")!=null && HttpContext.Session.GetString("userName").Equals("admin"))
                {
                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    return View("../Products/Index", _context.Product);
                }
                var userResult = _context.User.Where(u => u.Name == user.Name);
                if (userResult.Count() > 0)
                {
                    ViewData["Error"] = "שם משתמש קיים במערכת, בחר שם אחר!";
                    return View(user);
                }
                else
                {
                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    HttpContext.Session.SetString("userId", user.Id.ToString());
                    HttpContext.Session.SetString("userName", user.Name);
                    return View("../Products/Index", _context.Product);
                }
            }
            return View(user);
        }

        // GET: Users/Create
        public IActionResult LogIn()
        {
            ViewData["ShoppingCartId"] = new SelectList(_context.ShoppingCart, "Id", "Id");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn([Bind("Id,Name,Passowrd")] User user)
        {
            var userResult = await _context.User.Where(u => u.Name == user.Name && u.Passowrd == user.Passowrd).FirstOrDefaultAsync();
            if (userResult != null)
            {
                HttpContext.Session.SetString("userId", userResult.Id.ToString());
                HttpContext.Session.SetString("userName", userResult.Name);

                return View("../Products/Index", _context.Product);
            }
            else
            {
                ViewData["Error"] = "משתמש לא קיים במערכת נא להירשם!";
            }
            return View();
        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("userId");
            HttpContext.Session.Remove("userName");
            return View("../Products/Index", _context.Product);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetString("userId")==null)
            {
                return View("../Products/Index", _context.Product);
            }            
            var user = await _context.User.FindAsync(Int32.Parse(HttpContext.Session.GetString("userId")));
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,EmailAddress,Passowrd,City,Street")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var userResult = _context.User.Where(u => u.Name == user.Name);
                    if (userResult.Count() > 0 && user.Name != HttpContext.Session.GetString("userName"))
                    {
                        ViewData["Error"] = "שם משתמש קיים במערכת, בחר שם אחר!";
                        return View(user);
                    }
                    else
                    {
                        _context.Update(user);
                        await _context.SaveChangesAsync();
                        return View("../Products/Index", _context.Product);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || HttpContext.Session.GetString("userName")==null || !HttpContext.Session.GetString("userName").Equals("admin"))
            {
                return View("../Products/Index", _context.Product);
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("userName") == null || !HttpContext.Session.GetString("userName").Equals("admin"))
            {
                return View("../Products/Index", _context.Product);
            }
            var user = await _context.User.FindAsync(id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
