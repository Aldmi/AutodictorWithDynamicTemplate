using DAL.Abstract.Abstract;
using DAL.Abstract.Entitys;
using DAL.Abstract.Entitys.Authentication;

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


    /// <summary>
    /// Доступ к пользователям
    /// </summary>
    public interface IUsersRepository : IGenericDataRepository<User>
    {
    }


    /// <summary>
    /// Доступ к расписанию
    /// </summary>
    public interface ITrainTableRecRepository : IGenericDataRepository<TrainTableRec>
    {
    }
}