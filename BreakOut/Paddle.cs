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
        /// ����һ����
        /// </summary>
        /// <param name="padBrush">�����brush</param>
        /// <param name="initialX">���Ͻ� X-����</param>
        /// <param name="initialY">���Ͻ� Y-����</param>
        /// <param name="initialWidth">��ʼ������</param>
        /// <param name="initialHeight">��ʼ����߶�</param>
        public Paddle(Brush padBrush, int initialX, int initialY, int initialWidth, int initialHeight)
        {
            paddleBrush = padBrush;
            width = initialWidth;
            height = initialHeight;
            xpos = initialX;
            ypos = initialY;
        }

        /// <summary>
        /// ��һ�������Σ������ã�
        /// </summary>
        /// <param name="paper"></param>
        public void Draw(Graphics paper)
        {
            paper.FillRectangle(paddleBrush, xpos, ypos, width, height);
        }


        /// <summary>
        /// ����ǰλ�ô���һ��������
        /// ÿһʱ�Ӽ���һ�ΰ���ٶȣ�������������ײʱ����Ӱ������ٶ�
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
