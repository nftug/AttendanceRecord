namespace BrowserBridge;

public interface IViewModelResolver
{
    string Type { get; }
    IOwnedService<IViewModel> Resolve();
}

public class ViewModelResolver<TViewModel>(IContainerInstance container) : IViewModelResolver
    where TViewModel : class, IViewModel
{
    public string Type => typeof(TViewModel).Name.Replace("ViewModel", string.Empty);

    public IOwnedService<IViewModel> Resolve()
        => container.Resolve<TViewModel>();
}
