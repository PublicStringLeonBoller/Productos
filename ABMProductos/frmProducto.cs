using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ABMProductos
{
    public partial class frmProducto : Form
    {
        
        //const int tam = 100;
        //Producto[] aP = new Producto[tam];
        //bool nuevo;
        int c;
        Daatos oBD = new Daatos(@"Data Source=138.99.7.66,1433;Initial Catalog=TUP_PII_2020;User ID=tup_2020;password=tup2020_!@NH");

        List<Producto> lP = new List<Producto>();     // aca definimos q no usamos un arreglo o una      constante es MI LISTA PRODUCTO en lugar de mi arreglo producto.

        enum Accion
        {
            nuevo,
            editado
        }

        Accion miAccion = Accion.editado; // definimos una instancia de accion 
        public frmProducto()
        {
            InitializeComponent();
            
        }
        private void frmProducto_Load(object sender, EventArgs e)
        {
            //CargarCombo(cboMarca, "marca", "id_tipo_marca", "n_tipo_marca");
            //CargarLista("producto");

            Habiliteishon(false);

            CargarCombo(cboMarca, "marca");
            CargarLista("producto");
        }
        //--
        private void CargarLista(string nomTabla)
        {
            
            lP.Clear();
            c = 0;
            oBD.LeerTabla(nomTabla);
            while (oBD.Dr.Read())           // si hace read, hay datos y avanza
            {
                Producto p = new Producto();
                if (!oBD.Dr.IsDBNull(0))    // valida q no traiga nulos
                    p.Codigo = oBD.Dr.GetInt32(0);

                if (!oBD.Dr.IsDBNull(1))
                    p.Detalle = oBD.Dr.GetString(1);

                if (!oBD.Dr.IsDBNull(2))
                    p.Tipo = oBD.Dr.GetInt32(2);

                if (!oBD.Dr.IsDBNull(3))
                    p.Marca = oBD.Dr.GetInt32(3);

                if (!oBD.Dr.IsDBNull(4))
                    p.Precio = oBD.Dr.GetDouble(4);

                if (!oBD.Dr.IsDBNull(5))
                    p.Fecha = oBD.Dr.GetDateTime(5);

                //aP[c] = p;
                //c++;
                lP.Add(p);  //mi lista prod le agrego un producto p (no tiene limite como el arreglo tam)


            }

            oBD.Dr.Close();
            oBD.Desconectar();
            lstProducto.Items.Clear();

            //for (int i = 0; i < c; i++)  // hasta c no todos para q sea solo los cargados
            //{
            //    lstProducto.Items.Add(aP[i].ShowProducto());
            //}

            for (int i = 0; i < lP.Count; i++)  //lp.count --> q vaya hasta el count, hasta donde haya algo
            {
                lstProducto.Items.Add(lP[i].ShowProducto());
            }

            lstProducto.SelectedIndex = 0;
        }
        //--
        public void Habiliteishon(bool j)
        {
            txtCodigo.Enabled = j;
            txtDetalle.Enabled = j;
            cboMarca.Enabled = j;
            rbtNetBook.Enabled = j;
            rbtNoteBook.Enabled = j;
            txtPrecio.Enabled = j;
            dtpFecha.Enabled = j;
            lstProducto.Enabled = j;
            btnGrabar.Enabled = j;
            btnCancelar.Enabled = j;
            btnNuevo.Enabled = !j;
            btnEditar.Enabled = !j;
            btnBorrar.Enabled = !j;
            btnSalir.Enabled = j;
        }
        //--
        private void Limpieishon()
        {
            txtCodigo.Clear();
            txtDetalle.Clear();
            cboMarca.SelectedIndex = -1;
            rbtNetBook.Checked = false;
            rbtNoteBook.Checked = false;
            txtPrecio.Clear();
            dtpFecha.Checked = false;
        }
        //--
        //private void CargarCombo(ComboBox combo, string nomTabla, string pk, string nomCampo)
        //{
        //    DataTable tabla = new DataTable();

        //    tabla = Datito.ConsultarTabla(nomTabla);

        //    combo.DataSource = tabla;
        //    combo.DisplayMember = nomCampo;
        //    combo.ValueMember = pk;

        //}
        private void CargarCombo(ComboBox combo, string nomTabla)
        {

            DataTable dt = new DataTable();
            dt = oBD.ConsultarTabla(nomTabla);
            combo.DataSource = dt;
            combo.ValueMember = dt.Columns[0].ColumnName;
            combo.DisplayMember = dt.Columns[1].ColumnName;

            combo.DropDownStyle = ComboBoxStyle.DropDownList; // para q no se pueda escribir

        }
        //--
        private bool ValidarDatos()
        {
            if (txtCodigo.Text == "")
            {
                MessageBox.Show("Debe ingresar Codigo...");
                txtCodigo.Focus();
                return false;
            }
            if (txtDetalle.Text == "")
            {
                MessageBox.Show("Debe ingresar Detalle...");
                txtDetalle.Focus();
                return false;
            }
            if (cboMarca.SelectedIndex == -1)
            {
                MessageBox.Show("Debe ingresar Marca...");
                cboMarca.Focus();
                return false;
            }
            if (!rbtNetBook.Checked && !rbtNoteBook.Checked)
            {
                MessageBox.Show("Debe Seleccionar Tipo...");
                return false;
            }
            if (txtPrecio.Text == "")
            {
                MessageBox.Show("Debe ingresar Precio...");
                txtPrecio.Focus();
                return false;
            }
            if (!dtpFecha.Checked)
            {
                MessageBox.Show("Debe Seleccionar Fecha...");
                dtpFecha.Focus();
                return false;
            }
            return true;
        }
        //--
        private void btnNuevo_Click(object sender, EventArgs e)
        {
            Habiliteishon(true);
            Limpieishon();
            txtCodigo.Focus();
            miAccion = Accion.nuevo;
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            Habiliteishon(true);
            txtCodigo.Enabled = false;
            txtDetalle.Focus();
            btnBorrar.Enabled = true;
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Esta segurisisimisimo de querer borrar este producto?", "BORRANDO...",
                                                        MessageBoxButtons.YesNo,
                                                        MessageBoxIcon.Warning,
                                                        MessageBoxDefaultButton.Button2)
                                                        == DialogResult.Yes)
            {
                string consultaSQL = $"delete from producto" +
                                     $" where codigo = {lP[lstProducto.SelectedIndex].Codigo}";

                //Datito.Actualizar(consultaSQL);
                CargarLista("producto");
                Limpieishon();
            }
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            string consultaSQL = "";
            if (ValidarDatos())
            {
                Producto p = new Producto();
                p.Codigo = int.Parse(txtCodigo.Text);
                p.Detalle = txtDetalle.Text;
                p.Marca = (int)cboMarca.SelectedValue;
                if (rbtNoteBook.Checked)
                {
                    p.Tipo = 1;
                }
                else { p.Tipo = 2; }
                p.Precio = Convert.ToDouble(txtPrecio.Text);
                p.Fecha = dtpFecha.Value;

                if (miAccion == Accion.nuevo)
                {
                    consultaSQL = $"insert into producto (detalle, tipo, marca, precio, fecha)" +
                                      $"values ('{p.Detalle}'," +
                                              $" {p.Tipo}, " +
                                               $"{p.Marca}, " +
                                               $"{p.Precio}, " +
                                              $"'{p.Fecha}')";

                    oBD.Actualizar(consultaSQL);
                    CargarLista("producto");
                }
                else
                {

                    consultaSQL = $"update producto set detalle = '{p.Detalle}'," +
                                                      $"tipo = {p.Tipo}," +
                                                      $"marca = {p.Marca}," +
                                                     $"precio = {p.Precio}," +
                                                      $"fecha = '{p.Fecha}'" +

                                                      $" where codigo = {p.Codigo}";

                    oBD.Actualizar(consultaSQL);
                    CargarLista("producto");                   
                }
            }

            miAccion = Accion.editado;      //en lugar de usar la bandera nuevo y ponerlo en falso aca hacemos asi y lo cambiamos de estado por editado o por nuevo

            CargarLista("producto");

            Habiliteishon(false);
        }


        //private bool Existe(int pk)
        //{
        //    bool h = false;
        //    for (int i = 0; i < c; i++)
        //    {
        //        if (aP[i].Codigo == pk)
        //        {
        //            h = true;
        //        }
        //    }
        //    return h;
        //}

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Habiliteishon(false);
            Limpieishon();
            miAccion = Accion.editado;
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Limpieishon();
            Close();
        }

        private void frmProducto_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Esta segurisimisimisimo de querer cancelar la operacion", "SALIENDO",
                                                              MessageBoxButtons.YesNo,
                                                              MessageBoxIcon.Question,
                                                              MessageBoxDefaultButton.Button1) ==
                                                                DialogResult.Yes)
                e.Cancel = false;
            else
                e.Cancel = true;
        }

        private void lstProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarCampos(lstProducto.SelectedIndex);
        }
        //--
        private void CargarCampos(int posicion)
        {
            txtCodigo.Text = lP[posicion].Codigo.ToString();
            txtDetalle.Text = lP[posicion].Detalle;
            cboMarca.SelectedValue = lP[posicion].Marca;
            if (lP[posicion].Tipo == 1)
            {
                rbtNoteBook.Checked = true;
            }
            if (lP[posicion].Tipo == 2)
            {
                rbtNetBook.Checked = true;
            }
            txtPrecio.Text = lP[posicion].Precio.ToString();
            dtpFecha.Value = lP[posicion].Fecha;
        }
    }
}
     

