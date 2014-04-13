using System;
using System.Collections.Generic;
using System.Text;
using Tao.OpenGl;
using System.Windows.Forms;
using ShadowEngine;
using ShadowEngine.Sound;
using ShadowEngine.ContentLoading;
using WMPLib; 

namespace Naves
{
    public class SpaceShip
    {
        int vidas = 3;
        Position p = new Position(0, 0, 0);
        int count;
        int compare = 10;
        ModelContainer m;
        bool moviendo;

        public bool Moviendo
        {
            get { return moviendo; }
            set { moviendo = value; }
        }

        public int Vidas
        {
            get { return vidas; }
            set { vidas = value; }
        }

        public void Reiniciar()
        {
            vidas = 3;
            p = new Position(0, 0, 0);
        }

        public void Create()
        {
             m = ContentManager.GetModelByName("nave.3DS");
             m.CreateDisplayList(); 
        }

        public void Dibujar()
        {
            if (vidas > 0)
            {
                switch (Main.Moviendo)
                {
                    case 1:
                        {
                            if (p.x < 3)
                                p.x += 0.08f;
                            break;
                        }
                    case -1:
                        {
                            if (p.x > -3)
                                p.x -= 0.08f;
                            break;
                        }
                    case 2:
                        {
                            if (p.y < 3)
                                p.y += 0.08f;
                            break;
                        }
                    case -2:
                        {
                            if (p.y > -3)
                                p.y -= 0.08f;
                            break;
                        }
                    default:
                        break;
                }

                count++;
                if (count == compare)
                {
                    if (compare == 25)
                    {
                        compare = 10;
                    }
                    Position wingLeft = new Position(this.p.x - 1f, this.p.y, this.p.z);
                    Position wingRight = new Position(this.p.x + 1f, this.p.y, this.p.z);
                    if (AsteroidGenerator.CheckCollision(this.p, 1.1f))
                    {
                        RestarVida(); 
                    }
                    else
                        if (AsteroidGenerator.CheckCollision(wingRight, 1.1f))
                        {
                            RestarVida(); 
                        }
                        else
                            if (AsteroidGenerator.CheckCollision(wingLeft, 1.1f))
                            {
                                RestarVida(); 
                            }
                    count = 0;
                }


                Gl.glPushMatrix();
                Gl.glTranslatef(p.x, p.y, p.z);
                Gl.glScalef(0.3f, 0.3f, 0.3f);
                m.DrawWithTextures();
                Gl.glPopMatrix();
                Gl.glDisable(Gl.GL_TEXTURE_2D);
            }

           
        }
        WMPLib.WindowsMediaPlayer Player;

        public void PlayFile(String url)
        {
            Player = new WMPLib.WindowsMediaPlayer();
            Player.PlayStateChange +=
                new WMPLib._WMPOCXEvents_PlayStateChangeEventHandler(Player_PlayStateChange);
            Player.MediaError +=
                new WMPLib._WMPOCXEvents_MediaErrorEventHandler(Player_MediaError);
            Player.URL = url;
            Player.controls.play();
        }
        private void Player_PlayStateChange(int NewState)
        {
            if ((WMPLib.WMPPlayState)NewState == WMPLib.WMPPlayState.wmppsStopped)
            {
                //this.Close();
            }
        }

        private void Player_MediaError(object pMediaObject)
        {
           // MessageBox.Show("Cannot play media file.");
           // this.Close();
        }

        void RestarVida()
        {
            try
            {
                PlayFile("approaching-alien.mp3");

            }
            catch (Exception ex)
            { }
            compare = 25;
            vidas--;
            p = new Position(0, 0, 0);
        }
    }
}
