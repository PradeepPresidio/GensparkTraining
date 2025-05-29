using System.Threading.Tasks;
using FirstAPI.Interfaces;
using FirstAPI.Models;
using FirstAPI.Models.DTOs.DoctorSpecialities;
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
		public async Task<ICollection<Doctor>> GetDoctorsBySpeciality(string speciality)
		{
			var allDoctors = await _doctorRepository.GetAll();
			var filteredDoctors=  allDoctors.Where(doc => doc.DoctorSpecialities.Any(ds => ds.Speciality.Name == speciality)).ToList();
			return filteredDoctors;
		}
		public async Task<Doctor> AddDoctor(DoctorAddRequestDto doctor)
		{
			//insert doc into doctor table, then if speciality isnt in parent table, we insert it, then the same into doctor specialities
			Doctor newDoctor = mapDTOtoDoctor(doctor);
			var addedDoctor  = await _doctorRepository.Add(newDoctor);
			int[] specialityIds = await AddNewSpecialities(doctor.Specialities.ToList());
			await addNewDoctorSpecialities(addedDoctor.Id,specialityIds);
			return addedDoctor;
			//create entries into DoctorSpeciality table mapping specialityIds and doctorId
		}
		public async Task addNewDoctorSpecialities(int docId, int[] specialitiesIds)
		{
			for(int i =0;i<specialitiesIds.Length;i++)
			{
				DoctorSpeciality docSpec = new DoctorSpeciality()
				{
					DoctorId = docId,
					SpecialityId = specialitiesIds[i]
				};
				await _doctorSpecialityRepository.Add(docSpec);
			}
		}
		public async Task<int[]> AddNewSpecialities(List<SpecialityAddRequestDto> specDtos)
		{
			//return ids of speciality objs of new doctor. create and push the parent speciality table if speciality is not present 
			var allSpecialities = await _specialityRepository.GetAll();
			int[] specialityIds = new int[specDtos.Count];
			for (int i = 0; i < specDtos.Count; i++)
			{
				Speciality oldSpeciality = allSpecialities.SingleOrDefault(s => s.Name == specDtos[i].Name);
				if (oldSpeciality != null) {
					specialityIds[i] = oldSpeciality.Id;
				}
				else
				{
					//craete a new speciality
					Speciality newSpec = await CreateSpeciality(specDtos[i].Name);
					specialityIds[i] = newSpec.Id;
				}
			}
			return specialityIds;
		}
		public async Task<Speciality> CreateSpeciality(string name)
		{
			Speciality newSpec = new();
			newSpec.Name = name;
			newSpec.Status = "Active";
			return await _specialityRepository.Add(newSpec);
		}
		public Doctor mapDTOtoDoctor(DoctorAddRequestDto doctorDto)
		{
			return new Doctor()
			{
				Name = doctorDto.Name,
				Status = "Active",
				YearsOfExperience = doctorDto.YearsOfExperience,
				DoctorSpecialities = new List<DoctorSpeciality>(),
				Appointments = new List<Appointment>()
			};
		}
		public async Task<DoctorAddRequestDto> MapDoctorToDto(Doctor doctor)
		{
			var docSpecs = doctor.DoctorSpecialities;
			var allSpecialites = await _specialityRepository.GetAll();
			List<SpecialityAddRequestDto> dtoSpecs = new();
			//find the name of doctor's specialities and create specialityDtos
			foreach(var docSpec in docSpecs)
			{
				int specialityId = docSpec.SpecialityId;
				Speciality sp = allSpecialites.SingleOrDefault(s => s.Id == specialityId);
				string name = sp.Name;
				SpecialityAddRequestDto dto = new();
				dto.Name = name;
				dtoSpecs.Add(dto);
			}
			return new DoctorAddRequestDto()
			{
				Name = doctor.Name,
				YearsOfExperience = doctor.YearsOfExperience,
				Specialities = dtoSpecs
			};
		}
	}
}