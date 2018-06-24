using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Game1
{
    enum UICase
    {
        Invincible,
        Reloading,
        Ammo
    }

    enum ColorCase
    {
        Black,
        Red,
        Blue,
        Yellow,
        White
    }

    class UserInterface
    {
        private SpriteFont font;
        private List<Texture2D> primaryColor;
        private List<Texture2D> complementColor;
        private List<Texture2D> whiteLine;
        private Texture2D blackBG;
        private Rectangle spriteBox;

        private double score;
        private int multiplier;
        private int ammo;
        private Timer timeLeft;
        private string time;

        private ColorCase cCase;
        private UICase uiCase;

        public UserInterface(SpriteFont font, List<Texture2D> primaryColor, List<Texture2D> complementColor, List<Texture2D> whiteLine, Texture2D background)
        {
            this.font = font;
            this.primaryColor = primaryColor;
            this.complementColor = complementColor;
            this.whiteLine = whiteLine;
            blackBG = background;
            spriteBox = new Rectangle(950, 0, 200, 950);
            timeLeft = new Timer(1000 * 60 * 2); // 3 minutes
            time = "";


        }

        public bool Update(Player p, GameTime gameTime)
        {
            score = p.PlayerScore;
            multiplier = p.Multiplier;
            ammo = p.ShotCount;

            if (p.IsInvincible)
            {
                uiCase = UICase.Invincible;
                cCase = ColorCase.Black;
            }
            else if (p.IsReloading)
            {
                uiCase = UICase.Reloading;
                cCase = ColorCase.White;
            }
            else
            {
                if(p.Streak > 20)
                {
                    uiCase = UICase.Ammo;
                    cCase = ColorCase.Yellow;
                }
                else if(p.Streak > 10)
                {
                    uiCase = UICase.Ammo;
                    cCase = ColorCase.Blue;
                }
                else
                {
                    uiCase = UICase.Ammo;
                    cCase = ColorCase.Red;
                }
            }

            return timeLeft.UpdateTimer(gameTime);
        }

        public void Draw(SpriteBatch sb)
        {
            time = "" + (((int)timeLeft.TimeLeft / 1000) / 60) + " : " + (((int)timeLeft.TimeLeft / 1000) % 60);
            sb.Draw(blackBG, spriteBox, Color.White);
            sb.DrawString(font, "SCORE", new Vector2(53 + 950, 32), Color.White);
            sb.DrawString(font, "" + score, new Vector2(53 + 950, 84), Color.White);
            sb.DrawString(font, "MULTI", new Vector2(53 + 950, 182), Color.White);
            sb.DrawString(font, "x" + multiplier, new Vector2(53 + 950, 232), Color.White);
            switch (uiCase)
            {
                case UICase.Invincible:
                    sb.DrawString(font, "INVINCIBLE", new Vector2(10 + 950, 345), Color.White);
                    sb.Draw(whiteLine[0], new Rectangle(40 + 950, 375, 120, 25), Color.White);
                    break;
                case UICase.Reloading:
                    sb.DrawString(font, "RELOADING", new Vector2(20 + 950, 336), Color.White);
                    sb.Draw(whiteLine[1], new Rectangle(60 + 950, 375, 100, 25), Color.White);
                    break;
                case UICase.Ammo:
                    sb.DrawString(font, "AMMO", new Vector2(53 + 950, 336), Color.White);
                    sb.DrawString(font, "" + ammo + " / INF", new Vector2(35 + 950, 375), Color.White);
                    break;
            }

            sb.DrawString(font, "COLOR", new Vector2(54 + 950, 455), Color.White);
            sb.DrawString(font, "COMP", new Vector2(63 + 950, 632), Color.White);

            switch (cCase)
            {
                case ColorCase.Black:
                    sb.Draw(primaryColor[3], new Rectangle(68 + 950, 516, 64, 64), Color.White);
                    sb.Draw(complementColor[2], new Rectangle(68 + 950, 693, 64, 64), Color.White);
                    break;
                case ColorCase.White:
                    sb.Draw(complementColor[3], new Rectangle(68 + 950, 516, 64, 64), Color.White);
                    sb.Draw(complementColor[3], new Rectangle(68 + 950, 693, 64, 64), Color.White);
                    break;
                case ColorCase.Yellow:
                    sb.Draw(complementColor[3], new Rectangle(68 + 950, 516, 21, 64), Color.Fuchsia);
                    sb.Draw(primaryColor[2], new Rectangle(68 + 950, 516, 64, 64), Color.White);
                    sb.Draw(complementColor[2], new Rectangle(68 + 950, 693, 64, 64), Color.White);
                    break;
                case ColorCase.Blue:
                    sb.Draw(complementColor[3], new Rectangle(68 + 950, 516, 32, 64), Color.Fuchsia);
                    sb.Draw(primaryColor[1], new Rectangle(68 + 950, 516, 64, 64), Color.White);
                    sb.Draw(complementColor[1], new Rectangle(68 + 950, 693, 64, 64), Color.White);
                    break;
                case ColorCase.Red:
                    sb.Draw(primaryColor[0], new Rectangle(68 + 950, 516, 64, 64), Color.White);
                    sb.Draw(complementColor[0], new Rectangle(68 + 950, 693, 64, 64), Color.White);
                    break;
            }

            sb.DrawString(font, time, new Vector2(53 + 950, 850), Color.White);
        }
    }
}
