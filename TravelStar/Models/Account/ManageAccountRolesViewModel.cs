namespace TravelStar.Site.Models.Account
{
    public class ManageAccountRolesViewModel
    {
        public string? UserId { get; set; }
        public string? Email { get; set; }
        public IList<AccountRolesViewModel>? AccountRoles { get; set; } = new List<AccountRolesViewModel>();
    }
}
