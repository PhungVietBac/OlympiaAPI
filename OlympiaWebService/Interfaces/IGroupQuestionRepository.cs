using OlympiaWebService.Models;

namespace OlympiaWebService.Interfaces {
    public interface IGroupQuestionRepository {
        ICollection<GroupQuestion> GetGroupQuestions();
        bool IsGroupQuestExists(string id);
        ICollection<Question> GetQuestionMembers(string id);
        ICollection<Question> GetMainQuestion(string id);
    }
}
