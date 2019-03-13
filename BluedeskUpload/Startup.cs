using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BluedeskUpload.Startup))]
namespace BluedeskUpload
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
