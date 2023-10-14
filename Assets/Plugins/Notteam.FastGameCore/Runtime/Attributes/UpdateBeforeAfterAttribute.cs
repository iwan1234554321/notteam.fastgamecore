using System;

namespace Notteam.FastGameCore
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
    public sealed class UpdateBeforeAttribute : Attribute
    {
        public Type ComponentType { get; }

        public UpdateBeforeAttribute(Type type)
        {
            ComponentType = type ?? throw new ArgumentNullException(nameof(type));
        }
    }
    
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
    public sealed class UpdateAfterAttribute : Attribute
    {
        public Type ComponentType { get; }

        public UpdateAfterAttribute(Type type)
        {
            ComponentType = type ?? throw new ArgumentNullException(nameof(type));
        }
    }
}
