using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseLab.BLL.Helper
{
    public static class Upload
    {
        public static string UploadFile(string FolderName, IFormFile File)
        {
            if (File == null || File.Length == 0)
            {
                return null;
            }

            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo projectDir = new DirectoryInfo(baseDir);
            while (projectDir != null && !projectDir.Name.Contains("CourseLab.PL") && projectDir.Parent != null)
            {
                projectDir = projectDir.Parent;
            }

            string FolderPath;
            if (projectDir != null && projectDir.Name.Contains("CourseLab.PL"))
            {
                FolderPath = Path.Combine(projectDir.FullName, "wwwroot", FolderName);
            }
            else
            {
                FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", FolderName);
            }

            if (!Directory.Exists(FolderPath))
            {
                Directory.CreateDirectory(FolderPath);
            }

            string FileName = Guid.NewGuid().ToString() + Path.GetExtension(File.FileName);
            string FilePath = Path.Combine(FolderPath, FileName);

            using (var stream = new FileStream(FilePath, FileMode.Create))
            {
                File.CopyTo(stream);
            }

            return $"/Files/{FileName}";
        }


        public static string RemoveFile(string FolderName, string FileName)
        {
            try
            {
                string cleanFileName = FileName.TrimStart('/');
                if (cleanFileName.StartsWith(FolderName))
                {
                    cleanFileName = cleanFileName.Replace($"{FolderName}/", "");
                }
                var directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", FolderName, cleanFileName);

                if (File.Exists(directory))
                {
                    File.Delete(directory);
                    return "File deleted successfully";
                }
                else
                {
                    return "File not found";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
