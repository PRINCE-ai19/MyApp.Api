using System;
using System.Collections.Generic;

namespace MyApp.Domain.Entities;

public partial class Permission
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public DateTime? CreatedPer { get; set; }

    public DateTime? UpdatePer { get; set; }

    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

    public virtual ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
}
