using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GBCSporting2021_TEC.Startup))]
namespace GBCSporting2021_TEC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }

        
    }
}
