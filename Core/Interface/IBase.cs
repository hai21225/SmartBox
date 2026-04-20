public interface IBase<T>
{
    public Task<T?> GetById(int id);
    public Task<List<T>> GetAll();
    public Task<bool> Add(T item);
    public Task<bool> Delete(int id);
    public Task<bool> Update (T item);

}