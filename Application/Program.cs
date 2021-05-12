using Library.DataGenerator;
using Library.Display;
using Library.Models.Rooms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;
using Library.Config;
using Library.Repository;
using System.Threading;
using System.Threading.Tasks;
using Library.Services;

namespace Application
{
    class Program
    {
        static async Task Main()
        {
            ServiceCollection Services = new();

            ConfigureServices(Services);

            ServiceProvider ServiceProvider = Services.BuildServiceProvider();
            //TODO update to use repositories instead
            DataGenerator Dg = new(ServiceProvider.GetService<IBirthRepository>(), ServiceProvider.GetService<IClinicianRepository>(), ServiceProvider.GetService<IRoomRepository>());
            await Dg.GenerateStaticData();
            await Dg.GenerateData();

            Display Disp = new(ServiceProvider.GetService<IBirthService>());


            while (true)
            {
                
                Char Input = Display.ReadSingleCharFromDisplay();

                //TODO switch to repositories
                switch (Input)
                {
                    case 'A':
                        Disp.Case1();
                        Display.Reset();
                        break;
                    case 'B':
                        Disp.Case3();
                        Display.Reset();
                        break;
                    case 'C':
                        Disp.Case5();
                        Display.Reset();
                        break;
                    case 'M':
                        Display.MarioFunny();
                        Display.ForceReset("");
                        break;
                    default:
                        Display.ForceReset("Unacceptable input");
                        break;
                }
            }


        }

        public static void ConfigureServices(ServiceCollection sc)
        {

            var MongoConn = new MongoConnection("mongodb://localhost:27017");
            sc.AddSingleton(new MongoClient(MongoConn.ConnString));
            sc.AddSingleton<IBirthRepository, BirthRepository>();
            sc.AddSingleton<IClinicianRepository, ClinicianRepository>();
            sc.AddSingleton<IRoomRepository, RoomRepository>();
            sc.AddSingleton<IBirthService, BirthService>();
        }
    }
}
