using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIServices.Models.DTO
{
    public class ReservationMovementLog
    {
        public int IdLog { get; set; }
        public Nullable<System.DateTime> Fecha { get; set; }
        public int IdUsuario { get; set; }
        public string Usuario { get; set; }
        public string NoReservacion { get; set; }
        public byte Accion { get; set; }
        public string Data_Antes { get; set; }
        public string Data_Despues { get; set; }
        public string Motivo { get; set; }
        public string Comentarios { get; set; }

    }
}
