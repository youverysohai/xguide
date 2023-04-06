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
    internal class ManipulatorDb : DbServiceBase, IManipulatorDb
    {


        public ManipulatorDb(IMapper mapper, DbContextFactory contextFactory) : base(contextFactory, mapper)
        {
        }

        public async Task<bool> Add(ManipulatorModel manipulator)
        {

            return await AsyncQuery((context) =>
            {
                context.Manipulators.Add(MapTo<Manipulator>(manipulator));
                context.SaveChanges();
                return true;
            });
        }

        public async Task<IEnumerable<string>> GetAllNames()
        {
            return await AsyncQuery((context) => context.Manipulators.Select(r => r.Name).ToList());

        }

        public async Task<ManipulatorModel> Get(string name)
        {
            return await AsyncQuery((context) => MapTo<ManipulatorModel>(context.Manipulators.SingleOrDefault(r => r.Name == name)));

        }

        public async Task<IEnumerable<ManipulatorModel>> GetAll()
        {
            return await AsyncQuery((context) =>
            {
                IEnumerable<Manipulator> manipulators = context.Manipulators.ToList();
                return manipulators.Select(r => MapTo<ManipulatorModel>(r));
            });
        }


        public async Task<bool> Update(ManipulatorModel manipulator)
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


        public string GetDelimiter(string name)
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

        public async Task<ManipulatorModel> Get(int id)
        {
            return await AsyncQuery((context) =>
            {
                var i = context.Manipulators.Find(id);
                return MapTo<ManipulatorModel>(i);
            });
        }
    }
}
