using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GameStore.Web.Controllers.API.Admin
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class APIAdminController : ControllerBase
    {
        private readonly IGameRepository _repository;


        public APIAdminController(IGameRepository repository)
        {
            _repository = repository;
        }

        // GET: api/APIAdmin
        [HttpGet]
        public async Task<IActionResult> GetGames()
        {
            return Ok(await _repository.GetGamesAsync());
        }

        // GET: api/APIAdmin/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGame([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var game = await _repository.FindAsync(id);

            if (game == null)
            {
                return NotFound();
            }

            return Ok(game);
        }

        // PUT: api/APIAdmin/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame([FromRoute] int id, [FromBody] Game game)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != game.GameId)
            {
                return BadRequest();

            }
            await _repository.SaveGameAsync(game);
            return NoContent();
        }

        // POST: api/APIAdmin
        [HttpPost]
        public async Task<IActionResult> PostGame([FromBody] Game game)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _repository.SaveGameAsync(game);
            return CreatedAtAction("GetGame", new { id = game.GameId }, game);
        }

        // DELETE: api/APIAdmin/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var game = await _repository.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            await _repository.DeleteGameAsync(id);
            return Ok(game);
        }
    }
}