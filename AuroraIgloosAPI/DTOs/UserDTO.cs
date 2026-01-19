using System.ComponentModel.DataAnnotations;

namespace AuroraIgloosAPI.DTOs;

public class UserDTO
{
    public int Id { get; set; }

    public string Login { get; set; } = "";

    public int UserRoleId { get; set; }
    public string RoleName { get; set; } = "";

    public int UserTypeId { get; set; }
    public string UserTypeName { get; set; } = "";

    public int? EmployeeId { get; set; }
    public int? CustomerId { get; set; }

    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Email { get; set; }
    public DateOnly LastModifiedAt { get; set; }
}


public class UserUpsertDTO
{
    public int? Id { get; set; } // null => create, != null => update

    [Required(ErrorMessage = "Login is required")]
    public string Login { get; set; } = "";


    public string? Password { get; set; }

    [Required(ErrorMessage = "UserRoleId is required")]
    public int UserRoleId { get; set; }

    [Required(ErrorMessage = "UserTypeId is required")]
    public int UserTypeId { get; set; }

    // Dokładnie jedno z nich powinno być ustawione
    public int? EmployeeId { get; set; }
    public int? CustomerId { get; set; }
}

public class CreateCustomerUserDTO
{
    public string Login { get; set; }
    public string Password { get; set; }
    public int UserRoleId { get; set; } = 3;
    public int UserTypeId { get; set; } = 2;
}