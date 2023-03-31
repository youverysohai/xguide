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
    internal class ManipulatorDbService : DbServiceBase, IManipulatorDbService
    {
    

        public ManipulatorDbService(IMapper mapper, DbContextFactory contextFactory) : base(contextFactory, mapper)
        { 
        }

        public void CreateManipulator(ManipulatorModel manipulator)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<string>> GetAllManipulatorName()
        {
            return await AsyncQuery((context) => context.Manipulators.Select(r => r.Name).ToList());

        }

        public async Task<ManipulatorModel> GetManipulator(string name)
        {
            return await AsyncQuery((context) => MapTo<ManipulatorModel>(context.Manipulators.SingleOrDefault(r => r.Name == name)));
           
        }

        public async Task<IEnumerable<ManipulatorModel>> GetAllManipulator()
        {
            return await AsyncQuery((context) =>
            {
                IEnumerable<Manipulator> manipulators = context.Manipulators.ToList();
                return manipulators.Select(r => MapTo<ManipulatorModel>(r));
            });
        }


        public async Task<bool> SaveManipulator(ManipulatorModel manipulator)
        {
            return await AsyncQuery((context) =>
            {
                var result = context.Manipulators.Find(manipulator.Id);
       
                if (result != null)
                {
                    context.Entry(result).CurrentValues.SetValues(MapTo<Manipulator>(manipulator));
                    context.SaveChanges();
                    return true;
                }
                return false;
            });

        }


        public string GetManipulatorDelimiter(string name)
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
