
using System.Net.Http.Headers;

namespace ShootingGame
{
    public partial class Form1 : Form
    {
        public static Size FormClientSize = Size.Empty;

        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        // �t�B�[���h�ϐ�
        int TickCount = 0;
        int Score = 0;
        // �G�@�C���X�^���X
        List<Enemy> Enemies = new List<Enemy>();
        // ���@�C���X�^���X
        Jiki Jiki = null;
        // ���@�e�C���X�^���X
        List<Burret> JikiBurrets = new List<Burret>();
        // �G�@�e�C���X�^���X
        List<Burret> EnemyBurrets = new List<Burret>();
        // �V���ɓG������
        Random Random = new Random();
        // �����\��
        List<Ellipse> Ellipses = new List<Ellipse>();

        // Bitmap
        Bitmap CharactersBitmap = null;
        Bitmap JikiBitmap = null;
        Bitmap EnemyBitmap1 = null;
        Bitmap EnemyBitmap2 = null;
        Bitmap EnemyBitmap3 = null;
        Bitmap BurretBitmap = null;

        public Form1()
        {
            InitializeComponent();

            DoubleBuffered = true;
            BackColor = Color.Black;

            // ���@�A�G�@��`�悷�邽�߂ɕK�v��Bitmap���擾����
            GetBitmaps();

            // �^�C�}�[��������
            InitTimer();

            Jiki = new Jiki(JikiBitmap);
        }

        // ���@�A�G�@��`�悷�邽�߂ɕK�v��Bitmap���擾����
        void GetBitmaps()
        {
            CharactersBitmap = Properties.Resources.sprite2;
            JikiBitmap = GetJikiBitmap();
            EnemyBitmap1 = GetEnemyBitmap1();
            EnemyBitmap2 = GetEnemyBitmap2();
            EnemyBitmap3 = GetEnemyBitmap3();
            BurretBitmap = GetBurretBitmap();
        }

