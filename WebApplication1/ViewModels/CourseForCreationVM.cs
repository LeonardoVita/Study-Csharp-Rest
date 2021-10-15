using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.ValidationAttributes;

namespace WebApplication1.ViewModels
{
    [CourseTitleMustBeDifferentFromDescription(ErrorMessage = "Titulo e Descrição não podem ser iguais")]
    public class CourseForCreationVM 
    {
        [Required(ErrorMessage = "Titulo não pode ser nulo.")]
        [MaxLength(100, ErrorMessage = "Titulo não pode ter mais de 100 caracteres.")]
        public string Title { get; set; }
        [MaxLength(1500 , ErrorMessage = "Descrição não pode ter mais de 1500 caracteres.")]
        public string Description { get; set; }       
    }
}
