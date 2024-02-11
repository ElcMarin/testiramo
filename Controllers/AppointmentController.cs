using Microsoft.AspNetCore.Mvc;

namespace maturitetna.Controllers
{
    [ServiceFilter(typeof(SessionCheckFilterUser))]
    public class AppointmentController : Controller
    {
    }
}
