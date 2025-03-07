namespace Constructor_API.Core.Shared
{
    public abstract class Entity<TKey>
    {
        public TKey Id { get; set; }
    }
}
