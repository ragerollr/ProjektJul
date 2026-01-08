using System.ComponentModel.DataAnnotations;

namespace Projekt.Web.ViewModels
{
    public class CreateSkillViewModel
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Range(1, 5)]
        public int Level { get; set; }
    }
}
