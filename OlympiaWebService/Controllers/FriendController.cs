using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OlympiaWebService.Dto;
using OlympiaWebService.Interfaces;
using OlympiaWebService.Models;
using OlympiaWebService.Repository;

namespace OlympiaWebService.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class FriendController : ControllerBase {
        private readonly IFriendRepository _friendRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IMapper _mapper;

        public FriendController(IFriendRepository friendRepository, IPlayerRepository playerRepository,
            IMapper mapper) {
            _friendRepository = friendRepository;
            _playerRepository = playerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<FriendDto>))]
        public IActionResult GetFriends() {
            var friends = _mapper.Map<List<FriendDto>>(_friendRepository.GetFriends());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(friends);
        }

        [HttpGet("{idPlayer}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PlayerDto>))]
        [ProducesResponseType(404)]
        public IActionResult GetFriend (string idPlayer) {
            if (!_friendRepository.IsFriendExists(idPlayer))
                return NotFound();

            var friends = _mapper.Map<List<PlayerDto>>(_friendRepository.GetFriend(idPlayer));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(friends);
        }

        [HttpGet("Request/{idPlayer}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PlayerDto>))]
        [ProducesResponseType(404)]
        public IActionResult GetRequest(string idPlayer) {
            if (!_friendRepository.IsFriendExists(idPlayer))
                return NotFound();

            var friends = _mapper.Map<List<PlayerDto>>(_friendRepository.GetRequest(idPlayer));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(friends);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult CreateFriend(string idPlayer, string idFriend) {
            if (idPlayer == "" || idFriend == "")
                return BadRequest(ModelState);

            var friendQuery = _friendRepository.GetFriends()
                .Where(f => f.IDSelf == idPlayer && f.IDFriend == idFriend)
                .FirstOrDefault();

            if (friendQuery != null) {
                ModelState.AddModelError("", "Friend relationship already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_playerRepository.IsPlayerExists("id", idPlayer) || !_playerRepository.IsPlayerExists("id", idFriend)) {
                ModelState.AddModelError("", "Player is not available");
                return StatusCode(400, ModelState);
            }

            FriendDto friend = new FriendDto() {
                IDSelf = idPlayer,
                IDFriend = idFriend,
                IsAcpt = false
            };

            var friendMap = _mapper.Map<Friend>(friend);

            if (!_friendRepository.CreateFriend(friendMap)) {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult UpdateFriend(string idPlayer, string idFriend) {
            int select = 0;
            if (idPlayer == "" || idFriend == "")
                return BadRequest(ModelState);

            if (!_friendRepository.IsFriendPairExists(idPlayer, idFriend))
                select = 0;
            else
                select = 1;

            if (!_friendRepository.IsFriendPairExists(idFriend, idPlayer))
                select = 0;
            else
                select = 2;


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_playerRepository.IsPlayerExists("id", idPlayer) || !_playerRepository.IsPlayerExists("id", idFriend)) {
                ModelState.AddModelError("", "Player is not available");
                return StatusCode(400, ModelState);
            }

            FriendDto friend = new FriendDto();
            if (select == 1) {
                friend = new FriendDto() {
                    IDSelf = idPlayer,
                    IDFriend = idFriend,
                    IsAcpt = true
                };
            } else if (select == 2) {
                friend = new FriendDto() {
                    IDSelf = idFriend,
                    IDFriend = idPlayer,
                    IsAcpt = true
                };
            } else
                return NotFound();

            var friendMap = _mapper.Map<Friend>(friend);

            if (!_friendRepository.UpdateFriend(friendMap)) {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteFriend(string idSelf, string idFriend) {
            if (idSelf == "" || idFriend == "")
                return BadRequest(ModelState);

            if (!_playerRepository.IsPlayerExists("id", idSelf) ||
                !_playerRepository.IsPlayerExists("id", idFriend))
                return NotFound();

            var friendDelete1 = _friendRepository.GetFriends()
                .Where(f => f.IDSelf == idSelf && f.IDFriend == idFriend)
                .FirstOrDefault();

            var friendDelete2 = _friendRepository.GetFriends()
                .Where(f => f.IDSelf == idFriend && f.IDFriend == idSelf)
                .FirstOrDefault();

            if (!ModelState.IsValid)
                return BadRequest();

            if (friendDelete1 != null) {
                if (!_friendRepository.DeleteFriend(friendDelete1)) {
                    ModelState.AddModelError("", "Something went wrong while deleting");
                    return StatusCode(500, ModelState);
                }
            } else if (friendDelete2 != null) {
                if (!_friendRepository.DeleteFriend(friendDelete2)) {
                    ModelState.AddModelError("", "Something went wrong while deleting");
                    return StatusCode(500, ModelState);
                }
            } else {
                return NotFound();
            }

            return Ok("Successfully deleted");
        }

    }
}
