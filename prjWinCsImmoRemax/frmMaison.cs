using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prjWinCsImmoRemax
{
    public partial class frmMaison : Form
    {
        DataTable tabEmployes;
        DataTable tabClients;
        DataTable tabMaisons;
        int indexMaison;
        string mode = ""; //Ajout ou Modif

        public frmMaison()
        {
            InitializeComponent();
        }

        private void frmMaison_Load(object sender, EventArgs e)
        {
            //Recharger depuis le Bd pour avoir les dernieres donnees
            clsSourceDeDonnees.ChargerBD();

            //Charger les tables globales
            tabMaisons = clsGlobal.setImmoRemaxDB.Tables["Maisons"];
            tabClients = clsGlobal.setImmoRemaxDB.Tables["Clients"];
            tabEmployes = clsGlobal.setImmoRemaxDB.Tables["Employes"];



            //Filtrage selon le role connecte
            if (frmPrincipal.UserConnecteRole == "Agent")
            {
                string idAgent = frmPrincipal.UserConnecteId;

                // Recuperer seulement les maisons de cet agent
                tabMaisons = clsSourceDeDonnees.GetMaisonsDeLAgent(idAgent);

                //si aucune maison
                if (tabMaisons.Rows.Count == 0)
                {
                    indexMaison = -1;  // Pas de maison a afficher
                }
                else
                {
                    indexMaison = 0;  // Afficher la premiere maison
                }
            }

            //Remplir le ComboBox Provinces
            cboProvince.Items.Clear();
            cboProvince.Items.Add("Quebec");
            cboProvince.Items.Add("Ontario");
            cboProvince.Items.Add("Alberta");
            cboProvince.Items.Add("British-Columbia");
            cboProvince.Items.Add("Manitoba");

            //Remplir le ComboBox Type
            cboType.Items.Clear();
            cboType.Items.Add("Condo");
            cboType.Items.Add("Uni Familial");
            cboType.Items.Add("Chalet");

            //Remplir les combos Agent et Client
            RemplirListeAgents();
            RemplirListeClients();

            //Remplir la liste des maisons
            RemplirListeMaisons();

            //Afficher la 1ere maison
            indexMaison = 0;
            AfficherTabDansTextBox();
            MontrerBoutons(true, false);
            

        }
        private void btnAjouter_Click(object sender, EventArgs e)
        {
            mode = "ajout";

            //Recharger depuis la BD
            clsSourceDeDonnees.ChargerBD();

            //Pointer sur les tables du clsGlobal
            tabMaisons = clsGlobal.setImmoRemaxDB.Tables["Maisons"];
            tabClients = clsGlobal.setImmoRemaxDB.Tables["Clients"];
            tabEmployes = clsGlobal.setImmoRemaxDB.Tables["Employes"];

            //  Rafraichir les donnees pour avoir les derniers clients
            //  si les formes sont ouvertes
            //RafraichirDonnees();

            //Vider les text box
            txtMaisonId.Text = txtRue.Text = txtVille.Text = txtCPostal.Text = txtPrix.Text = "";
            cboProvince.SelectedIndex = cboType.SelectedIndex = -1;
            txtMaisonId.Focus();

            //Recharger les listes
            RemplirListeAgents();
            RemplirListeClients();

            lblInfosMaisons.Text = "Ajout d'une nouvelle maison";
            MontrerBoutons(false, true);
        }

        private void btnModifier_Click(object sender, EventArgs e)
        {
            mode = "modif";
            txtMaisonId.Enabled = false; //L'ID ne peut pas etre modifie
            txtRue.Focus();
            lblInfosMaisons.Text = "Modification de la maison ID: " + txtMaisonId.Text;
            MontrerBoutons(false, true);
        }

        private void btnSupprimer_Click(object sender, EventArgs e)
        {
            //Demander confirmation avant de supprimer
            string msg = "Êtes-vous sur de vouloir supprimer cette maison?";
            string tit = "Confirmation de suppression";
            if(MessageBox.Show(msg, tit,MessageBoxButtons.YesNo, MessageBoxIcon.Warning) 
                == DialogResult.Yes)
            {
                //Supprimer la maison du DataTable
                //tabMaisons.Rows[indexMaison].Delete();
                //Supprimer du DataSet principal
                DataRow[] rows = clsGlobal.setImmoRemaxDB.Tables["Maisons"].Select(
                    "MaisonId = '" + tabMaisons.Rows[indexMaison]["MaisonId"] + "'"
                );

                if (rows.Length > 0)
                {
                    rows[0].Delete();
                }
                //Sauvegarder les changements dans la base de donnees
                clsSourceDeDonnees.SaveMaisons();
                //Recharger le DataSet
                clsSourceDeDonnees.ChargerBD();

                //Recharher la liste filtree
                if (frmPrincipal.UserConnecteRole == "Agent")
                {
                    string idAgent = frmPrincipal.UserConnecteId;
                    tabMaisons = clsSourceDeDonnees.GetMaisonsDeLAgent(idAgent);
                }
                else
                {
                    tabMaisons = clsGlobal.setImmoRemaxDB.Tables["Maisons"];
                }

                //Mettre a jour l'affichage
                RemplirListeMaisons();
                // Repositionner l'indexMaison
                // ⭐ Repositionner l'index
                if (tabMaisons.Rows.Count > 0)
                {
                    if (indexMaison >= tabMaisons.Rows.Count)
                        indexMaison = tabMaisons.Rows.Count - 1;

                    AfficherTabDansTextBox();
                }
                else
                {
                    indexMaison = -1;
                    MessageBox.Show("Aucune maison a afficher.", "Information");
                }
                AfficherTabDansTextBox();
            }
        }

        private void btnSauvegarder_Click(object sender, EventArgs e)
        {
            //Recuperer les valeurs saisies
            string idSaisi = txtMaisonId.Text.Trim();
            string rueSaisi = txtRue.Text.Trim();
            string villeSaisi = txtVille.Text.Trim();
            string provSaisi = cboProvince.Text;
            string codPosSaisi = txtCPostal.Text;
            string prixSaisi = txtPrix.Text.Trim();
            string typSaisi = cboType.Text;
            string empSaisi = TrouverNumAgent(cboAgent.Text);
            string cliSaisi = TrouverNumClient(cboClient.Text);


            if (mode == "ajout")
            {
                //Verifier dans ajout si employe id existe deja (Cle Primaire)
                DataRow[] result = tabMaisons.Select("MaisonId = '" + idSaisi + "'");
                if (result.Length > 0)
                {
                    MessageBox.Show("Cet identifiant existe deja. Veuillez en choisir un autre.",
                        "Doublon detecte",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    txtMaisonId.Focus();
                    return;
                }
                //Creer une nouvelle ligne dans le DataTable
                DataRow myRow = clsGlobal.setImmoRemaxDB.Tables["Maisons"].NewRow();
                myRow["MaisonId"] = idSaisi;
                myRow["Rue"] = rueSaisi;
                myRow["Ville"] = villeSaisi;
                myRow["Province"] = provSaisi;
                myRow["CodePostal"] = codPosSaisi;
                myRow["Prix"] = prixSaisi;
                myRow["Type"] = typSaisi;
                myRow["EmpId"] = empSaisi;
                myRow["IdClient"] = cliSaisi;

                clsGlobal.setImmoRemaxDB.Tables["Maisons"].Rows.Add(myRow);
            }
            else if (mode == "modif")
            {
                //Pointer sur la maison courante via l'indexMaison
                DataRow myRow = tabMaisons.Rows[indexMaison];
                myRow["MaisonId"] = idSaisi;
                myRow["Rue"] = rueSaisi;
                myRow["Ville"] = villeSaisi;
                myRow["Province"] = provSaisi;
                myRow["CodePostal"] = codPosSaisi;
                myRow["Prix"] = prixSaisi;
                myRow["Type"] = typSaisi;
                myRow["EmpId"] = empSaisi;
                myRow["IdClient"] = cliSaisi;

            }
            //Sauvegarder le contenu du DataSet dans la BD (synchronisation)
            clsSourceDeDonnees.SaveMaisons();

            //Recharger les donnees depuis la BD
            clsSourceDeDonnees.ChargerBD();

            //Recharger la liste filtree selon le role
            if(frmPrincipal.UserConnecteRole == "Agent")
            {
                string idAgent = frmPrincipal.UserConnecteId;
                tabMaisons = clsSourceDeDonnees.GetMaisonsDeLAgent(idAgent);
            }
            else
            {
                tabMaisons = clsGlobal.setImmoRemaxDB.Tables["Maisons"];
            }
            if(mode == "ajout")
            {
                //Chercher la maison qu'on vient d;ajouter
                for(int i = 0; i < tabMaisons.Rows.Count; i++)
                {
                    indexMaison = i;
                    break;
                }
            }

            RemplirListeMaisons();
            if(tabMaisons.Rows.Count > 0 && indexMaison >= 0)
            {
                AfficherTabDansTextBox();
            }

            MontrerBoutons(true, false);
            txtMaisonId.Enabled = true;
        }

        private void btnAnnuler_Click(object sender, EventArgs e)
        {
            AfficherTabDansTextBox();
            MontrerBoutons(true, false);
            txtMaisonId.Enabled = true;

        }
        private void btnPremier_Click(object sender, EventArgs e)
        {
            indexMaison = 0;
            AfficherTabDansTextBox();
        }

        private void btnPrecedent_Click(object sender, EventArgs e)
        {
            if (indexMaison > 0)
            {
                indexMaison--;
                AfficherTabDansTextBox();
            }
        }

        private void btnSuivant_Click(object sender, EventArgs e)
        {
            if (indexMaison < tabMaisons.Rows.Count - 1)
            {
                indexMaison++;
                AfficherTabDansTextBox();
            }
        }

        private void btnDernier_Click(object sender, EventArgs e)
        {
            indexMaison = tabMaisons.Rows.Count - 1;
            AfficherTabDansTextBox();
        }

        
        private void MontrerBoutons(bool btnsAjMod, bool btnsSauvAn)
        {
            btnAjouter.Visible = btnModifier.Visible = btnSupprimer.Visible = btnsAjMod;
            btnSauvegarder.Visible = btnAnnuler.Visible = btnsSauvAn;
        }

        private void AfficherTabDansTextBox()
        {
            // ⭐ Verifier s'il y a des maisons
            if (indexMaison < 0 || tabMaisons.Rows.Count == 0)
            {
                // Vider tous les champs
                txtMaisonId.Text = txtRue.Text = txtVille.Text = txtCPostal.Text = txtPrix.Text = "";
                cboProvince.SelectedIndex = cboType.SelectedIndex = -1;
                lblInfosMaisons.Text = "Aucune maison a afficher";
                return;
            }

            txtMaisonId.Text = tabMaisons.Rows[indexMaison]["MaisonId"].ToString();
            txtRue.Text = tabMaisons.Rows[indexMaison]["Rue"].ToString();
            txtVille.Text = tabMaisons.Rows[indexMaison]["Ville"].ToString();
            cboProvince.Text = tabMaisons.Rows[indexMaison]["Province"].ToString();
            txtCPostal.Text = tabMaisons.Rows[indexMaison]["CodePostal"].ToString();
            txtPrix.Text = tabMaisons.Rows[indexMaison]["Prix"].ToString();
            cboType.Text = tabMaisons.Rows[indexMaison]["Type"].ToString();
            //Afficher le nom de l'agent responsable
            string idAg = tabMaisons.Rows[indexMaison]["EmpId"].ToString();
            cboAgent.Text = TrouverNomAgent(idAg);
            //Afficher le nom du client bproprietaire de la maison
            string idCli = tabMaisons.Rows[indexMaison]["IdClient"].ToString();
            cboClient.Text = TrouverNomClient(idCli);

            lblInfosMaisons.Text = "Maison " + (indexMaison + 1) + " sur un total de " + tabMaisons.Rows.Count;

        }
        private void RemplirListeAgents()
        {
            cboAgent.Items.Clear();

            //Recharger tabEmployes depuis le DataSet global
            tabEmployes = clsGlobal.setImmoRemaxDB.Tables["Employes"];

            if (frmPrincipal.UserConnecteRole == "Agent")
            {
                string id = frmPrincipal.UserConnecteId;

                DataRow[] agent = tabEmployes.Select("EmployeeId = '" + id + "'");
                if (agent.Length > 0)
                {
                    string tmp = agent[0]["EmployeeId"] + " - " + agent[0]["Nom"];
                    cboAgent.Items.Add(tmp);
                }

                cboAgent.SelectedIndex = 0;
                cboAgent.Enabled = false; // ne peut pas changer d'agent
                return;
            }

            // Admin voit tous les agents
            foreach (DataRow emp in tabEmployes.Rows)
            {
                if (emp["Role"].ToString() == "Agent")
                {
                    string tmp = emp["EmployeeId"] + " - " + emp["Nom"];
                    cboAgent.Items.Add(tmp);
                }
            }
        }
        private void RemplirListeClients()
        {
            cboClient.Items.Clear();

            //Recharger depuis le Dataset global
            tabClients = clsGlobal.setImmoRemaxDB.Tables["Clients"];

            if (frmPrincipal.UserConnecteRole == "Agent")
            {
                string id = frmPrincipal.UserConnecteId;

                DataRow[] clients = tabClients.Select("EmpId = '" + id + "'");

                foreach (DataRow client in clients)
                {
                    string tmp = client["ClientId"] + " - " + client["Nom"];
                    cboClient.Items.Add(tmp);
                }
                return;
            }

            // Admin voit tous les clients
            foreach (DataRow client in tabClients.Rows)
            {
                string tmp = client["ClientId"] + " - " + client["Nom"];
                cboClient.Items.Add(tmp);
            }
        }


        private void RemplirListeMaisons()
        {
            //Vider la liste avant de l'afficher
            lstNumMaisons.Items.Clear();
            //Parcourir le DataTable tabCompagnies et afficher les noms dans la liste
            foreach (DataRow myRow in tabMaisons.Rows)
            {
                lstNumMaisons.Items.Add(myRow["MaisonId"].ToString());
            }

        }

        private string TrouverNumAgent(string nomEtNum)
        {
            int posTiret = nomEtNum.IndexOf(" - ");
            if (posTiret > 0)
                return nomEtNum.Substring(0, posTiret).Trim();

            return ""; // si rien de valide n'est selectionne
        }

        private string TrouverNumClient(string nomEtNum)
        {
            int posTiret = nomEtNum.IndexOf(" - ");
            if (posTiret > 0)
                return nomEtNum.Substring(0, posTiret).Trim();

            return ""; // si rien de valide n'est selectionne
        }
        // Trouver le nom complet de l’agent a partir de son ID
        private string TrouverNomAgent(string idAg)
        {
            foreach (DataRow emp in tabEmployes.Rows)
            {
                if (emp["EmployeeId"].ToString() == idAg)
                    return emp["Nom"].ToString();
            }
            return "";
        }

        // Trouver le nom complet du client a partir de son ID
        private string TrouverNomClient(string idCli)
        {
            foreach (DataRow cli in tabClients.Rows)
            {
                if (cli["ClientId"].ToString() == idCli)
                    return cli["Nom"].ToString();
            }
            return "";
        }
        private void lstNumMaisons_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Mettre a jour l'index courant
            indexMaison = lstNumMaisons.SelectedIndex;
            AfficherTabDansTextBox();

        }

        public void RafraichirDonnees()
        {
            // Recharger depuis la BD
            clsSourceDeDonnees.ChargerBD();

            // Recharger les tables locales
            tabMaisons = clsGlobal.setImmoRemaxDB.Tables["Maisons"];
            tabClients = clsGlobal.setImmoRemaxDB.Tables["Clients"];
            tabEmployes = clsGlobal.setImmoRemaxDB.Tables["Employes"];

            // Rafraîchir les listes
            RemplirListeAgents();
            RemplirListeClients();
            RemplirListeMaisons();

            // Réafficher les données
            if (tabMaisons.Rows.Count > 0)
            {
                indexMaison = 0;
                AfficherTabDansTextBox();
            }
        }

    }
}
