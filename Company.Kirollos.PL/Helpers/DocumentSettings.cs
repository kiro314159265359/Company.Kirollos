namespace Company.Kirollos.PL.Helpers
{
    public static class DocumentSettings
    {
        // 1. Upload
        // Image name the result 
        public static string UploadFile(IFormFile file, string folderName)
        {
            // 1. Get Folder Location
            //var folderPath = Directory.GetCurrentDirectory() + "\\wwwroot\\files\\" + folderName;

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files", folderName);

            // File Path 
            // 2. file name  + and make it unique

            var fileName = $"{Guid.NewGuid()}{file.FileName}";
            var filePath = Path.Combine(folderPath, fileName);
            
            using var fileStream = new FileStream(filePath , FileMode.Create);

            file.CopyTo(fileStream);

            return fileName;
        }
        // 2. Delete

        public static void DeleteFile(string fileName, string folderName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files", folderName , fileName);

            if (File.Exists(filePath))
                File.Delete(filePath);            
        }
    }
}
