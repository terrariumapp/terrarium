using System.ComponentModel;
using System.Windows;
using System.Windows.Forms.Integration;
using Terrarium.Renderer;

namespace TerrariumWPF
{
    /// <summary>
    /// Interaction logic for MainForm.xaml
    /// </summary>
    public partial class MainForm : Window
    {
        private readonly TerrariumDirectDrawGameView _tddGameView;

        public MainForm()
        {
            InitializeComponent();
            _tddGameView = new TerrariumDirectDrawGameView();
            var host = new WindowsFormsHost {Child = _tddGameView};
            tddGameView.Children.Add(host);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(!DesignerProperties.GetIsInDesignMode(this))
            {
                if (_tddGameView.InitializeDirectDraw(false))
                {
                }
            }
        }
    }
}