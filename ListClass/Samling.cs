using System;

namespace GuldKortKlass
{
    /*
     *OBS!! 
     *denna generisk samlingsklass
     *används inte mycket av programmet. 
     *Se anteckningar i rapporten för anledningar
     *
     *Den finns dock kvar för att den, tror jag, 
     *fungerar!
     * 
     * */
    
    
    /// <summary>
    /// Generisk listklass
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Samling<T>
    {
        //deklarerar variabler
        protected int buffert;
        protected T[] lista;
        protected int längd;
        protected int antal;

        /// <summary>
        /// konstruktormetod
        /// </summary>
        public Samling()
        {
            buffert = 30;
            antal = 0;
            längd = 30;
            lista = new T[längd];
        }

        /// <summary>
        /// öka antal element som listan kan lagra
        /// </summary>
        /// <param name="storlek"></param>
        protected void Expandera (int storlek)
        {
            if (storlek < 1) return;

            T[] temp = new T[längd + storlek];

            for (int i = 0; i < antal; i++)
            {
                temp[i] = lista[i];
            }
            lista = temp;
            längd += storlek;
        }

        /// <summary>
        /// minska antal element som listan kan lagra
        /// </summary>
        protected void Reducera()
        {
            T[] temp = new T[antal];

            for (int i = 0; i < antal; i++)
            {
                temp[i] = lista[i];
            }
            lista = temp;
            längd = antal;
        }

        /// <summary>
        /// metod som lägga till ett element i listan
        /// </summary>
        /// <param name="e"> Samling<T> </param>
        public void LäggTill (T e)
        {
            if (antal + 1 > längd)
            {
                Expandera(1 + buffert);
            }
            lista[antal++] = e;
        }

        /// <summary>
        /// metod som tar bort ett element från listan
        /// och minska antal möjliga element
        /// </summary>
        /// <param name="index"> int index av element som tas bort</param>
        /// <returns></returns>
        public T TaBort(int index)
        {
            T temp = lista[index];

            for (int i = index; i < antal - 1; i++)
            {
                lista[i] = lista[i + 1];
            }
            antal--;
            
            if (längd - antal > buffert)
            {
                Reducera();
            }
            return temp;
        } 

        /// <summary>
        /// metod som returnerar värdet från ett element
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T ElementFrån(int index)
        {
            return lista[index];
        }

        /// <summary>
        /// metod som söker objektet och returnerar int
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public int Sök (T element)
        {
            for (int i = 0; i < lista.Length; i++)
            {
                if (lista[i].Equals(element))
                {
                    return i;
                }
            }
            return -1;
        }  
    }
}
