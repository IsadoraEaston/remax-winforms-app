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
    public partial class frmRecherche : Form
    {
        DataTable tabEmployes;
        DataTable tabMaisons;
        string agentSelectionne = "";

        public frmRecherche()
        {
            InitializeComponent();
        }

        private void frmRecherche_Load(object sender, EventArgs e)
        {
            // Charger les tables
            tabEmployes = clsGlobal.setImmoRemaxDB.Tables["Employes"];
            tabMaisons = clsGlobal.setImmoRemaxDB.Tables["Maisons"];


            // Afficher tous les agents au depart
            AfficherAgents();

            // Vider le grid des maisons
            grdMaisons.DataSource = tabMaisons;

        }

        private void AfficherAgents()
        {
            //Filtrer seulement les agents (pas les admins)
            DataRow[] agents = tabEmployes.Select("Role = 'Agent'");


            if (agents.Length > 0)
            {
                grdAgents.DataSource = agents.CopyToDataTable();
            }
            else
            {
                grdAgents.DataSource = null;
                MessageBox.Show("Aucun agent trouve.", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        //Méthode pour afficher les maisons d'un agent
        private void AfficherMaisonsDeLAgent(string idAgent)
        {
            //Recuperer les maisons de l'agent sélectionné
            DataTable maisonsDeLAgent = clsSourceDeDonnees.GetMaisonsDeLAgent(idAgent);

            //Vérifier s'il y a des maisons
            if (maisonsDeLAgent.Rows.Count > 0)
            {
                grdMaisons.DataSource = maisonsDeLAgent;
            }
            else
            {
                grdMaisons.DataSource = null;
                MessageBox.Show("Cet agent n'a aucune maison.", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void grdAgents_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //Verifier qu'on a clique sur une ligne valide
            if (e.RowIndex < 0)
                return;

            //Recuperer l'ID de l'agent sélectionne
            agentSelectionne = grdAgents.Rows[e.RowIndex].Cells["EmployeeId"].Value.ToString();

            //Afficher les maisons de cet agent
            AfficherMaisonsDeLAgent(agentSelectionne);
        }

        private void btnFiltrerMaisons_Click(object sender, EventArgs e)
        {
            string filtre = "";

            //Filtre prix
            if (!string.IsNullOrEmpty(txtPrix.Text.Trim()))
            {
                decimal prix;
                if (decimal.TryParse(txtPrix.Text, out prix))
                {
                    filtre = "Prix <= " + prix;
                }
                else
                {
                    MessageBox.Show("Prix invalide. Entrez un nombre.", "Erreur",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            //Filtre ville
            if (!string.IsNullOrEmpty(txtVille.Text.Trim()))
            {
                if (!string.IsNullOrEmpty(filtre))
                    filtre += " AND ";
                filtre += "Ville LIKE '%" + txtVille.Text.Trim() + "%'";
            }

            //Appliquer le filtre
            if (!string.IsNullOrEmpty(filtre))
            {
                DataRow[] maisons = tabMaisons.Select(filtre);

                if (maisons.Length > 0)
                {
                    grdMaisons.DataSource = maisons.CopyToDataTable();
                }
                else
                {
                    grdMaisons.DataSource = null;
                    MessageBox.Show("Aucune maison trouvee avec ces criteres.", "Information",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                //Aucun filtre, afficher tout
                grdMaisons.DataSource = tabMaisons;
            }
        }

        private void btnAllMaisons_Click(object sender, EventArgs e)
        {
            //Vider les filtres
            txtPrix.Text = "";
            txtVille.Text = "";

            //Afficher toutes les maisons
            grdMaisons.DataSource = tabMaisons;
        }

        private void btnFermer_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
