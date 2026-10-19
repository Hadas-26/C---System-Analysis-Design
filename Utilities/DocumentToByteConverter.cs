/*
 * Written by Josh Green
 */

namespace MedicaidEmploymentVerificationApplication.Utilities
{
    public static class DocumentToByteConverter
    {

        /// <summary>
        /// Converts an IFormFile into a byte array that can be stored in a database
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static async Task<byte[]> Convert(IFormFile file)
        {
            if(file == null || file.Length == 0)
                throw new ArgumentNullException(nameof(file), "The provided file was null or empty. Could not convert");

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            byte[] converted = stream.ToArray();
            return converted;
        }

        //public static async Task<byte[]> Revert(IFormFile file)
        //{

        //}


    }
}
