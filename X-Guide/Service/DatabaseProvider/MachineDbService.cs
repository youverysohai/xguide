using AutoMapper;
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
    internal class MachineDbService : DbServiceBase, IMachineDbService
    {
        private readonly IMapper _mapper;

        public MachineDbService(IMapper mapper, DbContextFactory contextFactory) : base(contextFactory)
        {
            _mapper = mapper;
        }
        public void CreateMachine(MachineModel machine)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<string>> GetAllMachineName()
        {
            return await AsyncQuery((context) => context.Machines.Select(r => r.Name).ToList());

        }

        public async Task<MachineModel> GetMachine(string name)
        {
            return await AsyncQuery((context) => DBToModel(context.Machines.SingleOrDefault(r => r.Name == name)));
           
        }

        public async Task<IEnumerable<MachineModel>> GetAllMachine()
        {
            return await AsyncQuery((context) =>
            {
                IEnumerable<Machine> machines = context.Machines.ToList();
                return machines.Select(r => DBToModel(r));
            });
        }


        public async Task<bool> SaveMachine(MachineModel machine)
        {
            return await AsyncQuery((context) =>
            {
                var result = context.Machines.Find(machine.Id);
       
                if (result != null)
                {
                    context.Entry(result).CurrentValues.SetValues(ModelToDB(machine));
                    context.SaveChanges();
                    return true;
                }
                return false;
            });

        }

        Machine ModelToDB(MachineModel machine)
        {
            return _mapper.Map<Machine>(machine);
        }

        MachineModel DBToModel(Machine machine)
        {
            return _mapper.Map<MachineModel>(machine);
        }

        public string GetMachineDelimiter(string name)
        {
            /*   using (var context = _contextFactory.CreateDbContext())
               {
                   var result = context.Machines.FirstOrDefault(x => x.Name == name);


                   if (result != null)
                   {
                       return result.Terminator;
                   }

                   else return string.Empty;
               }*/
            return "";
        }

     
    }
}
