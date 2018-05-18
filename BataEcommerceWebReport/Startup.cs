using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BataEcommerceWebReport.Startup))]
namespace BataEcommerceWebReport
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
