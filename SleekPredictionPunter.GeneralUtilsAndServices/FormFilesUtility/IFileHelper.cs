using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace SleekPredictionPunter.GeneralUtilsAndServices
{
	public interface IFileHelper : IDisposable
	{
		Task<FileCreationResult> SaveFileToCustomWebRootPath(string directory, IFormFile formFile);
		Task<FileCreationResult> SaveFileToTeamWebRootPath(IFormFile formFile);
	}
}