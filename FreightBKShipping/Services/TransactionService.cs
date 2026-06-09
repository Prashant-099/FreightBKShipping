using FreightBKShipping.Data;
using FreightBKShipping.DTOs;
using FreightBKShipping.Models;
using Microsoft.EntityFrameworkCore;

namespace FreightBKShipping.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly AppDbContext _context;

        public TransactionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveTransaction(TransactionDto dto)
        {
            // Validation
            double totalDr = dto.Details.Sum(x => x.Debit);
            double totalCr = dto.Details.Sum(x => x.Credit);

            totalDr = Math.Round(totalDr, 2);

            totalCr = Math.Round(totalCr, 2);

            if (Math.Abs(totalDr - totalCr) > 0.01)
            {
                throw new Exception(
                    $"Debit and Credit mismatch. DR={totalDr}, CR={totalCr}");
            }

            // Header
            var transaction = new Transaction
            {
                TransactionCompanyId = dto.CompanyId,
                TransactionVoucherId = dto.VoucherId,
                TransactionVoucherTypeId = dto.VoucherTypeId,
                TransactionAccountId = dto.AccountId,
                TransactionAmount = dto.Amount,
                TransactionDate = dto.Date,
                TransactionRemarks = dto.Remarks,
                TransactionVoucherNo = dto.VoucherNo,
                TransactionVoucherType = dto.VoucherType,
                TransactionStatus = true,
                TransactionCreated = DateTime.Now,
                TransactionUpdated = DateTime.Now
            };

            _context.Transactions.Add(transaction);

            await _context.SaveChangesAsync();

            // Details
            foreach (var item in dto.Details)
            {
                var detail = new TransactionDetail
                {
                    TransactionDetailCompanyId = dto.CompanyId,
                    TransactionDetailTransactionId = transaction.TransactionId,
                    TransactionDetailPartyId = item.PartyId,
                    TransactionDetailAccountId = item.AccountId,
                    Debit = item.Debit,
                    Credit = item.Credit,
                    TransactionDetailStatus = true,
                    TransactionDetailCreated = DateTime.Now,
                    TransactionDetailUpdated = DateTime.Now
                };

                _context.TransactionDetails.Add(detail);
            }

            await _context.SaveChangesAsync();

            return transaction.TransactionId;
        }


        //for  SOFT DELETE TRANSCTION AND DETAILS
        public async Task DeleteTransactionByVoucher(
    int voucherId,
    int voucherTypeId)
        {
            var transaction = await _context.Transactions
                .FirstOrDefaultAsync(x =>
                    x.TransactionVoucherId == voucherId &&
                    x.TransactionVoucherTypeId == voucherTypeId &&
                    x.TransactionStatus == true);

            if (transaction == null)
                return;

            // =========================
            //  DETAILS hard delete
            // =========================

            var details = await _context.TransactionDetails
                .Where(x =>
                    x.TransactionDetailTransactionId ==
                    transaction.TransactionId)
                .ToListAsync();

            //foreach (var item in details)
            //{
            //    item.TransactionDetailStatus = false;
            //    item.TransactionDetailUpdated = DateTime.UtcNow;
            //}
            if (details.Any())
            {
                _context.TransactionDetails.RemoveRange(details);
            }
            // =========================
            // HEADER
            // =========================

            transaction.TransactionStatus = false;
            transaction.TransactionUpdated = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }


        //update

        public async Task UpdateTransaction(TransactionDto dto)
        {
            // =========================
            // FIND TRANSACTION
            // =========================

            var transaction = await _context.Transactions
                .Include(x => x.TransactionDetails)
                .FirstOrDefaultAsync(x =>
                    x.TransactionVoucherId == dto.VoucherId &&
                    x.TransactionVoucherTypeId == dto.VoucherTypeId);

            if (transaction == null)
            {
                throw new Exception("Transaction not found.");
            }

            // =========================
            // DELETE OLD DETAILS
            // =========================

            if (transaction.TransactionDetails.Any())
            {
                _context.TransactionDetails.RemoveRange(
                    transaction.TransactionDetails);
            }

            // =========================
            // UPDATE HEADER
            // =========================

            transaction.TransactionDate = dto.Date;
            transaction.TransactionAmount = dto.Amount;
            transaction.TransactionRemarks = dto.Remarks;
            transaction.TransactionAccountId = dto.AccountId;
            transaction.TransactionUpdated = DateTime.UtcNow;

            // =========================
            // ADD NEW DETAILS
            // =========================

            foreach (var item in dto.Details)
            {
                transaction.TransactionDetails.Add(
                    new TransactionDetail
                    {
                        TransactionDetailCompanyId = dto.CompanyId,
                        TransactionDetailPartyId = item.PartyId,
                        TransactionDetailAccountId = item.AccountId,

                        Debit =
                            Math.Round(item.Debit, 2),

                        Credit =
                            Math.Round(item.Credit, 2),

                        TransactionDetailCreated =
                            DateTime.UtcNow,

                        TransactionDetailUpdated =
                            DateTime.UtcNow,

                        TransactionDetailStatus = true
                    });
            }

            // =========================
            // VALIDATION
            // =========================

            double totalDr = dto.Details.Sum(x => x.Debit);
            double totalCr = dto.Details.Sum(x => x.Credit);

            totalDr = Math.Round(totalDr, 2);
            totalCr = Math.Round(totalCr, 2);

            if (Math.Abs(totalDr - totalCr) > 0.01)
            {
                throw new Exception(
                    $"Debit and Credit mismatch. DR={totalDr}, CR={totalCr}");
            }

            await _context.SaveChangesAsync();
        }
    }
}
