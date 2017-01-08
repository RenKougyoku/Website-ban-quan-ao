using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ShopOnline.Startup))]
namespace ShopOnline
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
