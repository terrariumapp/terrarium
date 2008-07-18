using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Terrarium.Glass
{
    [DefaultProperty("Gradient")]
    public class GlassPanel : Panel
    {
        protected GlassBorders borders;
        protected GlassGradient gradient;
        protected bool isGlass = true;
        protected bool isSunk;

        protected Image texture;
        protected bool useStyles = true;

        public GlassPanel()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.Opaque, true);

            gradient = new GlassGradient(Color.FromArgb(96, 96, 96), Color.FromArgb(0, 0, 0));
            borders = GlassBorders.All;
        }

        [Category("Glass UI")]
        public bool IsGlass
        {
            get { return isGlass; }
            set
            {
                isGlass = value;
                Invalidate();
            }
        }

        [Category("Glass UI")]
        public bool UseStyles
        {
            get { return useStyles; }
            set
            {
                useStyles = value;
                Invalidate();
            }
        }

        [Category("Glass UI")]
        public bool IsSunk
        {
            get { return isSunk; }
            set
            {
                isSunk = value;
                Invalidate();
            }
        }

        [Category("Glass UI")]
        public GlassBorders Borders
        {
            get { return borders; }
            set
            {
                borders = value;
                Invalidate();
            }
        }

        [Category("Glass UI")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public GlassGradient Gradient
        {
            get { return gradient; }
            set { gradient = value; }
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rect = new Rectangle(0, 0, Width, Height);

            if (BackgroundImage != null)
            {
                GlassHelper.FillTexturedRectangle(rect, BackgroundImage, true, e.Graphics);
            }
            else
            {
                GlassGradient activeGradient = Gradient;

                if (UseStyles)
                    activeGradient = GlassStyleManager.Active.Panel;

                GlassHelper.FillRectangle(rect, activeGradient, IsSunk,
                                          UseStyles ? GlassStyleManager.Active.PanelIsGlass : IsGlass, e.Graphics);
            }

            GlassHelper.DrawBorder(rect, Borders, e.Graphics);
        }
    }
}