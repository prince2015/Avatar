using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace BreakOut
{
    class Paddle
    {
        private int width, height, xpos, ypos, oldX, oldY;
        private Brush paddleBrush;

        /// <summary>
        /// 创造一个板
        /// </summary>
        /// <param name="padBrush">画板的brush</param>
        /// <param name="initialX">左上角 X-坐标</param>
        /// <param name="initialY">右上角 Y-坐标</param>
        /// <param name="initialWidth">初始化板宽度</param>
        /// <param name="initialHeight">初始化板高度</param>
        public Paddle(Brush padBrush, int initialX, int initialY, int initialWidth, int initialHeight)
        {
            paddleBrush = padBrush;
            width = initialWidth;
            height = initialHeight;
            xpos = initialX;
            ypos = initialY;
        }

        /// <summary>
        /// 画一个长方形（当板用）
        /// </summary>
        /// <param name="paper"></param>
        public void Draw(Graphics paper)
        {
            paper.FillRectangle(paddleBrush, xpos, ypos, width, height);
        }


        /// <summary>
        /// 将球当前位置存在一个变量里
        /// 每一时钟计算一次板的速度，这样在他们碰撞时可以影响球的速度
        /// </summary>
        public void StorePosition()
        {
            oldX = xpos;
            oldY = ypos;
        }

        public int X
        {
            get { return xpos; }
            set { xpos = value; }
        }
        public int Y
        {
            get { return ypos; }
            set { ypos = value; }
        }

        public int OldX { get { return oldX; } }
        public int OldY { get { return oldY; } }
        public int Width
        {
            get { return width; }
            set { width = value; }
        }
        public int Height
        {
            get { return height; }
            set { height = value; }
        }
        public int Left { get { return xpos; } }
        public int Right { get { return xpos + width; } }
        public int Top { get { return ypos; } }
        public int Bottom { get { return ypos + height; } }
        public Rectangle Rectangle { get { return new Rectangle(xpos, ypos, width, height); } }
    }

}
