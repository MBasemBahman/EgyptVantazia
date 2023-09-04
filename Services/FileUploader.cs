using Microsoft.AspNetCore.Http;
using System.Drawing;
using System.Drawing.Imaging;

namespace Services
{
    public class FileUploader : IFileUploader
    {
        private readonly string _rootPath;

        public FileUploader(string rootPath)
        {
            _rootPath = rootPath;
        }

        public void DeleteFile(string filePath)
        {
            if (File.Exists(Path.Combine(_rootPath, filePath)))
            {
                File.Delete(Path.Combine(_rootPath, filePath));
            }
        }

        public async Task<string> UploudFile(IFormFile file, string storagePath)
        {
            string fileName = file.FileName + DateTime.UtcNow.ToString("ddMMyyyyhhmmssfffffffK");
            fileName += Path.GetExtension(file.FileName);


            string folderName = Path.Combine(_rootPath, storagePath);
            if (!Directory.Exists(folderName))
            {
                _ = Directory.CreateDirectory(folderName);
            }

            string fullPath = Path.Combine(folderName, fileName);
            long size = file.Length / 1000; // in kb

            if (file.ContentType.Contains("image") && size > 300)
            {

                using FileStream localFile = File.OpenWrite(fullPath);
                using Image uploadedFile = Image.FromStream(file.OpenReadStream());

                int Width = uploadedFile.Width;
                int Height = uploadedFile.Height;

                while (Width > 400 || Height > 400)
                {
                    Width /= 3;
                    Height /= 3;
                }

                Bitmap resized = new(uploadedFile, new Size(Width, Height));

                resized.Save(localFile, ImageFormat.Png);
            }
            else
            {
                using FileStream localFile = File.OpenWrite(fullPath);
                using Stream uploadedFile = file.OpenReadStream();

                await uploadedFile.CopyToAsync(localFile);
            }

            return storagePath + "/" + fileName;
        }
    }
}
