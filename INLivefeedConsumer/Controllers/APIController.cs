using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text;
using Helper;
using Microsoft.Extensions.Configuration;

namespace INLiveFeecd.Controllers
{
    [Route("api")]
    public class APIController : Controller
    {


        //[HttpGet("produce")]
        //public async Task<IActionResult> Test1()
        //{
        //    var msg = this.GetParameterValue("message");
        //    var x = await Manager.ProduceOne("cdr_sdp_01" , msg);
        //    return new ContentResult() { Content = x.ToString(), StatusCode = 200, ContentType = "application/json" };
        //}

        //[HttpGet("printTPS")]
        //public IActionResult PrintTPS()
        //{
        //    return new ContentResult() { Content = $"Max throughput: {Manager.MAXTPS} TPS", StatusCode = 200, ContentType = "application/json" };
        //}
    }
}
