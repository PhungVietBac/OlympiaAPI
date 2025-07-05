using OlympiaWebService.Data;
using OlympiaWebService.Interfaces;
using OlympiaWebService.Models;

namespace OlympiaWebService.Repository {
    public class GroupQuestionRepository : IGroupQuestionRepository {
        private readonly DataContext _context;

        public GroupQuestionRepository(DataContext context) {
            _context = context;
        }

        public ICollection<GroupQuestion> GetGroupQuestions() {
            return _context.GroupQuestions.OrderBy(g => g.IDQuestion).ToList();
        }

        public ICollection<Question> GetMainQuestion(string id) {
            return _context.GroupQuestions.Where(g => (g.IDQuestion == id || g.IDGroup ==id))
                .Select(q => q.Member).ToList();
        }

        public ICollection<Question> GetQuestionMembers(string id) {
            return _context.GroupQuestions.Where(g => g.IDGroup == id).Select(q => q.Main).ToList();
        }

        public bool IsGroupQuestExists(string id) {
            return _context.GroupQuestions.Any(g => g.IDQuestion == id) ||
                _context.GroupQuestions.Any(g => g.IDGroup == id);
        }
    }
}
