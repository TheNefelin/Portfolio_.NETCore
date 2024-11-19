using ClassLibrary_DTOs;

namespace ClassLibrary_Services.Portfolio
{
    public interface ISimpleCRUDService<T>
    {
        Task<ResponseApiDTO<IEnumerable<T>>> GetAllAsync(CancellationToken cancellationToken);
        Task<ResponseApiDTO<T>> InsertAsync(T dto, CancellationToken cancellationToken);
        Task<ResponseApiDTO<T>> DeleteAsync(T dto, CancellationToken cancellationToken);
    }
}
