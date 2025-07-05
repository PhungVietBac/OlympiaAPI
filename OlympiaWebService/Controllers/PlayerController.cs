using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OlympiaWebService.Dto;
using OlympiaWebService.Interfaces;
using OlympiaWebService.Models;

namespace OlympiaWebService.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase {
        private readonly IPlayerRepository _playerRepository;
        private readonly IMapper _mapper;
        private readonly IMatchRepository _matchRepository;
        private readonly IFriendRepository _friendRepository;
        private readonly IRatingRepository _ratingRepository;

        public PlayerController(IPlayerRepository playerRepository, 
            IMatchRepository matchRepository, IFriendRepository friendRepository,
            IRatingRepository ratingRepository, IMapper mapper) { 
            _playerRepository = playerRepository;
            _matchRepository = matchRepository;
            _friendRepository = friendRepository;
            _ratingRepository = ratingRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PlayerDto>))]
        public IActionResult GetPlayers() {
            var players = _mapper.Map<List<PlayerDto>>(_playerRepository.GetPlayers());

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(players);
        }

        [HttpGet("{select}")]
        [ProducesResponseType(200, Type = typeof(PlayerDto))]
        [ProducesResponseType(404)]
        public IActionResult GetPlayerBy(string select, string lookup) {
            if (!_playerRepository.IsPlayerExists(select, lookup))
                return NotFound();

            var players = _mapper.Map<PlayerDto>(_playerRepository.GetPlayerBy(select, lookup));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(players);
        }

        [HttpGet("ByName")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PlayerDto>))]
        [ProducesResponseType(404)]
        public IActionResult GetPlayerByName(string name) {
            if (!_playerRepository.IsPlayerExists("name", name))
                return NotFound();

            var player = _mapper.Map<List<PlayerDto>>(_playerRepository.GetPlayerByName(name));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(player);
        }

        [HttpGet("ByAvartar")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PlayerDto>))]
        [ProducesResponseType(404)]
        public IActionResult GetPlayerByAvartar(string data) {
            if (!_playerRepository.IsPlayerExists("avatar", data))
                return NotFound();

            var players = _mapper.Map<List<PlayerDto>>(_playerRepository.GetPlayerByAvatar(data));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(players);
        }

        [HttpGet("Match/{select}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<IEnumerable<MatchDto>>))]
        [ProducesResponseType(404)]
        public IActionResult GetMatchFromPlayer(string select, string lookup) {
            if (!_playerRepository.IsPlayerExists(select, lookup))
                return NotFound();

            var matches = _mapper.Map<List<List<MatchDto>>>(_playerRepository.GetMatchFromPlayer(select, lookup));
 
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(matches);
        }

        [HttpGet("Rating/{select}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<IEnumerable<RatingDto>>))]
        [ProducesResponseType(404)]
        public IActionResult GetRaringFromPlayer(string select, string lookup) {
            if (!_playerRepository.IsPlayerExists(select, lookup))
                return NotFound();

            var ratings = _mapper.Map<List<List<RatingDto>>>(_playerRepository.GetRatingFromPlayer(select, lookup));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(ratings);
        }

        [HttpGet("Friend/{select}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<IEnumerable<PlayerDto>>))]
        [ProducesResponseType(404)]
        public IActionResult GetFriendFromPlayer(string select, string lookup) {
            if (!_playerRepository.IsPlayerExists(select, lookup))
                return NotFound();

            var friends = _mapper.Map<List<List<PlayerDto>>>(_playerRepository.GetFriendFromPlayer(select, lookup));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(friends);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult CreatePlayer([FromBody] PlayerDtoSignup playerCreate) {
            Random random = new Random();
            if (playerCreate == null)
                return BadRequest(ModelState);

            var player = _playerRepository.GetPlayers()
                .Where(p => p.Username.Trim().ToUpper() == playerCreate.Username.Trim().ToUpper() ||
                p.Email.Trim().ToUpper() == playerCreate.Email.Trim().ToUpper() ||
                p.PhoneNumber.Trim().ToUpper() == playerCreate.PhoneNumber.Trim().ToUpper())
                .FirstOrDefault();

            if (player != null) {
                ModelState.AddModelError("", "Player already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var playerMap = _mapper.Map<Player>(playerCreate);

            do {
                int temp = random.Next(1, 999);
                string id;
                if (temp < 10)
                    id = "ID00" + temp;
                else if (temp < 100)
                    id = "ID0" + temp;
                else
                    id = "ID" + temp;
                bool test = _playerRepository.IsPlayerExists("id", id);
                if (!test) {
                    playerMap.IDPlayer = id;
                    break;
                }
            } while (true);

            if (!_playerRepository.CreatePlayer(playerMap)) {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdatePlayer([FromBody] PlayerDto playerUpdate) {
            if (playerUpdate == null)
                return BadRequest(ModelState);

            if (!_playerRepository.IsPlayerExists("id", playerUpdate.IDPlayer))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Player beforePlayer = _playerRepository.GetPlayerBy("id", playerUpdate.IDPlayer);
            List<Player> afterPlayer =
            [
                _playerRepository.GetPlayerBy("username", playerUpdate.Username),
                _playerRepository.GetPlayerBy("email", playerUpdate.Email),
                _playerRepository.GetPlayerBy("phone", playerUpdate.PhoneNumber)
            ];
            for (int i = 0; i < 3; i++) {
                if (afterPlayer[i] != null && afterPlayer[i].IDPlayer != beforePlayer.IDPlayer) {
                    if (i == 0) {
                        ModelState.AddModelError("", "Username already exists");
                        return StatusCode(422, ModelState);
                    } else if (i == 1) {
                        ModelState.AddModelError("", "Email already exists");
                        return StatusCode(422, ModelState);
                    } else {
                        ModelState.AddModelError("", "Phone number already exists");
                        return StatusCode(422, ModelState);
                    }
                }
            }

            var playerMap = _mapper.Map<Player>(playerUpdate);

            if (!_playerRepository.UpdatePlayer(playerMap)) {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeletePlayer(string idPlayer) {
            if (idPlayer == "")
                return BadRequest(ModelState);

            if (!_playerRepository.IsPlayerExists("id", idPlayer))
                return NotFound();

            var playerDelete = _playerRepository.GetPlayerBy("id", idPlayer);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var matches = _playerRepository.GetMatchFromPlayer("id", idPlayer);
            foreach (var temp in matches) {
                foreach (var match in temp)
                    _matchRepository.DeleteMatch(match);
            }

            var friends = _friendRepository.GetFriends()
                .Where(f => f.IDSelf == idPlayer || f.IDFriend == idPlayer).ToList();
            foreach (var friend in friends) {
                    _friendRepository.DeleteFriend(friend);
            }

            var ratings = _playerRepository.GetRatingFromPlayer("id", idPlayer);
            foreach (var temp in ratings) {
                foreach (var rating in temp)
                    _ratingRepository.DeleteRating(rating);
            }

            if (!_playerRepository.DeletePlayer(playerDelete)) {
                ModelState.AddModelError("", "Something went wrong while deleting");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully deleted");
        }
    }
}
