using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pluscourtchemin
{
    public class Node2 : GenericNode 
    {
        public int x;
        public int y;

        // Méthodes abstrates, donc à surcharger obligatoirement avec override dans une classe fille
        public override bool IsEqual(GenericNode N2)
        {
            Node2 N2bis = (Node2)N2;

            return (x == N2bis.x) && (y == N2bis.y);
        }

        public override double GetArcCost(GenericNode N2)
        {
            // Ici, N2 ne peut être qu'1 des 8 voisins, inutile de le vérifier
            Node2 N2bis = (Node2)N2;
            return Math.Sqrt((N2bis.x-x)*(N2bis.x-x)+(N2bis.y-y)*(N2bis.y-y));
        }

        public override bool EndState()
        {
            return (x == Form1.xfinal) && (y == Form1.yfinal);
        }

        public override List<GenericNode> GetListSucc()
        {
            List<GenericNode> lsucc = new List<GenericNode>();

            for (int dx=-1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if ((x + dx >= 0) && (x + dx < Form1.nbcolonnes)
                            && (y + dy >= 0) && (y + dy < Form1.nblignes) && ((dx != 0) || (dy != 0)))
                        if (Form1.matrice[x + dx, y + dy] != -1)
                        {
                            Node2 newnode2 = new Node2();
                            newnode2.x = x + dx;
                            newnode2.y = y + dy;
                            lsucc.Add(newnode2);
                        }
                }

            }
            return lsucc;
        }

        public override double CalculeHCost()
        {  //Environnement1
           Node2 N3 = new Node2(); //N3 est le noeux d'arrivée
           N3.x = Form1.xfinal;
           N3.y = Form1.yfinal;

            double distGlobale;

            //Distance euclidienne 

            //distGlobale = GetArcCost(N3);
           
            //Distance de Manhattan

           distGlobale = Math.Abs(N3.x - x) + Math.Abs(N3.y - y);
            //Selon les tests la distance de Manhattan est la plus pertinente (en comparaison de l'euclidienne) pour l'environnement 1 

            //Environnement2
            //On part du principe qu'il y a une barrière verticale avec un seul passage
           

            Node2 N4 = new Node2(); //N4 représente le passage dans la barrière 

            //Il faut repérer la case N4 par laquelle passer 

            for (int i=0; i<20 ;i++) //On repère dans quelle colonne se trouve la barrière
            {
                if ((Form1.matrice[i,1]==-1) || (Form1.matrice[i,2]==-1))
                {
                    N4.x=i;
                }
            }
                    
            for (int j=0; j<20;j++) //On repère dans quelle ligne se trouve le passage
            { 
                if (Form1.matrice[N4.x,j]==0)
                {
                    N4.y=j;
                }
            }

            if (((N3.x>N4.x)&&(x<N4.x))||((N3.x<N4.x)&&(x>N4.x))) //Si le point d'arrivée et le point actuel ne sont pas du même côté de la barrière
            {  
                int distInter1 = Math.Abs(N4.x - x) + Math.Abs(N4.y - y); //Calcul de la distance entre le point actuel et le passage 
                int distInter2= Math.Abs(N3.x - N4.x) + Math.Abs(N3.y - N4.y);//Calcul entre le passage et le point d'arrivée 
                distGlobale = distInter1 + distInter2 ;//Calcul de la distance total
            }  
            
            else 
            {
                distGlobale = Math.Abs(N3.x - x) + Math.Abs(N3.y - y); //Si le point actuel et le point d'arrivée sont du même côté: calcul direct
            } 

            // Environnement 3
            double coutCercle = 0;
           
            
            //Si le point a une abcisse entre 2 et 8 et une ordonnée entre 3 et 9, il est dans le cercle
            bool pointInitialDansCercle1 = (2 < Form1.xinitial) && (Form1.xinitial < 8) && (3 < Form1.yinitial) && (Form1.yinitial < 9);
            bool pointFinalDansCercle1 = (2 < Form1.xfinal) && (Form1.xfinal < 8) && (3 < Form1.yfinal) && (Form1.yfinal < 9);
            bool pointInitialDansCercle2 = (10 < Form1.xinitial) && (Form1.xinitial < 17) && (11 < Form1.yinitial) && (Form1.yinitial < 18);
            bool pointFinalDansCercle2 = (10 < Form1.xfinal) && (Form1.xfinal < 17) && (11 < Form1.yfinal) && (Form1.yfinal < 18);

            bool pointActuelDansCercle1 = (2 < x) && (x < 8) && (3 < y) && (y < 9);
            bool pointActuelDansCercle2 = (10 < x) && (x < 17) && (11 < y) && (y < 18);
            // On défnit les conditions sur les positions des points initiaux et finaux par rapport au cercle
            if ((pointInitialDansCercle1 == true && pointFinalDansCercle2== false) || (pointFinalDansCercle1 == true && pointInitialDansCercle2 == false))
            {
                Form1.matrice[16, 14] = -1;
                Form1.matrice[16, 15] = -1;
            }
            else if ((pointInitialDansCercle2 == true && pointFinalDansCercle1 == false) || (pointFinalDansCercle2 == true && pointInitialDansCercle1 == false))
            {
                Form1.matrice[3, 6] = -1;
                Form1.matrice[3, 7] = -1;

            }
            else if (!pointInitialDansCercle1 && !pointFinalDansCercle1 && !pointInitialDansCercle2 && !pointFinalDansCercle2)
            {
                Form1.matrice[3, 6] = -1;
                Form1.matrice[3, 7] = -1;
                Form1.matrice[16, 14] = -1;
                Form1.matrice[16, 15] = -1;
            }

            if ((pointActuelDansCercle1 && !pointFinalDansCercle1) || (pointFinalDansCercle1 && !pointActuelDansCercle1))
            {
                coutCercle += 10 * (Math.Abs(2 - x) + Math.Abs(6 - y)); //SI le point est dans le cercle 1, et doit aller vers un point hors du cercle 2, il en sort et on effectue la distance de manhattan
            }
            if ((pointActuelDansCercle2 && !pointFinalDansCercle2) || (pointFinalDansCercle2 && !pointActuelDansCercle2))
            {
                coutCercle += 10 * (Math.Abs(17 - x) + Math.Abs(15 - y));
            }

            double H1 = Math.Abs(Form1.xfinal - x) + Math.Abs(Form1.yfinal - y);

            return coutCercle + H1;
        }
      


        public override string ToString()
        {
            return Convert.ToString(x)+","+ Convert.ToString(y);
        }
    }
}


            
            
