using System.Web.Http;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using FacilitiesServices.Controllers;

namespace FacilitiesServices
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        System.Timers.Timer _timer1 = new System.Timers.Timer();
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            // timer event initialization
            _timer1.Enabled = true;
            _timer1.Interval = 86400000;
            _timer1.Elapsed += _timer1_Elapsed;
            _timer1.Start();
            
        }
        private void _timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {

                _timer1.Enabled = false;

                WorkOrderController workOrderController = new WorkOrderController();
                workOrderController.TestSchedularCalling();
                workOrderController.CreateServiceandWorkOrderByPMDataAndSendIt();

                _timer1.Enabled = true;
            }

            catch (Exception ex)
            { }
        }
    }
}
