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
    public class ForumCommentsController : ControllerBase
    {
        private readonly CompanyContext _context;

        public ForumCommentsController(CompanyContext context)
        {
            _context = context;
        }

        // GET: api/ForumComments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ForumCommentDTO>>> GetForumComment()
        {
            var comments = await _context.ForumComment
                .Include(c => c.Employee)
                    .ThenInclude(e => e.User)
                    .ThenInclude(u => u.Address)
                .Include(c => c.ForumPost)
                .Select(c => new ForumCommentDTO
                {
                    Id = c.Id,
                    IdPost = c.IdPost,
                    IdEmployee = c.IdEmployee,
                    Comment = c.Comment,
                    CommentDate = c.CommentDate,
                    EmployeeName = c.Employee.User.Name,
                    EmployeeSurname = c.Employee.User.Surname,
                    EmployeePhotoUrl = c.Employee.PhotoUrl,
                    PostTitle = c.ForumPost.Title
                })
                .ToListAsync();

            return Ok(comments);
        }

        // GET: api/ForumComments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ForumCommentDTO>> GetForumComment(int id)
        {
            var comment = await _context.ForumComment
                .Include(c => c.Employee)
                    .ThenInclude(e => e.User)
                    .ThenInclude(u => u.Address)
                .Include(c => c.ForumPost)
                .Where(c => c.Id == id)
                .Select(c => new ForumCommentDTO
                {
                    Id = c.Id,
                    IdPost = c.IdPost,
                    IdEmployee = c.IdEmployee,
                    Comment = c.Comment,
                    CommentDate = c.CommentDate,
                    EmployeeName = c.Employee.User.Name,
                    EmployeeSurname = c.Employee.User.Surname,
                    EmployeePhotoUrl = c.Employee.PhotoUrl,
                    PostTitle = c.ForumPost.Title
                })
                .FirstOrDefaultAsync();

            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment);
        }

        // PUT: api/ForumComments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutForumComment(int id, ForumCommentDTO forumCommentDto)
        {
            if(id != forumCommentDto.Id) return BadRequest("Id mismatch");

            if(!ModelState.IsValid) return BadRequest(ModelState);

            var employee = await _context.Employee
                .Include(e => e.User)
                    .ThenInclude(u => u.Address)
                .FirstOrDefaultAsync(e => e.Id == forumCommentDto.IdEmployee);

            if (employee == null) return NotFound($"Employee with id {forumCommentDto.IdEmployee} not found");

            var post = await _context.ForumPost
                .FirstOrDefaultAsync(p => p.Id == forumCommentDto.IdPost);

            if (post == null) return NotFound($"Post with id {forumCommentDto.IdPost} not found");

            var comment = await _context.ForumComment
                .Include(c => c.Employee)
                    .ThenInclude(e => e.User)
                    .ThenInclude(u => u.Address)
                .Include(c => c.ForumPost)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (comment == null) return NotFound($"Comment with id {id} not found");

            comment.IdPost = forumCommentDto.IdPost;
            comment.IdEmployee = forumCommentDto.IdEmployee;
            comment.Comment = forumCommentDto.Comment;
            comment.CommentDate = forumCommentDto.CommentDate ?? DateOnly.FromDateTime(DateTime.Now);
            comment.Employee = employee;
            comment.ForumPost = post;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ForumCommentExists(id))
                {
                    return NotFound($"Comment with id {id} not found");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();

        }

        // POST: api/ForumComments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ForumComment>> PostForumComment(ForumCommentDTO forumCommentDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var employee = await _context.Employee
                .Include(e => e.User)
                    .ThenInclude(u => u.Address)
                .FirstOrDefaultAsync(e => e.Id == forumCommentDto.IdEmployee);

            if (employee == null) return NotFound($"Employee with id {forumCommentDto.IdEmployee} not found");

            var post = await _context.ForumPost
                .FirstOrDefaultAsync(p => p.Id == forumCommentDto.IdPost);

            if (post == null) return NotFound($"Post with id {forumCommentDto.IdPost} not found");

            var comment = new ForumComment
            {

                IdPost = forumCommentDto.IdPost,
                IdEmployee = forumCommentDto.IdEmployee,
                Comment = forumCommentDto.Comment,
                CommentDate = forumCommentDto.CommentDate ?? DateOnly.FromDateTime(DateTime.Now),
                Employee = employee,
                ForumPost = post
            };

            _context.ForumComment.Add(comment);
            await _context.SaveChangesAsync();

            var raturnDto = new ForumCommentDTO
            {
                Id = comment.Id,
                IdPost = comment.IdPost,
                IdEmployee = comment.IdEmployee,
                Comment = comment.Comment,
                CommentDate = comment.CommentDate,
                EmployeeName = comment.Employee.User.Name,
                EmployeeSurname = comment.Employee.User.Surname,
                EmployeePhotoUrl = comment.Employee.PhotoUrl,
                PostTitle = comment.ForumPost.Title
            };

            return CreatedAtAction("GetForumComment", new { id = comment.Id }, raturnDto);
        }

        // DELETE: api/ForumComments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteForumComment(int id)
        {
            var comment = await _context.ForumComment
                .Include(c => c.Employee)
                    .ThenInclude(e => e.User)
                    .ThenInclude(u => u.Address)
                .Include(c => c.ForumPost)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (comment == null)
            {
                return NotFound();
            }

            _context.ForumComment.Remove(comment);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ForumCommentExists(int id)
        {
            return _context.ForumComment.Any(e => e.Id == id);
        }
    }
}
