using Microsoft.AspNetCore.Mvc;
using IBll;

namespace WebCarProject.Controllers
{
    public class MapInfoController : Controller
    { 
    //    IMapBll _mapBll;
    //    public MapController(IMapBll mapBll)
    //    {
    //        this._mapBll = mapBll;
    //    }
        
        public IActionResult ListView()
        {
           
            return View();
        }
    }
}
