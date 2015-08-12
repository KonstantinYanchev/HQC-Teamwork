namespace EasyHttp.Contracts
{
    public interface IEncoder
    {
        byte[] Encode(object input, string contentType);
    }
}