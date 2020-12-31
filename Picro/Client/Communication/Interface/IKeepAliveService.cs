using System.Threading.Tasks;

namespace Picro.Client.Communication.Interface
{
    public interface IKeepAliveService
    {
        Task InitializeConnection();
    }
}