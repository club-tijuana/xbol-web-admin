namespace Odasoft.XBOL.AdminPortal.States
{
    public class LoadingState
    {
        private int _loadingCount = 0;

        public bool IsLoading => _loadingCount > 0;

        public event Action? OnChange;

        public void Show()
        {
            _loadingCount++;
            NotifyStateChanged();
        }

        public void Hide()
        {
            if (_loadingCount > 0)
            {
                _loadingCount--;
                NotifyStateChanged();
            }
        }

        public void Reset()
        {
            _loadingCount = 0;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
