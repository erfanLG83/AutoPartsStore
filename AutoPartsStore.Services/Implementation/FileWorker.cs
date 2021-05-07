using AutoPartsStore.Services.Contract;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AutoPartsStore.Services.Implementation
{
    public class FileWorker : IFileWorker
    {
        private readonly string _rootPath;
        public FileWorker(string rootPath)
        {
            _rootPath = rootPath;
        }
        public async Task<string> AddFileToPathAsync(IFormFile file, string path)
        {
            string name = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            path = Path.Combine(_rootPath, path, name);
            using Stream stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);
            return name;
        }

        public async Task AddFileToPathAsync(IFormFile file, string path, string fileName)
        {
            path = Path.Combine(_rootPath, path, fileName);
            using (Stream stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
        }

        public void RemoveFileInPath(string path)
        {
            try
            {
                File.Delete(_rootPath + path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
