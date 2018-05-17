using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DAL.Abstract.Concrete;
using DAL.Abstract.Entitys;

namespace AutodictorBL.Services.DataAccessServices
{
    public class DirectionService
    {
        private readonly IDirectionRepository _repository;



        #region ctor

        public DirectionService(IDirectionRepository repository)
        {
            _repository = repository;
        }

        #endregion




        #region Methode

        public Direction GetById(int id)
        {
            return _repository.GetById(id);
        }

        public Direction GetByName(string name)
        {
            return _repository.List().FirstOrDefault(dir=>dir.Name == name);
        }

        public IEnumerable<Direction> GetAll()
        {
            return _repository.List();
        }

        public IEnumerable<Direction> GetAllByFilter(Expression<Func<Direction, bool>> predicate)
        {
            return _repository.List(predicate);
        }

        public IEnumerable<Station> GetStationsInDirectionByName(string directionName)
        {
            return GetByName(directionName)?.Stations;
        }

        public IEnumerable<Station> GetStationsInDirectionById(int directionId)
        {
            return GetById(directionId).Stations;
        }

        public Station GetStationInDirectionByNameStation(string directionName, string stationNameRu)
        {
            return GetStationsInDirectionByName(directionName)?.FirstOrDefault(st => st.NameRu == stationNameRu);
        }

        public Station GetStationInDirectionByCodeExpressStation(string directionName, int codeExpress)
        {
            return GetStationsInDirectionByName(directionName)?.FirstOrDefault(st => st.CodeExpress == codeExpress);
        }

        #endregion
    }
}