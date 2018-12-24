namespace Momento.Services.Implementations.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Momento.Services.Contracts.Utilities;

    public class UtilitiesService : IUtilitiesService
    {
        public void UpdatePropertiesReflection(
            object source,
            object target,
            string[] ignoredSourcePropNames = null,
            string[] exclusiveSourcePropNames = null,
            Type[] ignoredTypes = null,
            Type[] exclusiveTypes = null,
            Attribute[] ignoredAttributes = null,
            Attribute[] exclusiveAttributes = null,
            Dictionary<string, string> customNameMappingSourceTarget = null)
        {
            var sourceType = source.GetType();
            var targetType = target.GetType();

            var sourceProps = sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            if(exclusiveSourcePropNames != null && exclusiveSourcePropNames.Any())
            {
                sourceProps = sourceProps.Where(x => exclusiveSourcePropNames.Contains(x.Name)).ToArray();
            }

            if (exclusiveTypes != null && exclusiveTypes.Any())
            {
                sourceProps = sourceProps.Where(x => exclusiveTypes.Contains(x.PropertyType)).ToArray();
            }

            if (exclusiveAttributes != null && exclusiveAttributes.Any())
            {
                sourceProps = sourceProps.Where(x => x.GetCustomAttributes().Any(y => exclusiveAttributes.Contains(y))).ToArray();
            }

            if(ignoredSourcePropNames!= null && ignoredSourcePropNames.Any())
            {
                sourceProps = sourceProps.Where(x => !ignoredSourcePropNames.Contains(x.Name)).ToArray();
            }

            if (ignoredTypes != null && ignoredTypes.Any())
            {
                sourceProps = sourceProps.Where(x => !ignoredTypes.Contains(x.PropertyType)).ToArray();
            }

            if (ignoredAttributes != null && ignoredAttributes.Any())
            {
                sourceProps = sourceProps.Where(x => !x.GetCustomAttributes().Any(y => ignoredAttributes.Contains(y))).ToArray();
            }

            foreach (var sourceProp in sourceProps)
            {
                var targetPropName = sourceProp.Name;
                if (customNameMappingSourceTarget != null && customNameMappingSourceTarget.ContainsKey(sourceProp.Name))
                {
                    targetPropName = customNameMappingSourceTarget[sourceProp.Name];
                }

                var targetProp = targetType.GetProperty(targetPropName);
                var targetVal = targetProp.GetValue(target);
                var sourceVal = sourceProp.GetValue(source);
                if (targetVal != sourceVal)
                {
                    targetProp.SetValue(target, sourceVal);
                }
            }
        }
    }
}
