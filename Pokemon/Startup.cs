using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Pokemon.Startup))]
namespace Pokemon
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            


        }
    }
}
