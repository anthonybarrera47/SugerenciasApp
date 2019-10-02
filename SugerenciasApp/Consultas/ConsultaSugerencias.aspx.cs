using BLL;
using Entidades;
using Extensores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SugerenciasApp.Consultas
{
    public partial class ConsultaSugerencias : System.Web.UI.Page
    {
        List<Sugerencias> lista = new List<Sugerencias>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                FechaDesdeTextBox.Text = DateTime.Now.ToFormatDate();
                FechaHastaTextBox.Text = DateTime.Now.ToFormatDate();
            }
        }

        protected void BuscarButton_Click(object sender, EventArgs e)
        {
            Expression<Func<Sugerencias, bool>> filtro = x => true;
            RepositorioBase<Sugerencias> repositorio = new RepositorioBase<Sugerencias>();
            List<Sugerencias> TiposAnalisis = new RepositorioBase<Sugerencias>().GetList(x => true);
            int id;
            switch (BuscarPorDropDownList.SelectedIndex)
            {
                case 0:
                    filtro = x => true;
                    break;
                case 1://ID
                    id = (FiltroTextBox.Text).ToInt();
                    filtro = x => x.SugerenciasId == id;
                    break;
                case 2:
                    filtro = x => x.Descripcion.Contains(FiltroTextBox.Text);
                    break;
            }
            DateTime fechaDesde = FechaDesdeTextBox.Text.ToDatetime();
            DateTime FechaHasta = FechaHastaTextBox.Text.ToDatetime();
            if (FechaCheckBox.Checked)
                lista = repositorio.GetList(filtro).Where(x => x.Fecha.Date >= fechaDesde.Date && x.Fecha.Date <= FechaHasta.Date).ToList();
            else
                lista = repositorio.GetList(filtro);
            this.BindGrid(lista);
        }
        private void BindGrid(List<Sugerencias> lista)
        {
            DatosGridView.DataSource = lista;
            DatosGridView.DataBind();
        }
        protected void FechaCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (FechaCheckBox.Checked)
            {
                FechaDesdeTextBox.Visible = true;
                FechaHastaTextBox.Visible = true;
            }
            else
            {
                FechaDesdeTextBox.Visible = false;
                FechaHastaTextBox.Visible = false;
            }
        }
        protected void DatosGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DatosGridView.DataSource = lista;
            DatosGridView.PageIndex = e.NewPageIndex;
            DatosGridView.DataBind();
        }
    }
}