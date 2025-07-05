using OlympiaWebService.Data;
using OlympiaWebService.Interfaces;
using OlympiaWebService.Models;

namespace OlympiaWebService.Repository {
    public class QuestionRepository : IQuestionRepository {
        private readonly DataContext _context;
        public QuestionRepository(DataContext context) {
            _context = context;
        }

        public Question GetQuestionById(string id) {
            return _context.Questions.Where(q => q.IDQuestion == id).FirstOrDefault();
        }

        public ICollection<Question> GetQuestions() {
            return _context.Questions.OrderBy(q => q.IDQuestion).ToList();
        }

        public bool IsQuestionExists(string select, string lookup) {
            if (select == "id")
                return _context.Questions.Any(q => q.IDQuestion == lookup);
            else if (select == "round")
                return _context.Questions.Any(q => q.Round == int.Parse(lookup));
            return false;
        }

        public ICollection<Question> GetQuestionsByRound(int round) {
            return _context.Questions.Where(q => q.Round == round).ToList();
        }

        public ICollection<Question> GetMemberQuestions(string id) {
            if (IsMainQuestion(id)) {
                GroupQuestionRepository groupQuestionRepository = new GroupQuestionRepository(_context);
                return groupQuestionRepository.GetQuestionMembers(id);
            }
            return null;
        }

        public ICollection<Question> GetMainQuestions(string id) {
            GroupQuestionRepository groupQuestionRepository = new GroupQuestionRepository(_context);
            return groupQuestionRepository.GetMainQuestion(id);
        }

        public bool IsMainQuestion(string id) {
            return _context.Questions.Where(q => q.IDQuestion == id).FirstOrDefault().IsMain;
        }

        public Answer GetAnswerByQuestion(string idQuestion) {
            return _context.Questions.Where(q => q.IDQuestion == idQuestion).Select(q => q.Answer).FirstOrDefault();
        }

        public ICollection<Question> GetMainQuestionsByRound(int round) {
            return _context.Questions.Where(q => q.IsMain && q.Round == round).ToList();
        }

        public ICollection<Question> GetNormalQuestionsRound2() {
            return _context.Questions.Where(q => !q.IsMain && q.Round == 2).ToList();
        }
    }
}
