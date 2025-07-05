using AutoMapper;
using OlympiaWebService.Dto;
using OlympiaWebService.Models;

namespace OlympiaWebService.Helper {
    public class MappingProfiles : Profile {
        public MappingProfiles()
        {
            CreateMap<Player, PlayerDto>(); 
            CreateMap<PlayerDto, Player>();
            CreateMap<PlayerDtoSignup, Player>();

            CreateMap<Room, RoomDto>();

            CreateMap<Match, MatchDto>();
            CreateMap<MatchDto, Match>();

            CreateMap<Friend, FriendDto>();
            CreateMap<FriendDto, Friend>();

            CreateMap<Rating, RatingDto>();
            CreateMap<RatingDto, Rating>();

            CreateMap<Question, QuestionDto>();

            CreateMap<GroupQuestion, GroupQuestionDto>();

            CreateMap<Answer, AnswerDto>();
        }

    }
}
