namespace TravelStar.Site.Constants
{
    public class Permissions
    {
        public static List<string> GeneratePermissionsForModule(List<string> modules)
        {
            List<string> permissions = new List<string>();
            foreach (string module in modules)
            {
                List<string> permissionModule = new List<string>()
                {
                    $"Permissions.{module}.View",
                    $"Permissions.{module}.Create",
                    $"Permissions.{module}.Edit",
                    $"Permissions.{module}.Delete",
                };

                permissions.AddRange(permissionModule);
            }

            return permissions;
        }

        public static class ManageHotel
        {
            public const string View = "Permissions.ManageHotel.View";
            public const string Create = "Permissions.ManageHotel.Create";
            public const string Edit = "Permissions.ManageHotel.Edit";
            public const string Delete = "Permissions.ManageHotel.Delete";
        }

        public static class ManageUser
        {
            public const string View = "Permissions.ManageUser.View";
            public const string Create = "Permissions.ManageUser.Create";
            public const string Edit = "Permissions.ManageUser.Edit";
            public const string Delete = "Permissions.ManageUser.Delete";
        }
    }
}
