using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OlympiaWebService.Dto;
using OlympiaWebService.Interfaces;
using OlympiaWebService.Models;
using OlympiaWebService.Repository;

namespace OlympiaWebService.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase {
        private readonly IMatchRepository _matchRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;
        public MatchController(IMatchRepository matchRepository, IPlayerRepository playerRepository,
            IRoomRepository roomRepository, IMapper mapper) {
            _matchRepository = matchRepository;
            _playerRepository = playerRepository;
            _roomRepository = roomRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MatchDto>))]
        public IActionResult GetMatches() {
            var matches = _mapper.Map<List<MatchDto>>(_matchRepository.GetMatches());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(matches);
        }

        [HttpGet("{select}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MatchDto>))]
        [ProducesResponseType(404)]
        public IActionResult GetMatchBy(string select, string lookup) {
            if (!_matchRepository.IsMatchExists(select, lookup))
                return NotFound();
            var matches = _mapper.Map<List<MatchDto>>(_matchRepository.GetMatchBy(select, lookup));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(matches);
        }

        [HttpGet("Player/{select}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PlayerDto>))]
        [ProducesResponseType(404)]
        public IActionResult GetPlayerByMatch(string select, string lookup) {
            if (!_matchRepository.IsMatchExists(select, lookup))
                return NotFound();

            var players = _mapper.Map<List<PlayerDto>>(_matchRepository.GetPlayerByMatch(select, lookup));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(players);
        }

        [HttpGet("Room/{select}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<RoomDto>))]
        [ProducesResponseType(404)]
        public IActionResult GetRoomByMatch(string select, string lookup) {
            if (!_matchRepository.IsMatchExists(select, lookup))
                return NotFound();

            var rooms = _mapper.Map<List<RoomDto>>(_matchRepository.GetRoomByMatch(select, lookup));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(rooms);
        }

        [HttpGet("Time/{select}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<DateTime>))]
        [ProducesResponseType(404)]
        public IActionResult GetTimeByMatch(string select, string lookup) {
            if (!_matchRepository.IsMatchExists(select, lookup))
                return NotFound();

            var times = _matchRepository.GetTimeByMatch(select, lookup);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(times);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult CreateMatch(string idPlayer, string idRoom) {
            if (idPlayer == "" || idRoom == "")
                return BadRequest(ModelState);

            DateTime time = DateTime.Now;

            var matchQuery = _matchRepository.GetMatches()
                .Where(m => m.IDPlayer == idPlayer && m.IDRoom == idRoom && m.Time == time)
                .FirstOrDefault();

            if (matchQuery != null) {
                ModelState.AddModelError("", "Match already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_playerRepository.IsPlayerExists("id", idPlayer) || !_roomRepository.IsRoomExists(idRoom)) {
                ModelState.AddModelError("", "Player or room is not available");
                return StatusCode(400, ModelState);
            }

            var matchTest = _matchRepository.GetMatches()
                .Where(m => m.IDRoom == idRoom && (m.Time - time).TotalMinutes < 5).ToList();

            if (matchTest.Count() == 4) {
                ModelState.AddModelError("", "This room is full");
                return StatusCode(422, ModelState);
            } else if (matchTest.Count() > 1)
                time = matchTest[0].Time;

            MatchDto match = new MatchDto() {
                IDPlayer = idPlayer,
                IDRoom = idRoom,
                Time = time
            };

            var matchMap = _mapper.Map<Match>(match);

            if (!_matchRepository.CreateMatch(matchMap)) {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteMatch(string idPlayer, string idRoom, string time) {
            if (idPlayer == "" || idRoom == "" || time == "")
                return BadRequest(ModelState);

            DateTime dateTime = new DateTime();

            if (!_playerRepository.IsPlayerExists("id", idPlayer) ||
                !_roomRepository.IsRoomExists(idRoom) ||
                !DateTime.TryParse(time, out dateTime))
                return NotFound();

            var matchDelete = _matchRepository.GetMatches()
                .Where(m => m.IDPlayer == idPlayer && m.IDRoom == idRoom && m.Time == dateTime)
                .FirstOrDefault();

            if (!ModelState.IsValid)
                return BadRequest();

            if (!_matchRepository.DeleteMatch(matchDelete)) {
                ModelState.AddModelError("", "Something went wrong while deleting");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully deleted");
        }

    }
}
