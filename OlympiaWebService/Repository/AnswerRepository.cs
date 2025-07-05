using OlympiaWebService.Data;
using OlympiaWebService.Interfaces;
using OlympiaWebService.Models;

namespace OlympiaWebService.Repository {
    public class AnswerRepository : IAnswerRepository {
        private readonly DataContext _context;

        public AnswerRepository(DataContext context) {
            _context = context;
        }

        public Answer GetAnswerById(string id) {
            return _context.Answers.Where(a => a.IDAnswer == id).FirstOrDefault();
        }

        public ICollection<Answer> GetAnswers() {
            return _context.Answers.OrderBy(a => a.IDAnswer).ToList();
        }

        public ICollection<Answer> GetImageAnswers() {
            return _context.Answers.Where(a => a.Picture != null).ToList();
        }

        public ICollection<Question> GetQuestionByAnswer(string idAnswer) {
            return _context.Questions.Where(q => q.IDAnswer == idAnswer).ToList();
        }

        public bool IsAnswerExists(string id) {
            return _context.Answers.Any(a => a.IDAnswer == id);
        }

        public bool IsImageAnswer(string id) {
            return _context.Answers.Where(a => a.IDAnswer == id).FirstOrDefault()
                .Picture != null ? true : false;
        }
    }
}
