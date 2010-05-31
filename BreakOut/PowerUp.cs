using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace BreakOut
{
    class PowerUp
    {
        private string label;
        private int x, y;
        private int width = 40;
        private int height = 10;
        private int dY = 4;
        private Brush brush;

        /// <summary>
        /// Powerup属性
        /// </summary>
        public string Type { get { return label; } }
        public int YSpeed { get { return dY; } }
        public int Left { get { return x; } }
        public int Right { get { return x + width; } }
        public int Bottom { get { return y + height; } }
        public int Top { get { return y; } }
        public int Width { get { return width; } }
        public int Height { get { return height; } }
        public Rectangle Rectangle { get { return new Rectangle(x, y, width, height); } }

        /// <summary>
        ///创建一个增益道具powerup
        /// </summary>
        /// <param name="type">powerup的种类</param>
        /// <param name="powerUpBrush">powerup的颜色</param>
        /// <param name="xpos">powerup中心X坐标</param>
        /// <param name="ypos">powerup中心Y坐标</param>
        public PowerUp(string type, Brush powerUpBrush, int xpos, int ypos)
        {
            label = type;
            brush = powerUpBrush;
            x = xpos - width / 2; //将X坐标从中心转移到左边
            y = ypos - height / 2; //将Y坐标从中心转移到上边，实际上就是把原点移到了左上角
        }

        /// <summary>
        /// 画出powerup
        /// </summary>
        /// <param name="paper">画出powerup的Graphics</param>
        public void Draw(Graphics paper)
        {
            Font pFont = new Font("微软雅黑",10);
            paper.FillRectangle(brush, x, y, width, height);
            paper.DrawString(label, pFont, Brushes.Black, x, y);
        }

        /// <summary>
        /// powerup的下落函数
        /// </summary>
        public void Move()
        {
            y += dY;
        }
    }
}
