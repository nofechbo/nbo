using System.Threading.Tasks;
using Command;

namespace MyRPS
{
    public interface IRpsCommandHandler
    {
        Task HandleRequestAsync(string input);
    }
}