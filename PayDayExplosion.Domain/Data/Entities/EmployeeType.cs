using PayDayExplosion.Domain.Data.Common;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PayDayExplosion.Domain.Data.Entities
{
    public class EmployeeType : Base
    {
        [Display(Name = "Código")]
        public string Code { get; set; } = string.Empty;

        [Display(Name = "Nombre")]
        public string Name { get; set; } = string.Empty;






        //public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
