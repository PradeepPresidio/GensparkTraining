using FirstAPI.Contexts;
using FirstAPI.Interfaces;
using FirstAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstAPI.Repositories
{
	public class DoctorRepository : Repository<int,Doctor>
	{
		protected Doctor(ClinicContext clinicContext) : base(clinicContext) {
		}
		public override async Task<Doctor> Get(int key)
		{
			var doctor = await _clinicContext.Doctors.SingleOrDefaultAsync(d=>d.id == key);
			return doctor ?? throw new Exception("No such Doctor available");
		}
		public override async Task<IEnumerable<Doctor>> GetAll()
		{
			var doctors = await _clinicContext.Doctors;
			if (doctors.Count == 0) {
				throw new Exception("No doctors in database");
			}
			return (await doctors.ToListAsync());
		}
	}
	}`