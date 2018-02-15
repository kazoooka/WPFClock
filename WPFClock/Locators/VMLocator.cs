using Funq;
using WpfClock.ViewModels;

namespace WpfClock.Locators
{
    public class VMLocator
    {
        #region Fields
        private static readonly Container _container = new Container();
        #endregion

        #region Properties
        public static MainViewModel Main
        {
            get
            {
                return _container.Resolve<MainViewModel>();
            }
        }
        #endregion

        #region Constructor
        public VMLocator()
        {
            _container.Register<MainViewModel>(c => new MainViewModel());
        }
        #endregion
    }
}
