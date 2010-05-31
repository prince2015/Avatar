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
        /// Powerup����
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
        ///����һ���������powerup
        /// </summary>
        /// <param name="type">powerup������</param>
        /// <param name="powerUpBrush">powerup����ɫ</param>
        /// <param name="xpos">powerup����X����</param>
        /// <param name="ypos">powerup����Y����</param>
        public PowerUp(string type, Brush powerUpBrush, int xpos, int ypos)
        {
            label = type;
            brush = powerUpBrush;
            x = xpos - width / 2; //��X���������ת�Ƶ����
            y = ypos - height / 2; //��Y���������ת�Ƶ��ϱߣ�ʵ���Ͼ��ǰ�ԭ���Ƶ������Ͻ�
        }

        /// <summary>
        /// ����powerup
        /// </summary>
        /// <param name="paper">����powerup��Graphics</param>
        public void Draw(Graphics paper)
        {
            Font pFont = new Font("΢���ź�",10);
            paper.FillRectangle(brush, x, y, width, height);
            paper.DrawString(label, pFont, Brushes.Black, x, y);
        }

        /// <summary>
        /// powerup�����亯��
        /// </summary>
        public void Move()
        {
            y += dY;
        }
    }
}
