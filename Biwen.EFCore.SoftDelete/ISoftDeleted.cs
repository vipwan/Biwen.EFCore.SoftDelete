// Licensed to the Biwen.EFCore.SoftDelete under one or more agreements.
// The Biwen.EFCore.SoftDelete licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Biwen.EFCore.SoftDelete;

/// <summary>
/// 软删除接口
/// </summary>
public interface ISoftDeleted
{
    /// <summary>
    ///  是否已删除
    /// </summary>
    bool IsDeleted { get; set; }
}