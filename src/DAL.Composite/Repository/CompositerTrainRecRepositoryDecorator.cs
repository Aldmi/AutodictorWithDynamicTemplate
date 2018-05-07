using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DAL.Abstract.Concrete;
using DAL.Abstract.Entitys;

namespace DAL.Composite.Repository
{
    public class CompositerTrainRecRepositoryDecorator : ITrainTableRecRepository, IDisposable
    {
        private readonly ITrainTableRecRepository _trainTableRecRep;
        private readonly ITrainTypeByRyleRepository _trainTypeByRyleRep;
        private readonly IPathwaysRepository _pathwaysRep;
        private readonly IDirectionRepository _directionRep;




        #region ctor

        public CompositerTrainRecRepositoryDecorator(ITrainTableRecRepository trainTableRecRep,
                                                     ITrainTypeByRyleRepository trainTypeByRyleRep,
                                                     IPathwaysRepository pathwaysRep,
                                                     IDirectionRepository directionRep)
        {
            _trainTableRecRep = trainTableRecRep;
            _trainTypeByRyleRep = trainTypeByRyleRep;
            _pathwaysRep = pathwaysRep;
            _directionRep = directionRep;
        }

        #endregion




        #region Methode

        public TrainTableRec GetById(int id)
        {
           var tableRec= _trainTableRecRep.GetById(id);
           return RestorationDependencies(tableRec);
        }


        public IEnumerable<TrainTableRec> List()
        {
            var resultList = new List<TrainTableRec>();
            var tableRecs= _trainTableRecRep.List()?.ToList();
            if (tableRecs != null && tableRecs.Any())
            {
                foreach (var tableRec in tableRecs)
                {
                    resultList.Add(RestorationDependencies(tableRec));
                }
            }
            return resultList;
        }


        public IEnumerable<TrainTableRec> List(Expression<Func<TrainTableRec, bool>> predicate)
        {
            var resultList = new List<TrainTableRec>();
            var tableRecs = _trainTableRecRep.List(predicate)?.ToList();
            if (tableRecs != null && tableRecs.Any())
            {
                foreach (var tableRec in tableRecs)
                {
                    resultList.Add(RestorationDependencies(tableRec));
                }
            }
            return resultList;
        }


        public void Add(TrainTableRec entity)
        {
            _trainTableRecRep.Add(entity);
        }


        public void AddRange(IEnumerable<TrainTableRec> entitys)
        {
            _trainTableRecRep.AddRange(entitys);
        }


        public void Delete(TrainTableRec entity)
        {
            _trainTableRecRep.Delete(entity);
        }


        public void Delete(Expression<Func<TrainTableRec, bool>> predicate)
        {
           _trainTableRecRep.Delete(predicate);
        }


        public void Edit(TrainTableRec entity)
        {
            _trainTableRecRep.Edit(entity);
        }


        /// <summary>
        /// Восстановление связей (Direction, TrainTypeByRyle, TrainPathNumber) для TrainTableRec.
        /// </summary>
        private TrainTableRec RestorationDependencies(TrainTableRec trainTableRec)
        {
            var directionId = trainTableRec.Direction?.Id;
            if (directionId.HasValue)
            {
                var direction = _directionRep.GetById(directionId.Value);
                trainTableRec.Direction = direction;

                var stationsByDirection = direction.Stations;
                if (stationsByDirection != null && stationsByDirection.Any())
                {
                    trainTableRec.StationArrival = stationsByDirection.FirstOrDefault(st=> st.Id == trainTableRec.StationArrival.Id);
                    trainTableRec.StationDepart = stationsByDirection.FirstOrDefault(st=> st.Id == trainTableRec.StationDepart.Id);
                    for (var i = 0; i < trainTableRec.Route.Stations.Count; i++)
                    {
                        var station = trainTableRec.Route.Stations[i];
                        var statRestore = stationsByDirection.FirstOrDefault(st => st.Id == station.Id);
                        trainTableRec.Route.Stations[i] = statRestore;
                    }
                }
            }

            var trainTypeByRyleId = trainTableRec.TrainTypeByRyle?.Id;
            if (trainTypeByRyleId.HasValue)
            {
                var trainTypeByRyle = _trainTypeByRyleRep.GetById(trainTypeByRyleId.Value);
                trainTableRec.TrainTypeByRyle = trainTypeByRyle;
            }

            for (var i = 0; i < trainTableRec.TrainPathNumber.Count; i++)
            {
                var pathKey = trainTableRec.TrainPathNumber.Keys.ElementAt(i);
                var pathValue = trainTableRec.TrainPathNumber.Values.ElementAt(i);
                var pathId = pathValue?.Id;
                if (pathId.HasValue)
                {
                    var path= _pathwaysRep.GetById(pathId.Value);
                    trainTableRec.TrainPathNumber[pathKey] = path;
                }
            }

            return trainTableRec;
        }

        #endregion




        #region Disposable

        public void Dispose()
        {
            return;
            //throw new NotImplementedException();
        }

        #endregion

    }
}