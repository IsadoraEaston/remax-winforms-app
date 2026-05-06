using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prjWinCsImmoRemax
{
    public partial class frmEmployes : Form
    {
        //Declaration des variables globales a la forme
        DataTable tabEmployes;
        DataTable tabClients;
        int indexEmploye;
        string mode = ""; //Ajout ou Modif

        public frmEmployes()
        {
            InitializeComponent();
        }

        private void frmEmployes_Load(object sender, EventArgs e)
        {
            //Variable tabEmployes qui va pointer sur la table Employes du DataSet
            tabEmployes = clsGlobal.setImmoRemaxDB.Tables["Employes"];
            //Variable tabClients qui va pointer sur la table Clients du DataSet
            tabClients = clsGlobal.setImmoRemaxDB.Tables["Clients"];
            //Remplir le ComboBox Role
            cboRole.Items.Clear();
            cboRole.Items.Add("Admin");
            cboRole.Items.Add("Agent");

            //Afficher le premier employe
            indexEmploye = 0;
            MontrerBoutons(true, false);
            RemplirListeTypes();
            AfficherTabDansTextBox();
            AfficherClientsDeLEmploye();
        }
        private void btnAjouter_Click(object sender, EventArgs e)
        {
            //Passer en mode ajout
            mode = "Ajout";
            //Vider les TextBox
            txtEmpId.Text = txtTelephone.Text = txtNom.Text = txtPassword.Text = "";
            cboRole.SelectedIndex = -1;
            txtEmpId.Focus();
            lblInfosEmployes.Text = "Ajout d'un nouvel employé";
            //Cacher les boutons Ajouter, Modifier, Supprimer
            MontrerBoutons(false, true);

        }

        private void btnModifier_Click(object sender, EventArgs e)
        {
            //Passer en mode modification
            mode = "Modif";
            txtEmpId.Enabled = false; //L'ID ne peut pas etre modifie
            txtNom.Focus();
            lblInfosEmployes.Text = "Modification de l'employé " + (indexEmploye + 1);
            //Cacher les boutons Ajouter, Modifier, Supprimer
            MontrerBoutons(false, true);

        }

        private void btnSupprimer_Click(object sender, EventArgs e)
        {
            string tit = " Attention, Suppression d'un agent";
            string msg = "Etes-vous sur de vouloir supprimer cet employe ? ";
            if (MessageBox.Show(msg, tit, MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                               == DialogResult.Yes)
            {
                string empId = tabEmployes.Rows[indexEmploye]["EmployeeId"].ToString();
                //Verifier si l'employe gere des clients
                DataTable tabClientsDeLEmploye = clsSourceDeDonnees.GetClientsDeLAgent(empId);
                if (tabClientsDeLEmploye.Rows.Count > 0)//L'employe gere des clients
                {
                    MessageBox.Show("Impossible de supprimer cet agent car il gere des clients.",
                        "Suppression impossible", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; //Quitter la procedure
                }

                //Supprimer le client courant
                tabEmployes.Rows[indexEmploye].Delete();
                //Mise a jour de la BD
                clsSourceDeDonnees.SaveEmployes();
                // Si plus d’employes
                if (tabEmployes.Rows.Count == 0)
                {
                    MessageBox.Show("Aucun employé restant dans la base.");
                    txtEmpId.Text = txtNom.Text = txtTelephone.Text = txtPassword.Text = "";
                    cboRole.SelectedIndex = -1;
                    return;
                }

                //Repositionner l'indexClient                
                indexEmploye = 0;
                AfficherTabDansTextBox();
            }
        }

        private void btnSauvegarder_Click(object sender, EventArgs e)
        {
            //Recuperer les donnees saisies
            string numSaisi = txtEmpId.Text.Trim();
            string nomSaisi = txtNom.Text.Trim();
            string rolSaisi = cboRole.SelectedItem.ToString();
            string pwdSaisi = txtPassword.Text.Trim();
            string telSaisi = txtTelephone.Text.Trim();

            if (!ValiderTelephone(txtTelephone.Text)) 
            { 
                MessageBox.Show("Numero de telephone invalide. Entrez 10 chiffres.",
                       "Erreur de validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTelephone.Focus();
                return;
            }

            //si ajout
            if (mode == "Ajout")
            {
                //Verifier dans ajout si employe id existe deja (Cle Primaire)
                DataRow[] result = tabEmployes.Select("EmployeeId = '" + numSaisi + "'");
                if (result.Length > 0)
                {
                    MessageBox.Show("Cet identifiant existe deja. Veuillez en choisir un autre.",
                        "Doublon detecte",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    txtEmpId.Focus();
                    return;
                }
            }
            if (mode == "Ajout")
            {
                //Creer une nouvelle ligne dans la table Employes
                DataRow newRow = tabEmployes.NewRow();
                newRow["EmployeeId"] = numSaisi;
                newRow["Nom"] = nomSaisi;
                newRow["Role"] = rolSaisi;
                newRow["Password"] = pwdSaisi;
                newRow["Telephone"] = telSaisi;
                //Ajouter la nouvelle ligne a la table
                tabEmployes.Rows.Add(newRow);
                //Repositionner sur le nouvel employe
                indexEmploye = tabEmployes.Rows.Count - 1; //Derniere ligne


            }
            else if (mode == "Modif") //si modification
            {
                //pointer sur le client courant (DataRow)
                DataRow myRow = tabEmployes.Rows[indexEmploye];
                //Ecraser le DataRow avec les valeurs saisies
                //myRow["Numero"] = numSaisi; //le numero est Read only
                myRow["Nom"] = nomSaisi;
                myRow["Role"] = rolSaisi;
                myRow["Password"] = pwdSaisi;
                myRow["Telephone"] = telSaisi;
            }

            //Mise a jour de la BD
            clsSourceDeDonnees.SaveEmployes();
            AfficherTabDansTextBox();
            //Reafficher les boutons Ajouter, Modifier, Supprimer
            MontrerBoutons(true, false);
            txtEmpId.Enabled = true;
        }

        private void btnAnnuler_Click(object sender, EventArgs e)
        {
            MontrerBoutons(true, false);
            txtEmpId.Enabled = true;
            AfficherTabDansTextBox();
        }


        private void btnPremier_Click(object sender, EventArgs e)
        {
            indexEmploye = 0;
            AfficherTabDansTextBox();
            AfficherClientsDeLEmploye();
        }

        private void btnPrecedent_Click(object sender, EventArgs e)
        {
            if (indexEmploye > 0)
            {
                indexEmploye--;
                AfficherTabDansTextBox();
                AfficherClientsDeLEmploye();
            }
            
        }

        private void btnSuivant_Click(object sender, EventArgs e)
        {
            if (indexEmploye < tabEmployes.Rows.Count - 1)
            {
                indexEmploye++;
                AfficherTabDansTextBox();
                AfficherClientsDeLEmploye();
            }
        }

        private void btnDernier_Click(object sender, EventArgs e)
        {
            indexEmploye = tabEmployes.Rows.Count - 1;
            AfficherTabDansTextBox();
            AfficherClientsDeLEmploye();
        }
        private void RemplirListeTypes()
        {
            cboRole.Items.Clear();
            cboRole.Items.Add("Admin");
            cboRole.Items.Add("Agent");
            cboRole.Items.Add("Comptable");
            cboRole.Items.Add("Secretaire");
        }


        private void MontrerBoutons(bool btnsAjModSup, bool btnsSauvAn)
        {
            btnAjouter.Visible = btnModifier.Visible = btnSupprimer.Visible = btnsAjModSup;
            btnSauvegarder.Visible = btnAnnuler.Visible = btnsSauvAn;
        }

        private void AfficherTabDansTextBox()
        {
            txtEmpId.Text = tabEmployes.Rows[indexEmploye]["EmployeeId"].ToString();
            txtNom.Text = tabEmployes.Rows[indexEmploye]["Nom"].ToString();
            cboRole.Text = tabEmployes.Rows[indexEmploye]["Role"].ToString();
            txtPassword.Text = tabEmployes.Rows[indexEmploye]["Password"].ToString();
            txtTelephone.Text = tabEmployes.Rows[indexEmploye]["Telephone"].ToString();

            lblInfosEmployes.Text = "Employé " + (indexEmploye + 1) + " sur un total de " + tabEmployes.Rows.Count;
        }

        private void AfficherClientsDeLEmploye()
        {
            //Recuperer l'Id de l'employe courant
            string IdEmployeCherche = tabEmployes.Rows[indexEmploye]["EmployeeId"].ToString();
            //Pointer sur la table Clients
            DataTable tabClients = clsGlobal.setImmoRemaxDB.Tables["Clients"];
            //Afficher les clients des employes dans une DataGridView
            DataRow[] tabResultats = tabClients.Select("EmpId = '" + IdEmployeCherche+"'");
            //Lier le tableau de resultats au DataGridView
            if (tabResultats.Length > 0)
            {
                gridClients.DataSource = tabResultats.CopyToDataTable();
            }
            else //Pas de clients pour l'employe courant
            {
                gridClients.DataSource = null;
            }
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
