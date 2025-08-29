using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayDayExplosion.Application.Dtos.Panama;
using PayDayExplosion.Application.Interfaces.Panama;
using PayDayExplosion.Application.Services;
using PayDayExplosion.Domain.Data.Entities;
using System.Diagnostics;



namespace PayDayExplosion.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExplosionController : ControllerBase
    {
        private readonly ILogger<ExplosionController> _logger;
        private readonly IExplosionPAService _explosionService;

        public ExplosionController(ILogger<ExplosionController> logger, IExplosionPAService explosionService)
        {
            _logger = logger;
            _explosionService = explosionService;
        }



        [HttpPost]
        [Authorize]
        [Route("explosionTestPA")]
        public async Task<IActionResult> ExplosionTestPA([FromBody] ExplosionTestPADto explosion)
        {
            try
            {
                var result = await _explosionService.ExplosionTestPAAsync(explosion);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error interno en Explosion.");
                return StatusCode(500, "Error interno.");
            }
        }
    }
}