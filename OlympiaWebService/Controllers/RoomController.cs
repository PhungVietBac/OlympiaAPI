using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OlympiaWebService.Dto;
using OlympiaWebService.Interfaces;
using OlympiaWebService.Models;
using OlympiaWebService.Repository;

namespace OlympiaWebService.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase {
        private readonly IRoomRepository _roomRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly IMapper _mapper;
        public RoomController(IRoomRepository roomRepository, IMatchRepository matchRepository,
            IMapper mapper) {
            _roomRepository = roomRepository;
            _matchRepository = matchRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<RoomDto>))]
        public IActionResult GetRooms() {
            var rooms = _mapper.Map<List<RoomDto>>(_roomRepository.GetRooms());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(rooms);
        }

        [HttpGet("{idRoom}")]
        [ProducesResponseType(200, Type = typeof(RoomDto))]
        [ProducesResponseType(404)]
        public IActionResult GetRoomById(string idRoom) {
            if (!_roomRepository.IsRoomExists(idRoom))
                return NotFound();

            var room = _mapper.Map<RoomDto>(_roomRepository.GetRoomById(idRoom));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(room);
        }

        [HttpGet("TypeRoom/{type}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<RoomDto>))]
        [ProducesResponseType(404)]
        public IActionResult GetRoom(string type) {
            var rooms = _mapper.Map<List<RoomDto>>(_roomRepository.GetRoom(type));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(rooms);
        }

        [HttpGet("Match/{idRoom}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MatchDto>))]
        [ProducesResponseType(404)]
        public IActionResult GetMatchByRoom(string idRoom) {
            if (!_roomRepository.IsRoomExists(idRoom))
                return NotFound();

            var matches = _mapper.Map<List<RoomDto>>(_roomRepository.GetMatchByRoom(idRoom));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(matches);
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(RoomDto))]
        [ProducesResponseType(400)]
        public IActionResult CreateRoom() {
            Random random = new Random();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Room room = new Room();
            room.IsFull = false;

            do {
                int temp = random.Next(1, 9999);
                string id;
                if (temp < 10)
                    id = "R000" + temp;
                else if (temp < 100)
                    id = "R00" + temp;
                else if (temp < 1000)
                    id = "R0" + temp;
                else
                    id = "R" + temp;
                bool test = _roomRepository.IsRoomExists(id);
                if (!test) {
                    room.IDRoom = id;
                    break;
                }
            } while (true);

            if (!_roomRepository.CreateRoom(room)) {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

           var roomDto = _mapper.Map<RoomDto>(room);

            return Ok(roomDto);
        }

        [HttpDelete]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteRoom(string idRoom) {
            if (idRoom == "")
                return BadRequest(ModelState);

            if (!_roomRepository.IsRoomExists(idRoom))
                return NotFound();

            var roomDelete = _roomRepository.GetRoomById(idRoom);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var matches = _roomRepository.GetMatchByRoom(idRoom);
            foreach (var match in matches) {
                _matchRepository.DeleteMatch(match);
            }

            if (!_roomRepository.DeleteRoom(roomDelete)) {
                ModelState.AddModelError("", "Something went wrong while deleting");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully deleted");
        }

    }
}
