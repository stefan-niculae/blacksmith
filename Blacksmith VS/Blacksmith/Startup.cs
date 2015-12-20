using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Blacksmith.Startup))]
namespace Blacksmith
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
