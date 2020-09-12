using System.IO;
using System.Threading.Tasks;

namespace SleekPredictionPunter.GeneralUtilsAndServices
{
	public class FileReaderHelper
    {
        public async static Task<string> ReadAsync(string relativePath)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), relativePath);
            return await Task.FromResult(path);
        }
    }
}
