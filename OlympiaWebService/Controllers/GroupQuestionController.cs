using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OlympiaWebService.Dto;
using OlympiaWebService.Interfaces;
using OlympiaWebService.Models;

namespace OlympiaWebService.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class GroupQuestionController : ControllerBase {
        private readonly IGroupQuestionRepository _groupQuestionRepository;
        private readonly IMapper _mapper;

        public GroupQuestionController(IGroupQuestionRepository groupQuestionRepository, IMapper mapper) {
            _groupQuestionRepository = groupQuestionRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<GroupQuestionDto>))]
        public IActionResult GetGroupQuestions() {
            var grquests = _mapper.Map<List<GroupQuestionDto>>(_groupQuestionRepository.GetGroupQuestions());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(grquests);
        }

        [HttpGet("Question/Member/{idQuestion}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<QuestionDto>))]
        [ProducesResponseType(404)]
        public IActionResult GetMemberQuestions(string idQuestion) {
            if (!_groupQuestionRepository.IsGroupQuestExists(idQuestion))
                return NotFound();

            var members = _mapper.Map<List<QuestionDto>>(_groupQuestionRepository.GetQuestionMembers(idQuestion));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(members);
        }

        [HttpGet("Question/Main/{idQuestion}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<QuestionDto>))]
        [ProducesResponseType(404)]
        public IActionResult GetMainQuestions(string idQuestion) {
            if (!_groupQuestionRepository.IsGroupQuestExists(idQuestion))
                return NotFound();

            var members = _mapper.Map<List<QuestionDto>>(_groupQuestionRepository.GetMainQuestion(idQuestion));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(members);
        }
    }
}
