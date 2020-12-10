using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityApp.BL.DTOs
{
    public class DepartmentsDTO
    {
        public int DepartmentID { get; set; }
        public string Name { get; set; }
        public double Budget { get; set; }
        public DateTime StartDate { get; set; }
        public int InstructorID { get; set; }
        public InstructorDTO Instructor { get; set; }
    }
}
