using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;

namespace LinguaDecks.Business.Interfaces
{
	public interface IImageService
	{
		Task<Uri> SaveAsync(IFormFile file);

		Task DeleteAsync(Uri uri);
	}
}
