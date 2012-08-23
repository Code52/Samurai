using System.Web.Mvc;

namespace SamuraiServer.Models
{
    public class DynamicJsonAttribute : CustomModelBinderAttribute
    {
        public override IModelBinder GetBinder()
        {
            return new DynamicJsonBinder();
        }
    }
}