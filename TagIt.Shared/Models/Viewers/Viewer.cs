using System;
using TagIt.Shared.Models.Contents;

namespace TagIt.Shared.Models.Viewers
{
    public abstract class Viewer
    {
        private bool _isLoaded;

        public Viewer(Kinds kind) => Kind = kind;

        public Kinds Kind { get; }
        public IContent Content { get; private set; }

        public bool IsLoaded
        {
            get => _isLoaded;
            set
            {
                if (_isLoaded != value)
                {
                    _isLoaded = value;

                    if (_isLoaded)
                    {
                        OnLoaded();
                    }
                }
            }
        }

        public event EventHandler ElementLoaded;
        public event EventHandler ContentOpened;

        public virtual void Open(IContent content)
        {
            if (content.Kind != Kind) throw new ArgumentException("Could not open content kind " + content.Kind);
            Content = content;

            if (IsLoaded)
            {
                OpenContent();
                //ContentOpened?.Invoke()
            }
        }

        private void OnLoaded()
        {
            ElementLoaded?.Invoke(this, EventArgs.Empty);

            if (Content != null)
            {
                OpenContent();
            }
        }

        protected abstract void OpenContent();
    }
}
