using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bottles.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class BottlesController : ControllerBase
    {
        private IBottleRepository repo;

        public BottlesController(IBottleRepository repo)
        {
            this.repo = repo;
        }

        // GET: api/<BottlesController>

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpGet]
        public ActionResult<IEnumerable<Bottle>> Get(
            [FromQuery] string? nameStartsWith, 
            [FromQuery] int? minVolume,
            [FromQuery] string? sortOrder)
        {
            IEnumerable<Bottle> bottles = repo.GetBottles(nameStartsWith, minVolume, sortOrder);
            if (bottles.Any())
            {
                return Ok(bottles);
            }
            
            return NoContent();
        }

        // GET api/<BottlesController>/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public ActionResult<Bottle?> Get([FromRoute] int id)
        {
            Bottle? bottle = repo.GetById(id);
            if (bottle == null)
            {
                return NotFound("No such bottle, id: " + id);
            }
            return Ok(bottle);
        }

        // POST api/<BottlesController>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPost]
        public ActionResult<Bottle> Post([FromBody] Bottle value)
        {

            Bottle newBottle = repo.AddBottle(value);
            return CreatedAtAction(nameof(Get), new { id = newBottle.Id }, newBottle);
        }

        // PUT api/<BottlesController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Bottle> Put(int id, [FromBody] Bottle value)
        {
            Bottle? bottle = repo.UpdateById(id, value);
            if (bottle == null)
            {
                return NotFound("No such bottle, id: " + id);
            }
            return Ok(bottle);
        }


        // DELETE api/<BottlesController>/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public ActionResult<Bottle> Delete(int id)
        {
            Bottle? bottle = repo.DeleteById(id);
            if (bottle == null)
            {
                return NotFound("No such bottle, id: " + id);
            }
            return Ok(bottle);
        }
    }
}

