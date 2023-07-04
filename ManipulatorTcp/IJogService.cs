namespace ManipulatorTcp
{
    public interface IJogService
    {
        Task<bool> SendJogCommand(JogCommand command);

        void Enqueue(JogCommand jogCommand);

        void Start();

        void Stop();
    }
}