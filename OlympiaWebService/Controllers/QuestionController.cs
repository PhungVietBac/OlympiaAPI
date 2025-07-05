using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OlympiaWebService.Dto;
using OlympiaWebService.Interfaces;
using OlympiaWebService.Models;

namespace OlympiaWebService.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase {
        private readonly IQuestionRepository _questionRepository;
        private readonly IMapper _mapper;
        public QuestionController(IQuestionRepository questionRepository, IMapper mapper) {
            _questionRepository = questionRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<QuestionDto>))]
        public IActionResult GetQuestions () {
            var questions = _mapper.Map<List<QuestionDto>>(_questionRepository.GetQuestions());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(questions);
        }

        [HttpGet("ByID")]
        [ProducesResponseType(200, Type = typeof(QuestionDto))]
        [ProducesResponseType(404)]
        public IActionResult GetQuestionById(string id) {
            if (!_questionRepository.IsQuestionExists("id", id.ToString()))
                return NotFound();

            var question = _mapper.Map<QuestionDto>(_questionRepository.GetQuestionById(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(question);
        }

        [HttpGet("ByRound")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<QuestionDto>))]
        [ProducesResponseType(404)]
        public IActionResult GetQuestionsByRound(int round) {
            if (!_questionRepository.IsQuestionExists("round", round.ToString()))
                return NotFound();

            var question = _mapper.Map<List<QuestionDto>>(_questionRepository.GetQuestionsByRound(round));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(question);
        }

        [HttpGet("NormalRound2")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<QuestionDto>))]
        [ProducesResponseType(404)]
        public IActionResult GetNormalQuestionsRound2() {
            var question = _mapper.Map<List<QuestionDto>>(_questionRepository.GetNormalQuestionsRound2());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(question);
        }

        [HttpGet("MainQuestionsByRound")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<QuestionDto>))]
        [ProducesResponseType(404)]
        public IActionResult GetMainQuestionsByRound(int round) {
            var questions = _mapper.Map<List<QuestionDto>>(_questionRepository.GetMainQuestionsByRound(round));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(questions);
        }

        [HttpGet("MemberQuestions/{idQuestion}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<QuestionDto>))]
        [ProducesResponseType(404)]
        public IActionResult GetMemberQuestion(string idQuestion) {
            if (!_questionRepository.IsQuestionExists("id", idQuestion))
                return NotFound();

            var members = _mapper.Map<List<QuestionDto>>(_questionRepository.GetMemberQuestions(idQuestion));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(members);
        }

        [HttpGet("MainQuestionsFrom/{idQuestion}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<QuestionDto>))]
        [ProducesResponseType(404)]
        public IActionResult GetMainQuestionFrom(string idQuestion) {
            if (!_questionRepository.IsQuestionExists("id", idQuestion))
                return NotFound();

            var mains = _mapper.Map<List<QuestionDto>>(_questionRepository.GetMainQuestions(idQuestion));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(mains);
        }

        [HttpGet("Answer/{idQuestion}")]
        [ProducesResponseType(200, Type = typeof(AnswerDto))]
        [ProducesResponseType(404)]
        public IActionResult GetAnswerByQuestion(string idQuestion) {
            if (!_questionRepository.IsQuestionExists("id", idQuestion))
                return NotFound();

            var answer = _mapper.Map<AnswerDto>(_questionRepository.GetAnswerByQuestion(idQuestion));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(answer);
        }
    }
}
