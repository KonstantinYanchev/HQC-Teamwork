namespace EasyHttp.Contracts
{
    public interface IEncoderDecoderConfiguration
    {
        IEncoder GetEncoder();

        IDecoder GetDecoder();
    }
}