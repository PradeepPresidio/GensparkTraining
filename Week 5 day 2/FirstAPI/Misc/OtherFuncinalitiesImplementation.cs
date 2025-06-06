using FirstAPI.Models.DTOs.DoctorSpecialities;
using FirstAPI.Contexts;
using FirstAPI.Interfaces;
using System;

namespace FirstAPI.Misc
{
    public class OtherFuncinalitiesImplementation : IOtherContextFunctionities
    {
        private readonly ClinicContext _clinicContext;

        public OtherFuncinalitiesImplementation(ClinicContext clinicContext)
        {
            _clinicContext = clinicContext;
        }

        public async Task<ICollection<DoctorsBySpecialityResponseDto>> GetDoctorsBySpeciality(string specilaity)
        {
            Console.WriteLine("In OtherFuncionalitiesImplementation");
            return new List<DoctorsBySpecialityResponseDto>
            {
                new DoctorsBySpecialityResponseDto
                {
                    Id = 1,
                    Dname = "Dr. Smith",
                    Yoe = 10
                },
                new DoctorsBySpecialityResponseDto
                {
                    Id = 2,
                    Dname = "Dr. Jones",
                    Yoe = 5
                }
            };
        }
    }
}