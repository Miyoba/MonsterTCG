namespace MonsterTCG
{
    public interface IClientHandler
    {
        void ExecuteRequest();
        void CloseClient();
    }
}