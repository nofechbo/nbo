using System.Threading.Tasks;
using Command;

namespace mainServer
{
    public interface IRpsCommandHandler
    {
        Task HandleRequestAsync(string input);
    }
}