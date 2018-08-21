using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Soccer.Backend.Startup))]
namespace Soccer.Backend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
