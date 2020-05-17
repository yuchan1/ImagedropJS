using System.ComponentModel;
using System.Web.Mvc;

namespace WebApplication.Common {

    /// <summary>
    /// TrimModelBinder: Modelデータのtrim
    /// </summary>
    public class TrimModelBinder : DefaultModelBinder {
        protected override void SetProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, object value) {

            if (propertyDescriptor.PropertyType == typeof(string)) {
                var val = (string)value;

                if (!string.IsNullOrWhiteSpace(val)) {
                    value = val.Trim();
                } else {
                    value = val;
                }
            }

            base.SetProperty(controllerContext, bindingContext, propertyDescriptor, value);
        }
    }
}