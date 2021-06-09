using Avelraangame.Services.ServiceUtils;

namespace Avelraangame.Models.ViewModels
{
    public class RequestVm
    {
        public bool OperationSuccess { get; set; }

        public string Message { get; set; }

        public string Response { get; set; }

        public RequestVm()
        {
            OperationSuccess = true;
            Message = Scribe.Operation_success;
        }
    }
}
