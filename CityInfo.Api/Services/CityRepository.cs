using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.Api.Entity;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.Api.Services
{
    public class CityRepository : ICityRepository
    {
        CityDbContext context;
        public CityRepository(CityDbContext _context)
        {
            context = _context;
        }
        public IEnumerable<City> GetCities()
        {
            return context.City.OrderBy(p => p.Name).ToList();
        }

        public City GetCity(int id, bool includePointsOfInterest)
        {
            if (includePointsOfInterest)
                return context.City.Include(c => c.PointsOfInterest).FirstOrDefault(c => c.Id == id);
            return context.City.FirstOrDefault(p => p.Id == id);
        }

        public PointsOfInterest GetPointsOfInterest(int cityId, int id)
        {
            return context.PointsOfInterest.FirstOrDefault(p => p.CityId == cityId && p.Id == id);
        }

        public IEnumerable<PointsOfInterest> GetPointsOfInterests(int cityId)
        {
            return context.PointsOfInterest.Where(p=>p.CityId==cityId).ToList();
        }
        public bool CityExist(int id)
        {
            return context.City.Any(c => c.Id == id);
        }

        public void CreatePointOfInterest(int cityId,PointsOfInterest pointsOfInterest)
        {
            var city = GetCity(cityId, false);
            city.PointsOfInterest.Add(pointsOfInterest);
        }

        public bool Save()
        {
            return (context.SaveChanges() >= 0);
        }

        public void DeletePointOfInterest(PointsOfInterest pointsOfInterest)
        {
            context.PointsOfInterest.Remove(pointsOfInterest); 
        }
    }
}
