using Library.DataGenerator;
using Library.Display;
using Library.Models.Rooms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;
using Library.Config;
using Library.Repository;

namespace Application
{
    class Program
    {
        static void Main()
        {
            ServiceCollection Services = new();

            ConfigureServices(Services);

            ServiceProvider ServiceProvider = Services.BuildServiceProvider();

            //TODO update to use repositories instead
            DataGenerator.GenerateStaticData(ServiceProvider.GetService<ClinicianRepository>(), ServiceProvider.GetService<RoomRepository>());
            DataGenerator.GenerateData(ServiceProvider.GetService<ClinicianRepository>(), ServiceProvider.GetService<RoomRepository>(), ServiceProvider.GetService<BirthRepository>());

            Display Disp = new(ServiceProvider.GetService<BirthRepository>(), ServiceProvider.GetService<ClinicianRepository>(), ServiceProvider.GetService<RoomRepository>());


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
                    case 'C':
                        Disp.Case3();
                        Display.Reset();
                        break;
                    case 'E':
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

        public static void ConfigureServices(ServiceCollection SC)
        {

            var MongoConn = new MongoConnection("mongodb://localhost:27017");
            SC.AddSingleton(new MongoClient(MongoConn.ConnString));
            SC.AddSingleton<IBirthRepository, BirthRepository>();
            SC.AddSingleton<IClinicianRepository, ClinicianRepository>();
            SC.AddSingleton<IRoomRepository, RoomRepository>();
        }
    }
}
