using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_Guide.MVVM.DBContext;
using X_Guide.MVVM.Model;

namespace X_Guide.Service.DatabaseProvider
{
    internal class DatabaseMachineService : IMachineService
    {
        private readonly DbContextFactory _contextFactory;

        public DatabaseMachineService(DbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public void CreateMachine(MachineModel machine)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MachineModel>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        public MachineModel GetMachine(string name)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var Machine = context.Machines.SingleOrDefault(r => r.Name == name);
                return DBToModel(Machine);
            }
        }

        public void SaveMachine(MachineModel machine)
        {
            using(var context = _contextFactory.CreateDbContext())
            {
                var result = context.Machines.Find(machine.Id);
 
               
                if (result != null)
                {

                    context.Entry(result).CurrentValues.SetValues(ModelToDB(machine));
                    context.SaveChanges();
                }
            }
        }

        Machine ModelToDB(MachineModel machine)
        {
            return new Machine
            {
                Id = machine.Id,
                Name = machine.Name,
                Description = machine.Description,
                ManipulatorIP = machine.ManipulatorIP,
                ManipulatorPort = machine.ManipulatorPort,
                Type = machine.Type,
                VisionIP = machine.VisionIP,
                VisionPort = machine.VisionPort
            };
        }

        MachineModel DBToModel(Machine machine)
        {
            return new MachineModel
            {
                Id = machine.Id,
                Name = machine.Name,
                Description = machine.Description,
                Type = machine.Type,
                ManipulatorIP = machine.ManipulatorIP,
                ManipulatorPort = machine.ManipulatorPort,
                VisionIP = machine.VisionIP,
                VisionPort = machine.VisionPort

            };
        }
    }
}
