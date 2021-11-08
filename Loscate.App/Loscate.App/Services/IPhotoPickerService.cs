using System.IO;
using System.Threading.Tasks;

namespace Loscate.App.Services
{
    public interface IPhotoPickerService
    {
        Task<Stream> GetImageStreamAsync();
    }
}
