using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace CaliburnApp.UI.ViewModels
{
    public class ShellViewModel : PropertyChangedBase
    {
        public ShellViewModel(TreeListViewModel treeListViewModel)
        {
            TreeList = treeListViewModel;
        }

        public TreeListViewModel TreeList { get; set; }
    }
}
