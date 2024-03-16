
using System.Net.Http.Headers;

namespace ShootingGame
{
    public partial class Form1 : Form
    {
        public static Size FormClientSize = Size.Empty;

        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        // フィールド変数
        int TickCount = 0;
        int Score = 0;
        // 敵機インスタンス
        List<Enemy> Enemies = new List<Enemy>();
        // 自機インスタンス
        Jiki Jiki = null;
        // 自機弾インスタンス
        List<Burret> JikiBurrets = new List<Burret>();
        // 敵機弾インスタンス
        List<Burret> EnemyBurrets = new List<Burret>();
        // 新たに敵をつくる
        Random Random = new Random();
        // 爆発表示
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

            // 自機、敵機を描画するために必要なBitmapを取得する
            GetBitmaps();

            // タイマーを初期化
            InitTimer();

            Jiki = new Jiki(JikiBitmap);
        }

        // 自機、敵機を描画するために必要なBitmapを取得する
        void GetBitmaps()
        {
            CharactersBitmap = Properties.Resources.sprite2;
            JikiBitmap = GetJikiBitmap();
            EnemyBitmap1 = GetEnemyBitmap1();
            EnemyBitmap2 = GetEnemyBitmap2();
            EnemyBitmap3 = GetEnemyBitmap3();
            BurretBitmap = GetBurretBitmap();
        }

        // タイマーを初期化
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

        // 自機と敵機を描画する関数
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
            Font gameOverFont = new Font("MS ゴシック", 32);
            string retry = "Retry Press S Key";
            Font retryFont = new Font("MS ゴシック", 24);

            // 描画される文字の幅、高さを調べるために画面の外側に書いてある
            Point pt = new Point(10, this.Height);
            Size gameOverSize = TextRenderer.MeasureText(graphics, gameOver, gameOverFont, new Size(this.ClientSize.Width, this.ClientSize.Height), TextFormatFlags.NoPadding);
            Size retrySize = TextRenderer.MeasureText(graphics, retry, retryFont, new Size(this.ClientSize.Width, this.ClientSize.Height), TextFormatFlags.NoPadding);

            // 描画される文字の幅、高さが取得できる
            int gameOverWidth = gameOverSize.Width;
            int gameOverHeight = gameOverSize.Height;
            int retryWidth  = retrySize.Width;
            int retryHeight = retrySize.Height;

            // 画面の中央になるように「Game Over」と描画する
            int gameOverX = (this.ClientSize.Width - gameOverWidth) / 2;
            int gameOverY = (this.ClientSize.Height - gameOverHeight) / 2;
            graphics.DrawString(gameOver, gameOverFont, new SolidBrush(Color.White), new Point(gameOverX, gameOverY));

            // その下に「Retry Press S Key」と描画する
            int retryX = (this.ClientSize.Width - retryWidth) / 2;
            int retryY = gameOverY + gameOverHeight + 20;
            graphics.DrawString(retry, retryFont, new SolidBrush(Color.White), new Point(retryX, retryY));
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TickCount++;

            // 自機を移動させる
            Jiki.Move();

            // 弾丸を移動させる
            MoveBurrets();

            foreach (Enemy enemy in Enemies)
            {
                // 敵を移動させる
                enemy.Move();

                // 敵に弾丸を発射させる
                Burret burret = EnemyShot(enemy);
                if (burret != null)
                    EnemyBurrets.Add(burret);
            }

            // 当たり判定
            HitJudge();

            // 新たに敵をつくる
            CreateNewEnemy();

            // 爆発
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
            // 敵と自機から発射した弾丸の当たり判定
            foreach (Enemy enemy in Enemies)
            {
                // JikiBurrets配列要素の初めをみる 
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

            // 既に自機死亡のときは必要ない
            if (Jiki.IsDead)
                return;

            // 自機と敵から発射した弾丸の当たり判定 EnemyBurrets配列要素の初めをみる
            Burret enemyBurret = EnemyBurrets.FirstOrDefault(x => IsHit(Jiki, x));
            if (enemyBurret != null)
            {
                EnemyBurrets.Remove(enemyBurret);
                Jiki.IsDead = true;
                JikiDead();
            }
            EnemyBurrets = EnemyBurrets.Where(x => !x.IsDead).ToList();

            // 自機と敵そのものの当たり判定 Enemies配列要素の初めをみる
            Enemy enemy1 = Enemies.FirstOrDefault(x => IsHit(Jiki, x));
            if (enemy1 != null)
            {
                Enemies.Remove(enemy1);
                Jiki.IsDead = true;
                JikiDead();
            }

        }
        // まず短形の重なり判定
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

