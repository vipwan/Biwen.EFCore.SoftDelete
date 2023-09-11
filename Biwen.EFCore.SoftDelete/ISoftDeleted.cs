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
        public abstract bool IsDeleted { get; set; }

    }
}