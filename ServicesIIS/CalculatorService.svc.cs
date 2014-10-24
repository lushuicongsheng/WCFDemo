
using System.ServiceModel;
namespace ServicesIIS
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true, InstanceContextMode = InstanceContextMode.PerSession)]
    public class CalculatorService : Services.CalculatorService
    {

    }
}
