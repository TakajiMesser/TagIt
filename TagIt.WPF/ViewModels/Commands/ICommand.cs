namespace TagIt.WPF.ViewModels.Commands
{
    public interface ICommand
    {
        void Do();
        void Undo();
    }
}
