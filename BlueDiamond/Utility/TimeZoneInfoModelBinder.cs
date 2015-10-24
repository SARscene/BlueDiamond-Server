using System;
using System.Web.Mvc;

namespace BlueDiamond.Utility
{
    /// <summary>
    ///     To Display TimeZoneInfo
    /// http://romikoderbynew.com/2012/03/12/working-with-time-zones-in-asp-net-mvc/
    /// </summary>
    public class TimeZoneInfoModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

            if (valueProviderResult == null) return null;

            var attemptedValue = valueProviderResult.AttemptedValue;

            return ParseTimeZoneInfo(attemptedValue);
        }

        public static TimeZoneInfo ParseTimeZoneInfo(string attemptedValue)
        {
            return TimeZoneInfo.FindSystemTimeZoneById(attemptedValue);
        }

        public class TimeZoneModelBinderProvider : IModelBinderProvider
        {
            public IModelBinder GetBinder(Type modelType)
            {
                return modelType == typeof(TimeZoneInfo)
                    ? DependencyResolver.Current.GetService<TimeZoneInfoModelBinder>()
                    : null;
            }
        }
    }
}