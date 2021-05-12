using Library.Models.Births;
using Library.Models.Rooms;
using Library.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.Services
{
    public class BirthService : IBirthService
    {
        private readonly IBirthRepository _birthRepository;

        public BirthService(IBirthRepository birthRepository)
        {
            _birthRepository = birthRepository;
        }
        public IEnumerable<Birth> GetAllWithinTimespan(DateTime startDate, DateTime endDate)
        {
            return _birthRepository.GetAll().Where(b => b.BirthDate >= startDate && b.BirthDate <= endDate).ToList();
        }

        public IEnumerable<Birth> GetAllBirthsUsingABirthRoomAtTime(DateTime time)
        {
            var births = _birthRepository.GetAll().Where(b => b.Reservations.Any(r => r.Room.RoomType == RoomType.BIRTH && r.StartTime <= time && r.EndTime >= time)).ToList();
            return births;
        }

        public IEnumerable<Birth> GetAll()
        {
            return _birthRepository.GetAll().ToList();
        }
    }
}
