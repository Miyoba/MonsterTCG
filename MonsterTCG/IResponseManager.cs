namespace MonsterTCG
{
    public interface IResponseManager
    {
        IResponse Response { get; set; }
        string ProcessResponse();
    }
}