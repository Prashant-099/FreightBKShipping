using FreightBKShipping.DTOs;

namespace FreightBKShipping.Services
{
    public interface ITransactionService
    {
        Task<int> SaveTransaction(TransactionDto dto);
        Task DeleteTransactionByVoucher(int voucherId, int voucherTypeId);
        Task UpdateTransaction(TransactionDto dto);
    }
}
