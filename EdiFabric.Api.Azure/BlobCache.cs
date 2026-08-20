using System.Text;
using EdiFabric.Native.X12;

namespace EdiFabric.Api.Azure
{
    public class BlobCache
    {
        public static void Set(string serialKey)
        {
            try
            {
                var token = ReadTokenFromCache().Result;
                EdiFabricX12.SetToken(token);

                //  Refresh token before expiration
                Refresh(serialKey);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                //  Try one last time
                try
                {
                    var token = GetFromApi(serialKey);
                    WriteTokenToCache(token).Wait();
                    EdiFabricX12.SetToken(token);
                }
                catch (Exception ex2)
                {
                    Console.WriteLine(ex2.ToString());
                    //  Contact support@edifabric.com for assistance
                    throw;
                }
            }
        }

        private static void Refresh(string serialKey)
        {
            try
            {
                //  Refresh the token two days before it expires
                if (DaysToExpiration() < 3)
                    WriteTokenToCache(GetFromApi(serialKey)).Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                //  If can't get a token a day before the current expires - throw an exception
                //  Otherwise keep trying
                if (DaysToExpiration() <= 1)
                    throw;
            }
        }

        private static int DaysToExpiration()
        {
            var expiration = EdiFabricX12.GetTokenExpiration();
            if (expiration is null)
                return 0;

            return Math.Max(0, (int)Math.Ceiling((expiration.Value - DateTime.UtcNow).TotalDays));
        }

        private static string GetFromApi(string serialKey)
        {
            int retries = 3;
            int index = 0;

            //  Try to get a token with retries
            while (index < retries)
            {
                try
                {
                    return EdiFabricX12.GetToken(serialKey);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    index++;

                    if (index >= retries)
                        throw;
                }
            }

            throw new Exception("Can't get a token.");
        }

        private static async Task<string> ReadTokenFromCache()
        {
            var result = await BlobHelper.ReadFromCache(Configuration.ContainerName, Configuration.BlobName);
            return LoadString(result);
        }

        private static async Task WriteTokenToCache(string token)
        {
            await BlobHelper.WriteToCache(Configuration.ContainerName, Configuration.BlobName, LoadStream(token));
        }

        private static string LoadString(Stream stream)
        {
            return new StreamReader(stream, Encoding.UTF8).ReadToEnd();
        }

        private static MemoryStream LoadStream(string value)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(value));
        }
    }
}
