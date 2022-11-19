using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DorcasClub.Startup))]
namespace DorcasClub
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
