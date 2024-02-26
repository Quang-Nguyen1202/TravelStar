using TravelStar.Business.Interfaces;
using TravelStar.Entities;
using TravelStar.Model;

namespace TravelStar.Business.Implements;
public class ResourceReader : IResourceReader
{
    public Task<string> GetConfirmBookingEmailResourceAsync(string resourceName)
    {
        using Stream? stream = this.GetType().Assembly.GetManifestResourceStream(resourceName);

        if (stream is null)
        {
            throw new InvalidOperationException($"Email template '{resourceName}' was not found.");
        }

        using StreamReader reader = new StreamReader(stream);
        return reader.ReadToEndAsync();
    }
}
