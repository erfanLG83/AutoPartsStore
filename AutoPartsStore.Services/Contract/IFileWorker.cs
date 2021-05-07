using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace AutoPartsStore.Services.Contract
{
    public interface IFileWorker
    {
        public Task<string> AddFileToPathAsync(IFormFile file, string path);
        public Task AddFileToPathAsync(IFormFile file, string path, string fileName);
        public void RemoveFileInPath(string path);
    }
}
