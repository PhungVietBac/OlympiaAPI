using OlympiaWebService.Models;

namespace OlympiaWebService.Interfaces {
    public interface IAnswerRepository {
        ICollection<Answer> GetAnswers();
        bool IsAnswerExists(string id);
        Answer GetAnswerById(string id);
        ICollection<Answer> GetImageAnswers();
        bool IsImageAnswer(string id);
        ICollection<Question> GetQuestionByAnswer(string idAnswer);
    }
}
