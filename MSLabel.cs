using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace milano88.UI.Controls
{
    public class MSLabel : Label
    {
        public MSLabel()
        {
            Font = new Font("Segoe UI", 9F);
            ForeColor = Color.Black;
        }

        private TextFormatFlags GetFlags()
        {
            switch (TextAlign)
            {
                case ContentAlignment.BottomCenter:
                    return TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter | TextFormatFlags.EndEllipsis;
                case ContentAlignment.BottomLeft:
                    return TextFormatFlags.Bottom | TextFormatFlags.Left | TextFormatFlags.EndEllipsis;
                case ContentAlignment.BottomRight:
                    return TextFormatFlags.Bottom | TextFormatFlags.Right | TextFormatFlags.EndEllipsis;
                case ContentAlignment.MiddleCenter:
                    return TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter | TextFormatFlags.EndEllipsis;
                case ContentAlignment.MiddleLeft:
                    return TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis;
                case ContentAlignment.MiddleRight:
                    return TextFormatFlags.VerticalCenter | TextFormatFlags.Right | TextFormatFlags.EndEllipsis;
                case ContentAlignment.TopCenter:
                    return TextFormatFlags.Top | TextFormatFlags.HorizontalCenter | TextFormatFlags.EndEllipsis;
                case ContentAlignment.TopLeft:
                    return TextFormatFlags.Top | TextFormatFlags.Left | TextFormatFlags.EndEllipsis;
                case ContentAlignment.TopRight:
                    return TextFormatFlags.Top | TextFormatFlags.Right | TextFormatFlags.EndEllipsis;
                default:
                    return TextFormatFlags.Default;
            }
        }

        private Color _disabledColor = Color.Silver;
        [Category("Custom Properties")]
        [DefaultValue(typeof(Color), "Silver")]
        public Color DisabledColor
        {
            get { return _disabledColor; }
            set { _disabledColor = value; Invalidate(); }
        }

        private Color _shadowColor = Color.White;
        [Category("Custom Properties")]
        [DefaultValue(typeof(Color), "White")]
        public Color ShadowColor
        {
            get { return _shadowColor; }
            set { _shadowColor = value; Invalidate(); }
        }

        private int _shadowX = 0;
        [Category("Custom Properties")]
        [DefaultValue(0)]
        public int ShadowX
        {
            get => _shadowX;
            set { _shadowX = (value < 0) ? Math.Max(value, -25) : Math.Min(25, value); Invalidate(); }
        }

        private int _shadowY = 1;
        [Category("Custom Properties")]
        [DefaultValue(1)]
        public int ShadowY
        {
            get => _shadowY;
            set { _shadowY = (value < 0) ? Math.Max(value, -25) : Math.Min(25, value); Invalidate(); }
        }

        private byte _shadowOpacity = 255;
        [Category("Custom Properties")]
        [DefaultValue(255)]
        public byte ShadowOpacity
        {
            get => _shadowOpacity;
            set { _shadowOpacity = value; Invalidate(); }
        }

        private bool _showShadow;
        [Category("Custom Properties")]
        [DefaultValue(typeof(bool), "False")]
        public bool ShowShadow
        {
            get { return _showShadow; }
            set { _showShadow = value; Invalidate(); }
        }

        [Category("Custom Properties")]
        [DefaultValue(typeof(Font), "Segoe UI, 9pt")]
        public override Font Font { get => base.Font; set => base.Font = value; }

        [Category("Custom Properties")]
        [DefaultValue(null)]
        public override string Text { get => base.Text; set => base.Text = value; }

        [Category("Custom Properties")]
        [DefaultValue(typeof(Color), "Black")]
        public override Color ForeColor { get => base.ForeColor; set => base.ForeColor = value; }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Enabled)
            {
                if(!_showShadow) { base.OnPaint(e); return; }

                int xStart = Math.Min(Location.X, Location.X + _shadowX),
                xEnd = Math.Max(Location.X, Location.X + _shadowX),
                yStart = Math.Min(Location.Y, Location.Y + _shadowY),
                yEnd = Math.Max(Location.Y, Location.Y + _shadowY),
                steps, xIncrement, yIncrement, alphaIncrement;

                steps = Math.Max(xEnd - xStart, yEnd - yStart);
                xIncrement = (_shadowX < 0 ? -1 : 1) * (int)Math.Floor((xEnd - xStart) / (float)steps);
                yIncrement = (_shadowY < 0 ? -1 : 1) * (int)Math.Floor((yEnd - yStart) / (float)steps);
                alphaIncrement = (int)Math.Floor(_shadowOpacity / (float)steps);

                if (steps > 0)
                {
                    for (int i = steps; i > 0; i--)
                    {
                        TextRenderer.DrawText(e.Graphics, Text.Replace("&&", "&"), Font, new Point(xIncrement * i, yIncrement * i), Color.FromArgb(_shadowOpacity - (alphaIncrement * i), _shadowColor.R, _shadowColor.G, _shadowColor.B), GetFlags());
                        TextRenderer.DrawText(e.Graphics, Text.Replace("&&", "&"), Font, new Point(0, 0), ForeColor, GetFlags());
                    }
                }
            }
            else
            {
                TextRenderer.DrawText(e.Graphics, Text.Replace("&&", "&"), Font, ClientRectangle, _disabledColor, GetFlags());
            }
        }
    }
}
