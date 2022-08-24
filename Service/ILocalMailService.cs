namespace CItyInfo.API.Service
{
    public interface ILocalMailService
    {
        void Send(string subjeect, string message);
    }
}
