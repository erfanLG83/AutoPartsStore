using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace AutoPartsStore.Services.Contract
{
    public interface IFileWorker
    {
        public Task<string> AddFileToPath(IFormFile file, string path);
        public Task AddFileToPath(IFormFile file, string path, string fileName);
        public void RemoveFileInPath(string path);
    }
}
