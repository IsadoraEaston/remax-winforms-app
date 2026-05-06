using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prjWinCsImmoRemax
{
    public static class clsSourceDeDonnees
    {
        //Methode pour charger les donnees de la base de donnees dans le DataSet
        public static void ChargerBD()
        {
            //Si le DataSet exite deja, le vider au lieu d'en creer un nouveau
            if(clsGlobal.setImmoRemaxDB == null)
            {
                clsGlobal.setImmoRemaxDB = new DataSet();
            }
            else
            {
                clsGlobal.setImmoRemaxDB.Clear();
            }

            SqlConnection myCon = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ImmoRemax2DB;Integrated Security=True");
            myCon.Open();

            string sql = "SELECT EmployeeId, Nom, Role, Password, Telephone FROM Employes";
            SqlCommand myCmd = new SqlCommand(sql, myCon);
            clsGlobal.adpEmployes = new SqlDataAdapter(myCmd);
            clsGlobal.adpEmployes.Fill(clsGlobal.setImmoRemaxDB, "Employes");

            sql = "SELECT ClientId, Nom, Type, Courriel, Telephone, EmpId FROM Clients";
            myCmd = new SqlCommand(sql, myCon);
            clsGlobal.adpClients = new SqlDataAdapter(myCmd);
            clsGlobal.adpClients.Fill(clsGlobal.setImmoRemaxDB, "Clients");

            sql = "SELECT MaisonId, Rue, Ville, Province, CodePostal, Prix, Type, EmpId, IdClient FROM Maisons";
            myCmd = new SqlCommand(sql, myCon);
            clsGlobal.adpMaisons = new SqlDataAdapter(sql, myCon);
            clsGlobal.adpMaisons.Fill(clsGlobal.setImmoRemaxDB, "Maisons");

            myCon.Close();
        }

        public static void SaveEmployes()
        {
            SqlCommandBuilder myBuilder = new SqlCommandBuilder(clsGlobal.adpEmployes);
            clsGlobal.adpEmployes.Update(clsGlobal.setImmoRemaxDB, "Employes");
        }

        public static void SaveClients()
        {
            SaveEmployes();

            SqlCommandBuilder myBuilder = new SqlCommandBuilder(clsGlobal.adpClients);
            clsGlobal.adpClients.Update(clsGlobal.setImmoRemaxDB, "Clients");
        }

        public static void SaveMaisons()
        {
            SaveEmployes();
            SaveClients();

            SqlCommandBuilder myBuilder = new SqlCommandBuilder(clsGlobal.adpMaisons);
            clsGlobal.adpMaisons.Update(clsGlobal.setImmoRemaxDB, "Maisons");

        }
        public static DataTable GetClientsDeLAgent(string IdEmploye)
        {
            DataTable tabClient = clsGlobal.setImmoRemaxDB.Tables["Clients"];
            //Créer une table temporaire pour stocker les clients de l'employé
            DataTable tabClientsDeLAgent = tabClient.Clone();
            foreach (DataRow row in tabClient.Rows)
            {
                if (row["EmpId"].ToString() == IdEmploye)
                {
                    tabClientsDeLAgent.ImportRow(row);
                }
            }
            return tabClientsDeLAgent;
        }

        public static DataTable GetMaisonsDuClient(string IdClient)
        {
            DataTable tabMaison = clsGlobal.setImmoRemaxDB.Tables["Maisons"];

            //Créer une table temporaire pour stocker les maisons du client
            DataTable tabMaisonsDuClient = tabMaison.Clone();
            foreach (DataRow row in tabMaison.Rows)
            {
                if (row["IdClient"].ToString() == IdClient)
                {
                    tabMaisonsDuClient.ImportRow(row);
                }
            }
            return tabMaisonsDuClient;
        }

        public static DataTable GetMaisonsDeLAgent(string idAgent)
        {
            DataTable tabClients = clsGlobal.setImmoRemaxDB.Tables["Clients"];
            DataTable tabMaisons = clsGlobal.setImmoRemaxDB.Tables["Maisons"];

            //Creer une table 
            DataTable tabMaisonsAgents = tabMaisons.Clone();

            //Trouver les clients appartenant a cet agent
            DataRow[] clientsAgent = tabClients.Select("EmpId = '" + idAgent + "'");

            foreach (DataRow client in clientsAgent)
            {
                string idClient = client["ClientId"].ToString();

                //Recuperer les maisons de ce client
                DataTable maisonsDuClient = GetMaisonsDuClient(idClient);

                //Ajouter ces maisons dans la table finale
                foreach(DataRow maison in maisonsDuClient.Rows)
                {
                    tabMaisonsAgents.ImportRow(maison);
                }
            }
            return tabMaisonsAgents;
        }


    }
}
