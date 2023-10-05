using AutoMapper;
using LinguaDecks.Api.Models;
using LinguaDecks.Api.Models.AuthModels;
using LinguaDecks.Api.Models.DeckModels;
using LinguaDecks.Business.Models;
using LinguaDecks.DataAccess.Entities;
using System.Collections.Generic;
using System.Linq;

namespace LinguaDecks.Api.Mappers
{
	public class MapperConfig : Profile
	{
		public MapperConfig()
		{
			CreateMap<User, SimpleUserModel>().ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));
			
			CreateMap<RefreshToken, RefreshTokenModel>().ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));
			
			CreateMap<TokenResponse, TokenResponseModel>().ForMember(dest => dest.RefreshToken, opt => opt.MapFrom(src => src.RefreshToken));
			
			CreateMap<Card, CardModel>();

			CreateMap<Comment, CommentModel>()
				.ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));

			CreateMap<Deck, DeckModel>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
			
			CreateMap<Deck, DeckPreview>()
				.ForMember(dest => dest.CardsCount, opt => opt.MapFrom(src => src.Cards.Count))
				.ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author.Name))
				.ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Ratings.Any() ? src.Ratings.Sum(r => r.Value) / src.Ratings.Count : 0))//винести окремо обчислення
				.ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name));

			CreateMap<Category, CategoryModel>();

			CreateMap<CardAnswerModel, CardProgress>();

			CreateMap<SaveProgressRequest, IEnumerable<CardProgress>>()
				.ConvertUsing(request => request.Answers.Select(answer => new CardProgress
				{
					CardId = answer.CardId,
					UserId = request.UserId,
					IsKnown = answer.IsKnown
				}));
			CreateMap<TeacherRequest, TeacherRequestModel>()
				.ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.Name));

			CreateMap<AddCardRequest, Card>();

			CreateMap<(int positive, int negative, int total), ProgressModel>()
				.ForMember(dest => dest.Positive, opt => opt.MapFrom(src => src.positive))
				.ForMember(dest => dest.Negative, opt => opt.MapFrom(src => src.negative))
				.ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.total));

			CreateMap<IEnumerable<CardProgress>, ProgressModel>()
				.ForMember(dest => dest.Positive, opt => opt.MapFrom(src => src.Count(cp => cp.IsKnown)))
				.ForMember(dest => dest.Negative, opt => opt.MapFrom(src => src.Count(cp => !cp.IsKnown)))
				.ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.Count()));
		}
	}

}
