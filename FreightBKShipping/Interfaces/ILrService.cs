

using FreightBKShipping.Models;

namespace FreightBKShipping.Interfaces
{
    public interface ILrService
    {
        Task<List<Lr>> SearchByPartyAndDate(int partyId, DateTime? fromDate, DateTime? toDate);
        Task<List<Lr>> GetAll();
        Task<Lr?> GetById(int id);
    }
}