        // 敵と弾の当たり判定
        bool IsHit(Enemy enemy, Burret burret)
        {
            Rectangle rect1 = new Rectangle(enemy.X, enemy.Y, EnemyBitmap1.Width, EnemyBitmap1.Height);
            Rectangle rect2 = new Rectangle((int)burret.X, (int)burret.Y, BurretBitmap.Width, BurretBitmap.Height);
            return IsHit(rect1, rect2);
        }

        // 自機と弾の当たり判定
        bool IsHit(Jiki jiki, Burret burret)
        {
            Rectangle rect1 = new Rectangle(jiki.X, jiki.Y, JikiBitmap.Width, JikiBitmap.Height);
            Rectangle rect2 = new Rectangle((int)burret.X, (int)burret.Y, BurretBitmap.Width, BurretBitmap.Height);
            return IsHit(rect1, rect2);
        }

        // 自機と敵の当たり判定
        bool IsHit(Jiki Jiki, Enemy enemy)
        {
            Rectangle rect1 = new Rectangle(Jiki.X, Jiki.Y, JikiBitmap.Width, JikiBitmap.Height);
            Rectangle rect2 = new Rectangle((int)enemy.X, (int)enemy.Y, EnemyBitmap1.Width, EnemyBitmap1.Height);
            return IsHit(rect1, rect2);
        }

        void Explosion(int centerX, int centerY)
        {
            // 128個の直径4ピクセルの赤～オレンジ色の円をつくる
            // 乱数でうまい具合に広がるようにする
            for (int i=0; i< 128; i++)
            {
                double vx = (Random.Next(40) - 20) / 10.0;
                double vy = (Random.Next(40) - 20) / 10.0;
                Ellipses.Add(new Ellipse(centerX, centerY, vx, vy));
            }
        }
        void EnemyDead(Enemy enemy)
        {
            // 敵の中心が爆発の中心になるようにする
            int x = enemy.X + this.EnemyBitmap1.Width / 2;
            int y = enemy.Y + this.EnemyBitmap1.Height / 2;
            Explosion(x, y);
            Score += 10;    // 10点追加
        }

        void JikiDead()
        {
            // 自機の中心が爆発の中心になるようにする
            int x = Jiki.X + this.JikiBitmap.Width / 2;
            int y = Jiki.Y + this.JikiBitmap.Height / 2;
            Explosion(x, y);
        }

        void ShowScore(Graphics graphics)
        {
            string scoreString = Score.ToString("00000");
            graphics.DrawString(scoreString, new Font("MS ゴシック", 18), new SolidBrush(Color.White), new Point(10, 10));
        }

        Bitmap GetJikiBitmap()
        {
            //戦闘機であれば左上の座標は (57,1)、 幅44、高さ48
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
            // 黄色いひよこ。左上の座標は(4,61)、幅24、高さ28です。
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
            // ピンク色のひよこ。左上の座標は(4, 94)、幅24、高さ28
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
            // 青色のひよこ。左上の座標は(4,125)、幅24、高さ28です。
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
            // 弾丸。左上の座標は(41, 46)、幅14、高さ14
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
            // 敵と敵の弾丸をクリアする
            Enemies.Clear();
            EnemyBurrets.Clear();

            // 自機を初期位置に戻し、死亡状態から回復させる
            Jiki.X = (FormClientSize.Width - JikiBitmap.Width) / 2;
            Jiki.Y = (int)(FormClientSize.Height * 0.8);
            Jiki.IsDead = false;

            // スコアを0に戻してゲームスタート
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
