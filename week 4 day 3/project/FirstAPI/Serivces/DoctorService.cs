using FirstAPI.Interfaces;
using FirstAPI.Models;
namespace FirstAPI.Services
{
	public class DoctorService : IDoctorService
	{
		private readonly IRepository<int, Doctor> _doctorRepository;
		private readonly IRepository<int, Speciality> _specialityRepository;
		private readonly IRepository<int, DoctorSpeciality> _doctorSpecialityRepository;
		public DoctorService(IRepository<int, Doctor> doctorRepository,
							IRepository<int, Speciality> specialityRepository,
							IRepository<int, DoctorSpeciality> doctorSpecialityRepository)
		{
			_doctorRepository = doctorRepository;
			_specialityRepository = specialityRepository;
			_doctorSpecialityRepository = doctorSpecialityRepository;
		}
		public async Task<Doctor> GetDoctByName(string name)
		{
			var doctors = await _doctorRepository.GetAll();
			return doctors.FirstOrDefault(doc => doc.Name.Equals(name));
		}
		public async Task<ICollection<Doctor>> GetDoctorsbySpeciality(string speciality)
		{
			var allDoctors = await doctorRepository.GetAll();
			var filteredDoctors = await allDoctors.Where(doc => doc.DoctorSpecialities.Any(ds => ds.Speciality.Name == speciality)).ToList();
			return filteredDoctors;
		}
		public async Task<Doctor> AddDoctor(DoctorAddRequestDto doctor)
		{
			var newDoctor = new Doctor()
			{
				Name = doctor.Name,
				status = "Active",
				YearsOfExperience = doctor.YearsOfExperience,
			}
			var addedDoctor  = await _doctorRepository.Add(newDoctor);
			var newSpecialities = doctor.Specialities;
			newSpecialities.ForEach(spec =>
			{
				var speciality = new Speciality()
				{
					Name  = spec.Name,
					Status = "Active"
				}
				var addedSpeciality = await _specialityRepository.Add(speciality);
				var doctorSpec = new DoctorSpeciality()
				{
					DoctorId = addedDoctor.Id,
					SpecialityId = addedSpeciality.Id,
				};
				await _doctorSpecialityRepository.Add(doctorSpec);
			})
			return await _doctorRepository.add(newDoctor);
		}

	}
}