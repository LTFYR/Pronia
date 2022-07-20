using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Pronia.Utilities
{
    public static class FileValidator
    {
        public static async Task<string> FileCreate(this IFormFile form, string root, string folder)
        {
            string name = string.Concat(Guid.NewGuid(), form.FileName);
            string path = Path.Combine(root,folder);
            string file = Path.Combine(path, name);
            try
            {
                using (FileStream fileStream = new FileStream(file, FileMode.Create))
                {
                    await form.CopyToAsync(fileStream);
                }
            }
            catch (Exception)
            {

                throw new FileLoadException();
            }
            return name;
        }
        public static bool IsImageOk(this IFormFile file, int mb)
        {
            return file.Length / 1024 / 1024 < mb && file.ContentType.Contains("image/");
        }

        public static void FileDelete(string root,string folder,string image)
        {
            string path = Path.Combine(root, folder, image);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
