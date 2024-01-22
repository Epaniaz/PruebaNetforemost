using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnica.DAL.Model;
public class RetornoServicio
{
    public int Retorno { get; set; }
    public string Mensaje { get; set; }
    public List<ListadoUsuario> Data { get; set; }
}

public class RetornoServicioObjeto
{
    public int Retorno { get; set; }
    public string Mensaje { get; set; }
    public object Data { get; set; }
}
