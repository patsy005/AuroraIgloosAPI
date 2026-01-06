// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using AuroraIgloosAPI.Models;
// using AuroraIgloosAPI.Models.Contexts;
// using AuroraIgloosAPI.DTOs;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Identity;
//
// namespace AuroraIgloosAPI.Controllers;
//
// [Authorize(Roles = "Admin")]
// [Route("api/[controller]")]
// [ApiController]
//
// public class UserController : ControllerBase
// {
//     private readonly CompanyContext _context;
//     private readonly IPasswordHasher<User> _passwordHasher;
//
//     public UserController(CompanyContext context, IPasswordHasher<User> passwordHasher)
//     {
//         _context = context;
//         _passwordHasher = passwordHasher;
//     }
//     
//     // GET: api/Users
//     [Authorize(Roles = "Admin")]
//     [HttpGet]
//     public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
//     {
//         var users = await _context.User
//             .Include(u => u.Role)
//             .Include(u => u.UserType)
//             .Include(u => u.Employee)
//             .Include(u => u.Customer)
//             .Select(u => new UserDTO
//             {
//                 Id = u.Id,
//                 Login = u.Login,
//                 PasswordHash = u.PasswordHash,
//                 
//                 UserRoleId = u.Role.Id,
//                 Role = u.Role,
//                 
//                 UserTypeId = u.UserTypeId,
//                 UserType = u.UserType,
//                 
//                 Employee = u.Employee,
//                 Customer = u.Customer,
//             })
//             .ToListAsync();
//         
//         return Ok(users);
//     }
//     
//     // GET: api/Users/1
//     [Authorize(Roles = "Admin")]
//     [HttpGet("{id}")]
//     public async Task<ActionResult<User>> GetUser(int id)
//     {
//         var user = await _context.User
//             .Include(u => u.Role)
//             .Include(u => u.UserType)
//             .Include(u => u.Employee)
//             .Include(u => u.Customer)
//             .FirstOrDefaultAsync(u => u.Id == id);
//         
//         if (user == null) return NotFound();
//         
//         return user;
//     }
//     
//     // POST: api/Users
//     [Authorize(Roles = "Admin")]
//     [HttpPost]
//     public async Task<ActionResult<UserDTO>> PostUser(UserCreateDTO userDTO)
//     {
//         if (string.IsNullOrWhiteSpace(userDTO.Login) || string.IsNullOrWhiteSpace(userDTO.PasswordHash))
//             return BadRequest("Login and Password are required.");
//
//         var user = new User
//         {
//             Login = userDTO.Login,
//             UserRoleId = userDTO.UserRoleId,
//             UserTypeId = userDTO.UserTypeId,
//         };
//         
//         user.PasswordHash = _passwordHasher.HashPassword(user, userDTO.PasswordHash);
//         
//         _context.User.Add(user);
//         await _context.SaveChangesAsync();
//
//         var result = new UserDTO
//         {
//             Id = user.Id,
//             Login = user.Login,
//             PasswordHash = user.PasswordHash,
//             UserRoleId = user.Role.Id,
//             Role = user.Role,
//             UserTypeId = user.UserTypeId,
//             UserType = user.UserType,
//             Employee = user.Employee,
//             Customer = user.Customer,
//         };
//         
//         return CreatedAtAction(nameof(GetUser), new { id = user.Id }, result);
//     }
//     
//     // PUT: api/Users/1
//     [Authorize(Roles = "Admin")]
//     [HttpPut("{id}")]
//     public async Task<ActionResult<UserDTO>> PutUser(int id, UserUpdateDTO userDTO)
//     {
//         if(id != userDTO.Id) return BadRequest();
//         
//         var user = await _context.User.FirstOrDefaultAsync(u => u.Id == id);
//         if (user == null) return NotFound();
//         
//         user.Login = userDTO.Login;
//         user.PasswordHash = userDTO.PasswordHash;
//         user.UserRoleId = userDTO.UserRoleId;
//         user.UserTypeId = userDTO.UserTypeId;
//
//         try
//         {
//             await _context.SaveChangesAsync();
//         }
//         catch (DbUpdateConcurrencyException)
//         {
//             if (!UserExists(id))
//             {
//                 return NotFound();
//             }
//             else
//             {
//                 throw;
//             }
//         }
//         
//         return NoContent();
//     }
//     
//     // DELETE: api/Users/1
//     [Authorize(Roles = "Admin")]
//     [HttpDelete("{id}")]
//     public async Task<ActionResult<UserDTO>> DeleteUser(int id)
//     {
//         var user = await _context.User.FirstOrDefaultAsync(u => u.Id == id);
//         if (user == null) return NotFound();
//         
//         _context.User.Remove(user);
//         await _context.SaveChangesAsync();
//         
//         return NoContent();
//     }
//
//     private bool UserExists(int id)
//     {
//         return _context.User.Any(e => e.Id == id);
//     }
//     
// }

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuroraIgloosAPI.DTOs;
using AuroraIgloosAPI.Models;
using AuroraIgloosAPI.Models.Contexts;

