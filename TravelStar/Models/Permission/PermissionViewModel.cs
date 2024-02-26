namespace TravelStar.Site.Models.Permission
{
    public class PermissionViewModel
    {
        public string? RoleId { get; set; }
        public IList<RoleClaimsViewModel> RoleClaims { get; set; } = new List<RoleClaimsViewModel>();
    }
}
