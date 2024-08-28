using SimplePopupManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace Assets
{
    public class Installer : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IPopupManagerService>().To<PopupManagerServiceService>().AsSingle().NonLazy();
        }
    }
}
