namespace ConstructorAdminAPI.Core.Shared
{
    public abstract class Entity<TKey>
    {
        public TKey Id { get; set; }
    }
}
