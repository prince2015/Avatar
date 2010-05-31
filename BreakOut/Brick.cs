using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace BreakOut
{
    class Brick
    {
        private int x, y, width, height, strength, type;

        Image[] materials = { //��������ש��ͼƬ������
            Resource1.orangeBrick,
            Resource1.purpleBrick,
            Resource1.tanBrick,
            Resource1.stoneBrick };

        Brush[] strengthBrush = { //����ש��Ѫ����brush������ש���ϵ�Բ�㣩
            null, //ש��Ѫ��Ϊ0ʱ���ᱻ����
            Brushes.Lime,
            Brushes.Orange,
            Brushes.Red };

        private bool multiBall = false;
        private bool shrinkPaddle = false;
        private bool stretchPaddle = false;
        private bool ballInside = false;

        
        public int Strength { get { return strength; } }

        public Point Centre { get { return new Point(Left + Width / 2, Top + Height / 2); } }
        public int Width { get { return width; } }
        public int Height { get { return height; } }
        public int Left { get { return x; } }
        public int Right { get { return x + width; } }
        public int Top { get { return y; } }
        public int Bottom { get { return y + height; } }

        //�����������ߣ�����true
        public bool powerShrink { get { return shrinkPaddle; } }
        public bool powerStretch { get { return stretchPaddle; } }
        public bool powerMulti { get { return multiBall; } }
        public bool powerBall { get { return ballInside; } }

        /// <summary>
        /// ����һ���������Ѫ��������������ߵ�ש��
        /// </summary>
        /// <param name="xPos">ש������Ͻ�X����</param>
        /// <param name="yPos">ש������Ͻ�Y����</param>
        /// <param name="widthx">ש��Ŀ��</param>
        /// <param name="heighty">ש��߶�</param>
        public Brick(int xPos, int yPos, int widthx, int heighty, Random randy,int level)
        {
            x = xPos;
            y = yPos;
            width = widthx;
            height = heighty;
            switch (level)
            {
                case 1: strength = randy.Next(0, 2);
                    break;
                case 2: strength = randy.Next(0, 3);
                    break;
                case 3: strength = 0;
                    break;
                case 4: strength = randy.Next(0, 4);
                    break;
                case 5: strength = randy.Next(1, 4);
                    break;
                case 6: strength = 0;
                    break;
                case 7: strength = 2;
                    break;
                case 8: strength = randy.Next(2, 4);
                    break;
                case 9: strength = 0;
                    break;
                case 10: strength = 3;
                    break;
                case 11: strength = randy.Next(3, 4);
                    break;
                case 12: strength = 0;
                    break;

            }
            
            type = randy.Next(materials.Length); //����ש���������Σ������Ǹ�ͼƬ��

            int powers = randy.Next(0, 101); 
            AssignPowerUp(powers); 
        }

        /// <summary>
        /// �����м����ָʾѪ����־��ש��
        /// </summary>
        /// <param name="paper">����ש���graphics</param>
        public void Draw(Graphics paper)
        {
            Rectangle brickRect = new Rectangle(x, y, width, height);
            Image brickImage = materials[type];
            paper.DrawImageUnscaledAndClipped(brickImage,brickRect); //��ש��
            paper.DrawRectangle(Pens.Black, x, y, width, height); //��ש��ı�

            int radius = height/3;
            Rectangle trafficLight = new Rectangle(Centre.X - radius, Centre.Y - radius, 2 * radius, 2 * radius);
            paper.FillEllipse(strengthBrush[strength], trafficLight); //����ָʾ��
            paper.DrawEllipse(Pens.Black, trafficLight); //ָʾ�Ʊ߽�
        }

        /// <summary>
        /// ��ש��Ѫ��ÿ�μ���һ
        /// </summary>
        public void Hit()
        {
            if (strength > 0) strength--;
        }

        /// <summary>
        ///��ש�鱻���к����
        /// ש�鱻�ݻ٣��÷ָߣ�����÷ֵ�
        /// </summary>
        /// <returns>ש�鱻���к󷵻صķ���</returns>
        public int CalculateScore()
        {
            int score = 10 / (strength + 1); //Ѫ����Ϊ0����ǰһ��״̬��Ѫ���������
            return score;
        }

        /// <summary>
        /// ������������Ĳ�������һ������
        /// </summary>
        /// <param name="powers">0��100֮�����</param>
        private void AssignPowerUp(int powers)
        {
            if (0 <= powers && powers <= 4) ballInside = true;
            if (5 <= powers && powers <= 9) multiBall = true;
            if (91 <= powers && powers <= 95) shrinkPaddle = true;
            if (96 <= powers && powers <= 100) stretchPaddle = true;
        }

        public override string ToString()//����ש�����Ͻ�����
        {
            string s = "Brick at " + x + "," + y;
            return s;
        }
    }
}
