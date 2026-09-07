using HealthClinicDemo.Api.Data;
using HealthClinicDemo.Api.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace HealthClinicDemo.Api.Repositories
{
    public class TipsRepository
    {
        MyHealthContext _context;

        public TipsRepository(MyHealthContext dbcontext)
        {
            _context = dbcontext;
        }

        public async Task<Tip> GetNextAsync(int tenantId)
        {
            return await _context.Tips
                .Where(t => t.TenantId == tenantId)
                .OrderByDescending(t => t.Date)
                .FirstOrDefaultAsync();
        }
    }
}
