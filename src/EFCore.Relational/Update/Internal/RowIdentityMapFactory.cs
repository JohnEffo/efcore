// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Update.Internal;

/// <summary>
///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
///     the same compatibility standards as public APIs. It may be changed or removed without notice in
///     any release. You should only use it directly in your code with extreme caution and knowing that
///     doing so can result in application failures when updating to a new Entity Framework Core release.
/// </summary>
public class RowIdentityMapFactory : IRowIdentityMapFactory
{
    /// <summary>
    ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
    ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
    ///     any release. You should only use it directly in your code with extreme caution and knowing that
    ///     doing so can result in application failures when updating to a new Entity Framework Core release.
    /// </summary>
    public virtual IRowIdentityMap Create(IUniqueConstraint key)
        => (IRowIdentityMap)_createMethod
                .MakeGenericMethod(key.Columns.Count == 1 ? key.Columns.First().ProviderClrType : typeof(object[]))
                .Invoke(null, new object[] { key })!;

    private readonly static MethodInfo _createMethod = typeof(RowIdentityMapFactory).GetTypeInfo()
        .GetDeclaredMethod(nameof(CreateRowIdentityMap))!;

    [UsedImplicitly]
    private static IRowIdentityMap CreateRowIdentityMap<TKey>(IUniqueConstraint key)
        where TKey : notnull
        => new RowIdentityMap<TKey>(key);
}
