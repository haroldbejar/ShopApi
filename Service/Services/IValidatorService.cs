namespace Service.Services
{
    public interface IValidatorService<T> where T : class
    {
        Task<bool> EntityValidationsAsync(int id); 
    }
}
