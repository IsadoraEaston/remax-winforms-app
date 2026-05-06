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
    public partial class frmPrincipal : Form
    {
        string role = "";
        public static string UserConnecteId = "";
        public static string UserConnecteRole = "";

        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;

            // Centrer le group box manuellement
            grpLogIn.Left = (this.ClientSize.Width - grpLogIn.Width) / 2;
            grpLogIn.Top = (this.ClientSize.Height - grpLogIn.Height) / 2;

            //Remplir le combo box avec les roles
            cboRole.Items.Add("Admin");
            cboRole.Items.Add("Agent");
            cboRole.Items.Add("Utilisateur");

            //Vider le combo box au depart
            cboRole.SelectedIndex = -1;

            //Desactiver les menus au depart
            employesToolStripMenuItem.Enabled = false;
            clientsToolStripMenuItem.Enabled = false;
            maisonsToolStripMenuItem.Enabled = false;
            //Cacher les champs UserId et Password au depart
            txtUserId.Visible = txtPassword.Visible = false;
            lblUserId.Visible = lblPassword.Visible = false;

            //Ajouter une image de fond au formulaire MDI
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is MdiClient)
                {
                    ctrl.BackgroundImage = Properties.Resources.Remax_logo2;
                    ctrl.BackgroundImageLayout = ImageLayout.Stretch;
                }
            }
            clsSourceDeDonnees.ChargerBD();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {

            //Valider que l'ID utilisateur n'est pas vide
            if (cboRole.SelectedIndex == -1)
            {
                MessageBox.Show("Veuillez choisir un role.", "Erreur Login");
                return;
            }
            role = cboRole.SelectedItem.ToString();

            //Cas d'utilisateur pas de login requis
            if (role == "Utilisateur")
            {
                frmPrincipal.UserConnecteRole = "Utilisateur";
                frmPrincipal.UserConnecteId = ""; //Pas de ID

                employesToolStripMenuItem.Enabled = false;
                clientsToolStripMenuItem.Enabled = false;
                maisonsToolStripMenuItem.Enabled = false;
                grpLogIn.Visible = false;

                MessageBox.Show("Connexion réussie : Utilisateur", "Bienvenue", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                return;
            }
            //Cas d'admin ou d'agent, login requis
            string userId = txtUserId.Text.Trim();
            string pwd = txtPassword.Text.Trim();

            //Valider que l'ID utilisateur n'est pas vide
            if (userId == "")
            {
                MessageBox.Show("Veuillez entrer un ID utilisateur.", "Erreur Login");
                return;
            }

            //Chercher l'employe correspondant au UserId dans le DataSet
            DataTable tabEmployes = clsGlobal.setImmoRemaxDB.Tables["Employes"];
            DataRow[] trouve = tabEmployes.Select("EmployeeId = '" + userId + "'");
            if (trouve.Length == 0)
            {
                MessageBox.Show("ID utilisateur invalide.", "Erreur Login");
                return;
            }
            //Valider le mot de passe
            if (trouve[0]["Password"].ToString() != pwd)
            {
                MessageBox.Show("Mot de passe invalide.", "Erreur Login");
                return;
            }

            // Enregistrer l'etat de connexion afin que les formulaires enfants (MDI)
            // puissent appliquer les restrictions selon le role (Admin/Agent/Utilisateur)
            frmPrincipal.UserConnecteId = userId;
            frmPrincipal.UserConnecteRole = role;


            if (role == "Admin")
            {
                employesToolStripMenuItem.Enabled = true;
                clientsToolStripMenuItem.Enabled = true;
                maisonsToolStripMenuItem.Enabled = true;
            }
            else if(role == "Agent")
            {
                employesToolStripMenuItem.Enabled = false;
                clientsToolStripMenuItem.Enabled = true;
                maisonsToolStripMenuItem.Enabled = true;
            }
            //Masquer le group box de login
            grpLogIn.Visible = false;

            // Message de confirmation selon le rôle
            if (role == "Admin")
            {
                MessageBox.Show("Connexion reussie : Administrateur",
                                "Bienvenue", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (role == "Agent")
            {
                MessageBox.Show("Connexion reussie : Agent",
                                "Bienvenue", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void clientsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmClient fc = new frmClient();
            fc.MdiParent = this;
            fc.Show();
        }

        private void maisonsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMaison fm = new frmMaison();
            fm.MdiParent = this;
            fm.Show();

        }
        private void employesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmEmployes fe = new frmEmployes();
            fe.MdiParent = this;
            fe.Show();
        }

        private void fermerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string msg = "Etes-vous sur de vouloir quitter ce programme?";
            string tit = "Fermeture de l'application";
            if (MessageBox.Show(msg, tit, MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning) == DialogResult.OK)
            {
                Application.Exit();
            }

        }

        private void cboRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            role = cboRole.SelectedItem.ToString();

            if (role == "Admin" || role == "Agent")
            {
                txtUserId.Visible = txtPassword.Visible = true;
                lblUserId.Visible = lblPassword.Visible = true;
            }
            else 
            {
                txtUserId.Visible = txtPassword.Visible = false;
                lblUserId.Visible = lblPassword.Visible = false;
            }
        }

        private void maisonsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmRecherche fr = new frmRecherche();
            fr.MdiParent = this;
            fr.Show();
        }
    }
}
