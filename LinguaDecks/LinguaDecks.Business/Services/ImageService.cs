using System;
using System.IO;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using LinguaDecks.Business.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace LinguaDecks.Business.Services
{
	public class ImageService : IImageService
	{
		private readonly Cloudinary _cloudinary;

		public ImageService(IConfiguration configuration)
		{
			_cloudinary = new Cloudinary(new Account(
				configuration["CloudinarySettings:CloudName"],
				configuration["CloudinarySettings:ApiKey"],
				configuration["CloudinarySettings:ApiSecret"]));
		}

		public async Task<Uri> SaveAsync(IFormFile file)
		{
			ImageUploadResult uploadResult = new ImageUploadResult();

			if (file.Length > 0)
			{
				using (Stream stream = file.OpenReadStream())
				{
					ImageUploadParams uploadParams = new ImageUploadParams
					{
						File = new FileDescription(file.Name, stream),
						PublicId = Guid.NewGuid() + Path.GetFileNameWithoutExtension(file.Name)
					};
					uploadResult = await _cloudinary.UploadAsync(uploadParams);
				}
			}

			return uploadResult.StatusCode is System.Net.HttpStatusCode.OK ? uploadResult.Url : null;
		}

		public async Task DeleteAsync(Uri uri)
		{
			await _cloudinary.DestroyAsync(new DeletionParams(uri.ToString()));
		}
	}
}
