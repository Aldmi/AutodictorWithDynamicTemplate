using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using DAL.Abstract.Concrete;
using DAL.Abstract.Entitys;

namespace AutodictorBL.Services.DataAccessServices
{
    public class TrainRecWithSourceLoadService
    {
        #region field

        private readonly ITrainTableRecRepository _repLocalMain;
        private readonly ITrainTableRecRepository _repRemoteCis;
        private readonly ITrainTableRecRepository _repLocalOperative;
        private readonly TrainTypeByRyleService _trainTypeByRyleService;
        private readonly PathwaysService _pathwaysService;
        private readonly DirectionService _directionService;

        #endregion




        #region prop

        public TrainRecRepType SourceLoad { get; set; }

        #endregion





        #region Rx

        public Subject<TrainRecRepType> TableChangeRx { get; } = new Subject<TrainRecRepType>();

        #endregion



        #region ctor

        public TrainRecWithSourceLoadService(ITrainTableRecRepository repLocalMain,
                                             ITrainTableRecRepository repLocalOperative,
                                             ITrainTableRecRepository repRemoteCis,
                                             TrainTypeByRyleService trainTypeByRyleService,
                                             PathwaysService pathwaysService,
                                             DirectionService directionService)
        {
            _repLocalMain = repLocalMain;
            _repLocalOperative = repLocalOperative;
            _repRemoteCis = repRemoteCis;
            _trainTypeByRyleService = trainTypeByRyleService;
            _pathwaysService = pathwaysService;
            _directionService = directionService;
           
        }

        #endregion




        #region Methode

        public TrainTableRec GetById(int id)
        {
            var rep = (SourceLoad == TrainRecRepType.LocalMain) ? _repLocalMain : _repRemoteCis;
            return rep.GetById(id);
        }


        public IEnumerable<TrainTableRec> GetAll(TrainRecRepType? trainRecRepType = null)
        {
            var sourceLoad = trainRecRepType ?? SourceLoad;
            var rep = (sourceLoad == TrainRecRepType.LocalMain) ? _repLocalMain : _repRemoteCis;
            return rep.List();
        }

        public async Task<IEnumerable<TrainTableRec>> GetAllAsync(TrainRecRepType? trainRecRepType = null)
        {
            var sourceLoad = trainRecRepType ?? SourceLoad;
            var rep = (sourceLoad == TrainRecRepType.LocalMain) ? _repLocalMain : _repRemoteCis;
            return rep.List(); //TODO добавить async версии для репозиториев
        }


        public void ReWriteAll(IEnumerable<TrainTableRec> list, TrainRecRepType? trainRecRepType = null)
        {
            var sourceLoad = trainRecRepType ?? SourceLoad;
            var rep = (sourceLoad == TrainRecRepType.LocalMain) ? _repLocalMain : _repRemoteCis;
            rep.Delete(t => true);
            rep.AddRange(list);
            TableChangeRx.OnNext(SourceLoad);
        }


        public void DeleteItem(TrainTableRec item)
        {
            var rep = (SourceLoad == TrainRecRepType.LocalMain) ? _repLocalMain : _repRemoteCis;
            rep.Delete(item);
        }

        public void DeleteItem(Expression<Func<TrainTableRec, bool>> predicate)
        {
            var rep = (SourceLoad == TrainRecRepType.LocalMain) ? _repLocalMain : _repRemoteCis;
            rep.Delete(predicate);
        }


        public IEnumerable<Pathway> GetPathways(Expression<Func<Pathway, bool>> predicate = null)
        {
            return predicate == null ? _pathwaysService.GetAll() : _pathwaysService.GetAllByFilter(predicate);
        }

        public Pathway GetPathByName(string pathName)
        {
            return _pathwaysService.GetByName(pathName);
        }


        public IEnumerable<Direction> GetDirections(Expression<Func<Direction, bool>> predicate = null)
        {
            return predicate == null ? _directionService.GetAll() : _directionService.GetAllByFilter(predicate);
        }

        public Direction GetDirectionByName(string name)
        {
            return _directionService.GetByName(name);
        }

        public IEnumerable<Station> GetStationsInDirectionByName(string directionName)
        {
            return _directionService.GetStationsInDirectionByName(directionName);
        }

        public Station GetStationInDirectionByNameStation(string directionName, string stationNameRu)
        {
            return _directionService.GetStationInDirectionByNameStation(directionName, stationNameRu);
        }

        public Station GetStationInDirectionByCodeExpressStation(string directionName, int codeExpress)
        {
            return _directionService.GetStationInDirectionByCodeExpressStation(directionName, codeExpress);
        }

        public Station GetStationByCodeExpressStation(int codeExpress)
        {
            foreach (var direction in _directionService.GetAll())
            {
                var station = direction.Stations.FirstOrDefault(st => st.CodeExpress == codeExpress);
                if (station != null)
                    return station;
            }
            return null;
        }

        public TrainTypeByRyle GetTrainTypeByRyleById(int id)
        {
            return _trainTypeByRyleService.GetById(id);
        }

        public IEnumerable<TrainTypeByRyle> GetTrainTypeByRyles(Expression<Func<TrainTypeByRyle, bool>> predicate = null)
        {
            return predicate == null ? _trainTypeByRyleService.GetAll() : _trainTypeByRyleService.GetAllByFilter(predicate);
        }

        public int GetIndexOfRule(TrainTypeByRyle rule)
        {
            return _trainTypeByRyleService.GetIndexOf(rule);
        }


        public async Task SaveListByRemoteCis(IEnumerable<TrainTableRec> trainTableRecords)
        {
            ReWriteAll(trainTableRecords, TrainRecRepType.RemoteCis);
        }

        private ITrainTableRecRepository GetRepBySourceLoad()
        {
            switch (SourceLoad)
            {
                case TrainRecRepType.LocalMain:
                    return _repLocalMain;                    
                case TrainRecRepType.LocalOper:
                    return _repLocalOperative;
                case TrainRecRepType.RemoteCis:
                    return _repRemoteCis;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        #endregion




        public void Dispose()
        {
            if (!TableChangeRx.IsDisposed)
            {
                TableChangeRx.Dispose();
            }
        }
    }

}
