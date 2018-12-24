namespace Momento.Services.Contracts.Utilities
{
    using System;
    using System.Collections.Generic;

    public interface IUtilitiesService
    {
        void UpdatePropertiesReflection(
            object source,
            object target,
            string[] ignoredSourcePropNames = null,
            string[] exclusiveSourcePropNames = null,
            Type[] ignoredTypes = null,
            Type[] exclusiveTypes = null,
            Attribute[] ignoredAttributes = null,
            Attribute[] exclusiveAttributes = null,
            Dictionary<string, string> customNameMappingSourceTarget = null);
    }
}
