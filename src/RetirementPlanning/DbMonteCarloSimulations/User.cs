using System;
using System.Collections.Generic;

namespace RetirementPlanning.DbMonteCarloSimulations;

public partial class User
{
    public Guid Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateTime DateOfBirth { get; set; }

    public virtual ICollection<AccountInfo> AccountInfos { get; set; } = new List<AccountInfo>();

    public virtual ICollection<SocialSecurity> SocialSecurities { get; set; } = new List<SocialSecurity>();
}
