using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Terrarium.Glass
{
    [DefaultProperty("NormalGradient")]
    public class GlassButton : ButtonBase
    {
        protected Color borderColor = Color.Black;
        protected int depth = 3;
        protected GlassGradient disabledGradient;
        protected bool highlight;
        protected GlassGradient highlightGradient;
        protected GlassGradient hoverGradient;
        protected bool isGlass = true;

        protected bool mouseDown;
        protected bool mouseOver;
        protected GlassGradient normalGradient;
        protected GlassGradient pressedGradient;

        protected bool useStyles = true;

        public GlassButton()
        {
            normalGradient = new GlassGradient(Color.FromArgb(96, 96, 96), Color.FromArgb(0, 0, 0));
            hoverGradient = new GlassGradient(Color.FromArgb(0, 216, 0), Color.FromArgb(0, 64, 0));
            pressedGradient = new GlassGradient(Color.FromArgb(0, 64, 0), Color.FromArgb(0, 216, 0));
            disabledGradient = new GlassGradient(Color.FromArgb(64, 64, 64), Color.FromArgb(64, 64, 64));
            highlightGradient = new GlassGradient(Color.FromArgb(192, 192, 192), Color.FromArgb(96, 96, 96));

            TabStop = false;
            SetStyle(ControlStyles.Selectable, false);
            SetStyle(ControlStyles.Opaque, false);

            borderColor = Color.Black;
            depth = 3;

            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
        }

        [Category("Glass UI")]
        public bool Highlight
        {
            get { return highlight; }
            set
            {
                highlight = value;
                Invalidate();
            }
        }

        [Category("Glass UI")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public GlassGradient NormalGradient
        {
            get { return normalGradient; }
            set
            {
                normalGradient = value;
                Invalidate();
            }
        }

        [Category("Glass UI")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public GlassGradient HoverGradient
        {
            get { return hoverGradient; }
            set
            {
                hoverGradient = value;
                Invalidate();
            }
        }

        [Category("Glass UI")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public GlassGradient PressedGradient
        {
            get { return pressedGradient; }
            set
            {
                pressedGradient = value;
                Invalidate();
            }
        }

        [Category("Glass UI")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public GlassGradient DisabledGradient
        {
            get { return disabledGradient; }
            set
            {
                disabledGradient = value;
                Invalidate();
            }
        }

        [Category("Glass UI")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public GlassGradient HighlightGradient
        {
            get { return highlightGradient; }
            set
            {
                highlightGradient = value;
                Invalidate();
            }
        }

        [Category("Glass UI")]
        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                borderColor = value;
                Invalidate();
            }
        }

        [Category("Glass UI")]
        public int Depth
        {
            get { return depth; }
            set
            {
                depth = value;
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
        public bool IsGlass
        {
            get { return isGlass; }
            set
            {
                isGlass = value;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            GlassGradient activeGradient = normalGradient;

            if (UseStyles)
            {
                if (Enabled == false)
                    activeGradient = GlassStyleManager.Active.ButtonDisabled;
                else if (mouseDown)
                    activeGradient = GlassStyleManager.Active.ButtonPressed;
                else if (mouseOver)
                    activeGradient = GlassStyleManager.Active.ButtonHover;
                else if (highlight)
                    activeGradient = GlassStyleManager.Active.ButtonHighlight;
                else
                    activeGradient = GlassStyleManager.Active.ButtonNormal;
            }
            else
            {
                if (Enabled == false)
                    activeGradient = disabledGradient;
                else if (mouseDown)
                    activeGradient = pressedGradient;
                else if (mouseOver)
                    activeGradient = hoverGradient;
                else if (highlight)
                    activeGradient = highlightGradient;
                else
                    activeGradient = normalGradient;
            }

            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle clientRectangle = ClientRectangle;
            clientRectangle.Inflate(-1, -1);

            bool activeIsGlass;

            GlassGradient sunk = new GlassGradient(Color.FromArgb(48, Color.White), Color.FromArgb(160, Color.Black));
            if (Parent is GlassPanel)
            {
                GlassPanel parent = (GlassPanel) Parent;
                if (parent.UseStyles)
                {
                    activeIsGlass = GlassStyleManager.Active.PanelIsGlass;
                }
                else
                {
                    activeIsGlass = parent.IsGlass;
                }
            }
            else
            {
                activeIsGlass = false;
            }

            GlassHelper.FillRoundedRectangle(clientRectangle, sunk, true, activeIsGlass, e.Graphics);

            Rectangle rectangle = clientRectangle;
            rectangle.Inflate(-depth, -depth);

            if (UseStyles)
                activeIsGlass = GlassStyleManager.Active.ButtonIsGlass;
            else
                activeIsGlass = IsGlass;

            GlassHelper.FillRoundedRectangle(rectangle, activeGradient, false, activeIsGlass, e.Graphics);

            e.Graphics.SetClip(ClientRectangle);

            GlassHelper.DrawRoundedBorder(rectangle, BorderColor, Depth/4.0f, e.Graphics);

            StringAlignment vertical = StringAlignment.Center;
            StringAlignment horizontal = StringAlignment.Center;

            if (Image != null)
            {
                switch (ImageAlign)
                {
                    case ContentAlignment.BottomCenter:
                        {
                            horizontal = StringAlignment.Center;
                            vertical = StringAlignment.Far;
                            break;
                        }
                    case ContentAlignment.BottomLeft:
                        {
                            horizontal = StringAlignment.Near;
                            vertical = StringAlignment.Far;
                            break;
                        }
                    case ContentAlignment.BottomRight:
                        {
                            horizontal = StringAlignment.Far;
                            vertical = StringAlignment.Far;
                            break;
                        }

                    case ContentAlignment.MiddleCenter:
                        {
                            horizontal = StringAlignment.Center;
                            vertical = StringAlignment.Center;
                            break;
                        }
                    case ContentAlignment.MiddleLeft:
                        {
                            horizontal = StringAlignment.Near;
                            vertical = StringAlignment.Center;
                            break;
                        }
                    case ContentAlignment.MiddleRight:
                        {
                            horizontal = StringAlignment.Far;
                            vertical = StringAlignment.Center;
                            break;
                        }

                    case ContentAlignment.TopCenter:
                        {
                            horizontal = StringAlignment.Center;
                            vertical = StringAlignment.Near;
                            break;
                        }
                    case ContentAlignment.TopLeft:
                        {
                            horizontal = StringAlignment.Near;
                            vertical = StringAlignment.Near;
                            break;
                        }
                    case ContentAlignment.TopRight:
                        {
                            horizontal = StringAlignment.Far;
                            vertical = StringAlignment.Near;
                            break;
                        }
                }

                int imageLeft = 0;
                int imageTop = 0;

                switch (horizontal)
                {
                    case StringAlignment.Near:
                        imageLeft = 4;
                        break;
                    case StringAlignment.Center:
                        imageLeft = 1 + (ClientRectangle.Width/2 - Image.Width/2);
                        break;
                    case StringAlignment.Far:
                        imageLeft = Width - Image.Width - 4;
                        break;
                }

                switch (vertical)
                {
                    case StringAlignment.Near:
                        imageTop = 2;
                        break;
                    case StringAlignment.Center:
                        imageTop = 0 + (ClientRectangle.Height/2 - Image.Height/2);
                        break;
                    case StringAlignment.Far:
                        imageTop = Height - Image.Height - 2;
                        break;
                }

                if (Enabled == false)
                {
                    ControlPaint.DrawImageDisabled(e.Graphics, Image, imageLeft, imageTop,
                                                   GlassStyleManager.Active.ButtonDisabled.Bottom);
                }
                else
                {
                    if (mouseDown)
                    {
                        imageLeft++;
                        imageTop++;
                    }

                    e.Graphics.DrawImage(Image, imageLeft, imageTop, Image.Width, Image.Height);
                }
            }

            Rectangle textRectangle = ClientRectangle;
            int textPadding = Depth + (Depth/4) + 2;

            textRectangle.Inflate(-textPadding, -textPadding);

            if (Image != null)
            {
                switch (horizontal)
                {
                    case StringAlignment.Near:
                        textRectangle = new Rectangle(Image.Width + 4, 0, ClientRectangle.Width - Image.Width - 2,
                                                      ClientRectangle.Height);
                        break;
                    case StringAlignment.Center:
                        textRectangle = new Rectangle(0, Image.Height + 2, ClientRectangle.Width,
                                                      ClientRectangle.Height - Image.Height - 2);
                        break;
                    case StringAlignment.Far:
                        textRectangle = new Rectangle(0, 0, ClientRectangle.Width - Image.Width - 2,
                                                      ClientRectangle.Height);
                        break;
                }
            }

            if (Enabled == false)
            {
                GlassHelper.DrawText(Text, textRectangle, TextAlign, e.Graphics, true,
                                     ControlPaint.Dark(GlassStyleManager.Active.ForeColor), Color.Empty);
            }
            else
            {
                if (mouseDown)
                {
                    textRectangle.Offset(1, 1);
                }

                GlassHelper.DrawText(Text, textRectangle, TextAlign, e.Graphics, true, Color.Empty, Color.Empty);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            mouseDown = true;
            base.OnMouseDown(e);
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            mouseDown = false;
            base.OnMouseUp(e);
            Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            mouseOver = true;
            base.OnMouseEnter(e);
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            mouseOver = false;
            base.OnMouseLeave(e);
            Invalidate();
        }
    }
}