        // �^�C�}�[��������
        void InitTimer()
        {
            timer.Interval = 1000 / 60;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        void MoveBurrets()
        {
            foreach (Burret burret in JikiBurrets)
                burret.Move();

            foreach (Burret burret in EnemyBurrets)
                burret.Move();
        }

        void CreateNewEnemy()
        {
            int r = TickCount % 15;
            if (r != 0)
                return;

            if (Random.Next(4) != 0)
            {
                int kind = Random.Next(3);
                int x = Random.Next(this.Width);
                bool isRight = Random.Next(2) == 0 ? true : false;

                if (kind == 0)
                    Enemies.Add(new Enemy(EnemyBitmap1, x, 0, isRight));
                if (kind == 1)
                    Enemies.Add(new Enemy(EnemyBitmap2, x, 0, isRight));
                if (kind == 2)
                    Enemies.Add(new Enemy(EnemyBitmap3, x, 0, isRight));
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            FormClientSize = this.ClientSize;
            base.OnLoad(e);
        }

        protected override void OnResize(EventArgs e)
        {
            FormClientSize = this.ClientSize;
            base.OnResize(e);
        }

        // ���@�ƓG�@��`�悷��֐�
        protected override void OnPaint(PaintEventArgs e)
        {
            foreach (Burret burret in JikiBurrets)
                burret.Draw(e.Graphics);
            foreach (Burret burret in EnemyBurrets)
                burret.Draw(e.Graphics);

            Jiki.Draw(e.Graphics);
            foreach (Enemy enemy in Enemies)
                enemy.Draw(e.Graphics);
            
            foreach (Ellipse ellipse in Ellipses)
                ellipse.Draw(e.Graphics);

            ShowScore(e.Graphics);
            ShowGameOverIfDead(e.Graphics);
            base.OnPaint(e);
        }

        void ShowGameOverIfDead(Graphics graphics)
        {
            if (!Jiki.IsDead)
                return;

            string gameOver = "Game Over";
            Font gameOverFont = new Font("MS �S�V�b�N", 32);
            string retry = "Retry Press S Key";
            Font retryFont = new Font("MS �S�V�b�N", 24);

            // �`�悳��镶���̕��A�����𒲂ׂ邽�߂ɉ�ʂ̊O���ɏ����Ă���
            Point pt = new Point(10, this.Height);
            Size gameOverSize = TextRenderer.MeasureText(graphics, gameOver, gameOverFont, new Size(this.ClientSize.Width, this.ClientSize.Height), TextFormatFlags.NoPadding);
            Size retrySize = TextRenderer.MeasureText(graphics, retry, retryFont, new Size(this.ClientSize.Width, this.ClientSize.Height), TextFormatFlags.NoPadding);

            // �`�悳��镶���̕��A�������擾�ł���
            int gameOverWidth = gameOverSize.Width;
            int gameOverHeight = gameOverSize.Height;
            int retryWidth  = retrySize.Width;
            int retryHeight = retrySize.Height;

            // ��ʂ̒����ɂȂ�悤�ɁuGame Over�v�ƕ`�悷��
            int gameOverX = (this.ClientSize.Width - gameOverWidth) / 2;
            int gameOverY = (this.ClientSize.Height - gameOverHeight) / 2;
            graphics.DrawString(gameOver, gameOverFont, new SolidBrush(Color.White), new Point(gameOverX, gameOverY));

            // ���̉��ɁuRetry Press S Key�v�ƕ`�悷��
            int retryX = (this.ClientSize.Width - retryWidth) / 2;
            int retryY = gameOverY + gameOverHeight + 20;
            graphics.DrawString(retry, retryFont, new SolidBrush(Color.White), new Point(retryX, retryY));
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TickCount++;

            // ���@���ړ�������
            Jiki.Move();

            // �e�ۂ��ړ�������
            MoveBurrets();

            foreach (Enemy enemy in Enemies)
            {
                // �G���ړ�������
                enemy.Move();

                // �G�ɒe�ۂ𔭎˂�����
                Burret burret = EnemyShot(enemy);
                if (burret != null)
                    EnemyBurrets.Add(burret);
            }

            // �����蔻��
            HitJudge();

            // �V���ɓG������
            CreateNewEnemy();

            // ����
            Ellipses = Ellipses.Where(x => !x.IsDead).ToList();
            foreach (Ellipse ellipse in Ellipses)
                ellipse.Move();

            Invalidate();
        }

        Burret EnemyShot(Enemy enemy)
        {
            int a = TickCount % 30;
            if (a != 0)
                return null;

            int r = Random.Next(3);
            if (r == 0)
            {
                int x = enemy.X + this.EnemyBitmap1.Width / 2 - BurretBitmap.Width / 2;
                double angle = Math.Atan2(Jiki.Y - enemy.Y, Jiki.X - enemy.X);
                double vx = Math.Cos(angle) * 5;
                double vy = Math.Sin(angle) * 5;
                return new Burret(BurretBitmap, x, enemy.Y, vx, vy);
            }
            return null;
        }

        void HitJudge()
        {
            // �G�Ǝ��@���甭�˂����e�ۂ̓����蔻��
            foreach (Enemy enemy in Enemies)
            {
                // JikiBurrets�z��v�f�̏��߂��݂� 
                Burret burret = JikiBurrets.FirstOrDefault(x => IsHit(enemy, x));
                if (burret != null)
                {
                    JikiBurrets.Remove(burret);
                    enemy.IsDead = true;
                    EnemyDead(enemy);
                }
            }
            Enemies = Enemies.Where(x => !x.IsDead).ToList();
            JikiBurrets = JikiBurrets.Where(x => !x.IsDead).ToList();

            // ���Ɏ��@���S�̂Ƃ��͕K�v�Ȃ�
            if (Jiki.IsDead)
                return;

            // ���@�ƓG���甭�˂����e�ۂ̓����蔻�� EnemyBurrets�z��v�f�̏��߂��݂�
            Burret enemyBurret = EnemyBurrets.FirstOrDefault(x => IsHit(Jiki, x));
            if (enemyBurret != null)
            {
                EnemyBurrets.Remove(enemyBurret);
                Jiki.IsDead = true;
                JikiDead();
            }
            EnemyBurrets = EnemyBurrets.Where(x => !x.IsDead).ToList();

            // ���@�ƓG���̂��̂̓����蔻�� Enemies�z��v�f�̏��߂��݂�
            Enemy enemy1 = Enemies.FirstOrDefault(x => IsHit(Jiki, x));
            if (enemy1 != null)
            {
                Enemies.Remove(enemy1);
                Jiki.IsDead = true;
                JikiDead();
            }

        }
        // �܂��Z�`�̏d�Ȃ蔻��
        bool IsHit(Rectangle rect1, Rectangle rect2)
        {
            if (rect1.Right < rect2.Left)
                return false;
            if (rect1.Bottom < rect2.Top)
                return false;
            if (rect2.Right < rect1.Left)
                return false;
            if (rect2.Bottom < rect1.Top)
                return false;
            return true;
        }

        // �G�ƒe�̓����蔻��
        bool IsHit(Enemy enemy, Burret burret)
        {
            Rectangle rect1 = new Rectangle(enemy.X, enemy.Y, EnemyBitmap1.Width, EnemyBitmap1.Height);
            Rectangle rect2 = new Rectangle((int)burret.X, (int)burret.Y, BurretBitmap.Width, BurretBitmap.Height);
            return IsHit(rect1, rect2);
        }

        // ���@�ƒe�̓����蔻��
        bool IsHit(Jiki jiki, Burret burret)
        {
            Rectangle rect1 = new Rectangle(jiki.X, jiki.Y, JikiBitmap.Width, JikiBitmap.Height);
            Rectangle rect2 = new Rectangle((int)burret.X, (int)burret.Y, BurretBitmap.Width, BurretBitmap.Height);
            return IsHit(rect1, rect2);
        }

        // ���@�ƓG�̓����蔻��
        bool IsHit(Jiki Jiki, Enemy enemy)
        {
            Rectangle rect1 = new Rectangle(Jiki.X, Jiki.Y, JikiBitmap.Width, JikiBitmap.Height);
            Rectangle rect2 = new Rectangle((int)enemy.X, (int)enemy.Y, EnemyBitmap1.Width, EnemyBitmap1.Height);
            return IsHit(rect1, rect2);
        }

        void Explosion(int centerX, int centerY)
        {
            // 128�̒��a4�s�N�Z���̐ԁ`�I�����W�F�̉~������
            // �����ł��܂���ɍL����悤�ɂ���
            for (int i=0; i< 128; i++)
            {
                double vx = (Random.Next(40) - 20) / 10.0;
                double vy = (Random.Next(40) - 20) / 10.0;
                Ellipses.Add(new Ellipse(centerX, centerY, vx, vy));
            }
        }
        void EnemyDead(Enemy enemy)
        {
            // �G�̒��S�������̒��S�ɂȂ�悤�ɂ���
            int x = enemy.X + this.EnemyBitmap1.Width / 2;
            int y = enemy.Y + this.EnemyBitmap1.Height / 2;
            Explosion(x, y);
            Score += 10;    // 10�_�ǉ�
        }

        void JikiDead()
        {
            // ���@�̒��S�������̒��S�ɂȂ�悤�ɂ���
            int x = Jiki.X + this.JikiBitmap.Width / 2;
            int y = Jiki.Y + this.JikiBitmap.Height / 2;
            Explosion(x, y);
        }

        void ShowScore(Graphics graphics)
        {
            string scoreString = Score.ToString("00000");
            graphics.DrawString(scoreString, new Font("MS �S�V�b�N", 18), new SolidBrush(Color.White), new Point(10, 10));
        }

        Bitmap GetJikiBitmap()
        {
            //�퓬�@�ł���΍���̍��W�� (57,1)�A ��44�A����48
            Rectangle sourceRectangle = new Rectangle(new Point(57, 1), new Size(44, 48));

            int destWidth = 44;
            int destHeight = 46;
            Bitmap bitmap1 = new Bitmap(destWidth, destHeight);
            Graphics graphics = Graphics.FromImage(bitmap1);
            graphics.DrawImage(CharactersBitmap, new Rectangle(0, 0, destWidth, destHeight), sourceRectangle, GraphicsUnit.Pixel);
            graphics.Dispose();
            return bitmap1;
        }

        Bitmap GetEnemyBitmap1()
        {
            // ���F���Ђ悱�B����̍��W��(4,61)�A��24�A����28�ł��B
            Rectangle sourceRectangle = new Rectangle(new Point(4, 61), new Size(24, 28));

            int destWidth = 24;
            int destHeight = 28;
            Bitmap bitmap1 = new Bitmap(destWidth, destHeight);
            Graphics graphics = Graphics.FromImage(bitmap1);
            graphics.DrawImage(CharactersBitmap, new Rectangle(0, 0, destWidth, destHeight), sourceRectangle, GraphicsUnit.Pixel);
            graphics.Dispose();
            return bitmap1;
        }

        Bitmap GetEnemyBitmap2()
        {
            // �s���N�F�̂Ђ悱�B����̍��W��(4, 94)�A��24�A����28
            Rectangle sourceRectangle = new Rectangle(new Point(4, 94), new Size(24, 28));
            int destWidth = 24;
            int destHeight = 28;
            Bitmap bitmap1 = new Bitmap(destWidth, destHeight);
            Graphics graphics = Graphics.FromImage(bitmap1);
            graphics.DrawImage(CharactersBitmap, new Rectangle(0, 0, destWidth, destHeight), sourceRectangle, GraphicsUnit.Pixel);
            graphics.Dispose();
            return bitmap1;
        }

        Bitmap GetEnemyBitmap3()
        {
            // �F�̂Ђ悱�B����̍��W��(4,125)�A��24�A����28�ł��B
            Rectangle sourceRectangle = new Rectangle(new Point(4, 125), new Size(24, 28));
            int destWidth = 24;
            int destHeight = 28;
            Bitmap bitmap1 = new Bitmap(destWidth, destHeight);
            Graphics graphics = Graphics.FromImage(bitmap1);
            graphics.DrawImage(CharactersBitmap, new Rectangle(0, 0, destWidth, destHeight), sourceRectangle, GraphicsUnit.Pixel);
            graphics.Dispose();
            return bitmap1;
        }

        Bitmap GetBurretBitmap()
        {
            // �e�ہB����̍��W��(41, 46)�A��14�A����14
            Rectangle sourceRectangle = new Rectangle(new Point(41, 46), new Size(14, 14));
            
            int destWidth = 14;
            int destHeight = 14;
            Bitmap bitmap1 = new Bitmap(destWidth, destHeight);
            Graphics graphics = Graphics.FromImage(bitmap1);
            graphics.DrawImage(CharactersBitmap, new Rectangle(0, 0, destWidth, destHeight),sourceRectangle, GraphicsUnit.Pixel); 
            graphics.Dispose();
            return bitmap1;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
                Jiki.MoveLeft = true;
            if (e.KeyCode == Keys.Right)
                Jiki.MoveRight = true;
            if (e.KeyCode == Keys.Down)
                Jiki.MoveDown = true;
            if (e.KeyCode == Keys.Up)
                Jiki.MoveUp = true;
            if (e.KeyCode == Keys.Space && !Jiki.IsDead)
                Shot();
            if (e.KeyCode == Keys.S && Jiki.IsDead)
                Retry();
            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
                Jiki.MoveLeft = false;
            if (e.KeyCode == Keys.Right)
                Jiki.MoveRight = false;
            if (e.KeyCode == Keys.Down)
                Jiki.MoveDown = false;
            if (e.KeyCode == Keys.Up)
                Jiki.MoveUp = false;
            base.OnKeyDown(e);
        }

        void Retry()
        {
            // �G�ƓG�̒e�ۂ��N���A����
            Enemies.Clear();
            EnemyBurrets.Clear();

            // ���@�������ʒu�ɖ߂��A���S��Ԃ���񕜂�����
            Jiki.X = (FormClientSize.Width - JikiBitmap.Width) / 2;
            Jiki.Y = (int)(FormClientSize.Height * 0.8);
            Jiki.IsDead = false;

            // �X�R�A��0�ɖ߂��ăQ�[���X�^�[�g
            Score = 0;
        }

        void Shot()
        {
            int burretX = Jiki.X + JikiBitmap.Width / 2 - BurretBitmap.Width / 2;
            JikiBurrets.Add(new Burret(BurretBitmap, burretX, Jiki.Y, 0, -5));
            JikiBurrets.Add(new Burret(BurretBitmap, burretX, Jiki.Y, 0.5, -5));
            JikiBurrets.Add(new Burret(BurretBitmap, burretX, Jiki.Y, -0.5, -5));
        }

    }
 
}
