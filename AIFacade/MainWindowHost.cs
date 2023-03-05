using System.Windows.Forms;
using Rhino;
using Rhino.Commands;
using Rhino.Input.Custom;
using Rhino.UI;
using System.Runtime.InteropServices;
using System.Windows.Forms.Integration;
using AIFacade.View;


namespace DraftInsights.Commands
{
    [Guid("bab8f818-b9dd-4524-a411-4fb4a942b3b2")]

    public class MainWindowHost : RhinoWindows.Controls.WpfElementHost
    {

        public MainWindowHost(uint docSn)
          : base(new AIFacade.View.Panel(), null)
        {
        }
    }

    public class MainWindowCommand : Command
    {

        //Mousecallback og conduit
        public MainWindowCommand()
        {
            Instance = this;
            Panels.RegisterPanel(
              AIFacade.AIFacadePlugin.Instance,
              typeof(MainWindowHost),
              "AIMainwindow",
              System.Drawing.SystemIcons.WinLogo,
              PanelType.System
              );
        }

        public static MainWindowCommand Instance
        {
            get; private set;
        }

        public override string EnglishName => "AIMainwindow";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            var panel_id = typeof(MainWindowHost).GUID;

            if (mode == RunMode.Interactive)
            {
                Panels.OpenPanel(panel_id);
                return Result.Success;
            }

            var panel_visible = Panels.IsPanelVisible(panel_id);

            var prompt = (panel_visible)
              ? "Sample panel is visible. New value"
              : "Sample Manager panel is hidden. New value";

            var go = new GetOption();
            go.SetCommandPrompt(prompt);
            var hide_index = go.AddOption("Hide");
            var show_index = go.AddOption("Show");
            var toggle_index = go.AddOption("Toggle");
            go.Get();

            if (go.CommandResult() != Result.Success)
                return go.CommandResult();

            var option = go.Option();
            if (null == option)
                return Result.Failure;

            var index = option.Index;
            if (index == hide_index)
            {
                if (panel_visible)
                    Panels.ClosePanel(panel_id);
            }
            else if (index == show_index)
            {
                if (!panel_visible)
                    Panels.OpenPanel(panel_id);
            }
            else if (index == toggle_index)
            {
                if (panel_visible)
                    Panels.ClosePanel(panel_id);
                else
                    Panels.OpenPanel(panel_id);
            }
            return Result.Success;
        }
    }
}
