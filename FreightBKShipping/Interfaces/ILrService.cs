

using FreightBKShipping.Models;

namespace FreightBKShipping.Interfaces
{
    public interface ILrService
    {
        Task<List<Lr>> SearchByPartyAndDate(int partyId, DateTime? fromDate, DateTime? toDate);
        Task<List<Lr>> GetAll();
        Task<Lr?> GetById(int id);

        //Task<Lr> Create(Lr model);
        //Task<bool> Update(Lr model);

        Task<Lr> Create(Lr model, List<LRDetail> details, List<LRJournal> journals);
        Task<bool> Update(Lr model, List<LRDetail> details, List<LRJournal> journals);
        Task<bool> Delete(int id);
    }
}
