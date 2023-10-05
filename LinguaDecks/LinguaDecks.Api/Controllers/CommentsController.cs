using LinguaDecks.Business.Exceptions;
using LinguaDecks.Business.Interfaces;
using LinguaDecks.DataAccess.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LinguaDecks.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CommentsController : ControllerBase
	{
		private readonly ICommentService _commentService;
		private readonly ICommentDeletter _commentDeletter;

		public CommentsController(ICommentService commentService, ICommentDeletter commentDeletter)
		{
			_commentService = commentService;
			_commentDeletter = commentDeletter;
		}

		[Authorize(Roles = "Student,Teacher,Admin")]
		[HttpPost]
		public async Task<IActionResult> AddComment(int userID, int dekcId, string comment)
		{
			await _commentService.AddComment(userID, dekcId, comment);
			return Ok();
		}

		[Authorize(Roles = "Student,Teacher,Admin")]
		[HttpDelete]
		public async Task<IActionResult> DeleteComment(int commentId,int userId,int deckId)
		{
			await _commentDeletter.DeleteComment(commentId, userId, deckId);
			return Ok();
		}
	}
}
