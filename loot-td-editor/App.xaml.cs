#define TD_EDITOR

using Microsoft.Shell;
using System;
using System.Collections.Generic;
using System.Windows;

namespace loot_td_editor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, ISingleInstanceApp
    {
        private const string Unique = "loot_td_editor_u";

        [STAThread]
        public static void Main()
        {
            if (SingleInstance<App>.InitializeAsFirstInstance(Unique))
            {
                var application = new App();

                application.InitializeComponent();
                    application.Run();
                

                // Allow single instance code to perform cleanup operations
                SingleInstance<App>.Cleanup();
            }
        }

        public bool SignalExternalCommandLineArgs(IList<string> args)
        {
            MessageBox.Show("Application already running", "Error");
            return true;
        }
    }
}