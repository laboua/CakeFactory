using System.Threading.Tasks;

namespace CakeFactory.Service
{
    public interface ICakeService
    {
        Task RunAsync(Stock stock);
    }
}
