using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using bbs.Data;
using bbs.Models;
using Microsoft.AspNetCore.Authorization;

namespace bbs.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;
		private readonly ILogger<HomeController> _logger;

		public PostsController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
			_logger = logger;
		}

        // GET: Posts
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            var viewModel = new PostViewModel
            {
                SortOrder = sortOrder
            };

            IQueryable<Post> postQuery = _context.Post;
            _logger.LogInformation("serchcode:" + searchString);
            // 过滤帖子
            if (!string.IsNullOrEmpty(searchString))
            {          
                postQuery = postQuery.Where(
                    p => p.Content.Contains(searchString) || p.UserName.Contains(searchString)
                );
            }

            // 排序
            switch (sortOrder)
            {
                case "content_asc":
                    postQuery = postQuery.OrderBy(p => p.Content);
                    break;
                case "content_desc":
                    postQuery = postQuery.OrderByDescending(p => p.Content);
                    break;
                case "date_asc":
                    postQuery = postQuery.OrderBy(p => p.CreatedAt);
                    break;
                case "date_desc":
                    postQuery = postQuery.OrderByDescending(p => p.CreatedAt);
                    break;
                case "user_asc":
                    postQuery = postQuery.OrderBy(p => p.UserName);
                    break;
                case "user_desc":
                    postQuery = postQuery.OrderByDescending(p => p.UserName);
                    break;
                default:
                    postQuery = postQuery.OrderByDescending(p => p.CreatedAt);
                    break;
            }

            viewModel.Posts = await postQuery.ToListAsync();
            viewModel.SearchString = searchString;

            return View(viewModel);
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Content,UserId,UserName,CreatedAt")] Post post)
        {
            if (ModelState.IsValid)
            {
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Content,UserId,UserName,CreatedAt")] Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Post.FindAsync(id);
            if (post != null)
            {
                _context.Post.Remove(post);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Post.Any(e => e.Id == id);
        }
    }
}
