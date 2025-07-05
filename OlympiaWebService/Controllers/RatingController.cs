using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OlympiaWebService.Dto;
using OlympiaWebService.Interfaces;
using OlympiaWebService.Models;
using OlympiaWebService.Repository;

namespace OlympiaWebService.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase {
        private readonly IRatingRepository _ratingRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IMapper _mapper;
        public RatingController(IRatingRepository ratingRepository, IPlayerRepository playerRepository,
            IMapper mapper) {
            _ratingRepository = ratingRepository;
            _playerRepository = playerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<RatingDto>))]
        public IActionResult GetRatings() {
            var ratings = _mapper.Map<List<RatingDto>>(_ratingRepository.GetRatings());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(ratings);
        }

        [HttpGet("{select}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<RatingDto>))]
        [ProducesResponseType(404)]
        public IActionResult GetRatingBy(string select, string lookup) {
            if (!_ratingRepository.IsRatingExists(select, lookup))
                return NotFound();

            var rating = _mapper.Map<List<RatingDto>>(_ratingRepository.GetRatingBy(select, lookup));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(rating);
        }

        [HttpGet("Player")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PlayerDto>))]
        [ProducesResponseType(404)]
        public IActionResult GetPlayerByRating(string time) {
            if (!_ratingRepository.IsRatingExists("time", time))
                return NotFound();

            var players = _mapper.Map<List<PlayerDto>>(_ratingRepository.GetPlayerByRating(time));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(players);
        }

        [HttpGet("Time")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<DateTime>))]
        [ProducesResponseType(404)]
        public IActionResult GetTimeByRating(string idPlayer) {
            if (!_ratingRepository.IsRatingExists("idPlayer", idPlayer))
                return NotFound();

            var times = _ratingRepository.GetTimeByRating(idPlayer);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(times);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult CreateRating(string idPlayer, string comment) {
            if (idPlayer == "")
                return BadRequest(ModelState);

            DateTime time = DateTime.Now;

            var ratingQuery = _ratingRepository.GetRatings()
                .Where(r => r.IDPlayer == idPlayer && r.Time == time)
                .FirstOrDefault();

            if (ratingQuery != null) {
                ModelState.AddModelError("", "Rating already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_playerRepository.IsPlayerExists("id", idPlayer)) {
                ModelState.AddModelError("", "Player is not available");
                return StatusCode(400, ModelState);
            }

            RatingDto rating = new RatingDto() {
                IDPlayer = idPlayer,
                Comment = comment,
                Time = time
            };

            var ratingMap = _mapper.Map<Rating>(rating);

            if (!_ratingRepository.CreateRating(ratingMap)) {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteRating(string idPlayer, string time) {
            DateTime dateTime = new DateTime();

            if (!_playerRepository.IsPlayerExists("id", idPlayer) ||
                !DateTime.TryParse(time, out dateTime))
                return NotFound();

            var ratingDelete = _ratingRepository.GetRatings()
                .Where(r => r.IDPlayer == idPlayer && r.Time == dateTime)
                .FirstOrDefault();

            if (!ModelState.IsValid)
                return BadRequest();

            if (!_ratingRepository.DeleteRating(ratingDelete)) {
                ModelState.AddModelError("", "Something went wrong while deleting");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully deleted");
        }
    }
}
