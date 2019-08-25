using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(nForums.Startup))]
namespace nForums
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
