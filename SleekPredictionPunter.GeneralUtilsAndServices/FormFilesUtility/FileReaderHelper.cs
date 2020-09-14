using System;
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

		~FileReaderHelper()
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
	}
}
