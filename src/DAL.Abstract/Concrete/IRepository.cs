using DAL.Abstract.Abstract;
using DAL.Abstract.Entitys;

namespace DAL.Abstract.Concrete
{
    /// <summary>
    /// Доступ к правилам по типам поездов
    /// </summary>
    public interface ITrainTypeByRyleRepository : IGenericDataRepository<TrainTypeByRyle>
    {  
    }


    /// <summary>
    /// Доступ к направлениям
    /// </summary>
    public interface IDirectionRepository : IGenericDataRepository<Direction>
    {
    }


    /// <summary>
    /// Доступ к путям
    /// </summary>
    public interface IPathwaysRepository : IGenericDataRepository<Pathways>
    {
    }
}