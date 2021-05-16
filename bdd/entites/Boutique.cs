﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Core;
using Windows.System;
using System.ComponentModel;
using System.Data.SqlClient;

namespace VéloMax.bdd
{
    public class Boutique
    {
        /* Attributs */
        public readonly int numB;

        public string numBString { get { return numB.ToString(); } }

        public string nomB
        {
            get => ControlleurRequetes.ObtenirChampString("Boutique", "numB", numB, "nomB");
            set { ControlleurRequetes.ModifierChamp("Boutique", "numB", numB, "nomB", value); }
        }
        public int numA
        {
            get => ControlleurRequetes.ObtenirChampInt("Boutique", "numB", numB, "numA");
            set { ControlleurRequetes.ModifierChamp("Boutique", "numB", numB, "numA", value); }
        }

        public string numAString { get { return numA.ToString(); } }
        public string telB
        {
            get => ControlleurRequetes.ObtenirChampString("Boutique", "numB", numB, "telB");
            set { ControlleurRequetes.ModifierChamp("Boutique", "numB", numB, "telB", value); }
        }
        public string mailB
        {
            get => ControlleurRequetes.ObtenirChampString("Boutique", "numB", numB, "mailB");
            set { ControlleurRequetes.ModifierChamp("Boutique", "numB", numB, "mailB", value); }
        }

        public string remise = "0";
        public string adresse;

        /* Instantiation */
        public Boutique(int numB)
        {
            this.numB = numB;
        }
        public Boutique(string nomB, int numA, string telB, string mailB)
        {
            ControlleurRequetes.Inserer($"INSERT INTO Boutique (nomB, numA, telB, mailB) VALUES ('{nomB}', {numA}, '{telB}', '{mailB}')");
            this.numB = ControlleurRequetes.DernierIDUtilise();
        }

        /* Suppression */
        public void Supprimer()
        {
            new Adresse(numA).Supprimer();
            ControlleurRequetes.SupprimerElement("Boutique", "numB", numB);
        }

        /* Liste */
        public static ReadOnlyCollection<Boutique> Lister()
        {
            List<Boutique> list = new List<Boutique>();
            ControlleurRequetes.SelectionnePlusieurs($"SELECT numB FROM Boutique", (MySqlDataReader reader) => { list.Add(new Boutique(reader.GetInt32("numB"))); });
            return new ReadOnlyCollection<Boutique>(list);
        }

        public static ObservableCollection<Boutique> GetBoutiques()
        {
            var boutiques= new List<Boutique>();
            ControlleurRequetes.SelectionnePlusieurs($"select * from boutique natural join adresse;", (MySqlDataReader reader) => {
                Boutique boutique = new Boutique(reader.GetInt32("numB"));
                boutique.numA = reader.GetInt32(0);
                boutique.nomB = reader.GetString(2);
                boutique.telB = reader.GetString(3);
                boutique.mailB = reader.GetString(4);
                boutique.adresse = reader.GetString(5) + " " + reader.GetString(6) + " " + reader.GetString(7) + " " + reader.GetString(8) + " ";
                boutique.remise = "0";
                boutiques.Add(boutique); 
            });
            return new ObservableCollection<Boutique>(boutiques);

        }

    }
}
