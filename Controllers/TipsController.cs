using Microsoft.AspNetCore.Mvc;
using HealthClinicDemo.Api.Repositories;
using HealthClinicDemo.Api.Models;
using HealthClinicDemo.Api.Data;
using System.Threading.Tasks;

namespace HealthClinicDemo.Api.Controllers
{
    [ResponseCache(Duration = 0, NoStore = true, VaryByHeader = "*")]
    [Route("api/[controller]")]
    public class TipsController : Controller
    {
        private readonly TipsRepository _TipsRepository;

        public TipsController(TipsRepository tipsRepository)
        {
            _TipsRepository = tipsRepository;
        }

        [HttpGet("next")]
        public async Task<Tip> Get()
        {
            return await _TipsRepository.GetNextAsync(Request.GetTenant());
        }
    }
}
