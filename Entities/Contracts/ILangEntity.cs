namespace Entities.Contracts
{
    public interface ILangEntity<T>
    {
        public int Fk_Source { get; set; }

        public T Source { get; set; }
    }
}
