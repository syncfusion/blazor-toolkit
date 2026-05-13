using System.Reflection;
using System.Runtime.Serialization;

namespace Syncfusion.Blazor.Toolkit.Data
{
    /// <summary>
    /// Specifies the FilterType to be used in LINQ methods.
    /// </summary>
    public enum FilterType
    {
        /// <summary>
        /// Performs WildCard operation.
        /// </summary>
        WildCard,

        /// <summary>
        /// Performs IsNull operation.
        /// </summary>
        IsNull,

        /// <summary>
        /// Performs IsNotNull operation.
        /// </summary>
        IsNotNull,

        /// <summary>
        /// Performs IsEmpty operation.
        /// </summary>
        IsEmpty,

        /// <summary>
        /// Performs IsNotEmpty operation.
        /// </summary>
        IsNotEmpty,

        /// <summary>
        /// Performs LessThan operation.
        /// </summary>
        LessThan,

        /// <summary>
        /// Performs LessThan Or Equal operation.
        /// </summary>
        LessThanOrEqual,

        /// <summary>
        /// Checks Equals on the operands.
        /// </summary>
        Equals,

        /// <summary>
        /// Checks for Not Equals on the operands.
        /// </summary>
        NotEquals,

        /// <summary>
        /// Checks for Greater Than or Equal on the operands.
        /// </summary>
        GreaterThanOrEqual,

        /// <summary>
        /// Checks for Greater Than on the operands.
        /// </summary>
        GreaterThan,

        /// <summary>
        /// Checks for StartsWith on the string operands.
        /// </summary>
        StartsWith,

        /// <summary>
        /// Checks for Does Not StartsWith on the string operands.
        /// </summary>
        DoesNotStartWith,

        /// <summary>
        /// Checks for EndsWith on the string operands.
        /// </summary>
        EndsWith,

        /// <summary>
        /// Checks for Does Not EndsWith on the string operands.
        /// </summary>
        DoesNotEndWith,

        /// <summary>
        /// Checks for Contains on the string operands.
        /// </summary>
        Contains,

        /// <summary>
        /// Checks for Does Not Contains on the string operands.
        /// </summary>
        DoesNotContain,

        /// <summary>
        /// Checks for Like on the string operands.
        /// </summary>
        Like,

        /// <summary>
        /// Returns invalid type
        /// </summary>
        Undefined,

        /// <summary>
        /// Checks for Between two date on the operands.
        /// </summary>
        Between
    }

    /// <summary>
    /// Specifies the Filter Behaviour for the filter predicates.
    /// </summary>
    public enum FilterBehavior
    {
        /// <summary>
        /// Parses only StronglyTyped values.
        /// </summary>
        StronglyTyped,

        /// <summary>
        /// Parses all values by converting them as string.
        /// </summary>
        StringTyped
    }

    internal class EnumerationValue
    {
        internal static object GetValueFromEnumMember(string description, Type EnumType)
        {
            Type type = EnumType;
            Type? underlyingType = Nullable.GetUnderlyingType(type);
            if (underlyingType != null)
            {
                type = underlyingType;
            }
            if (!type.IsEnum)
            {
                throw new InvalidOperationException();
            }

            foreach (FieldInfo field in type.GetFields())
            {
                if (Attribute.GetCustomAttribute(
                    field,
                    typeof(EnumMemberAttribute)) is EnumMemberAttribute attribute)
                {
                    if (attribute.Value == description)
                    {
                        return field.GetValue(null)!;
                    }
                }
                else
                {
                    if (field.Name == description)
                    {
                        return field.GetValue(null)!;
                    }
                }
            }

            return null!;
        }
    }

}