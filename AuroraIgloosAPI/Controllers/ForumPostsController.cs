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
        public async Task<ActionResult<ForumPostDTO>> GetForumPost(int id)
        {
            var post = await _context.ForumPost
                .Include(p => p.Employee)
                    .ThenInclude(e => e.User)
                    .ThenInclude(u => u.Address)
                .Include(p => p.Category)
                .Include(p => p.ForumComment)
                .Where(p => p.Id == id)
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
                    }).ToList(),
                    NumberOfComment = p.ForumComment.Count


                })
                .FirstOrDefaultAsync();

            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }

        // PUT: api/ForumPosts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutForumPost(int id, ForumPostDTO forumPostDto)
        {
            if(id != forumPostDto.Id)
            {
                return BadRequest("id does not match");
            }

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var forumPost = await _context.ForumPost.FindAsync(id);

            if (forumPost == null)
            {
                return NotFound();
            }

           var category = await _context.ForumCategory.FindAsync(forumPostDto.CategoryId);
            if (category == null)
            {
                return BadRequest("Category not found");
            }

            var employee = await _context.Employee.FindAsync(forumPostDto.IdEmployee);
            if (employee == null)
            {
                return BadRequest("Employee not found");
            }

            
            var post = await _context.ForumPost
                .Include(p => p.Employee)
                .ThenInclude(e => e.User)
                .ThenInclude(u => u.Address)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if(post == null)
            {
                return NotFound($"Forum post with id {id} not found");
            }



            forumPost.Title = forumPostDto.Title;
            forumPost.PostContent = forumPostDto.PostContent;
            forumPost.PostDate = forumPostDto.PostDate;
            forumPost.CategoryId = forumPostDto.CategoryId;
            forumPost.Tags = forumPostDto.Tags;
            forumPost.IdEmployee = forumPostDto.IdEmployee;

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
        public async Task<ActionResult<ForumPost>> PostForumPost(ForumPostDTO forumPostDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = await _context.ForumCategory.FindAsync(forumPostDto.CategoryId);
            if (category == null)
            {
                return BadRequest("Category not found");
            }

            var employee = await _context.Employee.FindAsync(forumPostDto.IdEmployee);
            if (employee == null)
            {
                return BadRequest("Employee not found");
            }

            // Initialize ForumComment as an empty list if null
            var forumComments = forumPostDto.ForumComment?.Select(c => new ForumComment
            {
                Id = c.Id ?? 0, // Fix for CS0266 and CS8629
                IdPost = c.IdPost,
                IdEmployee = c.IdEmployee,
                Comment = c.Comment,
                CommentDate = c.CommentDate,
                Employee = employee // Assuming the employee is the same for all comments
            }).ToList() ?? new List<ForumComment>();

            var forumPost = new ForumPost
            {
                IdEmployee = employee.Id,
                Title = forumPostDto.Title,
                PostContent = forumPostDto.PostContent,
                PostDate = forumPostDto.PostDate ?? DateOnly.FromDateTime(DateTime.Now),
                CategoryId = category.Id,
                Tags = forumPostDto.Tags,
                Employee = employee,
                Category = category,
                ForumComment = forumComments
            };

            try
            {
                _context.ForumPost.Add(forumPost);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

            var forumPostDtoResponse = new ForumPostDTO
            {
                Id = forumPost.Id,
                IdEmployee = forumPost.IdEmployee,
                CategoryId = forumPost.CategoryId,
                Title = forumPost.Title ?? "",
                PostContent = forumPost.PostContent ?? "",
                PostDate = forumPost.PostDate,
                Category = category.Name ?? "",
                Tags = forumPost.Tags ?? "",
                EmployeeName = employee.User?.Name ?? "",
                EmployeeSurname = employee.User?.Surname ?? "",
                EmployeePhotoUrl = employee.PhotoUrl ?? "",
                ForumComment = forumComments.Select(c => new ForumCommentDTO
                {
                    Id = c.Id,
                    IdPost = c.IdPost,
                    IdEmployee = c.IdEmployee,
                    Comment = c.Comment ?? "",
                    CommentDate = c.CommentDate,
                    EmployeeName = employee.User?.Name ?? "",
                    EmployeeSurname = employee.User?.Surname ?? "",
                    EmployeePhotoUrl = employee.PhotoUrl ?? "",
                    PostTitle = forumPost.Title ?? "",
                }).ToList(),
                NumberOfComment = forumComments.Count
            };

            return CreatedAtAction(nameof(GetForumPost), new { id = forumPost.Id }, forumPostDtoResponse);

        }

        // DELETE: api/ForumPosts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteForumPost(int id)
        {
            var forumPost = await _context.ForumPost
                .Include(p => p.Employee)
                .ThenInclude(e => e.User)
                .ThenInclude(u => u.Address)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(Employee => Employee.Id == id);

            var forumComments = await _context.ForumComment.Where(c => c.IdPost == id).ToListAsync();

            if (forumPost == null)
            {
                return NotFound();
            }

            _context.ForumPost.Remove(forumPost);
            _context.ForumComment.RemoveRange(forumComments);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ForumPostExists(int id)
        {
            return _context.ForumPost.Any(e => e.Id == id);
        }
    }
}
