using System;
using System.Collections.Generic;

namespace MyApp.Domain.Entities;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? Code { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? RecordStatus { get; set; }

    public virtual ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

    public virtual ICollection<UsersToken> UsersTokens { get; set; } = new List<UsersToken>();
}
