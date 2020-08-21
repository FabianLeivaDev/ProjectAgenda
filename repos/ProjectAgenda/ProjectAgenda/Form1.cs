using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Diagnostics;

namespace ProjectAgenda
{
    public partial class Form1 : Form
    {
        Agendum contacto = new Agendum();
        public Form1()
        {
            InitializeComponent();
        }

              private void BtnCancelar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        void Limpiar()
        {
            txtNombre.Text = txtTelefono.Text = txtCorreo.Text = "";
            btnGuardar.Text = "Guardar";
            btnEliminar.Enabled = false;
            contacto.ContactoID = 0;
            validacionNombre.Text = "";
            validacionTelefono.Text = "";
            validacionEmail.Text = "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Limpiar();
            PoblarDataGridView();
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            Regex regex = new Regex(@"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$");
            bool isValid = regex.IsMatch(txtCorreo.Text);
            if (txtNombre.Text == "")
            {
                MessageBox.Show("Ingrese un nombre");
            }
            else if (txtTelefono.Text == "")
            {
                MessageBox.Show("Ingrese un telefono");
            }
            else if (!isValid)
            {
                MessageBox.Show("Ingrese un Email valido");
            }
            else
            {
                contacto.Nombre = txtNombre.Text.Trim();
                contacto.Telefono = txtTelefono.Text.Trim();
                contacto.Correo = txtCorreo.Text.Trim();
                using (AgendaDBEntities db = new AgendaDBEntities())
                {
                    if (contacto.ContactoID == 0) //Funcio de guardado
                        db.Agenda.Add(contacto);
                    else // Funcion de actualizacion 
                        db.Entry(contacto).State = EntityState.Modified;
                    db.SaveChanges();
                }
                Limpiar();
                PoblarDataGridView();
                MessageBox.Show("Contacto Guardado!");
            }
        }

        void PoblarDataGridView()
        {
            using(AgendaDBEntities db = new AgendaDBEntities())
            {
                dgvAgenda.DataSource = db.Agenda.ToList<Agendum>();
            }
        }

        private void DgvAgenda_DoubleClick(object sender, EventArgs e)
        {
            if(dgvAgenda.CurrentRow.Index != -1)
            {
                contacto.ContactoID = Convert.ToInt32(dgvAgenda.CurrentRow.Cells["ContactoID"].Value);
                using (AgendaDBEntities db = new AgendaDBEntities())
                {
                    contacto = db.Agenda.Where(x => x.ContactoID == contacto.ContactoID).FirstOrDefault();
                    txtNombre.Text = contacto.Nombre;
                    txtTelefono.Text = contacto.Telefono;
                    txtCorreo.Text = contacto.Correo;
                }
                btnGuardar.Text = "Actualizar";
                btnEliminar.Enabled = true;
            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("¿Desea eliminar el contacto?", "Aviso", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (AgendaDBEntities db = new AgendaDBEntities())
                {
                    var entry = db.Entry(contacto);
                    if (entry.State == EntityState.Detached)
                        db.Agenda.Attach(contacto);
                    db.Agenda.Remove(contacto);
                    db.SaveChanges();
                    PoblarDataGridView();
                    Limpiar();
                    MessageBox.Show("Contacto eliminado!");
                }
            }
        }

        private void TxtCorreo_Leave(object sender, EventArgs e)
        {
            Regex regex = new Regex(@"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$");
            bool isValid = regex.IsMatch(txtCorreo.Text);
            if (!isValid)
            {
                validacionEmail.Text = "Ingrese un Email valido";
            }
        }

        private void TxtNombre_Leave(object sender, EventArgs e)
        {
            if (txtNombre.Text == "")
            {
                validacionNombre.Text = "Ingrese un nombre";
            }
        }

        private void TxtTelefono_Leave(object sender, EventArgs e)
        {
            if (txtTelefono.Text == "")
            {
                validacionTelefono.Text = "Ingrese un numero de telefono";
            }
        }
    }
}
