using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebAuth1.Startup))]
namespace WebAuth1
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
