using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.ViewModels
{
    public class CourseForCreationVM : IValidatableObject
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [MaxLength(1500)]
        public string Description { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(Title == Description)
            {
                yield return new ValidationResult("The provide description should be different from the title.",
                    new[] { "CourseForCreationVM" });
            }
        }
    }
}
