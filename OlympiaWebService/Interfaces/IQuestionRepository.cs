using OlympiaWebService.Models;

namespace OlympiaWebService.Interfaces {
    public interface IQuestionRepository {
        ICollection<Question> GetQuestions();
        Question GetQuestionById(string id);
        bool IsQuestionExists(string select, string lookup);
        bool IsMainQuestion(string id);
        ICollection<Question> GetQuestionsByRound(int round);
        ICollection<Question> GetNormalQuestionsRound2();
        ICollection<Question> GetMainQuestionsByRound(int round);
        ICollection<Question> GetMemberQuestions(string id);
        ICollection<Question> GetMainQuestions(string id);
        Answer GetAnswerByQuestion(string idQuestion);
    }
}
