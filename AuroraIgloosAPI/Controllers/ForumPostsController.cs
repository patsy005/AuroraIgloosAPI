using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuroraIgloosAPI.Models;
using AuroraIgloosAPI.Models.Contexts;
using AuroraIgloosAPI.DTOs;

namespace AuroraIgloosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForumPostsController : ControllerBase
    {
        private readonly CompanyContext _context;

        public ForumPostsController(CompanyContext context)
        {
            _context = context;
        }

        // GET: api/ForumPosts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ForumPostDTO>>> GetForumPost()
        {
            var posts = await _context.ForumPost
                .Include(p => p.Employee)
                .ThenInclude(e => e.User)
                .ThenInclude(u => u.Address)
                .Include(p => p.Category)
                .Include(p => p.ForumComment)
                .Select(p => new ForumPostDTO
                {
                    Id = p.Id,
                    IdEmployee = p.IdEmployee,
                    CategoryId = p.CategoryId,
                    Title = p.Title ?? "",
                    PostContent = p.PostContent ?? "",
                    PostDate = p.PostDate,
                    Category = p.Category.Name ?? "",
                    Tags = p.Tags ?? "",
                    EmployeeName = p.Employee.User.Name ?? "",
                    EmployeeSurname = p.Employee.User.Surname ?? "",
                    EmployeePhotoUrl = p.Employee.PhotoUrl ?? "",
                    ForumComment = p.ForumComment.Select(c => new ForumCommentDTO
                    {
                        Id = c.Id,
                        IdPost = c.IdPost,
                        IdEmployee = c.IdEmployee,
                        Comment = c.Comment ?? "",
                        CommentDate = c.CommentDate,
                        EmployeeName = c.Employee.User.Name ?? "",
                        EmployeeSurname = c.Employee.User.Surname ?? "",
                        EmployeePhotoUrl = c.Employee.PhotoUrl ?? "",
                        PostTitle = p.Title ?? "",
                    }).ToList(), // Convert ForumComment to list here
                    NumberOfComment = p.ForumComment.Count
                })
                .ToListAsync();

            return Ok(posts);
        }

        // GET: api/ForumPosts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ForumPost>> GetForumPost(int id)
        {
            var forumPost = await _context.ForumPost.FindAsync(id);

            if (forumPost == null)
            {
                return NotFound();
            }

            return forumPost;
        }

        // PUT: api/ForumPosts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutForumPost(int id, ForumPost forumPost)
        {
            if (id != forumPost.Id)
            {
                return BadRequest();
            }

            _context.Entry(forumPost).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ForumPostExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ForumPosts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ForumPost>> PostForumPost(ForumPost forumPost)
        {
            _context.ForumPost.Add(forumPost);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetForumPost", new { id = forumPost.Id }, forumPost);
        }

        // DELETE: api/ForumPosts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteForumPost(int id)
        {
            var forumPost = await _context.ForumPost.FindAsync(id);
            if (forumPost == null)
            {
                return NotFound();
            }

            _context.ForumPost.Remove(forumPost);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ForumPostExists(int id)
        {
            return _context.ForumPost.Any(e => e.Id == id);
        }
    }
}
