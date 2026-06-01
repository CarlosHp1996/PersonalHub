using AssistantAPI.Domain.Entities;
using AssistantAPI.Domain.Enums;
using AssistantAPI.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AssistantAPI.Infrastructure.Data.Repositories;

public class BillRepository : IBillRepository
{
    private readonly AppDbContext _dbContext;

    public BillRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Bill>> GetAllAsync(BillStatus? status = null)
    {
        var query = _dbContext.Bills.AsNoTracking();

        if (status.HasValue)
        {
            query = query.Where(b => b.Status == status.Value);
        }

        return await query.ToListAsync();
    }

    public async Task<Bill?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Bills.FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<Bill> CreateAsync(Bill bill)
    {
        _dbContext.Bills.Add(bill);
        await _dbContext.SaveChangesAsync();
        return bill;
    }

    public async Task<Bill> UpdateAsync(Bill bill)
    {
        bill.UpdatedAt = DateTime.UtcNow;
        _dbContext.Bills.Update(bill);
        await _dbContext.SaveChangesAsync();
        return bill;
    }

    public async Task DeleteAsync(Guid id)
    {
        var bill = await _dbContext.Bills.FindAsync(id);
        if (bill != null)
        {
            _dbContext.Bills.Remove(bill);
            await _dbContext.SaveChangesAsync();
        }
    }
}