namespace AuroraIgloosAPI.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly CompanyContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserController(CompanyContext context, IPasswordHasher<User> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    // GET: api/User
    // Zwraca spłaszczone dane do tabeli (name/surname/email zależnie czy Employee/Customer)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
    {
        var users = await _context.User
            .Include(u => u.Role)
            .Include(u => u.UserType)
            .Include(u => u.Employee)
                .ThenInclude(e => e.Person)
            .Include(u => u.Customer)
                .ThenInclude(c => c.Person)
            .Select(u => new UserDTO
            {
                Id = u.Id,
                Login = u.Login,

                UserRoleId = u.UserRoleId,
                RoleName = u.Role != null ? u.Role.Name : "",

                UserTypeId = u.UserTypeId,
                UserTypeName = u.UserType != null ? u.UserType.Type : "",

                EmployeeId = u.Employee != null ? u.Employee.Id : null,
                CustomerId = u.Customer != null ? u.Customer.Id : null,

                Name = u.Employee != null ? u.Employee.Person.Name :
                       u.Customer != null ? u.Customer.Person.Name : null,

                Surname = u.Employee != null ? u.Employee.Person.Surname :
                          u.Customer != null ? u.Customer.Person.Surname : null,

                Email = u.Employee != null ? u.Employee.Person.Email :
                        u.Customer != null ? u.Customer.Person.Email : null,
            })
            .ToListAsync();

        return Ok(users);
    }

    // GET: api/User/5
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDTO>> GetUser(int id)
    {
        var u = await _context.User
            .Include(x => x.Role)
            .Include(x => x.UserType)
            .Include(x => x.Employee).ThenInclude(e => e.Person)
            .Include(x => x.Customer).ThenInclude(c => c.Person)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (u == null) return NotFound();

        var dto = new UserDTO
        {
            Id = u.Id,
            Login = u.Login,
            UserRoleId = u.UserRoleId,
            RoleName = u.Role?.Name ?? "",
            UserTypeId = u.UserTypeId,
            UserTypeName = u.UserType?.Type ?? "",
            EmployeeId = u.Employee?.Id,
            CustomerId = u.Customer?.Id,
            Name = u.Employee?.Person?.Name ?? u.Customer?.Person?.Name,
            Surname = u.Employee?.Person?.Surname ?? u.Customer?.Person?.Surname,
            Email = u.Employee?.Person?.Email ?? u.Customer?.Person?.Email,
        };

        return Ok(dto);
    }

    // POST: api/User/upsert
    [HttpPost("upsert")]
    public async Task<IActionResult> UpsertUser(UserUpsertDTO dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var hasEmp = dto.EmployeeId.HasValue;
        var hasCust = dto.CustomerId.HasValue;
        if (hasEmp == hasCust) // oba true albo oba false
            return BadRequest("Provide exactly one: EmployeeId OR CustomerId.");

        User user;

        // CREATE
        if (!dto.Id.HasValue)
        {
            var loginTaken = await _context.User.AnyAsync(u => u.Login == dto.Login);
            if (loginTaken) return Conflict("Login already exists.");

            if (string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest("Password is required for creating a new user.");

            user = new User
            {
                Login = dto.Login,
                UserRoleId = dto.UserRoleId,
                UserTypeId = dto.UserTypeId
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

            _context.User.Add(user);
            await _context.SaveChangesAsync(); // żeby user dostał Id
        }
        // UPDATE
        else
        {
            user = await _context.User.FirstOrDefaultAsync(u => u.Id == dto.Id.Value);
            if (user == null) return NotFound("User not found.");

            // jeśli zmieniasz login, sprawdź kolizję
            if (!string.Equals(user.Login, dto.Login, StringComparison.OrdinalIgnoreCase))
            {
                var loginTaken = await _context.User.AnyAsync(u => u.Login == dto.Login && u.Id != user.Id);
                if (loginTaken) return Conflict("Login already exists.");
            }

            user.Login = dto.Login;
            user.UserRoleId = dto.UserRoleId;
            user.UserTypeId = dto.UserTypeId;

            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);
            }

        }

        // PODPIĘCIE DO EMPLOYEE
        if (dto.EmployeeId.HasValue)
        {
            var emp = await _context.Employee.FirstOrDefaultAsync(e => e.Id == dto.EmployeeId.Value);
            if (emp == null) return BadRequest("Employee not found.");

            if (emp.IdUser != null && !dto.Id.HasValue)
                return Conflict("Employee already has an account.");

            if (emp.IdUser != null && emp.IdUser != user.Id)
                return Conflict("Employee is already linked to another user.");

            emp.IdUser = user.Id;
        }

        // PODPIĘCIE DO CUSTOMER
        if (dto.CustomerId.HasValue)
        {
            var cust = await _context.Customer.FirstOrDefaultAsync(c => c.Id == dto.CustomerId.Value);
            if (cust == null) return BadRequest("Customer not found.");

            if (cust.IdUser != null && !dto.Id.HasValue)
                return Conflict("Customer already has an account.");

            if (cust.IdUser != null && cust.IdUser != user.Id)
                return Conflict("Customer is already linked to another user.");

            cust.IdUser = user.Id;
        }

        await _context.SaveChangesAsync();

        return Ok(new { userId = user.Id });
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(int id, UserUpsertDTO dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (!dto.Id.HasValue) return BadRequest("Id is required for update.");
        if (dto.Id.Value != id) return BadRequest("Route id does not match body Id.");

        var hasEmp = dto.EmployeeId.HasValue;
        var hasCust = dto.CustomerId.HasValue;
        if (hasEmp == hasCust)
            return BadRequest("Provide exactly one: EmployeeId OR CustomerId.");

        var user = await _context.User.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null) return NotFound("User not found.");

        if (!string.Equals(user.Login, dto.Login, StringComparison.OrdinalIgnoreCase))
        {
            var loginTaken = await _context.User.AnyAsync(u => u.Login == dto.Login && u.Id != user.Id);
            if (loginTaken) return Conflict("Login already exists.");
        }

        user.Login = dto.Login;
        user.UserRoleId = dto.UserRoleId;
        user.UserTypeId = dto.UserTypeId;

        if (!string.IsNullOrWhiteSpace(dto.Password))
            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);
        
        var currentEmp = await _context.Employee.FirstOrDefaultAsync(e => e.IdUser == user.Id);
        var currentCust = await _context.Customer.FirstOrDefaultAsync(c => c.IdUser == user.Id);

        var userIsEmployee = currentEmp != null;
        var userIsCustomer = currentCust != null;

        if (userIsEmployee && userIsCustomer)
            return Conflict("Data integrity error: user linked to both Employee and Customer.");

        if (userIsEmployee)
        {
            if (!dto.EmployeeId.HasValue)
                return BadRequest("This user is linked to an employee account. EmployeeId is required.");

            if (dto.EmployeeId.Value != currentEmp!.Id)
                return Conflict("Cannot change EmployeeId because employee must always have a user account.");

        }

        if (userIsCustomer)
        {
            if (!dto.CustomerId.HasValue)
                return BadRequest("This user is linked to a customer account. CustomerId is required.");
            
            currentCust!.IdUser = null;

            var newCust = await _context.Customer.FirstOrDefaultAsync(c => c.Id == dto.CustomerId.Value);
            if (newCust == null) return BadRequest("Customer not found.");

            if (newCust.IdUser != null && newCust.IdUser != user.Id)
                return Conflict("Customer is already linked to another user.");

            newCust.IdUser = user.Id;
        }

        if (!userIsEmployee && !userIsCustomer)
        {
            if (dto.EmployeeId.HasValue)
            {
                var emp = await _context.Employee.FirstOrDefaultAsync(e => e.Id == dto.EmployeeId.Value);
                if (emp == null) return BadRequest("Employee not found.");

                if (emp.IdUser != 0 && emp.IdUser != user.Id)
                    return Conflict("Employee is already linked to another user.");

                emp.IdUser = user.Id;
            }
            else
            {
                var cust = await _context.Customer.FirstOrDefaultAsync(c => c.Id == dto.CustomerId.Value);
                if (cust == null) return BadRequest("Customer not found.");

                if (cust.IdUser != null && cust.IdUser != user.Id)
                    return Conflict("Customer is already linked to another user.");

                cust.IdUser = user.Id;
            }
        }

        await _context.SaveChangesAsync();
        return NoContent();
    }


    // DELETE: api/User/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.User.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null) return NotFound();

        var emp = await _context.Employee.FirstOrDefaultAsync(e => e.IdUser == id);
        if (emp != null)
            return Conflict("Cannot delete this user because it's linked to an employee account.");

        var cust = await _context.Customer.FirstOrDefaultAsync(c => c.IdUser == id);
        if (cust != null) cust.IdUser = null;

        _context.User.Remove(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
