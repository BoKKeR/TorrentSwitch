using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catel.Reflection;
using Orc.FileAssociation;

namespace TorrentSwitch
{
    class associate
    {
        public void ass()
        {
            var assembly = AssemblyHelper.GetEntryAssembly();
            var applicationInfo = new ApplicationInfo(assembly);

            //x.RegisterApplication(applicationInfo);;

            //_fileAssociationService.AssociateFilesWithApplication(applicationInfo.Name);

        }
    }
}
