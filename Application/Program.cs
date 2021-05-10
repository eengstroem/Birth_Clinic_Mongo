using Library.Context;
using Library.DataGenerator;
using Library.Display;
using Library.Models.Rooms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;

namespace Application
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceCollection Services = new();

            ConfigureServices(Services);

            ServiceProvider ServiceProvider = Services.BuildServiceProvider();
            var Context = ServiceProvider.GetService<BirthClinicDbContext>();
            DataGenerator.GenerateStaticData(Context);
            DataGenerator.GenerateData(Context);

            Display Disp = new();


            while (true)
            {
                
                Char Input = Disp.ReadSingleCharFromDisplay();

                switch (Input)
                {
                    case 'A':
                        Disp.Case1(Context);
                        Disp.Reset();
                        break;
                    case 'B':
                        Disp.Case2(Context);
                        Disp.Reset();
                        break;
                    case 'C':
                        Disp.Case3(Context);
                        Disp.Reset();
                        break;
                    case 'D':
                        Disp.Case4(Context);
                        Disp.Reset();
                        break;
                    case 'E':
                        Disp.Case5(Context);
                        Disp.Reset();
                        break;
                    case 'M':
                        Disp.MarioFunny();
                        Disp.ForceReset("");
                        break;
                    case 'F':
                        Console.Clear();
                        Console.WriteLine("Please type one of the following letters to make a choice on what to create.");
                        Console.WriteLine("B: Birth\nR: Reservations");
                        char Choice = Disp.ReadSingleCharFromDisplay();
                        switch (Choice)
                        {
                            case 'B':
                                if (DataGenerator.CreateBirth(Context))
                                {
                                    Disp.ForceReset("A birth has been added.");
                                }
                                else
                                {
                                    Disp.ForceReset("An error occoured.");
                                }
                                break;
                            case 'R':
                                Console.Clear();
                                Console.WriteLine("Please type one of the following letters to make a choice on what to reserve.");
                                Console.WriteLine("B: Birth room\nM: Maternity Room\nR: Rest room");
                                char ReservationChoice = Disp.ReadSingleCharFromDisplay();

                                switch(ReservationChoice){
                                    case 'B':
                                        if (DataGenerator.CreateReservation(Context, RoomType.BIRTH))
                                        {
                                            Disp.ForceReset("A Reservation has been added.");
                                        }
                                        else
                                        {
                                            Disp.ForceReset("An error occoured, there may be no more available rooms.");
                                        }
                                        break;
                                    case 'M':
                                        if (DataGenerator.CreateReservation(Context, RoomType.MATERNITY))
                                        {
                                            Disp.ForceReset("A Reservation has been added.");
                                        }
                                        else
                                        {
                                            Disp.ForceReset("An error occoured, there may be no more available rooms.");
                                        }
                                        break;
                                    case 'R':
                                        if (DataGenerator.CreateReservation(Context, RoomType.REST))
                                        {
                                            Disp.ForceReset("A Reservation has been added.");
                                        }
                                        else
                                        {
                                            Disp.ForceReset("An error occoured, there may be no more available rooms.");
                                        }
                                        break;
                                    default:
                                        Disp.ForceReset("Unacceptable input");
                                        break;
                                }
                                
                                break;
                            default:
                                Disp.ForceReset("Unacceptable input");
                                break;

                        }
                        break;
                    case 'H':
                        Console.Clear();
                        Console.WriteLine("Please type one of the following letters to make a choice on what to remove.");
                        Console.WriteLine("B: Birth\nR: Reservations");
                        char RemovalChoice = Disp.ReadSingleCharFromDisplay();
                        switch (RemovalChoice)
                        {
                            case 'B':
                                Disp.EndBirth(Context);
                                break;
                            case 'R':
                                Disp.RemoveReservation(Context);
                                break;
                            default:
                                Disp.ForceReset("Unacceptable input");
                                break;
                        }
                        break;
                    default:
                        Disp.ForceReset("Unacceptable input");
                        break;
                }
            }


        }

        public static void ConfigureServices(ServiceCollection SC)
        {
            SC.AddDbContext<BirthClinicDbContext>(options =>
            {
                options.UseSqlServer("server=[::1],1433; User Id=SA; Password=password_123; database=myDb; trusted_connection=false;");
                
        });
        }
    }
}
