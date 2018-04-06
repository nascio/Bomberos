using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberos.BLL {


    public enum EstadoBombero : sbyte {
        Vacio = 0,
        Inactivo = 1,
        Activo = 2
    }


    public class Bombero : Notificador {


        int id;
        string nombre;
        string apellido;
        string dpi;
        EstadoBombero estado;

        Usuario user;


        public Int32 ID {
            get {
                return this.id;
            }
            set {
                this.id = value;
            }
        }

        public String NombreCompleto {
            get {
                return String.Concat (this.nombre, " ", this.apellido);
            }
        }



        public String Nombre {
            get {
                return this.nombre;
            }
            set {
                this.nombre = value;
                base.OnPropertyChanged ( );
            }
        }

        public string Apellido {
            get {
                return this.apellido;
            }
            set {
                this.apellido = value;
                base.OnPropertyChanged ( );
            }
        }

        public string DPI {
            get {
                return this.dpi;
            }
            set {
                this.dpi = value;
                base.OnPropertyChanged ( );
            }
        }

        public EstadoBombero Estado {
            get {
                return this.estado;
            }
            set {
                this.estado = value;
                base.OnPropertyChanged ( );
            }
        }




        public String Tiempo {
            get {
                var d = DateTime.Now;

                if (d.Hour > 17 && d.Minute > 30) {
                    return "a buena noche.";
                }

                if (d.Hour > 12) {
                    return "a buena tarde.";
                }

                return " buen dia.";
            }
        }







        public bool AccesoValido (String nombre_control) {
            try {
                if (estado == EstadoBombero.Inactivo) {
                    return false;
                }

                return user.AccesoValido (nombre_control);
            }
            catch {
                return false;
            }
        }







        public static Bombero BuscarPorUsuario (String nickname) {
            try {
                using (var consulta = new DAL.DataSet1TableAdapters.bomberoTableAdapter ( )) {
                    var tabla = consulta.GetDataBy (nickname);

                    using (var sk = tabla.GetEnumerator ( )) {
                        if (!sk.MoveNext ( )) {
                            throw new InvalidOperationException ("Sin usuarios en la consulta.");
                        }

                        var fila = sk.Current;

                        if (sk.MoveNext ( )) {
                            throw new InvalidOperationException ("Multiples usuarios en la consulta.");
                        }

                        return new Bombero ( ) {
                            nombre = fila.nombre,
                            apellido = fila.apellido,
                            dpi = fila.dpi,
                            id = fila.id_bombero,
                            estado = fila.IsestadoNull ( ) ? EstadoBombero.Vacio : (EstadoBombero) fila.estado,
                            user = Usuario.BuscarPorUsuario (fila.id_usuario)
                        };
                    }
                }




            }
            catch (Exception ex) {
                Console.WriteLine (ex.ToString ( ));

                return null;
            }
        }


        public static List<Bombero> ObtenerBomberos ( ) {
            try {
                using (var consulta = new DAL.DataSet1TableAdapters.bomberoTableAdapter ( )) {
                    var tabla = consulta.GetData ( );

                    return tabla.Select (fila => {
                        return new Bombero ( ) {
                            nombre = fila.nombre,
                            apellido = fila.apellido,
                            dpi = fila.dpi,
                            id = fila.id_bombero,
                            estado = fila.IsestadoNull ( ) ? EstadoBombero.Vacio : (EstadoBombero) fila.estado
                        };
                    }).ToList ( );
                }
            }
            catch (Exception ex) {
                Console.WriteLine (ex.ToString ( ));
                return null;
                //throw;
            }
        }




    }
}
