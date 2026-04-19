using System;
using System.Collections.Generic;

namespace MyApp.Domain.Entities;

public partial class UsersToken
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string RefreshToken { get; set; } = null!;

    public DateTime Expires { get; set; }

    public DateTime? CreatedDate { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? Revoked { get; set; }

    public string? Code { get; set; }

    public virtual User User { get; set; } = null!;
}
