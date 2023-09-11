using System.ComponentModel.DataAnnotations.Schema;

namespace Biwen.EFCore.SoftDelete
{

    /// <summary>
    /// 软删除接口
    /// </summary>
    public interface ISoftDeleted
    {
        /// <summary>
        ///  是否已删除
        /// </summary>
        bool IsDeleted { get; set; }

        [NotMapped]
        bool? ForceDelete { get; set; }

    }
}