using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.ViewModels;

namespace WebApplication1.ValidationAttributes
{
    public class CourseTitleMustBeDifferentFromDescriptionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, 
            ValidationContext validationContext)
        {
            var course = (CourseForCreationVM)validationContext.ObjectInstance;

            if (course.Title == course.Description)
            {
                return new ValidationResult("The provide description should be different from the title.",
                    new[] { "CourseForCreationVM" });
            }

            return ValidationResult.Success;
        }
    }
}
