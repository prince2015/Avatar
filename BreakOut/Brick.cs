using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace BreakOut
{
    class Brick
    {
        private int x, y, width, height, strength, type;

        Image[] materials = { //包含所有砖块图片的数组
            Resource1.orangeBrick,
            Resource1.purpleBrick,
            Resource1.tanBrick,
            Resource1.stoneBrick };

        Brush[] strengthBrush = { //画出砖块血量的brush（就是砖块上的圆点）
            null, //砖块血量为0时不会被画出
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

        //如果有增益道具，返回true
        public bool powerShrink { get { return shrinkPaddle; } }
        public bool powerStretch { get { return stretchPaddle; } }
        public bool powerMulti { get { return multiBall; } }
        public bool powerBall { get { return ballInside; } }

        /// <summary>
        /// 产生一个具有随机血量随机外表随机道具的砖块
        /// </summary>
        /// <param name="xPos">砖块的左上角X坐标</param>
        /// <param name="yPos">砖块的左上角Y坐标</param>
        /// <param name="widthx">砖块的宽度</param>
        /// <param name="heighty">砖块高度</param>
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
            
            type = randy.Next(materials.Length); //产生砖块的随机外形（就是那个图片）

            int powers = randy.Next(0, 101); 
            AssignPowerUp(powers); 
        }

        /// <summary>
        /// 画出中间具有指示血量标志的砖块
        /// </summary>
        /// <param name="paper">画出砖块的graphics</param>
        public void Draw(Graphics paper)
        {
            Rectangle brickRect = new Rectangle(x, y, width, height);
            Image brickImage = materials[type];
            paper.DrawImageUnscaledAndClipped(brickImage,brickRect); //画砖块
            paper.DrawRectangle(Pens.Black, x, y, width, height); //画砖块的边

            int radius = height/3;
            Rectangle trafficLight = new Rectangle(Centre.X - radius, Centre.Y - radius, 2 * radius, 2 * radius);
            paper.FillEllipse(strengthBrush[strength], trafficLight); //画出指示灯
            paper.DrawEllipse(Pens.Black, trafficLight); //指示灯边界
        }

        /// <summary>
        /// 将砖块血量每次减少一
        /// </summary>
        public void Hit()
        {
            if (strength > 0) strength--;
        }

        /// <summary>
        ///在砖块被击中后调用
        /// 砖块被摧毁，得分高，否则得分低
        /// </summary>
        /// <returns>砖块被击中后返回的分数</returns>
        public int CalculateScore()
        {
            int score = 10 / (strength + 1); //血量能为0，由前一个状态的血量计算得来
            return score;
        }

        /// <summary>
        /// 根据这个函数的参数设置一个道具
        /// </summary>
        /// <param name="powers">0到100之间的数</param>
        private void AssignPowerUp(int powers)
        {
            if (0 <= powers && powers <= 4) ballInside = true;
            if (5 <= powers && powers <= 9) multiBall = true;
            if (91 <= powers && powers <= 95) shrinkPaddle = true;
            if (96 <= powers && powers <= 100) stretchPaddle = true;
        }

        public override string ToString()//返回砖块左上角坐标
        {
            string s = "Brick at " + x + "," + y;
            return s;
        }
    }
}
