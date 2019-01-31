using CityInfo.Api.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.Api.Services
{
    public interface ICityRepository
    {
        IEnumerable<City> GetCities();
        City GetCity(int id, bool includePointsOfInterest);
        IEnumerable<PointsOfInterest> GetPointsOfInterests(int cityId);
        PointsOfInterest GetPointsOfInterest(int cityId, int id);
        bool CityExist(int id);
        void CreatePointOfInterest(int cityId,PointsOfInterest pointsOfInterest);
        void DeletePointOfInterest(PointsOfInterest pointsOfInterest);
        bool Save();
    }
}
