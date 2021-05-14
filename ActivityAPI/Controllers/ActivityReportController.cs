using ActivityModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActivityAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ActivityReportController : ControllerBase
    {
        private static Dictionary<string, List<Report>> activityData = new Dictionary<string, List<Report>>(); 

        [HttpGet]
        public int GetActivityCount(string activity)
        {
            int sum = 0;

            if (activityData.Any(x => x.Key == activity))
            {
                sum = activityData[activity].Where(x => x.RequestTime >= DateTime.Now.AddHours(-12)).Sum(y => y.Count);
            }

            return sum;
        }

        [HttpPost]
        public bool LogActivityEvents(string activity, int count)
        {
            if (activityData.ContainsKey(activity))
            {
                activityData[activity].Add(new Report { Count = count, RequestTime = DateTime.Now });
                activityData[activity].RemoveAll(x => x.RequestTime < DateTime.Now.AddHours(-12));
            }
            else
            {
                List<Report> reports = new List<Report>();
                Report report = new Report { Count = count, RequestTime = DateTime.Now };
                reports.Add(report);
                activityData.Add(activity, reports);
            }

            return true;
        }
    }
}
