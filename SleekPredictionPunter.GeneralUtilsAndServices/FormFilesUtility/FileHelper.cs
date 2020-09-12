using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.GeneralUtilsAndServices
{
    public class FileCreationResult
    {
        public FileResultEnum FileStatueEnum { get; set; }
        public string CreatedFileRelativePath { get; set; }
    }
    public enum FileResultEnum
    {
        FileNotFound = 1,
        FileCreationError = 2,
        FileCreationSuccess = 3
    }
    public class FileHelper : IFileHelper
    {
        public async Task<FileCreationResult> SaveFileToCustomWebRootPath(string directory, IFormFile formFile)
        {
            System.Random random = new System.Random();
            int genNumber = random.Next(1234567890);

            if (formFile == null || formFile.Length == 0)
                return new FileCreationResult { FileStatueEnum = FileResultEnum.FileNotFound };
            var relativePath = Path.Combine($"wwwroot/{directory}", Path.GetFileName(genNumber + formFile.FileName));
            var path = Path.Combine(Directory.GetCurrentDirectory(), relativePath);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
                await stream.FlushAsync();
            }

            return new FileCreationResult { FileStatueEnum = FileResultEnum.FileCreationSuccess, CreatedFileRelativePath = relativePath };
        }

        public async Task<FileCreationResult> SaveFileToTeamWebRootPath(IFormFile formFile)
        {
            System.Random random = new System.Random();
            int genNumber = random.Next(1234567890);

            if (formFile == null || formFile.Length == 0)
                return new FileCreationResult { FileStatueEnum = FileResultEnum.FileNotFound };

            var relativePath = Path.Combine($"wwwroot/TeamLogo", Path.GetFileName(genNumber + formFile.FileName));
            var path = Path.Combine(Directory.GetCurrentDirectory(), relativePath);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
                await stream.FlushAsync();
            }

            return new FileCreationResult { FileStatueEnum = FileResultEnum.FileCreationSuccess, CreatedFileRelativePath = relativePath };
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~FileHelper()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
