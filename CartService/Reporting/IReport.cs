namespace CartService.Reporting
{
    public interface IReport<T>
    {
        void Create(T item);
    }
}