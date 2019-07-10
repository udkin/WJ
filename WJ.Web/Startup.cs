using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WJ.Web.Startup))]
namespace WJ.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
