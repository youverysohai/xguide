using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_Guide.MVVM.DBContext;
using X_Guide.MVVM.Model;

namespace X_Guide.Service.DatabaseProvider
{
    internal class MachineService : IMachineService
    {
        private readonly DbContextFactory _contextFactory;

        public MachineService(DbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public void CreateMachine(MachineModel machine)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<string>> GetAllMachineName()
        {
            return await Task.Run(() => {
                using (var context = _contextFactory.CreateDbContext())
                {
                   return context.Machines.Select(r => r.Name).ToList();
                }
            });
            
        }

        public MachineModel GetMachine(string name)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var Machine = context.Machines.SingleOrDefault(r => r.Name == name);
                return DBToModel(Machine);
            }
        }

        public IEnumerable<MachineModel> GetAllMachine()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Machine> machines = context.Machines.ToList();
                return machines.Select(r => DBToModel(r));
            }
        }


        public void SaveMachine(MachineModel machine)
        {
            using (var context = _contextFactory.CreateDbContext())
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
                VisionPort = machine.VisionPort,
                ManipulatorTerminator = machine.ManipulatorTerminator,
                VisionTerminator = machine.VisionTerminator
            };
        }

        MachineModel DBToModel(Machine machine)
        {
            if (machine != null)
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
                    VisionPort = machine.VisionPort,
                    ManipulatorTerminator = machine.ManipulatorTerminator,
                    VisionTerminator = machine.VisionTerminator

                };
            }
            return null;
        }

        public string GetMachineDelimiter(string name)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var result = context.Machines.FirstOrDefault(x => x.Name == name);


                if (result != null)
                {

                    return result.ManipulatorTerminator;
                }
                else return string.Empty;
            }
        }
    }
}
