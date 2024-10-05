using System.ComponentModel.DataAnnotations;

namespace DevForge_Connect.Views.Administration.View_Models;

public class CreateRoleViewModel
{
    [Required]
    public string RoleName { get; set; } = string.Empty;
}
