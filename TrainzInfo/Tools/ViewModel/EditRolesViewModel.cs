using System.Collections.Generic;

namespace TrainzInfo.Tools
{
    public class EditRolesViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<RoleCheckBox> Roles { get; set; }
    }
}
