// Copyright © 2001-2026 Syncfusion Inc. All rights reserved.
// Licensed under the MIT license. See LICENSE in the repository root for full license information.

using System.Collections;
using System.Reflection;
using Syncfusion.Blazor.Toolkit.Data;
using Xunit;

namespace Syncfusion.Blazor.Toolkit.BUnitTest.Base;

public sealed class ReflectionNullableRegressionTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("Missing")]
    public void CreateAccessor_MissingEmptyOrNullProperty_ReturnsNoOp(string? propertyName)
    {
        using IPropertyAccessor accessor = FastReflectionExtension.CreateAccessor(typeof(DerivedRecord), propertyName);

        Assert.Null(accessor.PropertyInfo);
        Assert.Null(accessor.GetValue(new DerivedRecord()));
    }

    [Fact]
    public void CreateAccessor_NullObjectType_ReturnsNoOp()
    {
        using IPropertyAccessor accessor = FastReflectionExtension.CreateAccessor(null, nameof(DerivedRecord.Name));

        Assert.Null(accessor.PropertyInfo);
        Assert.Null(accessor.GetValue(new DerivedRecord()));
    }

    [Fact]
    public void CreateAccessor_NullValuedProperty_ReturnsNull()
    {
        using IPropertyAccessor accessor = FastReflectionExtension.CreateAccessor(typeof(DerivedRecord), nameof(DerivedRecord.Name));

        Assert.NotNull(accessor.PropertyInfo);
        Assert.Null(accessor.GetValue(new DerivedRecord()));
    }

    [Fact]
    public void CreateAccessor_Disposed_ReturnsNullValueAndMetadata()
    {
        DerivedRecord record = new() { Name = "before disposal" };
        using IPropertyAccessor accessor = FastReflectionExtension.CreateAccessor(typeof(DerivedRecord), nameof(DerivedRecord.Name));
        Assert.Equal(record.Name, accessor.GetValue(record));

        accessor.Dispose();

        Assert.Null(accessor.PropertyInfo);
        Assert.Null(accessor.GetValue(record));
        accessor.Dispose();
        Assert.Null(accessor.GetValue(record));
    }

    [Fact]
    public void CreateAccessor_InheritedProperty_UsesDeclaringType()
    {
        DerivedRecord record = new() { Name = "inherited" };
        using IPropertyAccessor accessor = FastReflectionExtension.CreateAccessor(typeof(DerivedRecord), nameof(DerivedRecord.Name));

        Assert.Equal(typeof(BaseRecord), accessor.PropertyInfo?.DeclaringType);
        Assert.Equal(record.Name, accessor.GetValue(record));
    }

    [Fact]
    public void TryCreateInstance_NullType_RethrowsArgumentNullException()
    {
        // Invoke through reflection to exercise invalid runtime input without suppressing the non-null Type contract.
        MethodInfo method = typeof(ReflectionExtension).GetMethod(nameof(ReflectionExtension.TryCreateInstance))
            ?? throw new InvalidOperationException("The instance factory must exist.");

        TargetInvocationException error = Assert.Throws<TargetInvocationException>(
            () => method.Invoke(null, new object?[] { null, false }));

        ArgumentNullException cause = Assert.IsType<ArgumentNullException>(error.InnerException);
        Assert.Equal("type", cause.ParamName);
    }

    [Fact]
    public void TryCreateInstance_ParameterizedConstructor_PreservesDefaultArguments()
    {
        ParameterizedRecord record = Assert.IsType<ParameterizedRecord>(
            ReflectionExtension.TryCreateInstance(typeof(ParameterizedRecord)));

        Assert.Equal(string.Empty, record.Text);
        Assert.Equal(0, record.Number);
        Assert.NotNull(record.Child);
        Assert.Null(record.Child.Name);
        // Nullable<T>'s constructor is invoked with default(T), rather than substituting a null argument.
        Assert.Equal((int?)0, record.OptionalNumber);
    }

    [Fact]
    public void TryCreateInstance_MultipleConstructors_UsesFirstConstructor()
    {
        ConstructorInfo first = typeof(MultipleConstructors).GetConstructors().First();
        string expected = first.GetParameters().Length == 0 ? "parameterless" : "parameterized:";

        MultipleConstructors record = Assert.IsType<MultipleConstructors>(
            ReflectionExtension.TryCreateInstance(typeof(MultipleConstructors)));

        Assert.Equal(expected, record.SelectedConstructor);
    }

    [Fact]
    public void TryCreateInstance_InterfaceParameter_PreservesActivationFailure()
    {
        // The existing interface branch activates the containing type, not the parameter type.
        Assert.Throws<MissingMethodException>(
            () => ReflectionExtension.TryCreateInstance(typeof(InterfaceParameterRecord)));
    }

    [Fact]
    public void TryCreateInstance_ThrowingConstructor_PreservesInvocationException()
    {
        TargetInvocationException error = Assert.Throws<TargetInvocationException>(
            () => ReflectionExtension.TryCreateInstance(typeof(ThrowingRecord)));

        Assert.IsType<InvalidOperationException>(error.InnerException);
        Assert.Equal("constructor failure", error.InnerException?.Message);
    }

    [Fact]
    public void GroupResult_DefaultAndAssignedNulls_PreserveSentinels()
    {
        GroupResult group = new();
        Assert.Null(group.Key);
        Assert.Null(group.Items);
        Assert.Null(group.SubGroups);
        Assert.Equal(" (0)", group.ToString());

        group.Key = "key";
        group.Items = new[] { 1 };
        group.SubGroups = Array.Empty<GroupResult>();
        group.Key = null;
        group.Items = null;
        group.SubGroups = null;

        Assert.Null(group.Key);
        Assert.Null(group.Items);
        Assert.Null(group.SubGroups);
    }

    [Fact]
    public void GroupByMany_LeafGroup_PreservesNullSubGroups()
    {
        int[] values = { 1, 1 };

        GroupResult group = Assert.Single(values.GroupByMany(value => value));

        Assert.Equal(1, group.Key);
        Assert.Equal(2, group.Count);
        IEnumerable items = Assert.IsAssignableFrom<IEnumerable>(group.Items);
        Assert.Equal(values, items.Cast<int>());
        Assert.Null(group.SubGroups);
    }

    [Fact]
    public void SortDescription_EqualsNull_ReturnsFalse()
    {
        SortDescription sort = new(nameof(DerivedRecord.Name), ListSortDirection.Ascending);

        Assert.False(sort.Equals((object?)null));
    }

    [Fact]
    public void RemoteOptions_EqualsNull_PreservesTrue()
    {
        RemoteOptions options = new();

        Assert.True(options.Equals((object?)null));
    }

    [Theory]
    [InlineData("Missing")]
    [InlineData(nameof(DerivedRecord.Name))]
    public void GetObject_MissingOrNullProperty_ReturnsNull(string propertyName)
    {
        Assert.Null(DataUtil.GetObject(propertyName, new DerivedRecord()));
    }

    [Fact]
    public void NullableContracts_ExposeExistingNullResults()
    {
        NullabilityInfoContext context = new();
        foreach ((Type type, string name) in new[]
        {
            (typeof(IPropertyAccessor), nameof(IPropertyAccessor.PropertyInfo)),
            (typeof(GroupResult), nameof(GroupResult.Key)),
            (typeof(GroupResult), nameof(GroupResult.Items)),
            (typeof(GroupResult), nameof(GroupResult.SubGroups))
        })
        {
            PropertyInfo property = type.GetProperty(name)
                ?? throw new InvalidOperationException($"Missing property: {name}");
            Assert.Equal(NullabilityState.Nullable, context.Create(property).ReadState);
        }

        foreach ((Type type, string name) in new[]
        {
            (typeof(IPropertyAccessor), nameof(IPropertyAccessor.GetValue)),
            (typeof(DataUtil), nameof(DataUtil.GetObject))
        })
        {
            MethodInfo method = type.GetMethod(name)
                ?? throw new InvalidOperationException($"Missing method: {name}");
            Assert.Equal(NullabilityState.Nullable, context.Create(method.ReturnParameter).ReadState);
        }
    }

    public class BaseRecord
    {
        public string? Name { get; set; }
    }

    public sealed class DerivedRecord : BaseRecord
    {
    }

    public sealed class ParameterizedRecord
    {
        public ParameterizedRecord(string text, int number, BaseRecord child, int? optionalNumber)
        {
            Text = text;
            Number = number;
            Child = child;
            OptionalNumber = optionalNumber;
        }

        public string Text { get; }
        public int Number { get; }
        public BaseRecord Child { get; }
        public int? OptionalNumber { get; }
    }

    public sealed class MultipleConstructors
    {
        public MultipleConstructors(string text)
        {
            SelectedConstructor = "parameterized:" + text;
        }

        public MultipleConstructors()
        {
            SelectedConstructor = "parameterless";
        }

        public string SelectedConstructor { get; }
    }

    public sealed class InterfaceParameterRecord
    {
        public InterfaceParameterRecord(IDisposable dependency)
        {
        }
    }

    public sealed class ThrowingRecord
    {
        public ThrowingRecord()
        {
            throw new InvalidOperationException("constructor failure");
        }
    }
}
