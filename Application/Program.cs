using Library.Context;
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
        static void Main(string[] args)
        {
            ServiceCollection Services = new();

            ConfigureServices(Services);

            ServiceProvider ServiceProvider = Services.BuildServiceProvider();

            //TODO update to use repositories instead
            DataGenerator.GenerateStaticData(Context);
            DataGenerator.GenerateData(Context);

            Display Disp = new();


            while (true)
            {
                
                Char Input = Disp.ReadSingleCharFromDisplay();

                //TODO switch to repositories
                switch (Input)
                {
                    case 'A':
                        Disp.Case1(Context);
                        Disp.Reset();
                        break;
                    case 'C':
                        Disp.Case3(Context);
                        Disp.Reset();
                        break;
                    case 'E':
                        Disp.Case5();
                        Disp.Reset();
                        break;
                    case 'M':
                        Disp.MarioFunny();
                        Disp.ForceReset("");
                        break;
                    default:
                        Disp.ForceReset("Unacceptable input");
                        break;
                }
            }


        }

        public static void ConfigureServices(ServiceCollection SC)
        {

            var MongoConn = new MongoConnection("mongodb://localhost:27017");
            SC.AddSingleton(new MongoClient(MongoConn.ConnString));
            SC.AddSingleton<IBirthRepository, BirthRepository>();
            //SC.AddSingleton<IClinicianRepository, ClinicianRepository>();
            //SC.AddSingleton<IRoomRepository, RoomRepository>();
        }
    }
}
