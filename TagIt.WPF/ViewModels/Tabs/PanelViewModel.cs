using System;
using TagIt.WPF.ViewModels;

namespace TagIt.ViewModels.Tabs
{
    public abstract class PanelViewModel : ViewModel
    {
        private bool _isActive = false;

        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;

                    if (_isActive)
                    {
                        BecameActive?.Invoke(this, new EventArgs());
                    }
                }
            }
        }

        public event EventHandler<EventArgs> BecameActive;
    }
}
