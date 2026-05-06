using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prjWinCsImmoRemax
{
    public partial class frmClient : Form
    {
        DataTable tabEmployes;
        DataTable tabClients;
        DataTable tabMaisons;
        int indexClient;
        string mode = ""; //Ajout ou Modif
        public frmClient()
        {

            InitializeComponent();
        }

        private void frmClient_Load(object sender, EventArgs e)
        {
            //Variable tabClients qui va pointer sur les tables Clients, Employes et Maisons du DataSet
            tabClients = clsGlobal.setImmoRemaxDB.Tables["Clients"];
            tabEmployes = clsGlobal.setImmoRemaxDB.Tables["Employes"];
            tabMaisons = clsGlobal.setImmoRemaxDB.Tables["Maisons"];


            //Filtrade des clients si un agent est connecte
            if (frmPrincipal.UserConnecteRole == "Agent")
            {
                string idAgent = frmPrincipal.UserConnecteId;

                DataRow[] result = tabClients.Select("EmpId = '" + idAgent + "'");


                if (result.Length > 0)
                {
                   tabClients = result.CopyToDataTable();   // Ne garder que ceux de l’agent
                }
                else
                {
                    //Aucun client pour cet agent
                    tabClients = tabClients.Clone();
                }
           
            }


            //Remplir le ComboBox Role
            cboType.Items.Clear();
            cboType.Items.Add("Acheteur");
            cboType.Items.Add("Vendeur");

            //Afficher le premier client
            if(tabClients.Rows.Count > 0)
            {
                indexClient = 0;
                AfficherTabDansTextBox();
                AfficherMaisonsDuClient();

            }
            else
            {
                indexClient = -1;
                MessageBox.Show("Vous n'avez aucun client.", "Information");
            }

            RemplirListeEmploye();
            MontrerBoutons(true, false);
            
        }

        private void btnAjouter_Click(object sender, EventArgs e)
        {
            mode = "Ajout";
            //Vider les text box
            txtClientId.Text = txtNom.Text = txtCourriel.Text = txtTelephone.Text = "";
            cboType.SelectedIndex = -1;

            //S'assurer que l'agent est dselectionne
            if(frmPrincipal.UserConnecteRole == "Agent")
            {
                string id = frmPrincipal.UserConnecteId.ToString();
                string nom = TrouverNomAgent(id);
                cboAgent.Items.Clear();
                cboAgent.Items.Add(id + " - " + nom);   
                cboAgent.SelectedIndex = 0;             
                cboAgent.Enabled = false;
            }
            else
            {
                RemplirListeEmploye();
                cboAgent.Enabled = true;
            }
            txtClientId.Focus();
            lblInformations.Text = "Ajout d'un nouveau client";

            MontrerBoutons(false, true);
        }

        private void btnModifier_Click(object sender, EventArgs e)
        {
            mode = "Modif";
            txtClientId.Enabled = false; //L'ID ne peut pas etre modifie
            txtNom.Focus();
            lblInformations.Text = "Modification du client " + (indexClient + 1);

            MontrerBoutons(false, true);
        }

        private void btnSupprimer_Click(object sender, EventArgs e)
        {
            //Demander confirmation avant de supprimer
            string titre = " Attention, Suppression d'un client";
            string msg = "Etes-vous sur de vouloir supprimer ce client ? ";
            if (MessageBox.Show(msg, titre, MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                               == DialogResult.Yes)
            {
                string clientId = tabClients.Rows[indexClient]["ClientId"].ToString();
                //Verifier si le client a des maisons
                DataTable tabMaisonsDuClient = clsSourceDeDonnees.GetMaisonsDuClient(clientId);
                if (tabMaisonsDuClient.Rows.Count > 0)//Le client a des maisons
                {
                    MessageBox.Show("Impossible de supprimer ce client car il possede des maisons.",
                        "Suppression impossible", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; //Quitter la procedure
                }
                //Supprimer le client du DataTable
                tabClients.Rows[indexClient].Delete();

                clsSourceDeDonnees.SaveClients();

                clsSourceDeDonnees.ChargerBD();
                tabClients = clsGlobal.setImmoRemaxDB.Tables["Clients"];

                if (tabClients.Rows.Count > 0)
                {
                    if (indexClient >= tabClients.Rows.Count)
                        indexClient = tabClients.Rows.Count - 1;

                    AfficherTabDansTextBox();
                    AfficherMaisonsDuClient();
                }
                else
                {
                    // Plus aucun client
                    MessageBox.Show("Aucun client  aafficher.", "Information");
                    indexClient = -1;
                }

                MessageBox.Show("Client supprime avec succes!", "Suppression");

            }

        }

        private void btnSauvegarder_Click(object sender, EventArgs e)
        {
            //Recuperer les donnees saisies
            string numSaisi = txtClientId.Text.Trim();
            string nomSaisi = txtNom.Text.Trim();
            string typSaisi = cboType.Text;
            string courSaisi = txtCourriel.Text.Trim();
            string telSaisi = txtTelephone.Text.Trim();
            string agSaisi = TrouverNumeroAgent(cboAgent.Text);



            // VALIDATION: Verifier qu'un agent est selectionne
            if (string.IsNullOrEmpty(agSaisi))
            {
                MessageBox.Show("Vous devez sélectionner un agent pour ce client!",
                    "Agent requis", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboAgent.Focus();
                return;
            }


            //Validation du courriel et du telephone
            if (!ValiderCourriel(txtCourriel.Text))
            {
                MessageBox.Show("Courriel invalide. Exemple : nom@gmail.com",
                    "Erreur de validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCourriel.Focus();
                return;
            }

            if (!ValiderTelephone(txtTelephone.Text)) 
            {
                MessageBox.Show("Numero de telephone invalide. Entrez 10 chiffres.",
                       "Erreur de validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTelephone.Focus();
                return;
            }
            if (mode == "Ajout")
            {
                //Verifier dans ajout si employe id existe deja (Cle Primaire)

                DataRow[] result = tabClients.Select("ClientId = '" + numSaisi + "'");
                if (result.Length > 0)
                {
                    MessageBox.Show("Cet identifiant existe deja. Veuillez en choisir un autre.",
                            "Doublon detecte",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    txtClientId.Focus();
                    return;
                }

                //Ajouter le nouveau client au DataTable
                DataRow nouvClient = tabClients.NewRow();
                nouvClient["ClientId"] = numSaisi;
                nouvClient["Nom"] = nomSaisi;
                nouvClient["Type"] = typSaisi;
                nouvClient["Courriel"] = courSaisi;
                nouvClient["Telephone"] = telSaisi;
                nouvClient["EmpId"] = agSaisi;
                tabClients.Rows.Add(nouvClient);
                indexClient = tabClients.Rows.Count - 1; //Positionner l'index sur le nouveau client

            }
            else if (mode == "Modif")
            {
                //Pointer sur le client courant via l'indexClient
                DataRow myRow = tabClients.Rows[indexClient];
                //Ecraser les valeurs du DataRow avec les valeurs saisies
                //myRow["ClientId"] = numSaisi; //le numero est Read Only
                myRow["Nom"] = nomSaisi;
                myRow["Type"] = typSaisi;
                myRow["Courriel"] = courSaisi;
                myRow["Telephone"] = telSaisi;
                myRow["EmpId"] = agSaisi;

            }

            //Sauvegarder le contenu du DataSet dans la BD (synchronisation)
            clsSourceDeDonnees.SaveClients();

            //Recharger BD
            clsSourceDeDonnees.ChargerBD();
            // Retrouver le client ajouté
            DataRow[] res = tabClients.Select("ClientId = '" + numSaisi + "'");
            if (res.Length > 0)
            {
                indexClient = tabClients.Rows.IndexOf(res[0]);
            }

            tabClients = clsGlobal.setImmoRemaxDB.Tables["Clients"];

            if (frmPrincipal.UserConnecteRole == "Agent")
            {
                string idAgent = frmPrincipal.UserConnecteId;
                DataRow[] result = tabClients.Select("EmpId = '" + idAgent + "'");

                if (result.Length > 0)
                {
                    tabClients = result.CopyToDataTable();
                }
                else
                {
                    tabClients = tabClients.Clone();
                }
            }
            tabEmployes = clsGlobal.setImmoRemaxDB.Tables["Employes"];

            //Rafraichir combo des agents
            RemplirListeEmploye();

            AfficherTabDansTextBox();
            MontrerBoutons(true, false);
            txtClientId.Enabled = true;
        }

        private void btnAnnuler_Click(object sender, EventArgs e)
        {
            AfficherTabDansTextBox();
            MontrerBoutons(true, false);
            txtClientId.Enabled = true;
        }

        private void btnPremier_Click(object sender, EventArgs e)
        {
            indexClient = 0;
            AfficherTabDansTextBox();
            AfficherMaisonsDuClient();
        }

        private void btnPrecedent_Click(object sender, EventArgs e)
        {
            if (indexClient > 0)
            {
                indexClient--;
                AfficherTabDansTextBox();
                AfficherMaisonsDuClient();
            }

        }

        private void btnSuivant_Click(object sender, EventArgs e)
        {
            if (indexClient < tabClients.Rows.Count - 1)
            {
                indexClient++;
                AfficherTabDansTextBox();
                AfficherMaisonsDuClient();
            }
        }

        private void btnDernier_Click(object sender, EventArgs e)
        {
            indexClient = tabClients.Rows.Count - 1;
            AfficherTabDansTextBox();
            AfficherMaisonsDuClient();
        }

        private void MontrerBoutons(bool btnsAjModSup, bool btnsSauvAn)
        {
            btnAjouter.Visible = btnModifier.Visible = btnSupprimer.Visible = btnsAjModSup;
            btnSauvegarder.Visible = btnAnnuler.Visible = btnsSauvAn;

        }


        private void AfficherTabDansTextBox()
        {
            if (tabClients.Rows.Count == 0)
            {
                txtClientId.Text = txtNom.Text = txtCourriel.Text = txtTelephone.Text = "";
                cboAgent.Items.Clear();
                return;
            }

            if (indexClient < 0) indexClient = 0;
            if (indexClient >= tabClients.Rows.Count)
                indexClient = tabClients.Rows.Count - 1;

            txtClientId.Text = tabClients.Rows[indexClient]["ClientId"].ToString();
            txtNom.Text = tabClients.Rows[indexClient]["Nom"].ToString();
            cboType.Text = tabClients.Rows[indexClient]["Type"].ToString();
            txtCourriel.Text = tabClients.Rows[indexClient]["Courriel"].ToString();
            txtTelephone.Text = tabClients.Rows[indexClient]["Telephone"].ToString();
            //Afficher le nom de l'agent responsable
            string empId = tabClients.Rows[indexClient]["EmpId"].ToString();

            //Si agent connecte, toujours afficher l'agent connecte
            if (frmPrincipal.UserConnecteRole != "Agent")
            {
                //Pour un agent, le combo contient seulement lui meme
                if (cboAgent.Items.Count > 0)
                    cboAgent.SelectedIndex = 0;   // un seul agent dans la liste
            }
            else
            {
                //Pour un admin, chercher le bon agent dans la liste
                cboAgent.Text = TrouverNomAgent(empId);
            }
                lblInformations.Text = "Client " + (indexClient + 1) + "  sur un total de " + tabClients.Rows.Count;
        }
        //Remplir le comboAgent seulement avec des agents
        private void RemplirListeEmploye()
        {
            cboAgent.Items.Clear();

            if (frmPrincipal.UserConnecteRole == "Agent")
            {
                string idAgent = frmPrincipal.UserConnecteId;

                DataRow[] agent = tabEmployes.Select("EmployeeId = '" + idAgent + "'");


                if (agent.Length > 0)
                {
                    string tmp = agent[0]["EmployeeId"] + " - " + agent[0]["Nom"];
                    cboAgent.Items.Add(tmp);
                    cboAgent.SelectedIndex = 0;
                    cboAgent.Enabled = false; // un agent ne peut pas assigner un client a quelqu'un d'autre

                }

                
                return;
            }

            //ADMIN = Liste complete des agents
            foreach (DataRow emp in tabEmployes.Rows)
            {
                if (emp["Role"].ToString() == "Agent")   //  filtrer seulement les agents
                {
                    string tmp = emp["EmployeeId"].ToString() + " - " + emp["Nom"].ToString();
                    cboAgent.Items.Add(tmp);
                }
            }
        }

        private string TrouverNomAgent(string numero)
        {
            foreach (var itm in cboAgent.Items)
            {
                if (itm.ToString().Contains(numero) == true)
                {
                    return itm.ToString();
                }
            }
            return "";
        }

        private string TrouverNumeroAgent(string nomEtNum)
        {
            int posTiret = nomEtNum.IndexOf(" - ");
            if (posTiret > 0)
                return nomEtNum.Substring(0, posTiret);

            return ""; // si rien de valide n'est selectionne
        }

        private void AfficherMaisonsDuClient()
        {
            //Recuperer l'Id du client courant
            string IdClientCherche = tabClients.Rows[indexClient]["ClientId"].ToString();
            //Pointer sur la table Maisons
            DataTable tabMaisons = clsGlobal.setImmoRemaxDB.Tables["Maisons"];
            //Afficher les clients des employes dans une DataGridView
            DataRow[] tabResultats = tabMaisons.Select("IdClient = '" + IdClientCherche + "'");
            //Lier le tableau de resultats au DataGridView
            if (tabResultats.Length > 0)
            {
                grdMaison.DataSource = tabResultats.CopyToDataTable();
            }
            else //Pas de clients pour l'employe courant
            {
                grdMaison.DataSource = null;
            }
        }


        //Valider le courriel
        private bool ValiderCourriel(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) 
                return false;

            email = email.Trim();

            return email.Contains("@") &&
                   email.Contains(".") &&
                   email.IndexOf("@") > 0 &&
                   email.LastIndexOf(".") > email.IndexOf("@") + 1 &&
                   email.Length >= 5;
        }

        //Valider le numero de telephone
        private bool ValiderTelephone(string tel)
        {

            if (string.IsNullOrWhiteSpace(tel)) return false;

            // Garder seulement les chiffres
            string digits = new string(tel.Where(char.IsDigit).ToArray());

            // Verifier qu'il y a exactement 10 chiffres
            return digits.Length == 10;
        }


    }
}
