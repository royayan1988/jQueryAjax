using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(jQueryAjax.Startup))]
namespace jQueryAjax
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
