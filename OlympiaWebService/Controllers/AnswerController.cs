using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OlympiaWebService.Dto;
using OlympiaWebService.Interfaces;
using OlympiaWebService.Models;

namespace OlympiaWebService.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AnswerController : ControllerBase {
        private readonly IAnswerRepository _answerRepository;
        private readonly IMapper _mapper;

        public AnswerController(IAnswerRepository answerRepository, IMapper mapper) {
            _answerRepository = answerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<AnswerDto>))]
        public IActionResult GetAnswers() {
            var answers = _mapper.Map<List<AnswerDto>>(_answerRepository.GetAnswers());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(answers);
        }

        [HttpGet("{idAnswer}")]
        [ProducesResponseType(200, Type = typeof(AnswerDto))]
        [ProducesResponseType(404)]
        public IActionResult GetAnswer(string idAnswer) {
            if (!_answerRepository.IsAnswerExists(idAnswer))
                return NotFound();

            var answer = _mapper.Map<AnswerDto>(_answerRepository.GetAnswerById(idAnswer));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(answer);
        }

        [HttpGet("ImageAnswer")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<AnswerDto>))]
        [ProducesResponseType(404)]
        public IActionResult GetImageAnswer() {
            var answers = _mapper.Map<List<AnswerDto>>(_answerRepository.GetImageAnswers());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(answers);
        }

        [HttpGet("Question/{idAnswer}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<QuestionDto>))]
        [ProducesResponseType(404)]
        public IActionResult GetQuestionByAnswer(string idAnswer) {
            if (!_answerRepository.IsAnswerExists(idAnswer))
                return NotFound();

            var question = _mapper.Map<List<QuestionDto>>(_answerRepository.GetQuestionByAnswer(idAnswer));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(question);
        }
    }
}
